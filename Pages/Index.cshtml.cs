using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace buttons.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        // Button 1: Redirect to About Page
        public IActionResult OnPostGoToAbout()
        {
            return RedirectToPage("About");
        }

        // Button 2: Query Database for UserID
        public IActionResult OnPostQueryDatabase()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string userId = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 userID FROM tblUsers";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        userId = reader["userID"].ToString();
                    }
                }
            }

            _logger.LogInformation("Queried UserID: " + userId);
            TempData["UserId"] = userId; // Store userID temporarily for display

            return Page();
        }

        // Button 3: Show a Message
        public IActionResult OnPostShowMessage()
        {
            TempData["Message"] = "Hello, this is a test message!";
            return Page();
        }

        // Button 4: Log an Event
        public IActionResult OnPostLogEvent()
        {
            _logger.LogInformation("User clicked the Log Event button.");
            TempData["Message"] = "Event has been logged successfully.";
            return Page();
        }
    }
}
