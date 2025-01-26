using MongoDB.Driver;

namespace Ghtk.Api;

public static class MongoDbClientExtensions
{
    public static void AddMongoDbClient(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
        services.AddSingleton(mongoClient);
    }
}
