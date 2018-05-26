using MyBackgroundCheckService.Processor.DTOs;

namespace MyBackgroundCheckService.Processor.Transformers
{
    public interface IResultTransformer
    {
        object Transform(ResultDto result);
    }
}