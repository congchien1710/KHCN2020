using KHCN.Data.Helpers;
using KHCN.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult Index3()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult OnGetChartData()
        {
            var chart = new Chart
            {
                cols = new object[]
                {
            new { id = "topping", type = "string", label = "Topping" },
            new { id = "slices", type = "number", label = "Slices" }
                },
                rows = new object[]
                {
            new { c = new object[] { new { v = "Mushrooms" }, new { v = 3 } } },
            new { c = new object[] { new { v = "Onions" }, new { v = 1 } } },
            new { c = new object[] { new { v = "Olives" }, new { v = 1 } } },
            new { c = new object[] { new { v = "Zucchini" }, new { v = 1 } } },
            new { c = new object[] { new { v = "Pepperoni" }, new { v = 2 } } }
                }
            };

            return new JsonResult(chart);
        }

        //    public ActionResult OnGetChartData2()
        //    {
        ////        var pizza = new[]
        ////        {
        ////    new {Name = "Mushrooms", Count = 3},
        ////    new {Name = "Onions", Count = 1},
        ////    new {Name = "Olives", Count = 1},
        ////    new {Name = "Zucchini", Count = 1},
        ////    new {Name = "Pepperoni", Count = 2}
        ////};

        ////        var json = pizza.ToGoogleDataTable()
        ////                .NewColumn(new Column(ColumnType.String, "Topping"), x => x.Name)
        ////                .NewColumn(new Column(ColumnType.Number, "Slices"), x => x.Count)
        ////                .Build()
        ////                .GetJson();

        ////        return Content(json);
        //    }

        public void SendMail()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("sucoykhoa.medlatec@gmail.com"),
                Subject = "CTV Backup",
                Body = "Test email from Send Grid SMTP Settings"
            };

            mailMessage.To.Add("congchien1710@gmail.com");

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential("sucoykhoa.medlatec@gmail.com", "Chienkaka060991"),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            smtpClient.Send(mailMessage);

            string fromMail = "sucoykhoa.medlatec@gmail.com";
            string passMail = "Chienkaka060991";
            string fromName = "CTV Backup";
            string email = "congchien1710@gmail.com";
            string subject = "CTV Backup";
            string strBody = "Test email from Send Grid SMTP Settings";

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                            new System.Net.Mail.MailAddress(fromMail, fromName),
                            new System.Net.Mail.MailAddress(email));
            m.Subject = subject;
            m.Body = strBody;
            m.IsBodyHtml = true;

            //if (lstAttachFile != null)
            //{
            //    System.Net.Mail.Attachment attachment;

            //    foreach (var attachFile in lstAttachFile)
            //    {
            //        attachment = new System.Net.Mail.Attachment(attachFile);
            //        m.Attachments.Add(attachment);
            //    }
            //}

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Credentials = new System.Net.NetworkCredential(fromMail, passMail);
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        //private async Task SendEmail(string email, string subject, string htmlContent)
        //{
        //    var apiKey = "YOUR SENDGRID API Key";
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("support@dotnetthoughts.net", "Support");
        //    var to = new EmailAddress(email);
        //    var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}
    }

    public class Chart
    {
        public object[] cols { get; set; }
        public object[] rows { get; set; }
    }
}
