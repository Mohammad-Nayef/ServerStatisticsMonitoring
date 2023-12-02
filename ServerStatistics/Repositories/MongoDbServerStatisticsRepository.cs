using GlobalConfigurations;
using MongoDB.Driver;
using ServerStatistics.Models;

namespace ServerStatistics.Repositories
{
    public class MongoDbServerStatisticsRepository : IServerStatisticsRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ServerStatisticsWithServerIdentifierDTO> _serverStatistics;

        public MongoDbServerStatisticsRepository(IAppConfigurations config)
        {
            _client = new MongoClient(config.MongoDbConfig.ConnectionString);
            _database = _client.GetDatabase(config.MongoDbConfig.ServerMonitoringDatabaseName);

            _serverStatistics = _database.
                GetCollection<ServerStatisticsWithServerIdentifierDTO>(
                    config.MongoDbConfig.ServerStatisticsCollectionName);
        }

        public async Task InsertAsync(ServerStatisticsWithServerIdentifierDTO serverStatistics)
        {
            await _serverStatistics.InsertOneAsync(serverStatistics);
        }
    }
}
