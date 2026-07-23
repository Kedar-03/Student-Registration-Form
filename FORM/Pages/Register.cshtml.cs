using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FORM.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public RegisterModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit Contact Number")]
        public string Contact { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = _configuration.GetConnectionString("StudentDBConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Students
                                (FullName, Email, Contact, Address, Gender, DOB)
                                VALUES
                                (@FullName, @Email, @Contact, @Address, @Gender, @DOB)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FullName", FullName);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Contact", Contact);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@DOB", DOB);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            Message = "Registration Successful!";

            ModelState.Clear();

            FullName = "";
            Email = "";
            Contact = "";
            Address = "";
            Gender = "";
            DOB = DateTime.Today;

            return Page();
        }
    }
}