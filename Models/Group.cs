using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Group
    {
        public Group()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public int NumStudents { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
