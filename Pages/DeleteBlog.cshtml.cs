using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Pages
{
    [Authorize]
    public class DeleteBlogModel : PageModel
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = TUESDAY_BLOG_DB; Integrated Security = True; TrustServerCertificate=True;";

        public int blogId { get; set; }

        public IActionResult OnGet(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // 1. Verify that the blog belongs to the current user
                    string verifyQuery = "SELECT Blogger FROM Blog WHERE BlogID = @BlogId";
                    conn.Open();
                    using (SqlCommand verifyCmd = new SqlCommand(verifyQuery, conn))
                    {
                        verifyCmd.Parameters.AddWithValue("@BlogId", id);
                        var authorId = verifyCmd.ExecuteScalar()?.ToString();

                        if (authorId != currentUserId)
                        {
                            // Not the author, don't delete
                            return RedirectToPage("/Index");
                        }
                    }

                    // 2. Perform the deletion
                    string deleteQuery = "DELETE FROM Blog WHERE BlogID = @BlogId";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@BlogId", id);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["Message"] = "Blog deleted successfully.";
                        }
                        else
                        {
                            TempData["Message"] = "Blog not found or already deleted.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "An error occurred while deleting the blog.";
            }

            return RedirectToPage("/Index");
        }
    }
}
