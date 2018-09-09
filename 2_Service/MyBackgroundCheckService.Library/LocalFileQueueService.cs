using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

namespace MyBackgroundCheckService.Library
{
    public class LocalFileQueueService : IQueueService
    {
        private const string QueuePath = @"../Queues";
        private string _localQueueFile;
        private List<string> _queueItems = null;

        public LocalFileQueueService(){}

        public IQueueService Named(string queueName)
        {
            return new LocalFileQueueService(queueName);
        }

        public void Add(string queueItem)
        {
            _queueItems.Add(queueItem);

            // try x times before throwing an exception
            // upon exception, feedback to initiator
            File.WriteAllText(_localQueueFile, JsonConvert.SerializeObject(_queueItems));
        }

        public string GetAQueueItem()
        {
            if (_queueItems.Count > 0)
            {
                return _queueItems[0];
            }
            return string.Empty;
        }


        public void Remove(string queueItem)
        {
            var itemToDelete = _queueItems.Find(x => x == queueItem);
            
            if (itemToDelete != null)
                _queueItems.Remove(itemToDelete);
            
            File.WriteAllText(_localQueueFile, JsonConvert.SerializeObject(_queueItems));
        }

        private LocalFileQueueService(string queueName)
        {
            _localQueueFile = $@"{QueuePath}/{queueName}.json";
            _queueItems = QueueItems();
        }

        private List<string> QueueItems()
        {
            try
            {
                using (var r = new StreamReader(_localQueueFile))
                {
                    var json = r.ReadToEnd();
                    var queueItems = JsonConvert.DeserializeObject<List<string>>(json);
                    if (queueItems == null)
                    {
                        queueItems = new List<string>();
                    }

                    return queueItems;
                }
            }
            catch (FileNotFoundException)
            {
                return new List<string>();
            }
        }
    }
}
