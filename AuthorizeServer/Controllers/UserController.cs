using System;
using System.Collections.Generic;
using AuthorizeServer.ViewModels;
using AuthorizeServer.ViewModels.Request;
using AuthorizeServer.ViewModels.Response;
using AutoMapper;
using Buiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthorizeServer.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBll _userBll;
        private readonly IMapper _mapper;
        public UserController(IUserBll userBll, IMapper mapper)
        {
            _userBll = userBll;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> User([FromQuery]KeywordPageQueryRequest queryRequest)
        {
            var queryResult =
                await _userBll.GetUserList(queryRequest.PageIndex, queryRequest.PageSize, queryRequest.Key);
            var result = new QueryResponse<UserEntity>(queryResult.Item1)
            {
                data = queryResult.Item2
            };
            return new JsonResult(new { data = result });
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> User([FromBody] UserLoginModel model)
        {
            var result = new ActionResponse();
            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    result.ErrorMessage = "用户名不能为空";
                    return new JsonResult(new { data = result });
                }
                else if (model.UserName == "Admin")
                {
                    result.ErrorMessage = "不能添加用户名为Admin的用户";
                    return new JsonResult(new { data = result });
                }

                if (await  _userBll.ExistUserByCondition(x =>
                    x.UserName == model.UserName || x.NormalizedUserName == model.UserName))
                {
                    result.ErrorMessage = "已存在该用户名";
                    return new JsonResult(new { data = result });
                }
                if (await _userBll.ExistUserByCondition(x =>
                    x.Email == model.Email || x.NormalizedEmail == model.Email))
                {
                    result.ErrorMessage = "已存在该邮箱";
                    return new JsonResult(new { data = result });
                }
                var entity = _mapper.Map<UserLoginModel, UserEntity>(model, opt => opt.ConfigureMap(MemberList.None));
                var identityResult = await _userBll.AddUser(entity, model.Password);
                result.Successed = identityResult.Succeeded;
                result.ErrorMessage = identityResult.Errors.Join(";");
                return new JsonResult(new { data = result });
            }
            catch (Exception exception)
            {
                result.ErrorMessage = exception.Message;
                return new JsonResult(new { data = result });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> User([FromRoute] Guid id)
        {
            var result = new ActionResponse();
            try
            {
                var identityResult = await _userBll.DeleteUserById(id);
                result.Successed= identityResult.Succeeded;
                result.ErrorMessage = identityResult.Errors.Join(";");
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return  new JsonResult(new {data=result});
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("UserRole/{userId}")]
        public async Task<IActionResult> UserRole([FromRoute]Guid userId,[FromQuery] Guid roleId)
        {
            var result = new ActionResponse();
            try
            {
                var userEntity =await _userBll.GetUserById(userId);
                var identityResult =await _userBll.AddUserRoleAsync(userEntity, new List<string> {roleId.ToString()});
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