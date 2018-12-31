using System.IO;
using MyBackgroundCheckService.Library.DTOs;
using LanguageExt;

namespace MyBackgroundCheckService.Library.Domain
{
    public class AddInvitationCommand
    {
        // responsiblity: create a valid command basing on request
        public AddInvitationCommand(InvitationPostRequestBody invitation)
        {
            // ensure command is valid - definition of "valid command"? domain business logic? vendor logic?
            // TODO: railway - how?
            if (IsValid(invitation))
            {
                Invitation = invitation;
            }
            else
            {
                throw new InvalidDataException("invitation post body not valid");
            }
        }
        private bool IsValid(InvitationPostRequestBody invitation)
        {
            // some validation logic around invitation
            // e.g. Id value constraint
            return invitation != null && (invitation.Id > 0);
        }

        public InvitationPostRequestBody Invitation { get; }

    }
}
