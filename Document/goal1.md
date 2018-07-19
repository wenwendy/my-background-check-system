### Goal 
- 3 parties communicate to each other

### Assumptions
- All web requests are successful
- All payloads are well formed and valid
- 3_Provider is idenpotent
- Background check invitation load is low

### Flow
- https://www.draw.io/?state=%7B%22folderId%22:%220AP7GFen_gIlnUk9PVA%22,%22action%22:%22create%22,%22userId%22:%22112993802853673059325%22%7D#G1XgfRj18LJzliaHcWGPQP3B6lY-hiFEMw

### Feedback
- Current implementation is a middleware / mapper / data transformer between Monolith and Provider. A background check domain should be more than this.
- To notify Monolith upon Provider response, a webhook URL should be provided in initial contact between Monolith and Service. In current implementation, this URL is hard-coded.
- A simpler alternative to callback webhook is a GET end point to allow Monolith to retrieve status of an invitaiton.
