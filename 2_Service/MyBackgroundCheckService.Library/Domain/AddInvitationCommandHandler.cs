using MyBackgroundCheckService.Library.DAL;
using LanguageExt;
using static LanguageExt.Prelude;

namespace MyBackgroundCheckService.Library.Domain
{
    public class AddInvitationCommandHandler
    {
        private readonly IRepository _repository;

        public AddInvitationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        // Optional<Failure>?
        private Either<Unit, Failure> HandleAction(Unit u)
        {
            return u;
        }
        private Either<Unit, Failure> unitAction(InvitationAggregate agg)
        {
            return _repository.IdempotentAdd(agg)
                       .Match(f => f, 
                              HandleAction);  
        }

        public Either<Unit, Failure> Handle(AddInvitationCommand command)
        {
            var status = "New";

            return TransformToAggregate(status, command)
                .Match(f => f,
                       unitAction);
        }

        /*
        {
            // command -> aggregate
            // var status = _statusResolver.Resolve(typeof(AddInvitationCommand));
            var status = "New";
            var aggregate = TransformToAggregate(status, command);

            return 1==1 ?
                            Right<InvitationAggregate, Failure>(aggregate)
                    : Left<int, Failure>(1);
           
          

                .Match(
                    f => { return (Right<int, Failure>)f; },
                    agg =>
                    {
                        _repository.IdempotentAdd(agg);
                        return 0;
                    }
                );
                        //.HandleFailure( f => {return f;});
            // save invitation and event to DB 


            //raise event InvitationCreated

            //return 0;//Unit.Default;
        //}*/

        public Either<InvitationAggregate, Failure> TransformToAggregate(string status, AddInvitationCommand command) =>
            Left<InvitationAggregate, Failure>
            (
                new InvitationAggregate
                {
                    Id = command.Invitation.Id,
                    ApplicantProfile = command.Invitation.ApplicantProfile,
                    Status = status
                }
            );


       /* {
            // command is ensured to be valid at this point (validation done in command constructor)
            // what can result in Failure?
            var invitation = Left<InvitationAggregate, Failure>
            (
                new InvitationAggregate
                {
                    Id = command.Invitation.Id,
                    ApplicantProfile = command.Invitation.ApplicantProfile,
                    Status = status
                }
            );
            return invitation;
        }
        */
    }

}
