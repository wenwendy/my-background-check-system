using MyBackgroundCheckService.Library.DAL;

namespace MyBackgroundCheckService.Library.Domain
{
    public class AddInvitationCommandHandler
    {
        private readonly IRepository _repository;

        public AddInvitationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AddInvitationCommand command)
        {
            // command -> aggregate
            var aggregate = TransformToAggregate(command);
            // save invitation and event to DB 
            _repository.IdempotentAdd(aggregate);

            //raise event InvitationCreated
        }

        public static InvitationAggregate TransformToAggregate(AddInvitationCommand command)
        {
            return new InvitationAggregate
            {
                Id = command.Invitation.Id,
                ApplicantProfile = command.Invitation.ApplicantProfile,
                Status = "New"// for "add command", status is always defaulted to "New"
            };
        }
    }

}
