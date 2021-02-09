using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ClientCompanyName { get; set; }
        [Required]
        [MaxLength(500)]
        public string ClientContactName { get; set; }

        [MaxLength(14)]
        public string ClientCNPJ { get; set; }

        public DateTime JoiningDate { get; set; }

        [MaxLength(8)]
        public string LastEvaluationCategory { get; set; }
    }
}