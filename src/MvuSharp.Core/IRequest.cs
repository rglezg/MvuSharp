namespace MvuSharp
{
    public interface IRequest<TResponse>
    {
    }
    
    public interface IRequest : IRequest<Unit>
    {
    }
}