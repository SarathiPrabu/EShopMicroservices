namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
// Validate the id in Request
internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}