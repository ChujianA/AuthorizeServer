using IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels
{
    public class SecretViewModel
    {


        public string Description { get; set; }


        public string Value { get; set; }


        public DateTime? Expiration { get; set; }

        public string Type { get; set; }

        public SecretViewModel()
        {
            Type = IdentityServerConstants.SecretTypes.SharedSecret;
        }

    }
}
