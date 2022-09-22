using System;

namespace QASite.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
    }
}
