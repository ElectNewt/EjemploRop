using System.Collections.Generic;
using ROP.Ejemplo.CasoDeUso.DTO;
using System.Linq;
using System.Collections.Immutable;

namespace ROP.Ejemplo.CasoDeUso.AddUser
{
    public interface IAdduserROPServiceDependencies
    {
        Result<bool> AddUser(UserAccount userAccount);
        Result<bool> EnviarCorreo(string email);
    }

    public class AdduserROPService
    {
        private readonly IAdduserROPServiceDependencies _dependencies;
        public AdduserROPService(IAdduserROPServiceDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public Result<UserAccount> AddUser(UserAccount userAccount)
        {
            return ValidateUser(userAccount)
                .Bind(AddUserToDatabase)
                .Bind(SendEmail)
                .Map(_ => userAccount);
        }

        private Result<UserAccount> ValidateUser(UserAccount userAccount)
        {
            List<Error> errores = new List<Error>();

            if (string.IsNullOrWhiteSpace(userAccount.FirstName))
                errores.Add(Error.Create("El nombre propio no puede estar vacio"));
            if (string.IsNullOrWhiteSpace(userAccount.LastName))
                errores.Add(Error.Create("El apellido propio no puede estar vacio"));
            if (string.IsNullOrWhiteSpace(userAccount.UserName))
                errores.Add(Error.Create("El nombre de usuario no debe estar vacio"));

            return errores.Any() 
                ? Result.Failure<UserAccount>(errores.ToImmutableArray()) 
                : userAccount;
        }

        private Result<string> AddUserToDatabase(UserAccount userAccount)
        {
            return _dependencies.AddUser(userAccount)
                .Map(_ => userAccount.Email);
        }

        private Result<bool> SendEmail(string email)
        {
            return _dependencies.EnviarCorreo(email);
        }
    }
}
