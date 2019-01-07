using MyBackgroundCheckService.Library.Domain;
using LanguageExt;

namespace MyBackgroundCheckService.Library.DAL
{
    public interface IRepository
    {
        Either<Failure, Unit> IdempotentAdd(InvitationAggregate invitation);
        InvitationAggregate Get(int id);
        void Update(InvitationAggregate invitaion);

        InvitationReveivedEventEntity GetNextEvent();
        void DeleteEvent(int invitationId);
    }
}
