using GymManagement.Api.Endpoints.Internal;

namespace GymManagement.Api.Endpoints;

public class CommonEndpoints : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("common/smoke", () => Results.Ok("The thing is running!"))
            .WithName("GetSmoke")
            .WithOpenApi()
            .WithTags("Common");
    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //add some services
    }
}