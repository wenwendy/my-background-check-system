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



