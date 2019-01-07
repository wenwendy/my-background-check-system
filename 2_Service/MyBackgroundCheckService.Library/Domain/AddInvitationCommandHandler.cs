using MyBackgroundCheckService.Library.DAL;
using LanguageExt;

namespace MyBackgroundCheckService.Library.Domain
{
    public class AddInvitationCommandHandler
    {
        private readonly IRepository _repository;

        public AddInvitationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        // responsibility: orchestrates actions needed to handle the given valid command
        public Either<Failure, Unit> Handle(AddInvitationCommand command)
        {
            var status = "New";

            return TransformToAggregate(status, command)
                .Bind(_repository.IdempotentAdd);
        }

        // responsibility: convert a valid command with a valid status (?) to aggregate
        // what can go wrong?
        public Either<Failure, InvitationAggregate> TransformToAggregate(string status, AddInvitationCommand command) =>
            new InvitationAggregate(
                command.Invitation.Id,
                command.Invitation.ApplicantProfile,
                status);
    }

}
