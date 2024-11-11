using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Main_Page.Pages
{
    public class IndexModel : PageModel
    {
        // Sayfa verilerini burada tanımlayabilirsiniz
        public string Message { get; set; }

        public void OnGet()
        {
            // Sayfa yüklenirken yapılacak işlemler
            Message = "Hoşgeldiniz! Bu Razor Page örneği.";
        }
    }
}
