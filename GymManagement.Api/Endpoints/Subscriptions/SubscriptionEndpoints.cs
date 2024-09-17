using FluentValidation;
using FluentValidation.Results;
using GymManagement.Api.Endpoints.Internal;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Endpoints.Subscriptions;

public class SubscriptionEndpoints : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("subscriptions", CreateSubscriptionAsync)
            .WithName("PostCreateSubscription")
            .Accepts<CreateSubscriptionRequest>("application/json")
            .Produces<SubscriptionResponse>(201)
            .Produces<IEnumerable<ValidationFailure>>(400)
            .WithOpenApi()
            .WithTags("Subscriptions");
        
        app.MapGet("subscriptions/{id:guid}", GetSubscriptionByIdAsync)
            .WithName("GetSubscription")
            .WithOpenApi()
            .WithTags("Subscriptions");
    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<ISubscriptionService, SubscriptionService>();
    }
    
    private static async Task<IResult> CreateSubscriptionAsync(CreateSubscriptionRequest request, ISender mediator,
        IValidator<CreateSubscriptionRequest> validator, 
        LinkGenerator linker)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var subscriptionType = DomainSubscriptionType.FromName(request.SubscriptionType);
        
        var command = new CreateSubscriptionCommand(
            subscriptionType,
            request.AdminId);
        var result = await mediator.Send(command);

        var path = linker.GetPathByName("GetSubscription", new { id = request.AdminId })!;
        
        return result.Match(
            subscription => Results.Created(path,new SubscriptionResponse(subscription.Id, subscriptionType.Name)),
            errors => Results.Problem()
        );
    }
    
    private static async Task<IResult> GetSubscriptionByIdAsync(Guid id, ISender mediator)
    {
        var query = new GetSubscriptionQuery(id);
        var result = await mediator.Send(query);
        
        return result.Match(
            subscription => Results.Ok(new SubscriptionResponse(subscription.Id, subscription.SubscriptionType.Name)),
            errors => Results.NotFound()
        );
    }
}