using System;
using System.Collections.Generic;
using System.Text;

namespace QASite.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
    }
}
