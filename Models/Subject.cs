using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Subject
    {
        public Subject()
        {
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public int RoomId { get; set; }
        public int GroupId { get; set; }
        public int DayId { get; set; }
        public string LecNum { get; set; }

        public virtual Day Day { get; set; }
        public virtual Group Group { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
