using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparrow.Web.Models
{
    [Table("organization")]
    public class Organization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int ParentId { get; set; }

        public string IdPath { get; set; }
    }

    [Table("user_organization")]
    public class UserOrganization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserId { get; set; }

        public int OrganizationId { get; set; }

        [JsonIgnore]
        public virtual Organization Organization { get; set; }
    }
}
