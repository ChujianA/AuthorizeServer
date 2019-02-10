using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.Helper;
using AuthorizeServer.ViewModels;
using AuthorizeServer.ViewModels.Response;
using AutoMapper;
using Buiness;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityResourcesesController : ControllerBase
    {
        private readonly IIdentityResourcesBll _identityResources;
        private readonly IMapper _mapper;
        public IdentityResourcesesController(IIdentityResourcesBll identityResources,IMapper mapper)
        {
            _identityResources = identityResources;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> IdentityResourceses([FromBody]IdentityResourcesesViewModel model)
        {
            var result = new ActionResponse();
            try
            {
                var entity = IdentityResourceHelper.IdentityResource(model);
                result.Successed=(await _identityResources.AddIdentityResources(entity.ToEntity()))>0;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return  new JsonResult(new {data=result});
        }
    }
}