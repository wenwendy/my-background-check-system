using System;
namespace MyBackgroundCheckService.Library
{
    public class Failure
    {
        //public enum ActionToFix { get; set; }
        public string Message { get; set; }
     
    }

    public enum ActionToFix
    {
        ReviewInput,
        TryAgain,
        EnsureDBRecordExists
    }
}
