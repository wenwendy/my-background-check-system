using MyBackgroundCheckService.Library.Domain;
using LanguageExt;
using static LanguageExt.Prelude;

namespace MyBackgroundCheckService.Library.DAL
{
    public interface IRepository
    {
        Either<Unit, Failure> IdempotentAdd(InvitationAggregate invitation);
        InvitationAggregate Get(int id);
        void Update(InvitationAggregate invitaion);

        InvitationReveivedEventEntity GetNextEvent();
        void DeleteEvent(int invitationId);
    }
}
