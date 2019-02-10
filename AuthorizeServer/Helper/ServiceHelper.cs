using AuthorizeServer.ViewModels;
using AutoMapper;
using Buiness.Hepler;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using AuthorizeServer.Models;
using DataAccess.Models;
using IdServerModel = IdentityServer4.Models;

namespace AuthorizeServer.Helper
{
    public static class ServiceHelper
    {
        public static void AddAutoMapper(this IServiceCollection service)
        {
            service.AddSingleton(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<IdentityResource, IdServerModel.IdentityResource>().ForMember(x=>x.UserClaims,y=>y.MapFrom(o=>o.UserClaims));
                    cfg.CreateMap<ApiResource, IdServerModel.ApiResource>();
                    cfg.CreateMap<List<ApiScope>, List<IdServerModel.Scope>>();
                    cfg.CreateMap<ApiResource, IdServerModel.ApiResource>().ReverseMap();
                    cfg.CreateMap<ApiResourcesViewModel, IdServerModel.ApiResource>();
                    cfg.CreateMap<SecretViewModel, IdServerModel.Secret>();
                    cfg.CreateMap<ScopeViewModel, IdServerModel.Scope>();
                    cfg.CreateMap<ClientViewModel, IdServerModel.Client>().BeforeMap((x,y)=>y.AllowedGrantTypes=GrantTypeHelper.GetGrantType(x.AllowedGrantTypes)).ForMember(dest=>dest.AllowedGrantTypes,opt=>opt.Ignore());
                    cfg.CreateMap<RoleViewModel,RoleEntity>().ForMember(x=>x.RoleClaims,o=>o.Ignore()).ForMember(x=>x.UserRoles,o=>o.Ignore());
                }).CreateMapper());
        }

        public static void AddServices(this IServiceCollection service)
        {
            var bllTypes = typeof(ResourcesHelper).Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("Helper") && !x.IsInterface).Select(
                    x =>
                    {
                        var @interface = x.GetInterfaces()?.FirstOrDefault() ?? x;
                        return (InterfaceType: @interface, ImplementationType: x);
                    });
            foreach (var type in bllTypes)
            {
                service.AddScoped(type.InterfaceType, type.ImplementationType);
            }
          
        }
    }
}
