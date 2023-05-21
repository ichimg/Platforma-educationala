namespace EducationalPlatform.DataAccess.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public bool? IsMaster { get; set; }
        public Teacher()
        { }
        public Teacher(int personId)     
        {
            PersonId = personId;
        }

    }

}
