using KHCN.Api.Services;
using KHCN.Data.Entities.KHCN;
using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KHCN.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AccountController(IUnitOfWork unitOfWork, IConfiguration config, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        [ApiAuthorize]
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] CMS_User obj)
        {
            var user = _userRepository.GetByUsername(obj.UserName);
            if (user != null)
                return StatusCode(409);

            obj.Password = _passwordHasher.GenerateIdentityV3Hash(obj.Password);
            obj.IsActive = true;
            obj.CreatedBy = obj.UpdatedBy = "API";
            obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

            await _userRepository.AddAsync(obj);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                sw.Restart();
                var user = _userRepository.GetByUsername(username);
                if (user == null || !_passwordHasher.VerifyIdentityV3Hash(password, user.Password))
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[Account.Login]. Login failed from username [{username}]. Detail: Username or password incorrect.");
                    return BadRequest();
                }

                var lstRole = _userRoleRepository.GetByIdUser(user.Id).Select(m => m.IdRole).ToList();
                var usersClaims = new[]
                {
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("FullName", user.FullName.ToString()),
                new Claim("Role", string.Join(",", lstRole)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userRepository.UpdateAsync(user, user.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[Account.Login]. Login success from username [{username}]. Time process: {sw.Elapsed.Seconds} seconds.");

                return new ObjectResult(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[Account.Login]. Login exception from username: [{username}]. Detail: {ex}");
                return BadRequest();
            }
        }

        [ApiAuthorize]
        [HttpPost]
        public IActionResult SeedData()
        {
            //SeedCapBac();
            //SeedCapQuanLy();
            //SeedChucDanh();
            //SeedGiaiDoan();
            //SeedLoaiNhiemVu();
            //SeedNganh();
            //SeedPhongBan();
            //SeedDonViCheTao();
            SeedTrinhDo();
            SeedTienDoThucHien();

            return Ok();
        }

        private void SeedCapBac()
        {
            var lstObj = new List<KHCN_CapBac>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_CapBac
                {
                    Name = $"Cấp bậc {i.ToString()}",
                    Description = $"Cấp bậc {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_CapBac>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedCapQuanLy()
        {
            var lstObj = new List<KHCN_CapQuanLy>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_CapQuanLy
                {
                    Name = $"Cấp quản lý {i.ToString()}",
                    Description = $"Cấp quản lý {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_CapQuanLy>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedChucDanh()
        {
            var lstObj = new List<KHCN_ChucDanh>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_ChucDanh
                {
                    Name = $"Chức danh {i.ToString()}",
                    Description = $"Chức danh {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_ChucDanh>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedGiaiDoan()
        {
            var lstObj = new List<KHCN_GiaiDoan>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_GiaiDoan
                {
                    Code = $"GD_{i.ToString()}",
                    Name = $"Giai đoạn {i.ToString()}",
                    Description = $"Giai đoạn {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_GiaiDoan>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedLoaiNhiemVu()
        {
            var lstObj = new List<KHCN_LoaiNhiemVu>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_LoaiNhiemVu
                {
                    Name = $"Loại nhiệm vụ {i.ToString()}",
                    Description = $"Loại nhiệm vụ {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_LoaiNhiemVu>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedNganh()
        {
            var lstObj = new List<KHCN_Nganh>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_Nganh
                {
                    Name = $"Ngành {i.ToString()}",
                    Description = $"Ngành {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_Nganh>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedPhongBan()
        {
            var lstObj = new List<KHCN_PhongBan>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_PhongBan
                {
                    Name = $"Phòng ban {i.ToString()}",
                    Description = $"Phòng ban {i.ToString()}",
                    DonViChaId = null,
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_PhongBan>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedDonViCheTao()
        {
            var lstObj = new List<KHCN_DonViCheTao>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_DonViCheTao
                {
                    Code = $"DVCT_{i.ToString()}",
                    Name = $"Đơn vị {i.ToString()}",
                    Description = $"Đơn vị {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_DonViCheTao>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedTrinhDo()
        {
            var lstObj = new List<KHCN_TrinhDo>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_TrinhDo
                {
                    Name = $"Trình độ {i.ToString()}",
                    Description = $"Trình độ {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_TrinhDo>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }

        private void SeedTienDoThucHien()
        {
            var lstObj = new List<KHCN_TienDoThucHien>();
            var now = DateTime.Now;

            for (int i = 10; i < 10000; i++)
            {
                lstObj.Add(new KHCN_TienDoThucHien
                {
                    Name = $"Tiến độ {i.ToString()}",
                    Description = $"Tiến độ {i.ToString()}",
                    IsActive = true,
                    CreatedBy = "sys",
                    UpdatedBy = "sys",
                    CreatedDate = now,
                    UpdatedDate = now,
                });
            }

            if (lstObj != null && lstObj.Any())
            {
                _unitOfWork.Context.Set<KHCN_TienDoThucHien>().AddRange(lstObj);
                _unitOfWork.Commit();
            }
        }
    }
}