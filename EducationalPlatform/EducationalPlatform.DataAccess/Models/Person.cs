namespace EducationalPlatform.DataAccess.Models
{ 
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Cnp { get; set; }
        public ERole Role { get; set; }

        public Person(string fullName, string username, string password, string cnp, ERole role)
        {
            FullName = fullName;
            Password = password;
            Username = username;
            Role = role;
            Cnp = cnp;
        }

        public Person()
        { }
    }


    public enum ERole
    {
        Administrator = 0,
        Student = 1,
        Teacher = 2,
        None = 3
    }
}
