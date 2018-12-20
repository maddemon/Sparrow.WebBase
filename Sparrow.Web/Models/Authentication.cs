using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparrow.Web.Models
{
    [Table("authentication")]
    public class Authentication
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public AuthenticationType Type { get; set; }

        public string NameIdentified { get; set; }

        public string AccessToken { get; set; }
    }

    public enum AuthenticationType
    {
        [Description("密码")]
        Password,
    }
}
