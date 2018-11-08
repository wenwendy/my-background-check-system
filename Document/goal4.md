### Goal 4
- have a queue for status update 
- explain pros and cons

### Assumptions
- 1_Monolith will retry upon receiving 5xx errors returned by 2_Service API
- Before receiving 2xx response from 2_Service API, Monolith won't make a decesion based on GET result 

### Flow
- https://www.draw.io/#G1jJ0wcjcQSh0JHMd-ZG5kHuNhTnfCKusq
  
### Takeaways
- `Accepted (202)`
  > Indicates that request has been accepted for processing, but the processing has not been completed. This status code is useful when the actual operation is asynchronous in nature.
  > The entity returned with this response SHOULD describe the request’s current status and point to (or embed) a status monitor that can provide the user with (or without) an estimate of when the request will be fulfilled. 
  > -- https://restfulapi.net/http-status-202-accepted/
  - The status monitor can redirect caller to the resource once resource becomes available.
  - Process in this case is a light weight, simple DB save - not a long running async process. Hence the use and proper implementation of 202 is not necessary. 
- `OK (200)` meaning invitation record created in DB and is guaranteed to be passed on to vendor.
  - To correctly implement this, API controller will need to persist data directly into DB before returning 200. 
  - No queue between API controller and DB, meaning there's very limited margin for DB to fail, and added complexity if DB store requires upgrade / maintenance / revamp.
  - Persist an event into DB store with invitation creation within one transaction.
  - Have an event poller to read from DB store and push event to `invitation` queue.
  - Only upon successful pushing event to queue, will event poller remove it from DB store.
  - ------------- Alternatively --------------
  - Distributed transaction
  - e.g. Microsoft Distributed Transaction Coordinator (MSDTC) service
  - Can be done but very complex - not recommended
- SQS can be treated as a reliable enough queue implementation.
- Event vs Command. This is from the perspective of domain aggregator.
  - Command: xx to be done by domain aggregator. `DoSomethingCommand`
  - Event: Domain aggregator did xxx. `SomethingDoneEvent`
- Domain Aggregate - the center of everything
  - Properties / Collections
  - Actions
  - Version (e.g. used for concurrency control when there're multiple operations on DynamoDB)
- Benefits of using a queue
  - Can easily handle interface changes (e.g. API > FTP)  
  - Handles high load
  - Self healing after part of system is down 
  - Easy for maintenance. 
  - More robust overall system. More tolerant to bugs in processors.
  

