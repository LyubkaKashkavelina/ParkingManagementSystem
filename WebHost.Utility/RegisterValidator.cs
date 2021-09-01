namespace WebHost.Utility
{
    using System;
    using System.Text.RegularExpressions;

    public static class RegisterValidator
    {
        /// <summary>
        /// Between 4 and 21 characters starting with letter
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsValidUsername(string username)
        {
            Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{3,20}$");
            Match usernameMatch = usernameRegex.Match(username);

            if (!usernameMatch.Success)
            {
                throw new ArgumentException("Username is not valid.");
            }

            return usernameMatch.Success;
        }

        /// <summary>
        /// At least one uppercase, one lowercase and one number, at least 6 characters long
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            Regex passwordRegex = new Regex(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$");
            Match passwordMatch = passwordRegex.Match(password);

            if (!passwordMatch.Success)
            {
                throw new ArgumentException("Password is not valid.");
            }

            return passwordMatch.Success;
        }
    }
}
