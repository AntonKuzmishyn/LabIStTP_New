using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Faculty
    {
        public Faculty()
        {
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public string FacultyName { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
