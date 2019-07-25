using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cran.tests.Infra
{
    public class InMemoryDbSetup
    {
        public void SetUpInMemoryDb(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            //Tags
            IList<Tag> tags = new List<Tag>();
            for (int i = 1; i <= 4; i++)
            {
                Tag tag = new Tag()
                {
                    Name = $"Name{i}",
                    ShortDescDe = $"ShortDescDe{i}",
                    ShortDescEn = $"ShortDescEn{i}",
                    Description = $"Description{i}",
                    TagType = TagType.Standard,
                };
                context.Tags.Add(tag);
                tags.Add(tag);
            };

            //add a non standard Tag
            Tag nonStandard = new Tag()
            {
                Name = $"Deprecated",
                ShortDescDe = $"Veraltet",
                ShortDescEn = $"Deprecated",
                Description = $"Deprecated",
                TagType = TagType.Warning,
            };
            context.Tags.Add(nonStandard);
            tags.Add(nonStandard);

            context.SaveChanges();

            //Questions
            for (int i = 1; i <= 10; i++)
            {
                CreateMockQuestion(context, i, tags);
            }

            //Courses
            Course course = new Course()
            {
                Description = $"Description",
                Title = $"Title",
                NumQuestionsToAsk = 3,
                Language = Language.De,
            };
            context.Courses.Add(course);
            RelCourseTag relCourseTag = new RelCourseTag()
            {
                Course = course,
                Tag = tags.First(),
            };
            context.RelCourseTags.Add(relCourseTag);

            //User
            CranUser cranUser = new CranUser()
            {
                IsAnonymous = false,
                UserId = "testuser",                  
            };
            context.CranUsers.Add(cranUser);

            context.SaveChanges();
        }

        protected virtual void CreateMockQuestion(ApplicationDbContext context, int id, IList<Tag> tags)
        {

            Question question = new Question()
            {
                Explanation = $"Explanation{id}",
                Text = $"Text{id}",
                User = new CranUser() { UserId = $"UserId{id}", },
                Container = new Container() { },
                Status = QuestionStatus.Released,
                Language = Language.De,
                QuestionType = QuestionType.MultipleChoice,
            };
            context.Questions.Add(question);

            //Options
            for (int i = 1; i <= 4; i++)
            {
                QuestionOption option = new QuestionOption()
                {
                    IdQuestion = question.Id,
                    Text = $"OptionText{i}",
                    IsTrue = i % 2 == 0,
                    Question = question,
                };
                question.Options.Add(option);
                context.QuestionOptions.Add(option);
            }

            //Tags
            foreach(Tag tag in tags.Where(x => x.TagType == TagType.Standard))
            {
                RelQuestionTag relTag = new RelQuestionTag
                {
                    Question = question,
                    Tag = tag,
                };
                context.RelQuestionTags.Add(relTag);
            }          

            //Binary
            for (int i = 1; i <= 3; i++)
            {
                Binary binary = new Binary()
                {
                    ContentType = "image/png",
                    FileName = $"Filename{i + id * 1000}",
                    ContentDisposition = $"form-data; name=\"files\"; filename=\"Untitled.png\"",
                    Length = 20618,
                };
                context.Binaries.Add(binary);

                Image image = new Image()
                {
                    Binary = binary,
                    Height = 300,
                };
                context.Images.Add(image);

                RelQuestionImage relQuestionImage = new RelQuestionImage
                {
                    Question = question,
                    Image = image,
                };
                context.RelQuestionImages.Add(relQuestionImage);
            }

            context.SaveChanges();
        }
    }
}
