using System;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types


namespace QueueApp
{
    class Program
    {
        private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=jwilkincsa;AccountKey=VyynBwxJ8rTR2vARhy07nALIm5v/R+0PGyjFWaXkzI7nOeMh7t4LwqITbXIT2TeHZRByXE0R2krontiDRV0AqA==;EndpointSuffix=core.windows.net";
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                string value = String.Join(" ", args);
                SendArticleAsync(value).Wait();
                Console.WriteLine($"Sent: {value}");
            }
            else
            {

                string value = ReceiveArticleAsync().Result;
                Console.WriteLine($"Received {value}");
            }
        }
        static async Task SendArticleAsync(string newsMessage)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("newsqueue");
            bool createdQueue = await queue.CreateIfNotExistsAsync();
            if (createdQueue)
            {
                Console.WriteLine("The queue of news articles was created.");
            }

            CloudQueueMessage articleMessage = new CloudQueueMessage(newsMessage);
            await queue.AddMessageAsync(articleMessage);
        }
        static async Task<string> ReceiveArticleAsync()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("newsqueue");


            bool exists = await queue.ExistsAsync();
            if (exists)
            {
                CloudQueueMessage retrievedArticle = await queue.GetMessageAsync();
                if (retrievedArticle != null)
                {
                    string newsMessage = retrievedArticle.AsString;
                    await queue.DeleteMessageAsync(retrievedArticle);
                    return newsMessage;
                }
            }

            return "<queue empty or not created>";
        }
    }
}
