using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class QueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(string connectionString, string queueName)
    {
        _queueClient = new QueueClient(connectionString, queueName);
        CreateQueueIfNotExistsAsync().GetAwaiter().GetResult();
    }

    private async Task CreateQueueIfNotExistsAsync()
    {
        try
        {
            await _queueClient.CreateIfNotExistsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating queue: {ex.Message}");
            throw;
        }
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            await _queueClient.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
            throw;
        }
    }

    public async Task<string> ReceiveMessageAsync()
    {
        try
        {
            var response = await _queueClient.ReceiveMessagesAsync();
            var message = response.Value.FirstOrDefault();
            if (message != null)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return message.MessageText;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving message: {ex.Message}");
        }

        return null;
    }
}
