using MediatR;

namespace BuildingBlocks.CQRS;

// Unit type is similar to void in MediatR - Can be uses as a generic type 
// So commands return Unit type, actually don't return data
public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}