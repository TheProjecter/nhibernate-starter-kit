using System;
using Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace SchemaGenerator
{
    class Program
    {
        private static ISessionFactory _factory;

        public static int Main(string[] args)
        {
            //Create a new configuration class to build the session factory.
            Configuration configuration = new Configuration();
            configuration.Configure();
            _factory = configuration.BuildSessionFactory();
            
            GenerateSchema(configuration);

            GenerateData();

            Console.WriteLine("Schema generator finished.");

            Console.ReadLine();

            return 0;
        }

        public static void GenerateSchema(Configuration configuration)
        {
            //SchemaExport creates the database schema as defined in the mappings files. There must already be a database as defined in the App.Config file.
            SchemaExport schemaExport = new SchemaExport(configuration);

            schemaExport.Create(true, true);
        }

        public static void GenerateData()
        {
            //Set up some test data
            using (ISession session = _factory.OpenSession())
            {
                //Save venues
                Venue venue = new Venue { Name="Main Venue"};
                session.Save(venue);

                //Save courses
                Course course1 = new Course {CourseNumber = 1, Venue = venue};
                Course course2 = new Course { CourseNumber = 2, Venue = venue };
                session.Save(course1);
                session.Save(course2);

                //Save students
                Student student1 = new Student {Name="Student 1"};
                student1.Courses.Add(course1);
                student1.Courses.Add(course2);
                session.Save(student1);

                Student student2 = new Student { Name = "Student 2" };
                student2.Courses.Add(course1);
                session.Save(student2);

                session.Flush();
            }
        }
    }
}