namespace MyBackgroundCheckService.Library
{
    public interface IRepository
    {
        void Save(Invitation invitation);
        Invitation Get(int id);
    }
}
