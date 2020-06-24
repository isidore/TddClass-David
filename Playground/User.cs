namespace Playground
{
    public class User
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string UserName { get; }
        public string Password { get; }
        public bool IsLoggedIn { get; set; }
        public bool IsSeller { get; set; }

        public User(string firstName, string lastName, string email, string userName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            Password = password;
        }

        public override string ToString()
        {
            return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Email)}: {Email}, {nameof(UserName)}: {UserName}, {nameof(Password)}: {Password}";
        }
    }
}