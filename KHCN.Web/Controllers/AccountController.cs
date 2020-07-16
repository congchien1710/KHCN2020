using KHCN.Data.Entities.System;
using KHCN.Data.Repository;
using KHCN.Data.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KHCN.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AccountController(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public IActionResult Login(string requestPath)
        {
            ViewBag.RequestPath = requestPath ?? "/";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = Authenticated(model.Username, model.Password);
                if (res == null)
                {
                    ModelState.AddModelError("", "Username hoặc mật khẩu không đúng!");
                    return View();
                }

                var lstRole = _userRoleRepository.GetByIdUser(res.Id).Select(m => m.IdRole).Distinct().ToList();

                // Create Claims
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "CMS Cookie Authentication"),
                    new Claim("UserId", res.Id.ToString()),
                    new Claim("UserName", res.UserName),
                    new Claim("FullName", res.FullName.ToString()),
                    new Claim("Role", string.Join(",", lstRole)),
            };

                // create identity
                ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

                // Create Principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                // Sign-in
                await HttpContext.SignInAsync(
                        scheme: "CMSSecurityScheme",
                        principal: principal,
                        properties: new AuthenticationProperties
                        {
                        //IsPersistent = true, // for \'remember me\' feature
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(120)
                        });

                return Redirect(model.RequestPath ?? "/");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var res = Authenticated(model.Username, model.Password);
        //        if (res != null)
        //        {
        //            ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
        //            return View();
        //        }

        //        var user = new AffUser()
        //        {
        //            phone_number = model.Phone,
        //            user_name = model.Username,
        //            password = Encryptor.MD5Hash(model.Password),
        //            limit = nguongRutTien
        //        };

        //        // Check username
        //        var exsitUsername = _userRepository.GetByUsername(user.user_name);
        //        if (exsitUsername != null)
        //        {
        //            ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
        //            return View(user);
        //        }

        //        //// Check email
        //        //if (!string.IsNullOrEmpty(obj.email))
        //        //{
        //        //    var exsitEmail = _userRepository.GetByEmail(obj.email);
        //        //    if (exsitEmail != null)
        //        //    {
        //        //        ModelState.AddModelError("", "Email đã tồn tại!");
        //        //        return View("AddOrUpdateUser", obj);
        //        //    }
        //        //}

        //        // Check refer
        //        var existRefer = true;
        //        while (existRefer)
        //        {
        //            var randomReferCode = XUtil.GenerateRandomReferCode();
        //            if (_userRepository.GetByReferCode(randomReferCode) == null)
        //            {
        //                existRefer = false;
        //                user.refer_code = randomReferCode;
        //            }
        //        }

        //        user.id = Guid.NewGuid().ToString();
        //        user.status = UserStatus.ACTIVATED;
        //        user.creator = "user register";
        //        user.created_on = XUtil.TimeInEpoch(DateTime.Now);
        //        user.modifier = "user register";
        //        user.modified_on = user.created_on;

        //        user.full_name = string.Empty;
        //        user.email = string.Empty;
        //        user.account_holder = string.Empty;
        //        user.account_number = string.Empty;
        //        user.bank = string.Empty;
        //        user.branch_bank = string.Empty;
        //        user.address = string.Empty;

        //        if (_userRepository.Index(user))
        //            return RedirectToAction("Login");
        //        else
        //        {
        //            log.Error("_userRepository.Index " + user.full_name);
        //        }
        //    }

        //    return View(model);
        //}

        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "CMSSecurityScheme");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Access()
        {
            return View();
            //return NotFound();
        }

        private CMS_User Authenticated(string username, string password)
        {
            string strUserName = username.Trim();
            var res = _userRepository.Login(strUserName, out CMS_User userInfo);
            if (res)
            {
                if (PasswordHasher.VerifyIdentityV3Hash(password, userInfo.Password))
                    return userInfo;
            }

            return null;
        }

        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        [Authorize]
        public ActionResult ChangePassword(int userId)
        {
            ChangePWViewModel obj = new ChangePWViewModel();

            if (userId > 0)
            {
                var model = _userRepository.Get(userId);
                obj.UserId = model.Id;
            }

            return View("ChangePassword", obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePWViewModel obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = @User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var model = _userRepository.Get(int.Parse(userId));
                        if (model != null)
                        {
                            if (PasswordHasher.VerifyIdentityV3Hash(obj.CurrentPassword, model.Password))
                            {
                                model.Password = PasswordHasher.GenerateIdentityV3Hash(obj.NewPassWord);
                                model.UpdatedDate = DateTime.Now;
                                model.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type == "UserName").Value;

                                if (_userRepository.UpdatePassWord(model.Id, model))
                                {
                                    SetSuccess("Đổi mật khẩu thành công");
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    SetError("Đổi mật khẩu không thành công");
                                    log.Error("Update password " + model.Id);
                                    return View("ChangePassword", obj);
                                }
                            }
                            else
                            {
                                SetError("Mật khẩu hiện tại không đúng!");
                                return View("ChangePassword", obj);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    log.Error(ex.Message);
                }
            }

            SetError("Đổi mật khẩu không thành công");
            return View("ChangePassword", obj);
        }
    }
}