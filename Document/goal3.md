### Goal 3
- Monolith POST request to Service
  - Also add this request to queue (e.g. a flat file)
  - Assumption: queue add / retrieval can fail (e.g. do not use transaction if use sql as queue)
  - Request should be guaranteed to added to queue for at least once
- Service POST to provider 
   - successful scenario
   - fail (e.g. 5xx) retry up to ? times (then move to DLQ?)
   - fail (e.g. timeout) ?
   - fail (e.g. 4xx) no point to retry
- Provider calls Service when update is available 
  - successful scenario
  