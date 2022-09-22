using System;
using System.Collections.Generic;
using System.Text;

namespace QASite.Data
{
    public class Likes
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
