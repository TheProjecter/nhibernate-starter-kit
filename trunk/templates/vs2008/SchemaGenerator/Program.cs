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

            Console.WriteLine("Schema and data created successfully");

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
                session.Save(new MyEntity {Name = "MyEntityName1"});
                session.Save(new MyEntity {Name = "MyEntityName2"});
                session.Flush();
            }
        }
    }
}