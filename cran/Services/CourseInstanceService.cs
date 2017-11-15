using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace cran.Services
{
    public class CourseInstanceService : CraniumService, ICourseInstanceService
    {
        private Random _random;

        private readonly IQuestionService _questionService;

        public CourseInstanceService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IPrincipal principal,
            IQuestionService questionService) : base(context, dbLogService, principal)
        {
            _questionService = questionService;
            _random = new Random();
        }

        public async Task<CourseInstanceDto> StartCourseAsync(int courseId)
        {
            Course courseEntity = await _context.FindAsync<Course>(courseId);
            CranUser cranUserEntity = await GetCranUserAsync();

            CourseInstance courseInstanceEntity = new CourseInstance
            {
                User = cranUserEntity,
                Course = courseEntity,
                IdCourse = courseId,
            };
            _context.Add(courseInstanceEntity);

            await SaveChangesAsync();
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);

            return result;
        }

        public async Task<QuestionToAskDto> GetQuestionToAskAsync(int courseInstanceQuestionId)
        {
            QuestionToAskDto questionToAskDto = await _context.CourseInstancesQuestion
                .Where(x => x.Id == courseInstanceQuestionId)
                .Select(x => new QuestionToAskDto
                {
                    IdCourseInstanceQuestion = x.Id,
                    IdCourseInstance = x.CourseInstance.Id,
                    IdQuestion = x.Question.Id,
                    Text = x.Question.Text,
                    CourseEnded = x.CourseInstance.EndedAt.HasValue,
                    NumQuestionsAsked = x.Number,
                    NumQuestions = x.CourseInstance.Course.NumQuestionsToAsk,
                    Answered = x.AnsweredAt.HasValue,
                }).SingleAsync();



            //Images
            questionToAskDto.Images = await _context.RelQuestionImages
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
                Language language = await _context.CourseInstancesQuestion.Where(x => x.Id == courseInstanceQuestionId)
                    .Select(x => x.CourseInstance.Course.Language).SingleAsync();

                int possibleQuestions = await PossibleQuestionsQuery(questionToAskDto.IdCourseInstance, language).CountAsync();
                questionToAskDto.NumQuestions = possibleQuestions <= questionToAskDto.NumQuestions - questionToAskDto.NumQuestionsAsked ? possibleQuestions + questionToAskDto.NumQuestionsAsked : questionToAskDto.NumQuestions;

                //Id nicht leaken, wenn Kurs noch nicht beendet ist.
                questionToAskDto.IdQuestion = 0;
            }
            else
            {
                questionToAskDto.NumQuestions = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == questionToAskDto.IdCourseInstance).CountAsync();
            }


            questionToAskDto.Options = await _context.CourseInstancesQuestionOption
                            .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionId)
                            .OrderBy(x => x.CourseInstanceQuestion.Id)
                            .Select(x => new QuestionOptionToAskDto
                            {
                                IdCourseInstanceQuestionOption = x.Id,
                                Text = x.QuestionOption.Text,
                                IsChecked = x.Checked,
                            }).ToListAsync();


            return questionToAskDto;
        }

        public async Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            int questionId = await _context.CourseInstancesQuestion.Where(x => x.Id == answer.IdCourseInstanceQuestion)
                .Select(x => x.Question.Id).SingleAsync();

            CourseInstanceQuestion courseInstanceQuestion = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            await _context.SaveChangesCranAsync(_currentPrincipal);


            return await _questionService.GetQuestionAsync(questionId);
        }

        public async Task<CourseInstanceDto> NextQuestion(int courseInstanceId)
        {
            CourseInstance courseInstanceEntity = _context.Find<CourseInstance>(courseInstanceId);
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);
            await SaveChangesAsync();
            return result;
        }

        public async Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            //Nächste Frage vorbereiten.
            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            CourseInstance courseInstanceEntity = await _context.FindAsync<CourseInstance>(courseInstanceQuestionEntity.IdCourseInstance);
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);
            result.AnsweredCorrectly = courseInstanceQuestionEntity.Correct;
            return result;
        }

        public async Task<ResultDto> GetCourseResultAsync(int idCourseInstance)
        {
            CourseInstance courseInstance = await _context.FindAsync<CourseInstance>(idCourseInstance);
            Course course = await _context.FindAsync<Course>(courseInstance.IdCourse);
            ResultDto result = new ResultDto()
            {
                IdCourseInstance = idCourseInstance,
                CourseTitle = course.Title,
            };

            result.Questions = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == idCourseInstance)
                .Select(x => new QuestionResultDto
                {
                    IdCourseInstanceQuestion = x.Id,
                    IdQuestion = x.Question.Id,
                    Title = x.Question.Title,
                    Correct = x.Correct,
                }).ToListAsync();

            //Tags holen
            var tags = await _context.RelQuestionTags.Where(x => x.Question.CourseInstancesQuestion.Any(y => y.CourseInstance.Id == idCourseInstance))
                .Select(x => new
                {
                    IdQuestion = x.Question.Id,
                    IdTag = x.Tag.Id,
                    x.Tag.Name,
                    x.Tag.Description,
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
                            Description = tag.Description,
                            Name = tag.Name,
                        });
                    }
                }
            }

            return result;
        }

        public async Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync()
        {
            string userid = GetUserId();
            IQueryable<CourseInstanceListEntryDto> query = _context.CourseInstances.Where(x => x.User.UserId == userid)
                .Select(x => new CourseInstanceListEntryDto()
                {
                    IdCourseInstance = x.Id,
                    Title = x.Course.Title,
                    NumQuestionsCorrect = x.CourseInstancesQuestion.Count(y => y.Correct),
                    NumQuestionsTotal = x.CourseInstancesQuestion.Count(),
                    InsertDate = x.InsertDate,
                })
                .OrderByDescending(x => x.InsertDate);

            IList<CourseInstanceListEntryDto> result = await query.ToListAsync();
            return result;
        }

        public async Task DeleteCourseInstanceAsync(int idCourseInstance)
        {
            await CheckAccessToCourseInstance(idCourseInstance);

            CourseInstance instance = await _context.FindAsync<CourseInstance>(idCourseInstance);


            IList<CourseInstanceQuestionOption> courseInstanceQuestionOptions =
                    await _context.CourseInstancesQuestionOption
                    .Where(x => x.CourseInstanceQuestion.CourseInstance.Id == instance.Id)
                    .ToListAsync();

            foreach (CourseInstanceQuestionOption courseInstanceQuestionOption in courseInstanceQuestionOptions)
            {
                _context.CourseInstancesQuestionOption.Remove(courseInstanceQuestionOption);
            }

            IList<CourseInstanceQuestion> coureInstanceQuestions = await _context.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == instance.Id)
                .ToListAsync();

            foreach (CourseInstanceQuestion courseInstanceQuestion in coureInstanceQuestions)
            {
                _context.Remove(courseInstanceQuestion);
            }

            _context.Remove(instance);

            await SaveChangesAsync();

        }

        private IQueryable<int> PossibleQuestionsQuery(int idCourseInstance, Language language)
        {
            //Get Tags of course
            IQueryable<int> tagIds = _context.RelCourseTags.Where(x => x.Course.CourseInstances.Any(y => y.Id == idCourseInstance))
                .Select(x => x.Tag.Id);

            //Questions already asked
            IQueryable<int> questionIdsAlreadyAsked = _context.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == idCourseInstance)
                .Select(x => x.Question.Id);

            //Possible Quetions Query
            IQueryable<int> questionIds = _context.RelQuestionTags
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

            Course courseEntity = await _context.FindAsync<Course>(courseInstanceEntity.IdCourse);

            result.NumQuestionsAlreadyAsked = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
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
                Question questionEntity = await _context.FindAsync<Question>(questionId);

                //Course instance question
                CourseInstanceQuestion courseInstanceQuestionEntity = new CourseInstanceQuestion
                {
                    CourseInstance = courseInstanceEntity,
                    Question = questionEntity,
                    Number = result.NumQuestionsAlreadyAsked + 1,
                };
                _context.Add(courseInstanceQuestionEntity);

                //Course instance question options
                IList<QuestionOption> options = await _context.QuestionOptions.Where(option => option.Question.Id == questionEntity.Id).ToListAsync();
                foreach (QuestionOption questionOptionEntity in options)
                {
                    CourseInstanceQuestionOption courseInstanceQuestionOptionEntity = new CourseInstanceQuestionOption();

                    courseInstanceQuestionOptionEntity.QuestionOption = questionOptionEntity;

                    courseInstanceQuestionOptionEntity.CourseInstanceQuestion = courseInstanceQuestionEntity;
                    courseInstanceQuestionEntity.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);

                    _context.Add(courseInstanceQuestionOptionEntity);
                }

                await SaveChangesAsync();
                result.IdCourseInstanceQuestion = courseInstanceQuestionEntity.Id;
            }


            return result;
        }

        private async Task EndCourseAsync(int courseInstanceId)
        {
            CourseInstance courseInstance = await _context.FindAsync<CourseInstance>(courseInstanceId);
            courseInstance.EndedAt = DateTime.Now;
            await SaveChangesAsync();
        }

        
        private async Task SaveAnswers(QuestionAnswerDto answer)
        {

            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);

            //Check if already answered.
            if (courseInstanceQuestionEntity.AnsweredAt.HasValue)
            {
                return;
            }

            IList<CourseInstanceQuestionOption> options = await _context.CourseInstancesQuestionOption
                .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionEntity.Id)
                .OrderBy(x => x.Id)
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

            await SaveChangesAsync();
        }

        private async Task CheckAccessToCourseInstance(int idCourseInstance)
        {
            CourseInstance instance = await _context.FindAsync<CourseInstance>(idCourseInstance);

            //Security Check
            bool hasWriteAccess = await HasWriteAccess(instance.IdUser);

            //Security Check
            if (!hasWriteAccess)
            {
                throw new SecurityException("no access to this course instance");
            }
        }

    }
}
