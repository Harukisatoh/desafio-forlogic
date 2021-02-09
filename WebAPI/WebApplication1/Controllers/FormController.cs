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
            string storedProcedure = "InsertEvaluationAndReturnId";
            try
            {

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(storedProcedure, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@EvaluationReferenceDate", SqlDbType.VarChar)).Value = evaluationReferenceDate;
                    da.Fill(table);

                    var evaluationId = table.Rows[0]["Column1"];

                    for(int i = 0; i < form.ClientEvaluations.Length; i++)
                    {
                        string evaluationCategory;
                        if(form.ClientEvaluations[i].Grade >= 9)
                        {
                            evaluationCategory = "Promotor";
                        } else if(form.ClientEvaluations[i].Grade >= 7)
                        {
                            evaluationCategory = "Neutro";
                        } else
                        {
                            evaluationCategory = "Detrator";
                        }

                        string secondStoredProcedure = "InsertClientEvaluation";

                        cmd.CommandText = secondStoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int)).Value = form.ClientEvaluations[i].ClientId;
                        cmd.Parameters.Add(new SqlParameter("@EvaluationId", SqlDbType.Int)).Value = evaluationId;
                        cmd.Parameters.Add(new SqlParameter("@Grade", SqlDbType.Int)).Value = form.ClientEvaluations[i].Grade;
                        cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar)).Value = form.ClientEvaluations[i].Reason;
                        cmd.Parameters.Add(new SqlParameter("@LastEvaluationCategory", SqlDbType.VarChar)).Value = evaluationCategory;
                        da.Fill(table);

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Evaluation created succesfully" });
                }

            } catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }
    }
}
