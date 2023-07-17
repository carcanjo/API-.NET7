namespace SeuTempo.Core.Entities
{
    public abstract class Base
    {
        public long Id { get; set; }
        public abstract bool Validate();
    }
}
