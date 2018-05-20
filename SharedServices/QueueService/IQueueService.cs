namespace QueueService
{
    public interface IQueueService
    {
        void AddToQueue(string queueName, string content);
        string GetAQueueItem(string queueName);
        void RemoveFromQueue(string queueItem, string queueName);
    }
}