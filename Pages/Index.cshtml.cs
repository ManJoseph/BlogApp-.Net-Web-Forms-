using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using BlogApp.Models;

namespace BlogApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Blog> listOfBlogs = new List<Blog>();
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public void OnGet()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string retrieveQuery = @"
                        SELECT b.BlogID, b.Title, b.BlogPost, c.CategoryName, bl.Fullname, b.Blogger, b.CreatedAt 
                        FROM Blog b 
                        JOIN BlogCategory c ON b.BlogCategory = c.CategoryId 
                        JOIN Bloggers bl ON b.Blogger = bl.BloggerID 
                        ORDER BY b.CreatedAt DESC";

                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(retrieveQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listOfBlogs.Add(new Blog
                                {
                                    blogId = (int)reader["BlogID"],
                                    title = reader["Title"].ToString() ?? "",
                                    blogPost = reader["BlogPost"].ToString() ?? "",
                                    category = reader["CategoryName"].ToString() ?? "",
                                    blogger = reader["Fullname"].ToString() ?? "",
                                    bloggerId = reader["Blogger"].ToString() ?? "",
                                    CreatedAt = reader["CreatedAt"] != DBNull.Value ? (DateTime)reader["CreatedAt"] : DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Simple error handling for now
            }
        }
    }
}
