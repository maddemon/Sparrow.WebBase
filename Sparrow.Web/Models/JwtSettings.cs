using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow.Web.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }

        public static JwtSettings New(IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);
            return jwtSettings;
        }
    }
}
