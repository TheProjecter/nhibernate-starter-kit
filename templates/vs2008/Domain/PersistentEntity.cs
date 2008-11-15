using System;

namespace $safeprojectname$
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
            PersistentEntity other = obj as PersistentEntity;

            if (this == other)
            {
                return true;
            }

            if (other == null)
            {
                return false;
            }
            //Transient entities will both have the same id, and as such we must check for reference equality.
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
