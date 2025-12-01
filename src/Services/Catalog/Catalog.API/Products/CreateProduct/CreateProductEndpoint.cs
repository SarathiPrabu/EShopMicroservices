namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Categories { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageFiles { get; set; } = default!;
    public decimal Price { get; set; }
}

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct - Name")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new product - Summary")
            .WithDescription("Create a new product - Description");
    }
}