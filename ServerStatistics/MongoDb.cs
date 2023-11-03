using MongoDB.Driver;

namespace ServerStatistics
{
    public class MongoDb
    {
        private static readonly Lazy<MongoDb> _lazyInstance = new Lazy<MongoDb>(() => new MongoDb());
        private const string _connectionString = "mongodb://localhost:27017";
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ServerStatisticsDTO> _serverStatistics;

        public static MongoDb Instance => _lazyInstance.Value;

        private MongoDb()
        {
            _client = new MongoClient(_connectionString);
            _database = _client.GetDatabase("ServerMonitoring");
            _serverStatistics = _database.GetCollection<ServerStatisticsDTO>("ServerStatistics");
        }

        public async Task Add(ServerStatisticsDTO serverStatistics)
        {
            await _serverStatistics.InsertOneAsync(serverStatistics);
        }

        public void print()
        {
            var x = _serverStatistics.Find(_ => true).ToList();
            x.ForEach(a => Console.WriteLine(a.Timestamp));
        }
    }
}