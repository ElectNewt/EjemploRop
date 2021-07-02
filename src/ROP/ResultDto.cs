using System.Collections.Immutable;
using System.Linq;

namespace ROP
{
    public class ResultDto<T>
    {
        public T Value { get; set; }
        public ImmutableArray<ErrorDto> Errors { get; set; }
        public bool Success => Errors.Length == 0;

        public ResultDto()
        {
            Value = default;
            Errors = ImmutableArray<ErrorDto>.Empty;
        }

        public ResultDto(T value)
        {
            Value = value;
            Errors = ImmutableArray<ErrorDto>.Empty;
        }

        public ResultDto(ImmutableArray<Error> errors)
        {
            Value = default;
            Errors = errors.Select(x=>new ErrorDto()
            {
                ErrorCode = x.ErrorCode,
                Message = x.Message
            }).ToImmutableArray();
        }

    }
}
