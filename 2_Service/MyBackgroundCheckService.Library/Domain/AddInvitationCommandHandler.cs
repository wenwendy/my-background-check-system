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
        private Either<Unit, Failure> HandleAction(Unit u) => u;
        
        // responsibility: orchestrates actions needed to handle the given valid command
        public Either<Unit, Failure> Handle(AddInvitationCommand command)
        {
            var status = "New";

            return TransformToAggregate(status, command)
                .Match(f => f,
                    agg => _repository.IdempotentAdd(agg)
                        .Match(f => f,
                            HandleAction));
        }

        // responsibility: convert a valid command to aggregate
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
    }

}
