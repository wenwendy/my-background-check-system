namespace MyBackgroundCheckService.Processor.Senders
{
    public class BobProviderSender : ISender
    {
        public bool Send(object transformedInvitation)
        {
            //send to bob the provider
            return true;
        }
    }
}