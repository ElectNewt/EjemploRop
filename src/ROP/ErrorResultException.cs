using System;
using System.Collections.Immutable;
using System.Linq;

namespace ROP
{
    ///<summary>
    /// Excepción que encapsula los Errores
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
                throw new Exception("Debes incluir almenos un error");
            }

            if (errors.Length == 1)
            {
                return errors[0].Message;
            }

            return errors
                .Select(e => e.Message)
                .Prepend($"Han ocurrido {errors.Length} Errores:")
                .JoinStrings(System.Environment.NewLine);
        }
    }
}
