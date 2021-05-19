using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Room
    {
        public Room()
        {
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
