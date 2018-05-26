using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public class MonolithResultTransformer : IResultTransformer
    {
        public object Transform(ResultDto result)
        {
            return result;
        }
    }
}