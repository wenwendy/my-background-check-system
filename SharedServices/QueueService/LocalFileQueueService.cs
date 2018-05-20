﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace QueueService
{
    public class LocalFileQueueService : IQueueService
    {
        private const string QueuePath = @"../../SharedServices/Queues";
    
        public void AddToQueue(string queueName, string content)
        {
            var queueItems = QueueItems(queueName);
            queueItems.Add(content);
            
            File.WriteAllText(LocalQueueFile(queueName), JsonConvert.SerializeObject(queueItems));

        }

        public string GetAQueueItem(string queueName)
        {
            try
            {
                using (var r = new StreamReader(LocalQueueFile(queueName)))
                {
                    var queueItems = JsonConvert.DeserializeObject<List<string>>(r.ReadToEnd());

                    return queueItems[0];
                }   
            }
            catch(FileNotFoundException)
            {
                return string.Empty;
            }
        }
        
        
        private static string LocalQueueFile(string queueName)
        {
            return $@"{QueuePath}/{queueName}.json";
        }

        private List<string> QueueItems(string queueName)
        {
            try
            {
                using (var r = new StreamReader(LocalQueueFile(queueName)))
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
            catch(FileNotFoundException)
            {
                return new List<string>();
            }
        }

  
    }
}