namespace FORM.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Contact { get; set; } = "";

        public string Address { get; set; } = "";

        public string Gender { get; set; } = "";

        public DateTime DOB { get; set; }
    }
}