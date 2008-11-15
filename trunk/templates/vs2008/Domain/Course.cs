using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    public class Course : PersistentEntity
    {
        public Course()
        {
            Students = new List<Student>();
        }

        public virtual int CourseNumber { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual IList<Student> Students { get; private set; }
    }
}
