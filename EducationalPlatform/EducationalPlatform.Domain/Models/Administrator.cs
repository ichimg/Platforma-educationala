namespace EducationalPlatform.Domain.Models
{
    public class Administrator
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public Administrator()
        {
            
        }
        public Administrator(int personId)
        {
            PersonId = personId;
        }
    }
}
