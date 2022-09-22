using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QASite.Data;
using QASite.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Controllers
{
    public class QuestionController : Controller
    {
        private string _connectionString;

        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Authorize]
        [HttpPost]
        public void AddQuestionLikes(int questionId)
        {
            var repo = new QASiteRepo(_connectionString);
            var userId = repo.GetUserId(User.Identity.Name);
            repo.AddQuestionLikes(questionId, userId);
        }

        public int GetLikes(int id)
        {
            var repo = new QASiteRepo(_connectionString);
            return repo.GetLikes(id);
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Question question, List<string> tags)
        {
            var repo = new QASiteRepo(_connectionString);
            int? userId = repo.GetUserId(User.Identity.Name);
            question.UserId = userId.Value;
            question.Date = DateTime.Now;
            repo.AddQuestion(question, tags);
            return Redirect("/home/index");
        }

        public IActionResult ViewQuestion(int id)
        {
            var repo = new QASiteRepo(_connectionString);
            var question = repo.GetQuestion(id);
            var qvm = new QuestionViewModel
            {
                Question = question
            };
            qvm.AlreadyLiked = question.Likes.Any(l => l.User.Email == User.Identity.Name);
            return View(qvm);
        }

        [HttpPost]
        public IActionResult AddAnswer(Answer answer)
        {
            var repo = new QASiteRepo(_connectionString);
            int? userId = repo.GetUserId(User.Identity.Name);
            answer.UserID = userId.Value;
            answer.Date = DateTime.Now;
            repo.AddAnswer(answer);
            return Redirect($"/question/viewquestion?id={answer.QuestionId}");
        }
    }
}
