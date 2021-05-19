using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Teacher
    {
        public Teacher()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public int ChairId { get; set; }
        public int SubjectId { get; set; }

        public virtual Chair Chair { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
