using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.Models;
using AuthorizeServer.ViewModels;
using AuthorizeServer.ViewModels.Response;
using AutoMapper;
using Buiness;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiResourcesController : ControllerBase
    {
        private readonly IApiResourcesBll _apiResourcesBll;
        private readonly IMapper _mapper;
        public ApiResourcesController(IApiResourcesBll apiResourcesBll, IMapper mapper)
        {
            _apiResourcesBll = apiResourcesBll;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> ApiResources([FromBody] ApiResourcesViewModel model)
        {
            var result = new ActionResponse();
            try
            {
                foreach (var item in model.ApiSecrets)
                {
                    item.Value = item.Value.Sha256();
                }

                if (!model.Scopes.Any())
                {
                    model.Scopes.Add(new ScopeViewModel
                    {
                        Name = model.Name,
                        DisplayName = model.DisplayName
                    });
                }

                var entity= _mapper.Map<IdentityServer4.Models.ApiResource>(model);
              result.Successed= (await _apiResourcesBll.AddApiResourceAsync(entity.ToEntity()))>0;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return  new JsonResult(new{data=result});
        }

        [HttpDelete]
        public async Task<IActionResult> ApiResources(int id)
        {
            var result=new ActionResponse();
            try
            {
                result.Successed=(await _apiResourcesBll.DeleteApiResource(id))>0;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return  new JsonResult(new{data=result});
        }
    }
}