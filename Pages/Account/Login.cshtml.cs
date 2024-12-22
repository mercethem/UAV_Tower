using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UAV_Tower.Data;

namespace UAV_Tower.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public string username { get; set; }

        [BindProperty]
        public string password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            var user = _context.users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Başarılı giriş
                return RedirectToPage("/Home/Index"); // Kullanıcıyı yönlendirme
            }
            else
            {
                // Hata mesajı
                ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
                return Page();
            }
        }
    }
}
