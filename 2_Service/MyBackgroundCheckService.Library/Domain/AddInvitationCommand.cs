using MyBackgroundCheckService.Library.DTOs;
using MyBackgroundCheckService.Library.DAL;

namespace MyBackgroundCheckService.Library.Domain
{
    // TODO MC: give a constructor and bullet proof it.
    public class AddInvitationCommand
    {
        public InvitationDto InvitationDto;
    }

    public class AddInvitationCommandHandler
    {
        private IRepository _repository;

        public AddInvitationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AddInvitationCommand command)
        {
            // TODO MC: maybe consider renaming InvitationEntity to InvitationAggregate.
            // save to DB with event
            // command -> aggregate
            var aggregate = new InvitationEntity
            {
                Id = command.InvitationDto.Id,
                ApplicantProfile = command.InvitationDto.ApplicantProfile,
                Status = "New"
            };
            _repository.IdempotentAdd(aggregate);
            // machine can fail here
            //raise event InvitationCreated
        }

        public static InvitationEntity TransformToAggregate(AddInvitationCommand cmd)
        {}
        
    }

}
