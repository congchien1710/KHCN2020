using KHCN.Data.Entities;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KHCN.Web.Controllers
{
    public class DeTaiController : Controller
    {
        private readonly IDeTaiRepository _detaiRepository;
        private readonly ICapQuanLyRepository _capquanlyRepository;
        private readonly ILoaiNhiemVuRepository _loainhiemvuRepository;
        private readonly INganhRepository _nganhRepository;

        public DeTaiController(IDeTaiRepository detaiRepository, ICapQuanLyRepository capquanlyRepository, ILoaiNhiemVuRepository loainhiemvuRepository, INganhRepository nganhRepository)
        {
            _detaiRepository = detaiRepository;
            _capquanlyRepository = capquanlyRepository;
            _loainhiemvuRepository = loainhiemvuRepository;
            _nganhRepository = nganhRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.DDLCAPQUANLY = new SelectList(_capquanlyRepository.GetAll(), "Id", "TenCapQuanLy");
            ViewBag.DDLNGANH = new SelectList(_nganhRepository.GetAll(), "Id", "TenNganh");
            ViewBag.DDLLOAINHIEMVU = new SelectList(_loainhiemvuRepository.GetAll(), "Id", "TenLoaiNhiemVu");

            return View();
        }
    }
}