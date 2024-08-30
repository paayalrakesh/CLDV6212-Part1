using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retailers.Models
{
    public class User : ITableEntity
    {
        // Properties required by ITableEntity
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Additional properties
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
