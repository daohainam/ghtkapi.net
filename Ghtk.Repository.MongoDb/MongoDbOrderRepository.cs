using Ghtk.Repository.Abstractions.Entities;
using MongoDB.Driver;

namespace Ghtk.Repository;
public class MongoDbOrderRepository : IOrderRepository
{
    private readonly MongoClient mongoClient;

    private readonly IMongoDatabase database;

    private readonly IMongoCollection<Order> orderCollection;

    public MongoDbOrderRepository(MongoClient mongoClient)
    {
        this.mongoClient = mongoClient;
        this.database = this.mongoClient.GetDatabase("ghtk");
        this.orderCollection = this.database.GetCollection<Order>("orders");
    }

    public async Task<bool> CancelOrderAsync(string trackingId, string partnerId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.TrackingId, trackingId)
            & Builders<Order>.Filter.Eq(o => o.PartnerId, partnerId);
        var update = Builders<Order>.Update.Set(o => o.Status, -1);
        var r = await this.orderCollection.UpdateOneAsync(filter, update);

        return r.ModifiedCount > 0;
    }

    public async Task CreateOrderAsync(Order orderEntity)
    {
        await this.orderCollection.InsertOneAsync(orderEntity);

    }

    public Task<Order> FindOrderAsync(string trackingId, string partnerId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.TrackingId, trackingId) 
            & Builders<Order>.Filter.Eq(o => o.PartnerId, partnerId);
        return this.orderCollection.Find(filter).FirstOrDefaultAsync();
    }
}
