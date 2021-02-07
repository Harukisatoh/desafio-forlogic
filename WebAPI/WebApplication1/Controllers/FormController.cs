using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FormController : ApiController
    {
        public HttpResponseMessage Post(Form form)
        {
            string evaluationReferenceDate = $"{form.EvaluationReferenceDate.Year}-{form.EvaluationReferenceDate.Month}-01";
            string firstQuery = $"INSERT INTO dbo.Evaluation VALUES (CAST('{evaluationReferenceDate}' AS DATE)); SELECT SCOPE_IDENTITY()";
            try
            {

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(firstQuery, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);

                    var evaluationId = table.Rows[0]["Column1"];

                    for(int i = 0; i < form.ClientEvaluations.Length; i++)
                    {
                        string secondQuery = $"INSERT INTO dbo.ClientEvaluation VALUES({form.ClientEvaluations[i].ClientId}, {evaluationId}, {form.ClientEvaluations[i].Grade}, '{form.ClientEvaluations[i].Reason}')";

                        cmd.CommandText = secondQuery;
                        cmd.CommandType = CommandType.Text;
                        da.Fill(table);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

            } catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }
    }
}
