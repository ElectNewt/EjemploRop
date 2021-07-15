using System.Collections.Immutable;
using System.Linq;

namespace ROP.APIExtensions
{
    public class ResultDto<T>
    {
        public T Value { get; set; }
        public ImmutableArray<ErrorDto> Errors { get; set; }
        public bool Success => Errors.Length == 0;
    }
}
