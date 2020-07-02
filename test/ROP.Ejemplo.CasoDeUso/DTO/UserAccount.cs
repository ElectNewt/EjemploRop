namespace ROP.Ejemplo.CasoDeUso.DTO
{
    public class UserAccount
    {
        public readonly string UserName;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Email;

        public UserAccount(string userName, string firstName, string lastName, string email)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
