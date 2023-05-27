using System;
using System.Collections.Immutable;
using System.Linq;

namespace ROP
{
    ///<summary>
    /// custom exception that encapsulates the errors
    /// </summary>
    public class ErrorResultException : Exception
    {
        public ImmutableArray<Error> Errors { get; }

        public ErrorResultException(ImmutableArray<Error> errors)
            : base(ValidateAndGetErrorMessage(errors))
        {
            Errors = errors;
        }

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
