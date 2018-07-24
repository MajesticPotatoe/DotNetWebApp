namespace WebApps
{
    public class User
    {
        /// <summary>
        /// Username of user taken from windows login
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Users Full Name taken from windows login
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Secret Key Used for Portal or other logins
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Constructor of a User
        /// </summary>
        /// <param name="username">Users username</param>
        /// <param name="fullname">Users fullname</param>
        public User(string username, string fullname, string password)
        {
            UserName = username;
            FullName = fullname;
            Password = password;
        }
    }
}