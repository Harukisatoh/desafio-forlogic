using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EvaluationController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = "SELECT * FROM dbo.Evaluation";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public HttpResponseMessage Post(Evaluation evaluation)
        {
            try
            {
                string firstDayOfMonth = $"{evaluation.EvaluationReferenceDate.Year}-{evaluation.EvaluationReferenceDate.Month}-01";

                string query = $"INSERT INTO dbo.Evaluation VALUES (CAST('{firstDayOfMonth}' AS DATE))";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Evaluation created succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }

        }

        public HttpResponseMessage Put(Evaluation evaluation)
        {
            try
            {
                string firstDayOfMonth = $"{evaluation.EvaluationReferenceDate.Year}-{evaluation.EvaluationReferenceDate.Month}-01";

                string query = $"UPDATE dbo.Evaluation SET " +
                        $"EvaluationReferenceDate='{firstDayOfMonth}'" +
                    $"WHERE EvaluationId={evaluation.EvaluationId}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Evaluation updated succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                string query = $"DELETE FROM dbo.Evaluation WHERE EvaluationId={id}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Evaluation deleted succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }
    }
}
