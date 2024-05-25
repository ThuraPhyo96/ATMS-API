using Microsoft.AspNetCore.Mvc;

namespace ATMS.Web.BankMvc.Controllers
{
    public class ChatController : Controller
    {
        [ActionName("Index")]
        public IActionResult ChatMessage()
        {
            return View("ChatMessage");
        }
    }
}
