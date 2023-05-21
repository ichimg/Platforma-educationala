namespace EducationalPlatform.DataAccess.Models
{
    public class Subject
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Specialization> Specializations { get; set; }

        public Subject()
        {
        }

        public Subject(int id, string name) { 
            Id = id;
            Name = name;   
        }

    }

   
}
