namespace MyBackgroundCheckService.Library.DAL
{
    public interface IRepository
    {
        void UpSert(InvitationEntity invitation);
        InvitationEntity Get(int id);
    }
}
