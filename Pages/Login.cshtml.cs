using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BlogApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string ErrorMessage { get; set; } = "";

        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT BloggerID, Fullname, PasswordHash FROM Bloggers WHERE Email = @Email";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Input.Email);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["PasswordHash"].ToString();
                                
                                if (BCrypt.Net.BCrypt.Verify(Input.Password, storedHash))
                                {
                                    // Successful login
                                    var claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, reader["Fullname"].ToString()),
                                        new Claim(ClaimTypes.NameIdentifier, reader["BloggerID"].ToString()),
                                        new Claim(ClaimTypes.Email, Input.Email)
                                    };

                                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                    
                                    await HttpContext.SignInAsync(
                                        CookieAuthenticationDefaults.AuthenticationScheme,
                                        new ClaimsPrincipal(claimsIdentity));

                                    // Store in session as well for convenience
                                    HttpContext.Session.SetString("UserName", reader["Fullname"].ToString());
                                    HttpContext.Session.SetInt32("UserId", (int)reader["BloggerID"]);

                                    return RedirectToPage("/Index");
                                }
                            }
                        }
                    }
                }

                ErrorMessage = "Invalid email or password.";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }

        public class LoginInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            public string Password { get; set; } = "";
        }
    }
}
