namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    Guid id,
    string name,
    List<string> categories,
    string description,
    string imageFiles,
    decimal price
);

public record CreateProductResponse(Guid id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var resonse = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{resonse.id}", resonse);
            })
            .WithName("CreateProduct - Name")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new product - Summary")
            .WithDescription("Create a new product - Description");

        app.MapGet("/", () => "Hello World!");
    }
}