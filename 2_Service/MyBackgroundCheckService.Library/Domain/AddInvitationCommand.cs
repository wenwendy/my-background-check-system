using System.IO;
using MyBackgroundCheckService.Library.DTOs;

namespace MyBackgroundCheckService.Library.Domain
{
    // TODO MC: give a constructor and bullet proof it.
    public class AddInvitationCommand
    {
        public AddInvitationCommand(InvitationPostRequestBody invitation)
        {
            // ensure command is valid
            // TODO: railway
            if (IsValid(invitation))
            {
                Invitation = invitation;
            }
            throw new InvalidDataException("invitation post body not valid");
        }

        private bool IsValid(InvitationPostRequestBody invitation)
        {
            // some validation logic around invitation
            return true;
        }

        public InvitationPostRequestBody Invitation { get; }
    }
}
