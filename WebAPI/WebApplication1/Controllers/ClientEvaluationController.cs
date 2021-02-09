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
    public class ClientEvaluationController : ApiController
    {
        [Route("api/ClientEvaluation/all")]
        public HttpResponseMessage Get()
        {
            string query = "SELECT * FROM dbo.ClientEvaluation";

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

        [Route("api/Client/{clientId}/Evaluation/{evaluationId}")]
        public HttpResponseMessage Post(ClientEvaluation clientEvaluation, int clientId, int evaluationId)
        {
            try
            {
                string firstQuery = $"INSERT INTO dbo.ClientEvaluation " +
                    $"VALUES({clientId}, " +
                        $"{evaluationId}, " +
                        $"{clientEvaluation.Grade}, " +
                        $"'{clientEvaluation.Reason}')";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(firstQuery, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client evaluation created succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.ToString() });
            }
        }

        [Route("api/Client/{clientId}/Evaluation/{evaluationId}")]
        public HttpResponseMessage Put(ClientEvaluation clientEvaluation, int clientId, int evaluationId)
        {
            try
            {
                string query = $"UPDATE dbo.ClientEvaluation SET " +
                        $"Grade='{clientEvaluation.Grade}', " +
                        $"Reason='{clientEvaluation.Reason}' " +
                    $"WHERE ClientId={clientId} AND EvaluationId={evaluationId}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client evaluation updated succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        [Route("api/Client/{clientId}/Evaluation/{evaluationId}")]
        public HttpResponseMessage Delete(int clientId, int evaluationId)
        {
            try
            {
                string query = $"DELETE FROM dbo.ClientEvaluation WHERE ClientId={clientId} AND EvaluationId={evaluationId}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client evaluation deleted succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        [Route("api/ClientEvaluation/{evaluationId}")]
        public HttpResponseMessage GetEvaluationsFromId(int evaluationId)
        {
            string storedProcedure = "GetClientEvaluationsFromEvaluationId";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(storedProcedure, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EvaluationId", SqlDbType.Int)).Value = evaluationId;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }
    }
}
