using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QASite.Data
{
    public class QASiteRepo
    {
        private readonly string _connectionString;

        public QASiteRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        private int AddTag(string name)
        {
            using var context = new QASiteContext(_connectionString);
            var tag = new Tag { Name = name };
            context.Tag.Add(tag);
            context.SaveChanges();
            return tag.Id;
        }

        private Tag GetTag(string name)
        {
            using var context = new QASiteContext(_connectionString);
            return context.Tag.FirstOrDefault(t => t.Name == name);
        }


        public void AddQuestion(Question question, List<string> tags)
        {
            using var context = new QASiteContext(_connectionString);
            context.Question.Add(question);
            context.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                context.QuestionTags.Add(new QuestionTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }
            context.SaveChanges();
        }

        public List<Question> GetQuestions()
        {
            using var context = new QASiteContext(_connectionString);
            return context.Question.Include(q => q.User).Include(q => q.Likes)
                .Include(q => q.Answers).Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag).OrderByDescending(q => q.Date)
                .ToList();
        }

        public Question GetQuestion(int id)
        {
            using var context = new QASiteContext(_connectionString);
            var question = context.Question.Include(q => q.User).Include(q => q.Likes)
                .Include(q => q.Answers).ThenInclude(a => a.User)
                .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                .FirstOrDefault(q => q.Id == id);
            return question;
        }

        public void AddAnswer(Answer answer)
        {
            using var context = new QASiteContext(_connectionString);
            context.Answer.Add(answer);
            context.SaveChanges();
        }

        public void AddQuestionLikes(int questionId, int userId)
        {
            using var context = new QASiteContext(_connectionString);
            context.Likes.Add(new Likes
            {
                UserId = userId,
                QuestionId = questionId
            });
            context.SaveChanges();
        }

        public int GetLikes(int id)
        {
            using var context = new QASiteContext(_connectionString);
            return context.Question.Include(q => q.Likes).Where(q => q.Id == id).Select(q => q.Likes.Count).FirstOrDefault();
        }

        public int GetUserId(string email)
        {
            using var context = new QASiteContext(_connectionString);
            return context.User.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
        }
    }
}
