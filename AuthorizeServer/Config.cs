using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthorizeServer
{
    public class Config
    {
        /// <summary>
        /// api资源,允许客户端请求API的访问令牌
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //return new List<ApiResource>
            //{
            //    new ApiResource("Api","ApiDemo")
            //    {
            //        ApiSecrets =
            //        {
            //            new Secret("secret2".Sha256())
            //        },
            //        UserClaims =
            //        {
            //            JwtClaimTypes.Name,JwtClaimTypes.Email
            //        },
            //       Scopes ={
            //            new Scope()
            //            {
            //                Name = "Api_1",
            //                DisplayName = "Api_1"
            //            },
            //           new Scope()
            //           {
            //               Name = "Api_2",
            //               DisplayName = "Api_2"
            //           }
            //       }
            //    }
            //};
            return new List<ApiResource>
            {
                new ApiResource("Api","ApiDemo")
                
            };
        }
        /// <summary>
        /// 定义认证资源（认证资源即身份资源，是用户的用户ID，名称，电子邮件地址，电话等等，也可以自定义标识资源）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResourceses()
        {
            var custonerProfile=new IdentityResource
            {
                Name = "userInfo",
                DisplayName = "UserInfo",
                UserClaims = {"name","email" } //应包含在身份令牌中的关联用户声明类型的列表
            };
            return new List<IdentityResource>
            {
               new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                custonerProfile
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            //return new List<Client>
            //{
            //    new Client
            //    {
            //        ClientId="code_client",
            //        ClientName="code_clientDemo",
            //        AllowedGrantTypes=GrantTypes.Code,
            //        RequirePkce = true,
            //        ClientSecrets={ new Secret("secret1".Sha256())},
            //        RedirectUris={"http://localhost:5001/account/oAuth2"},
            //        PostLogoutRedirectUris = {},
            //        AccessTokenType = AccessTokenType.Jwt,
            //        AllowedScopes=new List<string>{"Home"},
                    
            //    },
            //    new Client
            //    {
            //        ClientId="client1",
            //        ClientName="code_clientDemo",
            //        ClientUri = "http://localhost:5001",
            //        AllowedGrantTypes=GrantTypes.Hybrid,
            //        RequirePkce = true,
            //        ClientSecrets={ new Secret("secret2".Sha256())},
            //        RedirectUris={"http://localhost:5001/signin-oidc"},
            //        PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },
            //        AllowOfflineAccess = true,
            //        AccessTokenType = AccessTokenType.Jwt,
            //        AllowedScopes=new List<string>{ "Api2" }
            //    }
            //};
            return new List<Client>
            {
              new Client
              {
                    ClientId = "openid",
                  ClientName = "openid client",
                  AllowedGrantTypes = GrantTypes.Implicit,
                  RedirectUris = { "http://localhost:5001/signin-oidc" },
                  PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                  AllowedScopes = new List<string>
                  {
                      IdentityServerConstants.StandardScopes.OpenId,
                      IdentityServerConstants.StandardScopes.Profile
                  }
              }
            };
        }
        internal static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser{
                    SubjectId="100",
                    Username="testUser",
                    Password="123456",
                    Claims = new List<Claim>()
                }
            };
        }
    }
}
