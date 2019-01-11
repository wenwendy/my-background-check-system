using System;
using Xunit;
using MyBackgroundCheckService.Library;
using MyBackgroundCheckService.Library.DAL;
using LanguageExt;
using MyBackgroundCheckService.Library.Domain;
using System.Collections.Generic;

namespace MyBackgroundCheckService.Tests
{
    public class HandlerTests
    {
        [Fact]
        public void SuccessfulHandle()
        {
            var repository = new InMemoryRepository();
            var request = new InvitationPostRequestBody
            {
                Id = 1,
                ApplicantProfile = new ApplicantProfile
                {
                    Name = "Alice"
                }
            };
            var command = new AddInvitationCommand(request);
            var handler = new AddInvitationCommandHandler(repository);

            var result = handler.Handle(command);
            Assert.Equal(Unit.Default, result);

            var repoAggregate = repository.Get(1);
            Assert.Equal("Alice", repoAggregate.ApplicantProfile.Name);
            Assert.Equal("New", repoAggregate.Status);
        }

        [Fact]
        public void FailedHandle()
        {
            var repository = new InMemoryRepository();
            var request = new InvitationPostRequestBody
            {
                Id = InMemoryRepository.INVITATION_ID_TO_SIMULATE_DB_ERROR,
                ApplicantProfile = new ApplicantProfile
                {
                    Name = "Bob"
                }
            };
            var command = new AddInvitationCommand(request);
            var handler = new AddInvitationCommandHandler(repository);

            var result = handler.Handle(command);
            Assert.Equal(InMemoryRepository.DB_ERROR_MESSAGE, ((Failure)result).Message);
        }
    }


    public class InMemoryRepository: IRepository
    {
        public const int INVITATION_ID_TO_SIMULATE_DB_ERROR = 2;
        public const string DB_ERROR_MESSAGE = "DB connection error";

        private Dictionary<int, InvitationAggregate> _allRecords = new Dictionary<int, InvitationAggregate>();

        public Either<Failure, Unit> IdempotentAdd(InvitationAggregate invitation)
        {
            if (invitation.Id == INVITATION_ID_TO_SIMULATE_DB_ERROR)
            {
                return new Failure { Message = DB_ERROR_MESSAGE };
            }

            _allRecords.Add(invitation.Id, invitation);
            return Unit.Default;
        }

        public InvitationAggregate Get(int id)
        {
            return _allRecords[id];
        }

        public void Update(InvitationAggregate invitaion)
        {
            throw new NotImplementedException();
        }

        public InvitationReveivedEventEntity GetNextEvent()
        {
            throw new NotImplementedException();
        }

        public void DeleteEvent(int invitationId)
        {
            throw new NotImplementedException();
        }
    }
}
