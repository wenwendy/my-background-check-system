namespace MyBackgroundCheckService.Library
{
    public interface IQueueService
    {
        IQueueService Named(string queueName);
        void Add(string serializedQueueItem);
        string GetAQueueItem();
        void Remove(string serializedQueueItem);
    }
}
