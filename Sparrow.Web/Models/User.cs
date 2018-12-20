using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparrow.Web.Models
{
    [Table("user")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public bool Deleted { get; internal set; }

        [JsonIgnore]
        public virtual ICollection<Authentication> Authentications { get; set; }
    }

    public enum UserRole
    {
        [Description("游客")]
        Guest = 0,
        [Description("管理员")]
        Manager = 1,
        [Description("超级管理员")]
        Administrator = 2
    }
}
