using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Day
    {
        public Day()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string DayName { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
