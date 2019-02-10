using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdModel = IdentityServer4.Models;

namespace Buiness.Hepler
{
    public class ResourcesHelper:IResourcesBll
    {
        private readonly IIdentityResourcesBll _identityResources;
        private readonly IApiResourcesBll _apiResourcesBll;
        private readonly IMapper _iMapper;
        public ResourcesHelper(IIdentityResourcesBll identityResources, IApiResourcesBll apiResourcesBll, IMapper iMapper)
        {
            _identityResources = identityResources;
            _apiResourcesBll = apiResourcesBll;
            _iMapper = iMapper;
        }

        public async Task<IdModel.Resources> FindResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            
               var identityResources =await _identityResources.FindIdentityResourcesByScopeAsync(scopeNames);
            var apiResources = await _apiResourcesBll.FindApiResourceByScopeAsync(scopeNames);
            Validate(identityResources, apiResources);
            var apis=new List<IdModel.ApiResource>();
            foreach (var apiResource in apiResources)
            {
                var apieResource = _iMapper.Map<ApiResource,IdModel.ApiResource>(apiResource, opt =>
                    {
                        opt.BeforeMap((src, dest) =>
                            {
                                src.Scopes = src.Scopes?.Where(x => scopeNames.Contains(x.Name)).ToList();
                            });
                    });
                apis.Add(apieResource);
            }
            var identityResourcesModel = _iMapper.Map<List<IdentityResource>, List<IdModel.IdentityResource>>(identityResources.ToList());
            return  new IdModel.Resources(identityResourcesModel, apis);
        }

        private void Validate(IEnumerable<IdentityResource> identityResources, IEnumerable<ApiResource> apiResources)
        {
            var identityResourceNames = (from ir in identityResources
                select ir.Name).ToArray();
            
            var apiScopeName = (from api in apiResources
                from scope in api.Scopes??new List<ApiScope>()
                select scope.Name).ToArray();
           var SameValue=identityResourceNames.Intersect(apiScopeName);
            if (SameValue.Any())
            {
                var names = SameValue.Aggregate((x, y) => x + "|" + y);
                throw new Exception($"identity scope和api scope中Name属性具有相同的值,值为:{names}");
            }
        }
    }
}
