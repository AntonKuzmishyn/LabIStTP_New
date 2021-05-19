using System;
using System.Collections.Generic;

#nullable disable

namespace Variant5
{
    public partial class Chair
    {
        public Chair()
        {
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public string ChairName { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
