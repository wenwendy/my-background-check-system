using MyBackgroundCheckService.Library.Domain;

namespace MyBackgroundCheckService.Library.DAL
{
    public interface IRepository
    {
        void IdempotentAdd(InvitationAggregate invitation);
        InvitationAggregate Get(int id);
        void Update(InvitationAggregate invitaion);

        InvitationReveivedEventEntity GetNextEvent();
        void DeleteEvent(int invitationId);
    }
}
