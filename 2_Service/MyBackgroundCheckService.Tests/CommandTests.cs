using System;
using Xunit;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.Domain;

namespace MyBackgroundCheckService.Tests
{
    public class CommandTests
    {
        [Fact]
        public void SuccessfulCommand()
        {
            var request = new InvitationPostRequestBody
            {
                Id = 1,
                ApplicantProfile = new ApplicantProfile
                {
                    Name = "Alice"
                }
            };
            var generator = new CommandGenerator();
            var result = generator.CreateAddInvitationCommand(request);

            Assert.Equal("Alice", ((AddInvitationCommand)result).Invitation.ApplicantProfile.Name);
        }

        [Fact]
        public void FailedCommand()
        {
            var request = new InvitationPostRequestBody
            {
                ApplicantProfile = new ApplicantProfile
                {
                    Name = "Alice"
                }
            };
            var generator = new CommandGenerator();
            var result = generator.CreateAddInvitationCommand(request);

            Assert.Equal("invitation post body not valid", ((Failure)result).Message);
        }
    }
}
