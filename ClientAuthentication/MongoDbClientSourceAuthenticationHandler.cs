using MongoDB.Driver;

namespace ClientAuthentication
{
    public class MongoDbClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler, IDisposable
    {
        private readonly string _connectionString;
        private readonly MongoClient client;
        private bool disposedValue;

        private readonly IMongoDatabase database;
        private readonly IMongoCollection<ClientSource> clientSourceCollection;

        public MongoDbClientSourceAuthenticationHandler(string connectionString)
        {
            _connectionString = connectionString;

            client = new MongoClient(_connectionString);
            database = client.GetDatabase("ClientAuthentication");
            clientSourceCollection = database.GetCollection<ClientSource>("ClientSources");
        }

        public async Task<bool> ValidateAsync(string clientSource)
        {
            var filter = Builders<ClientSource>.Filter.Eq(c => c.ClientId, clientSource)
                & Builders<ClientSource>.Filter.Lte(c => c.ValidFrom, DateTime.UtcNow)
                & Builders<ClientSource>.Filter.Gte(c => c.ValidTo, DateTime.UtcNow)
                & Builders<ClientSource>.Filter.Eq(c => c.IsEnable, true);

            var result = await clientSourceCollection.FindAsync(filter);
            return await result.AnyAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public class ClientSource
        {
            public string ClientId { get; set; } = string.Empty;
            public DateTime ValidFrom { get; set; }
            public DateTime ValidTo { get; set; }
            public bool IsEnable { get; set; }
        }
    }
}
