using System;
using MyBackgroundCheckService.Library;

namespace MyBackgroundCheckService.StatusUpdator
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new Repository();

            Console.WriteLine("To update invitation 123 to In Progress, type '123|In Progress'");
            var command = Console.ReadLine();

            var id = int.Parse(command.Split('|')[0]);
            var statusToBe = command.Split('|')[1];
            var invitation = repository.Get(id);
            invitation.Status = statusToBe;
            repository.UpSert(invitation);

        }
    }
}
