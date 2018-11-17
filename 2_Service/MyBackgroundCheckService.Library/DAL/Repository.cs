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

        public void IdempotentAdd(InvitationEntity invitation)
        {
            //save to postgres db

            var profile = JsonConvert.SerializeObject(invitation.ApplicantProfile);
            var commandString = $@"
            BEGIN TRANSACTION;
            
            INSERT INTO public.invitation (invitation, applicant_profile, status)
                    VALUES ({invitation.Id}, '{profile}', '{invitation.Status}')
                    ON CONFLICT(invitation)
                    DO
                      UPDATE
                        SET applicant_profile = EXCLUDED.applicant_profile, status = EXCLUDED.status;
                        
            INSERT INTO public.invitation_received_event (invitation, applicant_profile)
                    VALUES ({invitation.Id}, '{profile}')
                    ON CONFLICT(invitation)
                    DO
                      UPDATE
                        SET applicant_profile = EXCLUDED.applicant_profile;
                         
            END TRANSACTION";

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

        public void Update(InvitationEntity invitation)
        {
            var profile = JsonConvert.SerializeObject(invitation.ApplicantProfile);
            var commandString = $@"
            UPDATE public.invitation
            SET applicant_profile = {profile}, status = {invitation.Status}
            WHERE invitation = {invitation.Id};";

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

        public InvitationReveivedEventEntity GetNextEvent()
        {
            var commandString = $"SELECT TOP 1 invitation AS \"Id\", applicant_profile AS \"ApplicantProfile\" " +
                "FROM public.invitation_received_event;";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // TODO: 
                    // get a record from DB
                    //connection.Execute(commandString);
                    // serialize into InvitationReceivedEventEntity
                    return new InvitationReveivedEventEntity
                    {
                        Id = 123,
                        SerializedApplicantProfile = "{\"Name\":\"Alice\",\"Dob\":\"1990 Dec 10\",\"Address\":\"42 Abc Street, Melbourne\",\"Education\":\"Swinburne Uni 2010\"}"
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public void DeleteEvent(int invitationId)
        {
            var commandString = $"DELETE FROM public.invitation_received_event WHERE invitation = {invitationId};";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    connection.Execute(commandString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
