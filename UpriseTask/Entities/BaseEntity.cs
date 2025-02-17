namespace UpriseTask.Entities
{
    public interface IEntity { }
    public abstract class BaseEntity <T>: IEntity
    {
        public T Id { get; set; } = default!;
    }
}
