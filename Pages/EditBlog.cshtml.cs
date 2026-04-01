using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Pages
{
    [Authorize]
    public class EditBlogModel : PageModel
    {
        [BindProperty]
        public Blog Blog { get; set; } = new Blog();

        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public IActionResult OnGet(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT BlogID, Title, BlogPost, BlogCategory FROM Blog WHERE BlogID = @BlogId";
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BlogId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Blog.blogId = (int)reader["BlogID"];
                                Blog.title = reader["Title"].ToString() ?? "";
                                Blog.blogPost = reader["BlogPost"].ToString() ?? "";
                                // Note: BlogCategory is stored as ID, but our model expects string for category name in some contexts.
                                // However, in the form we need the ID to pre-select the dropdown.
                                // Let's add a property for CategoryId if needed, or just use the string for now.
                                // For the edit form, we'll use the ID.
                                Blog.category = reader["BlogCategory"].ToString() ?? "";
                            }
                            else
                            {
                                return RedirectToPage("/Index");
                            }
                        }
                    }
                }
                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Index");
            }
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Blog SET Title = @title, BlogPost = @blogPost, BlogCategory = @category WHERE BlogID = @BlogId";
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", Blog.title);
                        cmd.Parameters.AddWithValue("@blogPost", Blog.blogPost);
                        cmd.Parameters.AddWithValue("@category", Blog.category);
                        cmd.Parameters.AddWithValue("@BlogId", id);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Index");
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}
