namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    Guid Id, 
    string Name,
    List<string> Categories,
    string Description,
    string ImageFiles,
    decimal Price 
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
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFiles = command.ImageFiles,
            Price = command.Price
            
        };
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}