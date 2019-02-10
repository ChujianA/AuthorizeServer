using AuthorizeServer.ViewModels;
using AuthorizeServer.ViewModels.Response;
using AutoMapper;
using Buiness;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientBll _clientBll;
        private readonly IMapper _mapper;
        public ClientController(IClientBll clientBll, IMapper mapper)
        {
            _clientBll = clientBll;
            _mapper = mapper;
        }

      

        [HttpPost]
        public async Task<JsonResult> Client([FromBody]ClientViewModel clientModel)
        {
            var result = new ActionResponse();
            try
            {
                if (string.IsNullOrEmpty(clientModel.ClientName))
                {
                    result.ErrorMessage = "客户端名称不能为空";
                    return new JsonResult(new { data = result });
                }
                if ((await _clientBll.GetClientByName(clientModel.ClientName)) != null)
                {
                    result.ErrorMessage = "已存在此客户端名称";
                    return new JsonResult(new { data = result });
                }

                foreach (var item in clientModel.ClientSecrets)
                {
                    item.Value = item.Value.Sha256();
                }
                var client = _mapper.Map<ClientViewModel, Client>(clientModel);
                result.Successed = (await _clientBll.AddClient(client)) > 0;
                result.Successed =true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return new JsonResult(new { data=result});
        }
    }
}