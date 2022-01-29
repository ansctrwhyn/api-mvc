using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Tb_M_AccountRole")]
    public class AccountRole
    {
       /* [Key]
        public int Id { get; set; }*/
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        [ForeignKey("Account")]
        public string Account_NIK { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
