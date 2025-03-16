using System.Collections.Immutable;

namespace ROP.APIExtensions
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a result, containing a value and a collection of errors.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// Gets or sets the value of the result.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the collection of errors associated with the result.
        /// </summary>
        public ImmutableArray<ErrorDto> Errors { get; set; }

        /// <summary>
        /// Gets a value indicating whether the result is successful.
        /// </summary>
        /// <returns>True if there are no errors; otherwise, false.</returns>
        public bool Success => Errors.Length == 0;
    }
}
