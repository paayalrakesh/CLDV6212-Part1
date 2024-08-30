using Azure;
using Azure.Data.Tables;
using ABC_Retailers.Models;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly TableClient _usersTableClient;
    private readonly TableClient _productsTableClient;

        public TableStorageService(string connectionString)
        {
            _usersTableClient = new TableClient(connectionString, "Users");
            _productsTableClient = new TableClient(connectionString, "Products");
        }

        // Methods for Customer Profiles
        public async Task<List<User>> GetAllUsersAsync()
        {
            var profiles = new List<User>();

            await foreach (var profile in _usersTableClient.QueryAsync<User>())
            {
                profiles.Add(profile);
            }

            return profiles;
        }

        public async Task AddCustomerProfileAsync(User profile)
        {
            if (string.IsNullOrEmpty(profile.PartitionKey) || string.IsNullOrEmpty(profile.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }

            try
            {
                await _usersTableClient.AddEntityAsync(profile);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage", ex);
            }
        }

        public async Task DeleteCustomerProfileAsync(string partitionKey, string rowKey)
        {
            await _usersTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // Methods for Product Information
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            await foreach (var product in _productsTableClient.QueryAsync<Product>())
            {
                products.Add(product);
            }

            return products;
        }

        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.PartitionKey) || string.IsNullOrEmpty(product.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }

            try
            {
                await _productsTableClient.AddEntityAsync(product);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage", ex);
            }
        }

        public async Task DeleteProductAsync(string partitionKey, string rowKey)
        {
            await _productsTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }

