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

### Assumptions
- 3_Provider is idempotent upon receiving duplicate invitations. e.g. no duplicates will be created

### Flow
- https://www.draw.io/#G1UZOk5b62lJsjzC2ej9Ne2VS7pkonkkrg
  
### Takeaways
- Background check UI for data collection
  - This is dependent on provider
  - May not belong to the domain
- Provider flows
  - May require limited info only. e.g. email, and respond with a link
  - May require all the personal info to perform a check
- Flat file
  - File locking issue will likely to appear after certain load threshold. (How much can it handle?)
  - Alternatives are file > folder / message > file (What's the limitations?)
  - What if node where the queue sits on goes down?
  