namespace Core.Domain;

public interface IEntity
{
}

public class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}