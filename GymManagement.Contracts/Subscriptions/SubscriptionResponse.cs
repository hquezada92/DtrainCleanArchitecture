namespace GymManagement.Contracts.Subscriptions;

public record SubscriptionResponse(
    Guid Id, 
    string SubscriptionType);