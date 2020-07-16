using Microsoft.AspNetCore.Mvc;

namespace KHCN.Web.Controllers
{
    public class HoSoQuyetToanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}