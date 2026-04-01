using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BlogApp.Models;

namespace BlogApp.Pages
{
    public class BlogDetailsModel : PageModel
    {
        public Blog Blog { get; set; } = new();
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public IActionResult OnGet(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT b.BlogID, b.Title, b.BlogPost, c.CategoryName, bl.Fullname, b.CreatedAt 
                        FROM Blog b 
                        JOIN BlogCategory c ON b.BlogCategory = c.CategoryId 
                        JOIN Bloggers bl ON b.Blogger = bl.BloggerID 
                        WHERE b.BlogID = @Id";

                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Blog = new Blog
                                {
                                    blogId = (int)reader["BlogID"],
                                    title = reader["Title"].ToString() ?? "",
                                    blogPost = reader["BlogPost"].ToString() ?? "",
                                    category = reader["CategoryName"].ToString() ?? "",
                                    blogger = reader["Fullname"].ToString() ?? "",
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };
                                return Page();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return RedirectToPage("/Index");
        }
    }
}
