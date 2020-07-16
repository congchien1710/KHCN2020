using Microsoft.AspNetCore.Mvc;

namespace KHCN.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void SetAlert(string message, string type)
        {
            @TempData["AlertMessage"] = message;
            if (type.ToLower().Trim() == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type.ToLower().Trim() == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type.ToLower().Trim() == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

        protected void SetSuccess(string message = "Thực hiện thành công!")
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = "alert-success";
        }

        protected void SetWarning(string message = "Thực hiện không thành công!")
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = "alert-warning";
        }

        protected void SetError(string message = "Thực hiện không thành công!")
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = "alert-danger";
        }
    }
}