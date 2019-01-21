using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizeServer.Models;
using AuthorizeServer.ViewModels;
using DataAccess.IRepositorys;
using DataAccess.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizeServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IClientRepository _clientRepository;
        public AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IIdentityServerInteractionService interaction, IAuthenticationSchemeProvider authenticationSchemeProvider, IClientRepository clientRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _clientRepository = clientRepository;
        }
        #region 注册
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserLoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var userInfo = new UserEntity
                {
                    UserName = model.UserName
                };
                var identityResult = await this._userManager.CreateAsync(userInfo);
                if (identityResult.Succeeded)
                {
                    var result = await this._userManager.AddPasswordAsync(userInfo, model.Password);
                    if (result.Succeeded)
                    {
                        //默认不持久化
                        await _signInManager.SignInAsync(userInfo, false);
                        return string.IsNullOrEmpty(returnUrl) ? Redirect("Home/Index") : Redirect(returnUrl);
                    }
                    else
                        ModelState.AddModelError(nameof(model.Password), "密码无效！");
                }
                else
                    identityResult.Errors.ToList().ForEach(error => ModelState.AddModelError(nameof(model), error.Description));
            }
            else
                ModelState.AddModelError(nameof(model), "数据验证没通过");
            return View(model);
        }

        #endregion

        /// <summary>
        /// 创建登录视图模型
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private async Task<LoginViewModel> BuildLoginViewModel(string returnUrl)
        {
           var context =await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
              return  new LoginViewModel
                {
                    EnableLocalLogin = context.IdP == IdentityServerConstants.LocalIdentityProvider,
                    UserName = context?.LoginHint,
                    ExternalProviders = new[] {new ExternalProvider{AuthenticationScheme = context.IdP}}.Where(x=>string.IsNullOrEmpty(x.DisplayName))
                };
               
            }

            var allschemes = await _authenticationSchemeProvider.GetAllSchemesAsync();
             var schemes= allschemes.Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider()
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name,
                    Type = x.HandlerType
                });
            var allowLocalLogin = true;
            if (context?.ClientId != null)
            {
                var client = await _clientRepository.GetClientByClientId(context.ClientId);
                if (client != null)
                {
                    allowLocalLogin = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        schemes = schemes.Where(x =>
                            client.IdentityProviderRestrictions.Select(n => n.Provider)
                                .Contains(x.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountConfig.AllowRememberLogin,
                EnableLocalLogin = allowLocalLogin && AccountConfig.AllowLocalLogin,
                UserName = context?.LoginHint,
                ExternalProviders = schemes.Where(x => !string.IsNullOrEmpty(x.DisplayName)).ToArray()
            };
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl">重定向地址</param>
        /// <returns></returns>
        public async Task<IActionResult> Login(string returnUrl)
        {
            ViewData["url"] = returnUrl;
            var loginViewModel =await BuildLoginViewModel(returnUrl);
            if (loginViewModel.IsExternalLoginOnly)
            {
                return RedirectToAction("ExternalLogin", new {loginViewModel.ExternalProviders,returnUrl });
            }

            return View(loginViewModel);
        }

        /// <summary>
        /// 其他方式登陆
        /// </summary>
        /// <param name="provider">登录方式（Google，qq等）</param>
        /// <param name="returnUrl">重定向地址</param>
        /// <returns></returns>
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            returnUrl = returnUrl ?? "~/";
            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                //link不正确
                return View("Error",new ErrorViewModel("URL不正确"));
            }
            if (AccountConfig.WindowsAuthenticationSchemeName == provider)
            {
                // return await ProcessWindowsLoginAsync(returnUrl);
                return Ok();
            }
            else
            {
                var props = new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(ExternalLoginCallback)),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
                };
                return Challenge(props, provider);
            }
        }

        /// <summary>
        /// 其他方式（外部）登录的回调地址
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ExternalLoginCallback()
        {
            //身份验证
            var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (authenticateResult?.Succeeded!=true)
            {
                return View("Error", new ErrorViewModel("身份验证失败"));
            }

            var (user, provider, providerUserId, claims) =await GetUserFromExternalProvider(authenticateResult);
            if (user == null)
            {
                user = await AutoCreateUserAsync(provider, providerUserId, claims);
            }

            return Ok();
        }

        /// <summary>
        /// 根据现有信息自动创建用户
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userId">用户ID</param>
        /// <param name="claims">用户信息声明</param>
        /// <returns></returns>
        private async Task<UserEntity> AutoCreateUserAsync(string provider, string userId, IEnumerable<Claim> claims)
        {
            var claimList = claims.Select(x =>
            {
                //if the external system sends a display name - translate that to the standard OIDC name claim
                if (x.Type == ClaimTypes.Name)
                    return new Claim(JwtClaimTypes.Name, x.Value);
                else if(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(x.Type))//if the JWT handler has an outbound mapping to an OIDC claim use that
                    return new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[x.Type], x.Value);
                return  x;
            }).ToList();
            if (!claimList.Any(x => x.Type == JwtClaimTypes.Name))
            {

            }

            return new UserEntity();
        }

        private async Task<(UserEntity user, string provider, string providerUserId, IEnumerable<Claim> claims)> GetUserFromExternalProvider(AuthenticateResult result)
        {
            var principal = result.Principal;
            //用其他方式登录的用户ID声明
            var userIdClaim = principal.FindFirst(JwtClaimTypes.Subject) ??
                              principal.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");
            var claim = principal.Claims.ToList();   //由于Ienumerable不能进行Add/Remove
            claim.Remove(userIdClaim);
            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);
            return (user, provider, providerUserId, claim);
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">用户登录相关的实体模型</param>
        /// <param name="url">重定向地址</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model, string url)
        {
            //if (ModelState.IsValid)
            //{
                var userInfo = await _userManager.FindByNameAsync(model.UserName);
                if (userInfo != null)
                {
                    if (await _userManager.CheckPasswordAsync(userInfo, model.Password))
                    {
                        //是否记住密码
                        AuthenticationProperties prop = null;
                        if (model.RememberMe)
                        {
                            prop = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                            };
                        }
                        await _signInManager.SignInAsync(userInfo, prop);
                        if (_interaction.IsValidReturnUrl(url))
                            return RedirectToUrl(url);
                        else
                            return Redirect("/");
                    }
                    else
                        ModelState.AddModelError(nameof(UserLoginModel.Password), "PassWord not exist");
                }else
                     ModelState.AddModelError(nameof(UserLoginModel.UserName), "UserName not exist");
            //}
            return View(model);
        }

        public async Task<IActionResult> Loginout(string returnUrl=null)
        {
            await _signInManager.SignOutAsync();
            return !string.IsNullOrEmpty(returnUrl) ?  RedirectToUrl(returnUrl) :  RedirectToAction("Login");
        }

        //跳转地址
        public IActionResult RedirectToUrl(string url)
        {
            if (Url.IsLocalUrl(url))
                return base.LocalRedirect(url);
            return Redirect(url);
        }
    }
}