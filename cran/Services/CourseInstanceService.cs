﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security;
using cran.Services.Util;

namespace cran.Services
{
    public class CourseInstanceService : ICourseInstanceService
    {
        private Random _random;

        private readonly IQuestionService _questionService;
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IBusinessSecurityService _businessSecurityService;
        private readonly IDbLogService _dbLogService;

        public CourseInstanceService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IQuestionService questionService,
            ISecurityService securityService,
            IUserService userService,
            IBusinessSecurityService businessSecurityService)
        {
            _questionService = questionService;
            _securityService = securityService;
            _dbContext = context;
            _userService = userService;
            _businessSecurityService = businessSecurityService;
            _dbLogService = dbLogService;
            _random = new Random();
        }

        public async Task<CourseInstanceDto> StartCourseAsync(int courseId)
        {
            

            Course courseEntity = await _dbContext.FindAsync<Course>(courseId);
            CranUser cranUserEntity = await _userService.GetOrCreateCranUserAsync();

            await _dbLogService.LogMessageAsync($"Starting course {courseId}. Name: {courseEntity.Title}");

            CourseInstance courseInstanceEntity = new CourseInstance
            {
                User = cranUserEntity,
                Course = courseEntity,
                IdCourse = courseId,
            };

            await _dbContext.AddAsync(courseInstanceEntity);

            await _dbContext.SaveChangesAsync();
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);

