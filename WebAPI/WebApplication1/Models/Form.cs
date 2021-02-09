using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Form
    {
        public DateTime EvaluationReferenceDate { get; set; }

        public ClientEvaluation[] ClientEvaluations { get; set; }
    }
}