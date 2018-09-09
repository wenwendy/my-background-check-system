using MyBackgroundCheckService.Library.DTOs;

namespace MyBackgroundCheckService.Library
{
    public interface IRepository
    {
        void UpSert(Invitation invitation);
        Invitation Get(int id);
    }
}
