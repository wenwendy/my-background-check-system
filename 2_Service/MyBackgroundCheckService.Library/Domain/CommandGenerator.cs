using System;
using MyBackgroundCheckService.Library.DTOs;
using LanguageExt;
using static LanguageExt.Prelude;

namespace MyBackgroundCheckService.Library.Domain
{
    public class CommandGenerator
    {
        public Either<AddInvitationCommand, Failure> CreateAddInvitationCommand(InvitationPostRequestBody invitation)
        {
            // how to avoid try catch here?
            try
            {
                return Left<AddInvitationCommand, Failure>
                    (new AddInvitationCommand(invitation));
            }
            catch (Exception ex)
            {
                return Right<AddInvitationCommand, Failure>(new Failure
                {
                    Message = ex.Message
                });
            }
        }
    }

}
