using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;


namespace BuildingBlocks.Behaviors;
// Added filter to validate the commands only. Queries are not validated
// Injecting IValidator to validate all the handle methods
// For example this can validate for CreateProductCommand and UpdateProduct
// This will be generic and follows DRY principle 
public class ValidationBehavior<TRequest, TResponse>
(IEnumerable<IValidator<TRequest>> validators)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : ICommand<TResponse>  // Validation for commands only
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = 
            validationResults
            .Where(v => v.Errors.Count != 0)
            .SelectMany(v => v.Errors)
            .ToList();
        if (failures.Any())
            throw new ValidationException(failures);
        return await next(cancellationToken);
    }
}