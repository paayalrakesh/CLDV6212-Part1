using Azure.Data.Tables;
using Azure;

namespace ABC_Retailers.Models
{
    public class Product : ITableEntity
    {
        // Properties required by ITableEntity
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }
}