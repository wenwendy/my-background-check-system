namespace MyBackgroundCheckService.Library
{
    public class Repository : IRepository
    {
        public void Save(Invitation invitation)
        {
            //save to postgres db
        }

        public Invitation Get(int id)
        {
            //read from postgres db
            return new Invitation();
        }
    }
}
