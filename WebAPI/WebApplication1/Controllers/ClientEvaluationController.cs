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
