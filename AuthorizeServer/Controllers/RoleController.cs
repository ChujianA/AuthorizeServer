using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.ViewModels;
using AuthorizeServer.ViewModels.Response;
using AutoMapper;
using Buiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace AuthorizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
         private readonly IRoleBll _roleBll;
        private readonly IMapper _mapper;
        public RoleController(IRoleBll roleBll, IMapper mapper)
        {
            _roleBll = roleBll;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Role(RoleViewModel model)
        {
            var result = new ActionResponse();
            try
            {
                var roleEntity = _mapper.Map<RoleEntity>(model);
                if ((await _roleBll.GetRoleByRoleName(model.Name)) != null)
                {
                    result.ErrorMessage = $"角色不能为：{model.Name}";
                    return new JsonResult(new { data = result });
                }
                var identityResult = await _roleBll.AddRole(roleEntity);
                result.Successed = identityResult.Succeeded;
                result.ErrorMessage = identityResult.Errors.Join(";");
               
            }
            catch (Exception e)
            {
                result.Successed = false;
                result.ErrorMessage = e.Message;
            }
            return new JsonResult(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Role(Guid id)
        {
            var result = new ActionResponse();
            try
            {
                var roleEntity = await _roleBll.FindRoleById(id);
                var identityResult = await _roleBll.DeleteRole(roleEntity);
                result.Successed = identityResult.Succeeded;
                result.ErrorMessage = identityResult.Errors.Join(";");

            }
            catch (Exception e)
            {
                result.Successed = false;
                result.ErrorMessage = e.Message;
            }
            return new JsonResult(new { data = result });
        }
       
    }
}