using cran.Model.Dto;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Mappers
{
    public static class FromCourseToCourseDtoMapper
    {
        public static CourseDto Map(this Course course, bool isEditable, bool isFavorite)
        {
            CourseDto courseVm = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Language = course.Language.ToString(),
                Description = course.Description,
                NumQuestionsToAsk = course.NumQuestionsToAsk,
                IsEditable = isEditable,
                IsFavorite = isFavorite,
            };

            foreach (RelCourseTag relTag in course.RelTags)
            {
                Tag tag = relTag.Tag;
                TagDto tagVm = new TagDto
                {
                    Id = tag.Id,
                    IdTagType = (int)tag.TagType,
                    Description = tag.Description,
                    Name = tag.Name,
                    ShortDescDe = tag.ShortDescDe,
                    ShortDescEn = tag.ShortDescEn,
                };
                courseVm.Tags.Add(tagVm);
            }
            return courseVm;
        }
    }
}
