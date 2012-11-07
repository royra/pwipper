using System;
using System.Collections.Generic;

namespace Pwipper.Models
{
    public class HomeModel
    {
        public string UserName { get; set; }

        public class PwipModel
        {
            public long Id { get; set; }
            public string Text { get; set; }
            public DateTime Time { get; set; }
        }

        public IList<PwipModel> Pwips { get; set; }
    }
}