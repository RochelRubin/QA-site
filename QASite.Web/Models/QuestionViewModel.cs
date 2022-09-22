using QASite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QASite.Web.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }

        public bool AlreadyLiked { get; set; }
    }
}
