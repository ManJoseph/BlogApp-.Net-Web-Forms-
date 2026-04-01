using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInput Input { get; set; } = new();

        public string ErrorMessage { get; set; } = "";

        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // 1. Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Bloggers WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", Input.Email);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            ErrorMessage = "This email is already registered.";
                            return Page();
                        }
                    }

                    // 2. Hash password
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(Input.Password);

                    // 3. Insert new blogger
                    string insertQuery = "INSERT INTO Bloggers (Fullname, Email, PasswordHash, CreatedAt) VALUES (@Fullname, @Email, @PasswordHash, @CreatedAt)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Fullname", Input.Fullname);
                        insertCmd.Parameters.AddWithValue("@Email", Input.Email);
                        insertCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Login", new { registered = true });
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during registration. Please try again.";
                return Page();
            }
        }

        public class RegisterInput
        {
            [Required]
            public string Fullname { get; set; } = "";

            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [MinLength(6)]
            public string Password { get; set; } = "";
        }
    }
}
