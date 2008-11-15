using System;

namespace Domain
{
    /// <summary>
    /// Base class for entities. Has an integer Id, which is used both for object identity and generating hash codes.
    /// </summary>
    public abstract class PersistentEntity
    {
        private int? _oldHashCode;

        virtual public Guid Id { get; set; }
        
        public override bool Equals(object obj)
        {
            PersistentEntity entity = obj as PersistentEntity;
            return Equals(entity);
        }

        public virtual bool Equals(PersistentEntity other)
        {
            if (this == other)
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }

            if (this.Id == Guid.Empty || other.Id == Guid.Empty)
            {
                return ReferenceEquals(this, other);
            }

            return this.Id == other.Id;
        }


        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (_oldHashCode.HasValue)
            {
                return _oldHashCode.Value;
            }

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (Id == Guid.Empty)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }

            return Id.GetHashCode();

        }
    }
}
