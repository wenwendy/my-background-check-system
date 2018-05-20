### My Background Check Service
- Api
  - Receive an invitation
  - Add to queue
- Processor
  - Pick up an invitation from queue
  - Transform
  - Send to Provider
  - Receive result from Provider
  - Transform
  - Send to Monolith
  
  