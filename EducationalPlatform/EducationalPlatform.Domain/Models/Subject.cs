namespace EducationalPlatform.Domain.Models
{
    public class Subject
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasThesis { get; set; }
        public ICollection<Specialization> Specializations { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

        public Subject()
        {
        }

        public Subject(int id, string name) { 
            Id = id;
            Name = name;   
        }

        public override string ToString()
        {
            return Name;
        }

    }

   
}
