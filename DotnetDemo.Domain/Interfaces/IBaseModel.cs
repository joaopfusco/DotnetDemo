namespace DotnetDemo.Domain.Interfaces
{
    public interface IBaseModel<T>
    {
        T Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
