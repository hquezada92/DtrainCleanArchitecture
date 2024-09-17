using FluentValidation;
using GymManagement.Contracts.Subscriptions;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Validators;

public class SubscriptionValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public SubscriptionValidator()
    {
        RuleFor(request => request.AdminId).NotEmpty();
        RuleFor(request => request.SubscriptionType).NotNull();
        RuleFor(request => request.SubscriptionType.ToString()).NotEmpty();
        DomainSubscriptionType? result;
        RuleFor(request => request.SubscriptionType).Must( subscriptionType =>
            DomainSubscriptionType.TryFromName(
                subscriptionType.ToString(), out result))
            .WithMessage("Invalid subscription type");
    }
}