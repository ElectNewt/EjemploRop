using System;
using System.Collections.Immutable;
using System.Linq;

namespace ROP
{
    ///<summary>
    /// Custom exception that encapsulates the errors
    /// </summary>
    public class ErrorResultException : Exception
    {
        /// <summary>
        /// The errors that occurred.
        /// </summary>
        public ImmutableArray<Error> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResultException"/> class.
        /// </summary>
        /// <param name="errors"></param>
        public ErrorResultException(ImmutableArray<Error> errors)
            : base(ValidateAndGetErrorMessage(errors))
        {
            Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResultException"/> class.
        /// </summary>
        /// <param name="error"></param>
        public ErrorResultException(Error error)
            : this(new[] { error }.ToImmutableArray())
        {
        }

        private static string ValidateAndGetErrorMessage(ImmutableArray<Error> errors)
        {
            if (errors.Length == 0)
            {
                throw new Exception("You should include at least one Error");
            }

            if (errors.Length == 1)
            {
                return errors[0].Message;
            }

            return errors
                .Select(e => e.Message)
                .Prepend($"{errors.Length} Errors occurred:")
                .JoinStrings(System.Environment.NewLine);
        }
    }
}
