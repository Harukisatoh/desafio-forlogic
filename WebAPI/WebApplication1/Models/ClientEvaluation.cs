using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ClientEvaluation
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; }
    }
}