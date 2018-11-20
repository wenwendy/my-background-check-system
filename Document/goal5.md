### Goal 5
- Solve issue during POST when 2xx is returned but invitation will never be processed. (e.g. machine failure between DB save and queuing)

### Assumptions
- DB is of high availability and capable of handling loads. 
- POST request to API is light weight.


### Flow
https://drive.google.com/open?id=1Dmr92cjSGeBbkH4iam0vJTXZuXOXiZPY
  
### Takeaways
- Purpose of DTO: break coupling
- Remove `Try / Catch` so that exception can be handled in one place. Configure correctly so that stack trace is not revealed to caller.
  - Controller: good candidate to apply railway programming (2xx / 5xx)
- Naming: "DTO" > what exactly it is   
- Creating a command
  - it needs to be valid
  - validation done in constructor
  - return `EitherSomeError` upon validation failure
  - Does 2 things: transform a request to a command / persist into DB
    - Test so that easily knows which part fails
- Aggregate 
  - Entity 
  - It shouldn't change when data store implementation changes
  - Repository layer: persists aggregate to data store   
  - Performs validation 
  - Having a getter encourages validation out side of aggregate (not good)
  - Regulates the writing. (CQRS - reading is regulated separately)
- Test command 
  - In memory implementation of IRepository
  - Avoid mocking up http
- Version
  - Part of repository, not aggregate
  - ORM version management  
- To validate a design
  - try to add some business logic and should expect minimum changes (e.g. no editing on existing handlers).
  - try to add more fields as per vendor requests
  

void Test()
{
	IRepository _repository = new InMemoryRepository();
	var commandHandler = new AddInvitationCommandHandler(_repository);
    commandHandler.Handle(addInvitationCommand);


	// check repository got something added to it.
}

class AggregateRepository<TId, TEntity>: IRepository
{
	Dictionary<TId, TEntity> _everything;
	void Add(Entity e)
	{
		_everything.Add(e.....)
	}
	
	
}


public class WrappedUpInvitationAggregate


  public class InvitationAggregate
  {
	private State _state;
	
	InvitationAggregate(stuff)
	{
		_state = ....(from stuff)
	}
	
	public void InProgress()
	{
	}
	
	public void Passed()
	{
	}
	
	public void Failed()
	{
	}
	
	public Status CurrentStatus() {return _state.Status;}
  }
  
  // GET /invitation/{id}
  
  public class InvitationResponse
  {
  }
  
  public class InvitationStatusQueryService
  {
	int GetStatus(guid invitationId)
	{
		return {select status from Invitation where Id = invitationId}
	}
  }


  
  void DoingStuff()
  {
	InvitationAggregate aggregate = repository.Get(aggregateId);
	if (aggregate.State == InProgress)
	{
		//SomeOtherThingyIGotFromWhoKnowsWhere.Something();
		aggregate.Passed();
	}
	
  }
  


  public void Handle(AddInvitationCommand command)
        {
			SomeData someOtherData = _queryServiceToWhereever.Get(blah, blah);
			InvitationAggregate aggregate = TransformToAggregate(someOtherData, command);
           
            _repository.IdempotentAdd(aggregate);
            // machine can fail here
            //raise event InvitationCreated
        }

        public static Either<Failure, InvitationEntity> TransformToAggregate(
															SomeData someOtherData, 
															AddInvitationCommand cmd)
        {
			
		}
		
		
		
Request -> Command 				(handler) -> Aggregate 			-> RequestResponse
			(no more http)		(might provide more data)
			
			
			Udi Dahan: Making responsibilities explicit.