            return result;
        }

        public async Task<QuestionToAskDto> GetQuestionToAskAsync(int courseInstanceQuestionId)
        {
            QuestionToAskDto questionToAskDto = await _dbContext.CourseInstancesQuestion
                .Where(x => x.Id == courseInstanceQuestionId)
                .Select(x => new QuestionToAskDto
                {
                    IdCourseInstanceQuestion = x.Id,
                    IdCourseInstance = x.CourseInstance.Id,
                    IdQuestion = x.Question.Id,
                    Text = x.Question.Text,
                    QuestionType = x.Question.QuestionType,
                    CourseEnded = x.CourseInstance.EndedAt.HasValue,
                    NumCurrentQuestion = x.Number,
                    NumQuestions = x.CourseInstance.Course.NumQuestionsToAsk,
                    Answered = x.AnsweredAt.HasValue,
                    AnswerShown = x.AnswerShown,
                }).SingleAsync();

            //Check access
            await CheckAccessToCourseInstance(questionToAskDto.IdCourseInstance);

            //Images
            questionToAskDto.Images = await _dbContext.RelQuestionImages
               .Where(x => x.Question.CourseInstancesQuestion.Any(y => y.Id == courseInstanceQuestionId))
               .Select(x => new ImageDto
               {
                   Id = x.Image.Id,
                   IdBinary = x.Image.Binary.Id,
                   Full = x.Image.Full,
                   Height = x.Image.Height,
                   Width = x.Image.Width,
               }).ToListAsync();

            //NumQuestions korrigieren, falls es nicht genügend Fragen hat. 
            //Diese Infos ist nur nötig, wenn Kurs noch nicht beendet ist.
            if (!questionToAskDto.CourseEnded)
            {
                Language language = await _dbContext.CourseInstancesQuestion.Where(x => x.Id == courseInstanceQuestionId)
                    .Select(x => x.CourseInstance.Course.Language).SingleAsync();

                int possibleQuestions = await PossibleQuestionsQuery(questionToAskDto.IdCourseInstance, language).CountAsync();
                questionToAskDto.NumQuestions = possibleQuestions <= questionToAskDto.NumQuestions - questionToAskDto.NumCurrentQuestion ? possibleQuestions + questionToAskDto.NumCurrentQuestion : questionToAskDto.NumQuestions;

                //Id nicht leaken, wenn Kurs noch nicht beendet ist.
                if (!questionToAskDto.AnswerShown)
                {
                    questionToAskDto.IdQuestion = 0;
                }
            }
            else
            {
                questionToAskDto.NumQuestions = await _dbContext.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == questionToAskDto.IdCourseInstance).CountAsync();
            }

            //Optionen
            questionToAskDto.Options = await _dbContext.CourseInstancesQuestionOption
                            .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionId)
                            .OrderBy(x => x.QuestionOption.Id)
                            .Select(x => new QuestionOptionToAskDto
                            {
                                IdCourseInstanceQuestionOption = x.Id,
                                Text = x.QuestionOption.Text,
                                IsChecked = x.Checked,                                
                            }).ToListAsync();


            //Questions already asked
            questionToAskDto.QuestionSelectors = await _dbContext.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == questionToAskDto.IdCourseInstance)
                .Select(x => new QuestionSelectorInfoDto()
                {
                    IdCourseInstanceQuestion = x.Id,
                    Number = x.Number,
                    Correct = x.Correct,
                    AnswerShown = x.AnswerShown,
                }).OrderBy(x => x.Number).ToListAsync();

            if (!questionToAskDto.CourseEnded)
            {
                foreach(var q in questionToAskDto.QuestionSelectors.Where(x => !x.AnswerShown))
                {
                    q.Correct = null;
                }
            }

            return questionToAskDto;
        }

        public async Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            int questionId = await _dbContext.CourseInstancesQuestion.Where(x => x.Id == answer.IdCourseInstanceQuestion)
                .Select(x => x.Question.Id).SingleAsync();

            CourseInstanceQuestion courseInstanceQuestion = await _dbContext.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            courseInstanceQuestion.AnswerShown = true;
            await _dbContext.SaveChangesAsync();


            return await _questionService.GetQuestionAsync(questionId);
        }

        public async Task<CourseInstanceDto> NextQuestion(int courseInstanceId)
        {
            CourseInstance courseInstanceEntity = _dbContext.Find<CourseInstance>(courseInstanceId);
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);
            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            //Nächste Frage vorbereiten.
            CourseInstanceQuestion courseInstanceQuestionEntity = await _dbContext.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            CourseInstance courseInstanceEntity = await _dbContext.FindAsync<CourseInstance>(courseInstanceQuestionEntity.IdCourseInstance);
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);
            result.AnsweredCorrectly = courseInstanceQuestionEntity.Correct;
            return result;
        }

        public async Task<ResultDto> GetCourseResultAsync(int idCourseInstance)
        {
            CourseInstance courseInstance = await _dbContext.FindAsync<CourseInstance>(idCourseInstance);
            Course course = await _dbContext.FindAsync<Course>(courseInstance.IdCourse);
            ResultDto result = new ResultDto()
            {
                IdCourse = course.Id,
                IdCourseInstance = idCourseInstance,
                CourseTitle = course.Title,
                StartedAt = courseInstance.InsertDate,
                EndedAt = courseInstance.EndedAt,
            };

            result.Questions = await _dbContext.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == idCourseInstance)
                .Select(x => new QuestionResultDto
                {
                    IdCourseInstanceQuestion = x.Id,
                    IdQuestion = x.Question.Id,
                    Title = x.Question.Title,
                    Correct = x.Correct,
                }).ToListAsync();

            //Tags holen
            var tags = await _dbContext.RelQuestionTags.Where(x => x.Question.CourseInstancesQuestion.Any(y => y.CourseInstance.Id == idCourseInstance))
                .Select(x => new
                {
                    IdQuestion = x.Question.Id,
                    IdTag = x.Tag.Id,
                    x.Tag.TagType,
                    x.Tag.Name,
                    x.Tag.Description,
                    x.Tag.ShortDescDe,
                    x.Tag.ShortDescEn,

                }).ToListAsync();
            var tagLookups = tags.ToLookup(x => x.IdQuestion, x => x);

            foreach (QuestionResultDto questionDto in result.Questions)
            {
                if (tagLookups.Contains(questionDto.IdQuestion))
                {
                    var tagLookup = tagLookups[questionDto.IdQuestion];
                    foreach (var tag in tagLookup)
                    {
                        questionDto.Tags.Add(new TagDto
                        {
                            Id = tag.IdTag,
                            IdTagType = (int) tag.TagType,
                            Description = tag.Description,
                            Name = tag.Name,
                            ShortDescDe = tag.ShortDescDe,
                            ShortDescEn = tag.ShortDescEn,
                        });
                    }
                }
            }

            return result;
        }
     

        public async Task DeleteCourseInstanceAsync(int idCourseInstance)
        {
            await CheckAccessToCourseInstance(idCourseInstance);

            CourseInstance instance = await _dbContext.FindAsync<CourseInstance>(idCourseInstance);


            IList<CourseInstanceQuestionOption> courseInstanceQuestionOptions =
                    await _dbContext.CourseInstancesQuestionOption
                    .Where(x => x.CourseInstanceQuestion.CourseInstance.Id == instance.Id)
                    .ToListAsync();

            foreach (CourseInstanceQuestionOption courseInstanceQuestionOption in courseInstanceQuestionOptions)
            {
                _dbContext.CourseInstancesQuestionOption.Remove(courseInstanceQuestionOption);
            }

            IList<CourseInstanceQuestion> coureInstanceQuestions = await _dbContext.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == instance.Id)
                .ToListAsync();

            foreach (CourseInstanceQuestion courseInstanceQuestion in coureInstanceQuestions)
            {
                _dbContext.Remove(courseInstanceQuestion);
            }

            _dbContext.Remove(instance);

            await _dbContext.SaveChangesAsync();

        }

        private IQueryable<int> PossibleQuestionsQuery(int idCourseInstance, Language language)
        {
            //Get Tags of course
            IQueryable<int> tagIds = _dbContext.RelCourseTags.Where(x => x.Course.CourseInstances.Any(y => y.Id == idCourseInstance))
                .Select(x => x.Tag.Id);

            //Questions already asked
            IQueryable<int> questionIdsAlreadyAsked = _dbContext.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == idCourseInstance)
                .Select(x => x.Question.Id);

            //Possible Questions Query
            IQueryable<int> questionIds = _dbContext.RelQuestionTags
                .Where(x => tagIds.Contains(x.Tag.Id))
                .Where(x => !questionIdsAlreadyAsked.Contains(x.Question.Id))
                .Where(x => x.Question.Status == QuestionStatus.Released)
                .Where(x => x.Question.Language == language)
                .Select(x => x.Question.Id);

            return questionIds;
        }

        private async Task<CourseInstanceDto> GetNextQuestion(CourseInstance courseInstanceEntity)
        {
            CourseInstanceDto result = new CourseInstanceDto();
            result.IdCourse = courseInstanceEntity.IdCourse;
            result.IdCourseInstance = courseInstanceEntity.Id;

            Course courseEntity = await _dbContext.FindAsync<Course>(courseInstanceEntity.IdCourse);

            result.NumQuestionsAlreadyAsked = await _dbContext.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .CountAsync();

            result.NumQuestionsTotal = courseEntity.NumQuestionsToAsk;

            //Possible Quetions Query
            IQueryable<int> questionIds = PossibleQuestionsQuery(courseInstanceEntity.Id, courseEntity.Language);

            int count = await questionIds.CountAsync();
            if (count == 0 || result.NumQuestionsAlreadyAsked >= courseEntity.NumQuestionsToAsk)
            {
                result.IdCourseInstanceQuestion = 0;
                result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked;
                result.Done = true;
                await EndCourseAsync(courseInstanceEntity.Id);
            }
            else
            {
                if (count <= result.NumQuestionsTotal - result.NumQuestionsAlreadyAsked)
                {
                    result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked + count;
                }
                int quesitonNo = _random.Next(0, count - 1);
                int questionId = await questionIds.Skip(quesitonNo).FirstAsync();
                Question questionEntity = await _dbContext.FindAsync<Question>(questionId);

                //Course instance question
                CourseInstanceQuestion courseInstanceQuestionEntity = new CourseInstanceQuestion
                {
                    CourseInstance = courseInstanceEntity,
                    Question = questionEntity,
                    Number = result.NumQuestionsAlreadyAsked + 1,
                };
                await _dbContext.AddAsync(courseInstanceQuestionEntity);

                //Course instance question options
                IList<QuestionOption> options = await _dbContext.QuestionOptions.Where(option => option.Question.Id == questionEntity.Id).ToListAsync();
                foreach (QuestionOption questionOptionEntity in options)
                {
                    CourseInstanceQuestionOption courseInstanceQuestionOptionEntity = new CourseInstanceQuestionOption();

                    courseInstanceQuestionOptionEntity.QuestionOption = questionOptionEntity;

                    courseInstanceQuestionOptionEntity.CourseInstanceQuestion = courseInstanceQuestionEntity;
                    courseInstanceQuestionEntity.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);

                    await _dbContext.AddAsync(courseInstanceQuestionOptionEntity);
                }

                await _dbContext.SaveChangesAsync();
                result.IdCourseInstanceQuestion = courseInstanceQuestionEntity.Id;
            }


            return result;
        }

        private async Task EndCourseAsync(int courseInstanceId)
        {
            CourseInstance courseInstance = await _dbContext.FindAsync<CourseInstance>(courseInstanceId);
            courseInstance.EndedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }

        
        private async Task SaveAnswers(QuestionAnswerDto answer)
        {

            CourseInstanceQuestion courseInstanceQuestionEntity = await _dbContext.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);

            //Check if already answered.
            if (courseInstanceQuestionEntity.AnsweredAt.HasValue && courseInstanceQuestionEntity.AnswerShown)
            {
                return;
            }

            IList<CourseInstanceQuestionOption> options = await _dbContext.CourseInstancesQuestionOption
                .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionEntity.Id)
                .OrderBy(x => x.QuestionOption.Id)
                .Include(x => x.QuestionOption).ToListAsync();

            if (options.Count != answer.Answers.Count)
            {
                throw new InvalidOperationException($"Num options in client-answer and database do not match");
            }
            for (int i = 0; i < options.Count; i++)
            {
                CourseInstanceQuestionOption optionInstanceEntity = options[i];
                bool thisAnswer = answer.Answers[i];
                bool optioncorrect = optionInstanceEntity.QuestionOption.IsTrue == thisAnswer;

                optionInstanceEntity.Checked = thisAnswer;
                optionInstanceEntity.Correct = optioncorrect;
            }
            courseInstanceQuestionEntity.Correct = options.All(x => x.Correct);
            courseInstanceQuestionEntity.AnsweredAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();
        }

        private async Task CheckAccessToCourseInstance(int idCourseInstance)
        {
            CourseInstance instance = await _dbContext.FindAsync<CourseInstance>(idCourseInstance);

            //Security Check
            bool hasWriteAccess = await _businessSecurityService.HasWriteAccess(instance.IdUser);

            //Security Check
            if (!hasWriteAccess)
            {
                throw new SecurityException("no access to this course instance");
            }
        }

        public async Task<PagedResultDto<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync(int page)
        {
            string userid = _securityService.GetUserId();
            IQueryable<CourseInstance> query = _dbContext.CourseInstances
                .Where(x => x.User.UserId == userid)              
                .OrderByDescending(x => x.InsertDate)
                .ThenBy(x => x.Id);

            PagedResultDto<CourseInstanceListEntryDto> resultDto = await PagedResultUtil.ToPagedResult(query, page, ToDto);
           
            return resultDto;
        }

        private async Task<IList<CourseInstanceListEntryDto>> ToDto(IQueryable<CourseInstance> query)
        {
            IList<CourseInstanceListEntryDto> result = await query.Select(x => new CourseInstanceListEntryDto()
            {
                IdCourseInstance = x.Id,
                Title = x.Course.Title,
                NumQuestionsCorrect = x.CourseInstancesQuestion.Count(y => y.Correct),
                NumQuestionsTotal = x.CourseInstancesQuestion.Count(),
                InsertDate = x.InsertDate,
            }).ToListAsync();
            return result;
        }

        public async Task AnswerQuestionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);
        }
    }
}
