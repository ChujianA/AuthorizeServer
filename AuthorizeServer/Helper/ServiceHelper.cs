using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Buiness.Hepler;
using DataAccess.IRepositorys;
using IdentityServer4.EntityFramework.Entities;
using IdServerModel=IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizeServer.Helper
{
    public static class ServiceHelper
    {
        public static void AddAutoMapper(this IServiceCollection service)
        {
            service.AddSingleton<IMapper>(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<IdentityResource, IdServerModel.IdentityResource>().ForMember(x=>x.UserClaims,y=>y.MapFrom(o=>o.UserClaims));
                    cfg.CreateMap<ApiResource, IdServerModel.ApiResource>();
                    cfg.CreateMap<List<ApiScope>, List<IdServerModel.Scope>>();
                }).CreateMapper());
        }

        public static void AddServices(this IServiceCollection service)
        {
           var types= typeof(IApiResourcesRepository).Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("Repository") && !x.IsInterface).Select(
                    x =>
                    {
                        var @interface = x.GetInterfaces()?.FirstOrDefault() ?? x;
                        return (InterfaceType: @interface, ImplementationType: x);
                    });
            var bllTypes = typeof(ResourcesHelper).Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("Helper") && !x.IsInterface).Select(
                    x =>
                    {
                        var @interface = x.GetInterfaces()?.FirstOrDefault() ?? x;
                        return (InterfaceType: @interface, ImplementationType: x);
                    });
            foreach (var type in types.Union(bllTypes))
            {
                service.AddScoped(type.InterfaceType, type.ImplementationType);
            }
          
        }
    }
}
