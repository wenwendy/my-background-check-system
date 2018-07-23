using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using System;
using Newtonsoft.Json;

namespace MyBackgroundCheckService.Library
{
    public class Repository : IRepository
    {
        private static List<Invitation> _invitations = new List<Invitation>();
        private const string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=abc123;Database=background_check;";

        public void UpSert(Invitation invitation)
        {
            //save to postgres db
            invitation.Status = invitation.Status ?? "New";
            _invitations.Add(invitation);
            /*
            const string commandStringSelect = "SELECT status FROM invitation;";
            var commandString = @"CREATE TABLE invitationww (
      invitation integer,
      applicant_profile json NOT NULL,
      status varchar(20),
      PRIMARY KEY (invitation)
    );";*/
            var profile = JsonConvert.SerializeObject(invitation.ApplicantProfile);
            var commandString = $@"INSERT INTO public.invitation (invitation, applicant_profile)
                    VALUES ({invitation.Id}, '{profile}')
                    ON CONFLICT(invitation)
                    DO
                      UPDATE
                        SET applicant_profile = EXCLUDED.applicant_profile;";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    //var ww = connection.Execute(commandString);

                    //var ww2 = connection.Query<string>(commandStringSelect).ToList();

                    connection.Execute(commandString);
                }
            }
            catch(Exception e)
            {
                
            }
        }

        public Invitation Get(int id)
        {
            //read from postgres db
            return _invitations.FirstOrDefault(i => i.Id == id);
        }
    }
}
