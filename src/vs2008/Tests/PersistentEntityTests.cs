using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using NUnit.Framework;

namespace Tests
{
    internal class TestEntity: PersistentEntity
    {
        
    }
    [TestFixture]
    public class PersistentEntityTests
    {
        [Test]
        public void Two_transient_persistent_entities_should_not_be_considered_equal()
        {
            TestEntity entity1 = new TestEntity();
            TestEntity entity2 = new TestEntity();
            Assert.IsFalse(entity1.Equals(entity2));

        }

        [Test]
        public void Two_transient_persistent_entities_should_have_different_hash_codes()
        {
            TestEntity entity1 = new TestEntity();
            TestEntity entity2 = new TestEntity();
            Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Test]
        public void Transient_persistent_entity_hashcode_should_remain_for_life_of_the_instance_even_after_id_has_been_set()
        {
            TestEntity entity = new TestEntity();
            int hash = entity.GetHashCode();
            entity.Id = new Guid();
            Assert.AreEqual(hash, entity.GetHashCode());
        }
    }
}
