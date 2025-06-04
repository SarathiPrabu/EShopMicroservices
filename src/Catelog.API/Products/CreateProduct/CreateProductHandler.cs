namespace Catelog.API.Products.CreateProduct;

public record CreateProductCommand(
    Guid id,
    string name,
    List<string> categories,
    string description,
    string imageFiles,
    decimal price
): ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product

        var product = new Product
        {
            Name = command.name,
            Categories = command.categories,
            Description = command.description,
            ImageFiles = command.imageFiles,
            Price = command.price
            
        };
        // TODO
        // Save to the DB
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}