using ClosedXML.Excel;
using FORM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FORM.Pages
{
    public class StudentsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public StudentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Student> Students { get; set; } = new();

        public void OnGet()
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            string connectionString =
                _configuration.GetConnectionString("StudentDBConnection");

            using SqlConnection con = new SqlConnection(connectionString);

            string query = "SELECT * FROM Students ORDER BY Id";

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Students.Add(new Student
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Contact = reader["Contact"].ToString(),
                    Address = reader["Address"].ToString(),
                    Gender = reader["Gender"].ToString(),
                    DOB = Convert.ToDateTime(reader["DOB"])
                });
            }

            reader.Close();
            con.Close();
        }

        public IActionResult OnPostExport()
        {
            LoadStudents();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Students");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Full Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Contact";
            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Cell(1, 6).Value = "Gender";
            worksheet.Cell(1, 7).Value = "Date of Birth";

            int row = 2;

            foreach (var student in Students)
            {
                worksheet.Cell(row, 1).Value = student.Id;
                worksheet.Cell(row, 2).Value = student.FullName;
                worksheet.Cell(row, 3).Value = student.Email;
                worksheet.Cell(row, 4).Value = student.Contact;
                worksheet.Cell(row, 5).Value = student.Address;
                worksheet.Cell(row, 6).Value = student.Gender;
                worksheet.Cell(row, 7).Value = student.DOB.ToShortDateString();

                row++;
            }

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Students.xlsx");
        }
    }
}