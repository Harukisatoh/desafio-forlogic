using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClientController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = "SELECT c.*, MAX(e.EvaluationReferenceDate) as LastEvaluationReferenceDate " +
                "FROM Evaluation as e " +
                "INNER JOIN ClientEvaluation as ce " +
                    "ON e.EvaluationId = ce.EvaluationId " +
                "INNER JOIN Client as c " +
                    "ON c.ClientId = ce.ClientId " +
                    "GROUP BY ce.ClientId, c.ClientId, c.ClientCompanyName, c.ClientContactName, " +
                            "c.ClientCNPJ, c.JoiningDate, c.LastEvaluationCategory";

            DataTable table = new DataTable();
            using(var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using(var cmd = new SqlCommand(query, con))
            using(var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public HttpResponseMessage Post(Client client)
        {
            try
            {
                string query = $"INSERT INTO dbo.Client" +
                    $"(ClientCompanyName, ClientContactName, ClientCNPJ) VALUES (" +
                    $"'{client.ClientCompanyName}'," +
                    $"'{client.ClientContactName}'," +
                    $"'{client.ClientCNPJ}')";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client created succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        public HttpResponseMessage Put(Client client)
        {
            try
            {
                string query = $"UPDATE dbo.Client SET " +
                        $"ClientCompanyName='{client.ClientCompanyName}'," +
                        $"ClientContactName='{client.ClientContactName}'," +
                        $"ClientCNPJ='{client.ClientCNPJ}'" +
                    $"WHERE ClientId={client.ClientId}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client updated succesfully!" });
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
                string query = $"DELETE FROM dbo.Client WHERE ClientId={id}";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client deleted succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }

        public class SearchRequest
        {
            public string ClientCompanyName { get; set; }
        }

        [Route("api/Client/search")]
        [HttpGet]
        public HttpResponseMessage GetClientsFromName([FromBody] SearchRequest searchRequest)
        {
            try
            {
                string query = "SELECT c.*, MAX(e.EvaluationReferenceDate) as LastEvaluationReferenceDate " +
                    "FROM Evaluation as e " +
                    "INNER JOIN ClientEvaluation as ce " +
                        "ON e.EvaluationId = ce.EvaluationId " +
                    "INNER JOIN Client as c " +
                        "ON c.ClientId = ce.ClientId " +
                        $"WHERE c.ClientCompanyName = '{searchRequest.ClientCompanyName}' " +
                        "GROUP BY ce.ClientId, c.ClientId, c.ClientCompanyName, c.ClientContactName, " +
                                "c.ClientCNPJ, c.JoiningDate, c.LastEvaluationCategory";

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
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
            
        }
    }
}
