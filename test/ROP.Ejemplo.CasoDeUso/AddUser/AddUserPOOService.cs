using ROP.Ejemplo.CasoDeUso.DTO;

namespace ROP.Ejemplo.CasoDeUso.AddUser
{

    public interface IAddUserPOOServiceDependencies
    {
        bool AddUser(UserAccount userAccount);
        bool EnviarCorreo(UserAccount userAccount);
    }


    /// <summary>
    /// Añadir el usuario utilizando una estructura de programación orientada a objetos.
    /// </summary>
    public class AddUserPOOService
    {
        private readonly IAddUserPOOServiceDependencies _dependencies;
        public AddUserPOOService(IAddUserPOOServiceDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string AddUser(UserAccount userAccount)
        {
            var validacionUsuario = ValidateUser(userAccount);
            if (!string.IsNullOrWhiteSpace(validacionUsuario))
            {
                return validacionUsuario;

            }

            var addUserDB = AddUserToDatabase(userAccount);
            if (!string.IsNullOrWhiteSpace(addUserDB))
            {
                return addUserDB;

            }
            var sendEmail = SendEmail(userAccount);
            if (!string.IsNullOrWhiteSpace(sendEmail))
            {
                return sendEmail;
            }

            return "Usuario añadido correctamente";
        }

        private string ValidateUser(UserAccount userAccount)
        {
            if (string.IsNullOrWhiteSpace(userAccount.FirstName))
                return "El nombre propio no puede estar vacio";
            if (string.IsNullOrWhiteSpace(userAccount.LastName))
                return "El apellido propio no puede estar vacio";
            if (string.IsNullOrWhiteSpace(userAccount.UserName))
                return "El nombre de usuario no debe estar vacio";

            return "";
        }

        private string AddUserToDatabase(UserAccount userAccount)
        {
            if (!_dependencies.AddUser(userAccount))
            {
                return "Error añadiendo el usuario en la base de datos";
            }

            return "";
        }
        private string SendEmail(UserAccount userAccount)
        {
            if (!_dependencies.EnviarCorreo(userAccount))
            {
                return "Error enviando el correo al email del usuario";
            }

            return "";
        }

    }
}
