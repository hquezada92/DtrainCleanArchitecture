using System.Reflection;

namespace GymManagement.Api.Endpoints.Internal;

public static class EndPointExtensions
{
    public static void AddEndPoints<TMarker>(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddEndPoints(services,typeof(TMarker),configuration);
    }
    public static void AddEndPoints(this IServiceCollection services,
        Type typeMarker, IConfiguration configuration)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);
        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.AddServices))!
                .Invoke(null, new object?[]{ services, configuration });
        }
    }

    public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        UseEndpoints(app,typeof(TMarker));
    }
    public static void UseEndpoints(this IApplicationBuilder app, Type typeMarker)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);
        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.DefineEndpoint))!
                .Invoke(null, new object?[]{ app });
        }
    }
    
    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
    {
        var endpointTypes = typeMarker.Assembly.DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(IEndpoints).IsAssignableFrom(x));
        return endpointTypes;
    }
}