using AuthorizeServer.Models;
using Buiness;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.ViewModels;

namespace AuthorizeServer.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientBll _clientBll;
        private readonly IResourcesBll _resourcesBll;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(IIdentityServerInteractionService interaction, IClientBll clientBll, IResourcesBll resourcesBll, ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientBll = clientBll;
            _resourcesBll = resourcesBll;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModel(returnUrl);
            if(model!=null)
              return View(model);
            return View("Error");
        
        }

        /// <summary>
        /// 提交同意/拒绝
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentViewModel model)
        {
            var result =await ProcessConsentResult(model);
            if (result.IsRedirect)
            {
                if (await _clientBll.IsPkceClientAsync(result.ClientId))
                {
                    return View("Redirect", new RedirectViewModel { RedirectUrl = result.RedirectUri });
                }

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError(string.Empty,result.ValidationError);
            }

            if (result.ShowView)
            {
                return View(result.ViewModel);
            }

            return View("Error");
        }

        private async Task<ProcessConsentResult> ProcessConsentResult(ConsentViewModel model)
        {
            var result = new ProcessConsentResult();
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (context == null) return result;
            ConsentResponse consentResponse = null;
            //拒绝
            if (model?.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }else if (model?.Button == "yes") //同意授权
            {
                if (model?.ScopesConsented != null || (model.ScopesConsented!=null&&model.ScopesConsented.Any()))
                {
                    var scopes = model.ScopesConsented;
                    if (!ConsentConfig.EnableOfflineAccess)
                    {
                        scopes = scopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess);
                    }

                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };
                    //Log
                }
                else
                {
                    result.ValidationError = ConsentConfig.MustChooseOneErrorMessage;
                }
            }
            else
                result.ValidationError = ConsentConfig.InvalidSelectionErrorMessage;

            if (consentResponse != null)
            {

                //将授权结果回传给ID4
                await _interaction.GrantConsentAsync(context, consentResponse);
                //是否重新定向到授权端点
                result.RedirectUri = model.ReturnUrl;
                result.ClientId = context.ClientId;
            }
            else
            {
                result.ViewModel = await BuildConsentViewModel(model.ReturnUrl,model);
            }
            return result;
        }
        

        /// <summary>
        /// 创建同意视图模型
        /// </summary>
        /// <param name="returnUrl">重定向url</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl, ConsentInputModel model =null)
        {
            //获取授权上下文
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
            {
                var client =await _clientBll.GetEnabledClientByClientId(context.ClientId);
                if (client != null)
                {
                    var resources =await _resourcesBll.FindResourcesByScopeAsync(context.ScopesRequested);
                    if (resources != null && resources.IdentityResources.Any() || resources.ApiResources.Any())
                    {
                        return CreateConsentViewModel(model, returnUrl, context, client, resources);
                    }
                    else
                    {
                        _logger.LogError("");
                    }
                }
                else
                {
                    _logger.LogError($"客户端ID:{context.ClientId}验证失败");
                }
            }
            else
            {
                _logger.LogError($"没有与{returnUrl}匹配的Consent");
            }
            return  new ConsentViewModel();
        }
        /// <summary>
        /// 创建同意视图模型
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <param name="request"></param>
        /// <param name="client"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        private ConsentViewModel CreateConsentViewModel(ConsentInputModel model, string returnUrl,
            AuthorizationRequest request,
            Client client, Resources resources)
        {
            var scopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();
            var consentViewModel = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = scopesConsented,
                ReturnUrl = returnUrl,
                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                IdentityScopes = resources.IdentityResources
                    .Select(x => CreateScopeViewModel(x, model != null || scopesConsented.Contains(x.Name))).ToArray(),
                ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(y =>
                    CreateScopeViewModel(y, scopesConsented.Contains(y.Name) || model != null)).ToArray()
            };

            return consentViewModel;
        }

        /// <summary>
        /// 创建Scope视图模型
        /// </summary>
        /// <param name="identityResource"></param>
        /// <param name="check">是否选中</param>
        /// <returns></returns>
        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Emphasize = identityResource.Emphasize,
                Required = identityResource.Required,
                Checked = check || identityResource.Required
            };
        }
        /// <summary>
        /// 创建Scope视图模型
        /// </summary>
        /// <param name="scope">ApiResources中的scope范围</param>
        /// <param name="check">是否选中</param>
        /// <returns></returns>
        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }
    }
}