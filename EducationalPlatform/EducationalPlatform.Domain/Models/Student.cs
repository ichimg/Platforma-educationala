namespace EducationalPlatform.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        public Student()
        {
        }

        public Student(int personId)
        {
            PersonId = personId;
        }
        
    }
}
