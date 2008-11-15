using Domain;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MyEntityTests
    {
        private ISessionFactory _factory;
        private ISession _session;

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

        private void StartNewSession()
        {
            //The session caches items. Flushing and starting a new session ensures that tests hit the database when intended.
            _session.Flush();
            _session.Dispose();
            _session = _factory.OpenSession();
        }

        [Test]
        public void CanSaveNewMyEntity()
        {
            //The SchemaGenerator must be run first before these tests. 
            //Optionally, the methods illustrated in the SchemaGenerator can be incorporated into the FixtureSetup/SetUp,
            //ensuring a clean database for each fixture/test
            MyEntity myEntity = new MyEntity();
            myEntity.Name = "TestName";
            _session.Save(myEntity);
            //Flush the session, and dispose, to ensure that we load from the database
            StartNewSession();
            MyEntity loadedEntity = _session.Load<MyEntity>(myEntity.Id);
            //Assert that the entity loaded has the same name as the entity that we saved
            Assert.AreEqual("TestName", loadedEntity.Name);
        }
    }
}
