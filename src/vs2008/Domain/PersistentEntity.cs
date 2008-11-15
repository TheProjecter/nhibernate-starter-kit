namespace Domain
{
    /// <summary>
    /// Base class for entities. Has an integer Id, which is used both for object identity and generating hash codes.
    /// </summary>
    public abstract class PersistentEntity
    {
        virtual public int Id { get; set; }
        
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

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
