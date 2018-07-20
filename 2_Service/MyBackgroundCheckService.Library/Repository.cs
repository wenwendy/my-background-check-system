using System.Collections.Generic;
using System.Linq;

namespace MyBackgroundCheckService.Library
{
    public class Repository : IRepository
    {
        private static List<Invitation> _invitations = new List<Invitation>();

        public void UpSert(Invitation invitation)
        {
            //save to postgres db
            invitation.Status = invitation.Status ?? "New";
            _invitations.Add(invitation);
        }

        public Invitation Get(int id)
        {
            //read from postgres db
            return _invitations.FirstOrDefault(i => i.Id == id);
        }
    }
}
