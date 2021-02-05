using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Evaluation
    {
        [Key]
        public int EvaluationId { get; set; }
        public DateTime EvaluationReferenceDate { get; set; }
    }
}