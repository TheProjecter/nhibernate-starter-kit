using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Venue : PersistentEntity
    {
        public Venue()
        {
            Courses = new List<Course>();
        }

        public virtual string Name { get; set; }
        public virtual IList<Course> Courses { get; private set; }
    }
}
