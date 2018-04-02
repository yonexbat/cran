using cran.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cran.Model.Entities;
using System.Security.Principal;
using cran.Model.Dto;
using cran.Model;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public abstract class CraniumService : Service
    {

        protected IDbLogService _dbLogService;       
        protected static int PageSize = 5;


        public CraniumService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) :
            base(context, principal)
        {
            _context = context;
            _dbLogService = dbLogService;
            _currentPrincipal = principal;
        }

        protected IList<T> ToDtoList<T, Q>(IList<Q> input, Func<Q, T> func)
        {
            IList<T> result = new List<T>();
            foreach (Q q in input)
            {
                T t = func(q);
                result.Add(t);
            }
            return result;
        }


        protected void UpdateRelation<Tdto, Tentity>(IList<Tdto> dtos, IList<Tentity> entities) 
            where Tdto: IDto 
            where Tentity : CranEntity, IIdentifiable, new()
        {
            IEnumerable<int> idsEntities = entities.Select(x => x.Id);
            IEnumerable<int> idsDtos = dtos.Select(x => x.Id);
            IEnumerable<IIdentifiable> entitiesToDelete = entities.Where(x => idsDtos.All(id => id != x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> entitiesToUpdate = entities.Where(x => idsDtos.Any(id => id == x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> dtosToAdd = dtos.Where(x => x.Id <= 0).Cast<IIdentifiable>();
            
            //Delete
            foreach(IIdentifiable entity in entitiesToDelete)
            {
                _context.Remove(entity);
            }

            //Update
            foreach(CranEntity entity in entitiesToUpdate)
            {
                IDto dto = dtos.Single(x => x.Id == entity.Id);
                CopyData(dto, entity);
            }
            
            //Add
            foreach(IDto dto in dtosToAdd)
            {
                Tentity entity = new Tentity();
                CopyData(dto, entity);
                _context.Set<Tentity>().Add(entity);
            }
        }

        protected virtual void CopyData(object dto, CranEntity entity)
        {
            if(dto is QuestionOptionDto && entity is QuestionOption)
            {
                QuestionOptionDto dtoSource = (QuestionOptionDto)dto;
                QuestionOption entityDestination = (QuestionOption) entity;
                entityDestination.IsTrue = dtoSource.IsTrue;
                entityDestination.Text = dtoSource.Text ?? string.Empty;
                entityDestination.IdQuestion = dtoSource.IdQuestion;
            }
            else if (dto is QuestionDto && entity is Question)
            {
                QuestionDto dtoSource = (QuestionDto )dto;
                Question entityDestination = (Question)entity;
                entityDestination.Title = dtoSource.Title;
                entityDestination.Text = dtoSource.Text ?? string.Empty;
                entityDestination.Explanation = dtoSource.Explanation;
                entityDestination.Language = Enum.Parse<Language>(dtoSource.Language);
            }
            else if(dto is RelQuestionTagDto && entity is RelQuestionTag)
            {
                RelQuestionTagDto dtoSource = (RelQuestionTagDto)dto;
                RelQuestionTag entityDestination = (RelQuestionTag)entity;
                entityDestination.IdQuestion = dtoSource.IdQuestion;
                entityDestination.IdTag = dtoSource.IdTag;
            }
            else if(dto is RelQuestionImageDto && entity is RelQuestionImage)
            {
                RelQuestionImageDto dtoSource = (RelQuestionImageDto)dto;
                RelQuestionImage entityDestination = (RelQuestionImage)entity;
                entityDestination.IdQuestion = dtoSource.IdQuestion;
                entityDestination.IdImage = dtoSource.IdImage;
            }
            else if(dto is ImageDto && entity is Image)
            {
                ImageDto dtoSource = (ImageDto)dto;
                Image entityDestination = (Image) entity;
                entityDestination.Width = dtoSource.Width;
                entityDestination.Height = dtoSource.Height;
                entityDestination.Full = dtoSource.Full;
            }
            else if(dto is CourseDto && entity is Course)
            {
                CourseDto dtoSource = (CourseDto)dto;
                Course entityDestination = (Course)entity;
                entityDestination.Title = dtoSource.Title;
                entityDestination.Language = Enum.Parse<Language>(dtoSource.Language);
                entityDestination.NumQuestionsToAsk = dtoSource.NumQuestionsToAsk;
                entityDestination.Description = dtoSource.Description;                
            }
            else if (dto is RelCourseTagDto && entity is RelCourseTag)
            {
                RelCourseTagDto dtoSource = (RelCourseTagDto)dto;
                RelCourseTag entityDestination = (RelCourseTag)entity;
                entityDestination.IdCourse = dtoSource.IdCourse;
                entityDestination.IdTag = dtoSource.IdTag;
            }
            else
            {
                throw new NotImplementedException();
            }                  
        }            
        

        protected async Task<bool> HasWriteAccess(int idUser)
        {
            CranUser cranUser = await _context.FindAsync<CranUser>(idUser);

            //Security Check
            if (cranUser.UserId == GetUserId() || _currentPrincipal.IsInRole(Roles.Admin))
            {
                return true;
            }
            return false;
        }
               

        private int InitPagedResult(IPagedResult pagedResult, int count, int page)
        {
            pagedResult.Count = count;
            pagedResult.Pagesize = PageSize;
            pagedResult.Numpages = CalculateNumPages(count);
            pagedResult.CurrentPage = pagedResult.Numpages >= page ? page : pagedResult.Numpages;
            return pagedResult.CurrentPage * PageSize;
        }

        protected async Task<PagedResultDto<TModel>> ToPagedResult<TEntity, TModel>(IQueryable<TEntity> queryBeforeSkipAndTake,
           int page, Func<IQueryable<TEntity>, Task<IList<TModel>>> toDtoFunc)
               where TModel : class
        {
            PagedResultDto<TModel> resultDto = new PagedResultDto<TModel>();

            //Count und paging.
            int count = await queryBeforeSkipAndTake.CountAsync();
            int startindex = InitPagedResult(resultDto, count, page);

            //Daten 
            IQueryable<TEntity> query = queryBeforeSkipAndTake.Skip(startindex).Take(PageSize);

            resultDto.Data = await toDtoFunc(query);

            return resultDto;
        }

        private int CalculateNumPages(int count)
        {
            return ((count + PageSize - 1) / PageSize);   
        }
        
    }
}
