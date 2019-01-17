using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViberBot.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}