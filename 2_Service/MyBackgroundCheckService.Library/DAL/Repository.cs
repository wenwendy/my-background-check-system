using System.Linq;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyBackgroundCheckService.Library.DTOs;

namespace MyBackgroundCheckService.Library.DAL
{
    public class Repository : IRepository
    {
        private const string _connectionString = "Host=localhost;Port=2345;Username=postgres;Password=abc123;Database=background_check;";

        public void UpSert(InvitationEntity invitation)
        {
            //save to postgres db

            var profile = JsonConvert.SerializeObject(invitation.ApplicantProfile);
            var commandString = $@"INSERT INTO public.invitation (invitation, applicant_profile, status)
                    VALUES ({invitation.Id}, '{profile}', '{invitation.Status}')
                    ON CONFLICT(invitation)
                    DO
                      UPDATE
                        SET applicant_profile = EXCLUDED.applicant_profile, status = EXCLUDED.status;";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    connection.Execute(commandString);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public InvitationEntity Get(int id)
        {
            List<InvitationTemp> invitations = null;
            var commandString = $"SELECT invitation AS \"Id\", applicant_profile AS \"ApplicantProfile\", status AS \"Status\" FROM public.invitation WHERE invitation = {id};";
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    invitations = connection.Query<InvitationTemp>(commandString).ToList();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            var temp = invitations.FirstOrDefault(i => i.Id == id);
            return new InvitationEntity
            {
                Id = temp.Id,
                ApplicantProfile = JsonConvert.DeserializeObject<ApplicantProfile>(temp.ApplicantProfile),
                Status = temp.Status
            };
        }
    }
}
