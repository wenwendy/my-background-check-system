using MyBackgroundCheckService.Library.DTOs;
using MyBackgroundCheckService.Library.DAL;

namespace MyBackgroundCheckService.Library.Domain
{
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
            // save to DB with event
            _repository.IdempotentAdd(new InvitationEntity
            {
                Id = command.InvitationDto.Id,
                ApplicantProfile = command.InvitationDto.ApplicantProfile,
                Status = "New"
            });
            // machine can fail here
        }
    }

}
