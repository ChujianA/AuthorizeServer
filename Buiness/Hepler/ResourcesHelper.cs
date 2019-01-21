using DataAccess.IRepositorys;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdModel=IdentityServer4.Models;
using IdentityServer4.EntityFramework.Entities;

namespace Buiness.Hepler
{
    public class ResourcesHelper:IResourcesBll
    {
        private readonly IIdentityResourcesRepository _identityResourcesRepository;
        private readonly IApiResourcesRepository _apiResourcesRepository;
        private readonly IMapper _iMapper;
        public ResourcesHelper(IIdentityResourcesRepository identityResourcesRepository, IApiResourcesRepository apiResourcesRepository,IMapper iMapper)
        {
            _identityResourcesRepository = identityResourcesRepository;
            _apiResourcesRepository = apiResourcesRepository;
            _iMapper = iMapper;
        }

        public async Task<IdModel.Resources> FindResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResources =await _identityResourcesRepository.FindIdentityResourcesByScopeAsync(scopeNames);
            var apiResources = await _apiResourcesRepository.FindApiResourceByScopeAsync(scopeNames);
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
