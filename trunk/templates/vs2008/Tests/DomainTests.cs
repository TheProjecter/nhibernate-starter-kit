using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace $safeprojectname$
{
    [TestFixture]
    public class DomainTests
    {
        //The SchemaGenerator must be run first before any tests. The setup methods could be modified to automatically regenerate the schema, to avoid this. 

        private ISessionFactory _factory;
        private ISession _session;

        #region Setup and Teardown
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            //Create a new configuration for building the session factory. This is an expensive operation, so built in the FixtureSetUp.
            Configuration configuration = new Configuration();
            configuration.Configure();
            _factory = configuration.BuildSessionFactory();
        }

        [SetUp]
        public void SetUp()
        {
            //Create a new session for every test. Prevents side effects of previously run tests affecting the rest.
            _session = _factory.OpenSession();
        }

        [TearDown]
        public void TearDown()
        {
            //Dispose the session to release resources
            _session.Dispose();
        }

        protected void StartNewSession()
        {
            //The session caches items. Flushing and starting a new session ensures that tests hit the database when intended.
            _session.Flush();
            _session.Dispose();
            _session = _factory.OpenSession();
        } 
        #endregion

        [Test]
        public void CanSaveNewVenue()
        {
            //Optionally, the methods illustrated in the SchemaGenerator can be incorporated into the FixtureSetup/SetUp,
            //ensuring a clean database for each fixture/test
            Venue venue = new Venue();
            venue.Name = "TestVenue";
            _session.Save(venue);
            //Flush the session, and dispose, to ensure that we load from the database
            StartNewSession();
            Venue loadedVenue = _session.Load<Venue>(venue.Id);
            //Assert that the entity loaded has the same name as the entity that we saved
            Assert.AreEqual("TestVenue", loadedVenue.Name);
        }

        [Test]
        public void CanSaveNewCourseAssociatedWithVenue()
        {
            Venue venue = new Venue();
            venue.Name = "CourseVenue";
            _session.Save(venue);
            Course course = new Course();
            course.CourseNumber = 123;
            course.Venue = venue;
            _session.Save(course);
            StartNewSession();
            Course loadedCourse = _session.Load<Course>(course.Id);
            Assert.AreEqual(123, loadedCourse.CourseNumber);
            Assert.AreEqual(venue, loadedCourse.Venue);
        }

        [Test]
        public void CanSaveNewVenueWithAddedCoursesAndCoursesWillBeSavedToo()
        {
            Venue venue = new Venue();
            venue.Name = "CourseVenue";
            Course course = new Course();
            course.CourseNumber = 123;
            course.Venue = venue;
            venue.Courses.Add(course);
            _session.Save(venue);
            StartNewSession();

            Venue loadedVenue = _session.Load<Venue>(venue.Id);
            Assert.IsTrue(loadedVenue.Courses.Contains(course));
        }

        [Test]
        public void CanAssociateStudentWithCourses()
        {
            Course course1 = new Course { CourseNumber = 1 };
            Course course2 = new Course { CourseNumber = 2 };

            _session.Save(course1);
            _session.Save(course2);

            Student student1 = new Student { Name = "Student 1" };
            student1.Courses.Add(course1);
            student1.Courses.Add(course2);
            _session.Save(student1);

            Student student2 = new Student { Name = "Student 2" };
            student2.Courses.Add(course1);
            _session.Save(student2);

            StartNewSession();

            Student loadedStudent1 = _session.Load<Student>(student1.Id);
            Assert.IsTrue(loadedStudent1.Courses.Contains(course1));
            Assert.IsTrue(loadedStudent1.Courses.Contains(course2));

            Student loadedStudent2 = _session.Load<Student>(student2.Id);
            Assert.IsTrue(loadedStudent2.Courses.Contains(course1));

            Course loadedCourse1 = _session.Load<Course>(course1.Id);
            Assert.IsTrue(loadedCourse1.Students.Contains(student1));
            Assert.IsTrue(loadedCourse1.Students.Contains(student2));
        }
    }
}
