using DeliveryOrderProcessor.Data;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace DeliveryOrderProcessor
{
    public class CosmosDbHandler
    {
        private const string COSMOS_CONNECTION_STRING = "AccountEndpoint=https://eshoponwebnosqlfinal.documents.azure.com:443/;AccountKey=6NwNgZK6zW7yE8o8zv3QODBaQpq9qKbkzVY9FvWm5s2gvWal5H2AkrJ7RgkDgNXhbG6WWngi2pspACDbsDAeMA==;";

        private Database _database;
        private Container _container;

        public async Task AddItemToDbAsync(DeliveryInfo item)
        {
            using (CosmosClient client = new(connectionString: COSMOS_CONNECTION_STRING))
            {
                await CreateDbAsync(client);
                await CreateContainerAsync();

                DeliveryInfo createdItem = await _container.CreateItemAsync<DeliveryInfo>(
                    item: item,
                    partitionKey: new PartitionKey(item.finalPrice.ToString())
                );
            }
        }

        private async Task CreateContainerAsync()
        {
            // New instance of Container class referencing the server-side container
            ContainerResponse containerResponse = await _database.CreateContainerIfNotExistsAsync(
                id: "Orders",
                partitionKeyPath: "/finalPrice",
                throughput: 400
            );

            _container = containerResponse.Container;
        }

        private async Task CreateDbAsync(CosmosClient client)
        {
            // New instance of Database response class referencing the server-side database
            DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync(
                id: "deliveryInformations"
            );

            _database = databaseResponse.Database;
        }
    }
}
