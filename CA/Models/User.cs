namespace CA.Models
{
    public class User
    {
        public User() { }

        public User(String username,String password)
        {
            Id = System.Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            UserType = "normal";
        }
        public String Id { get; set; }

        public String Username { get; set; }
        public String Password { get; set; }

        public String UserType { get; set; }

        public virtual List<Score> scores { get; set; }   
    }
}
