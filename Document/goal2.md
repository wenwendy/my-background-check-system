### Goal
- A GET end point to retrieve invitation status
- Persist invitation status in an RDB (e.g. use Postgres docker image)
- Define POST payload
- Editable https://www.draw.io/?state=%7B%22ids%22:%5B%221wVF2B12jF04gcvhpgalBU47iM-yuJ-aN%22%5D,%22action%22:%22open%22,%22userId%22:%22112993802853673059325%22%7D#G1wVF2B12jF04gcvhpgalBU47iM-yuJ-aN

- 
### Assumptions:
- One background check provider only at any time.
- Ignore provider implementation.
- POST request to API is low in load and will be communicated and actioned successfully.
- One requester only and Id provided by requester is unique.


### Takeaways:
- Returning DB auto-generated primary key in API is not a good design
  - It indicates a dependency between DB and API
  - DB auto primary key can be used purely for performance purpose (e.g. ensuring partitions are next to each other and cheap to find).
  - DB hits == $$
  - An option: API generates a uniqueid  before persisting it into DB (not necessarily as PK)
- PUT design
  - Payload should update exactly as is in DB. A following GET should return exactly the same payload used in PUT.
  - e.g. to update status of an invitation: PUT `api/invitation/123/status`
- Wrap DB transaction around Data Access Layer 
  - This can reduce the chance of multiple DB threads hanging around under high load (e.g. APIV2).
  - This does not address the network timeout issue (e.g. after 30") on API end point.
  - Caller retry can mitigate network timeout to some extent
