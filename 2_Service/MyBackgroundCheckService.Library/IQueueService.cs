namespace MyBackgroundCheckService.Library
{
    public interface IQueueService
    {
        void AddToQueue(string queueName, string content);
    }
}
