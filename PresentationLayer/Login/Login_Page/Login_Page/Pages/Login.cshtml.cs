using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Login_Page.Pages
{
    public class LoginModel : PageModel
    {
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
            // Sayfa yüklendiğinde yapılacak işlemler varsa buraya eklenecek.
            ErrorMessage = string.Empty;
        }
    }
}
