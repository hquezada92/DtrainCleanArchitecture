using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Common.Persistence;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(e=> e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property("_adminId")
            .HasColumnName("AdminId");
        builder.Property(e=>e.SubscriptionType)
            .HasConversion(
                type => type.Value,
                value => SubscriptionType.FromValue(value));
    }
}