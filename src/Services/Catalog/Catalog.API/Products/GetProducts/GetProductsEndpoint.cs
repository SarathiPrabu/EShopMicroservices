namespace Catalog.API.Products.GetProducts;

// public record GetProductsRequest();
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var resonse = result.Adapt<GetProductsResponse>();
            return Results.Ok(resonse);
        })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
    
}