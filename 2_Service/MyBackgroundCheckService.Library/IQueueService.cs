namespace MyBackgroundCheckService.Library
{
    public interface IQueueService
    {
        IQueueService Named(string queueName);
        void Add(string queueItem);
        string GetAQueueItem();
        void Remove(string queueItem);

    }
}
