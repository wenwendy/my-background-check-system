namespace MyBackgroundCheckService.Library.DAL
{
    public interface IRepository
    {
        void IdempotentAdd(InvitationEntity invitation);
        InvitationEntity Get(int id);
        void Update(InvitationEntity invitaion);

        InvitationReveivedEventEntity GetNextEvent();
        void DeleteEvent(int invitationId);
    }
}
