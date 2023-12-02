using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerStatistics.Models
{
    public class ServerStatisticsWithServerIdentifierDTO
    {
        [BsonId]
        public readonly ObjectId Id = ObjectId.GenerateNewId();
        public double MemoryUsage { get; set; }
        public double AvailableMemory { get; set; }
        public double CpuUsage { get; set; }
        public DateTime Timestamp { get; set; }
        public string ServerIdentifier { get; set; }
    }
}
