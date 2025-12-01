namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{Id:guid}", async (Guid Id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(Id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}