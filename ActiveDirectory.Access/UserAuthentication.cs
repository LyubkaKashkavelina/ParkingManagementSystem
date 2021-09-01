namespace ActiveDirectory.Access
{
    public class UserAuthentication
    {
        public static bool IsAuthenticatedUser(string name, string password)
        {
            return password == "1111" && !string.IsNullOrEmpty(name);
        }
    }
}
