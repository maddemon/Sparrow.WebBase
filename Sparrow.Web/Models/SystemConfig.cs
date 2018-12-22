using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow.Web.Models
{
    public class SystemConfig
    {
        public string SiteName { get; set; }

        public string Domain { get; set; }

        public string DbConnectionString { get; set; }

        public JwtConfig JwtConfig { get; set; }

        public WeixinConfig WeixinConfig { get; set; }
    }

    public class JwtConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }
    }

    public class WeixinConfig
    {
        public string CropId { get; set; }

        public string AppKey { get; set; }

        public string AppSecret { get; set; }
    }

}
