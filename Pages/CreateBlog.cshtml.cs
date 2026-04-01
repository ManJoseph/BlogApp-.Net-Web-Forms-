using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Pages
{
    [Authorize]
    public class CreateBlogModel : PageModel
    {
        public List<Blog> listOfBlogs = new List<Blog>();
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";
        
        public void OnGet()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string retrieveQuery = @"
                        SELECT b.Title, b.BlogPost, c.CategoryName, bl.Fullname, b.CreatedAt 
                        FROM Blog b 
                        JOIN BlogCategory c ON b.BlogCategory = c.CategoryId 
                        JOIN Bloggers bl ON b.Blogger = bl.BloggerID 
                        ORDER BY b.CreatedAt DESC";

                    conn.Open();
                    using(SqlCommand cmd = new SqlCommand(retrieveQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Blog blog = new Blog();
                                blog.title = reader["Title"].ToString();
                                blog.blogPost = reader["BlogPost"].ToString();
                                blog.category = reader["CategoryName"].ToString();
                                blog.blogger = reader["Fullname"].ToString();
                                blog.CreatedAt = (DateTime)reader["CreatedAt"];

                                listOfBlogs.Add(blog);
                            }
                        }
                    }
                }
            }catch (Exception)
            {
            }
        }
        
        public IActionResult OnPost()
        {
            try
            {
                string title = Request.Form["title"];
                string blogPost = Request.Form["content"];
                string category = Request.Form["category"];
                
                // Get the current logged-in user ID from claims
                var bloggerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(bloggerId))
                {
                    return RedirectToPage("/Login");
                }

                string query = "INSERT INTO Blog (title, blogPost, blogCategory, blogger, CreatedAt) VALUES (@title, @blogPost, @category, @blogger, @CreatedAt)";

                using (SqlConnection conn = new SqlConnection(connectionString)){

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@blogPost", blogPost);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@blogger", bloggerId);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
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
