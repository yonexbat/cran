using cran.Model.Entities;
using cran.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICraniumService
    {
        Task<int> AddQuestionAsync(QuestionViewModel question);

        Task<CoursesListViewModel> CoursesAsync();

        Task<QuestionViewModel> GetQuestionAsync(int id);
    }
}
