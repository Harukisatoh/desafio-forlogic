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
            string storedProcedure = "GetClients";

            DataTable table = new DataTable();
            using(var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using(var cmd = new SqlCommand(storedProcedure, con))
            using(var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public HttpResponseMessage Post(Client client)
        {
            try
            {
                string storedProcedure = "CreateClient";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                    ConnectionStrings["DesafioForLogicAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(storedProcedure, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientCompanyName", SqlDbType.VarChar)).Value = client.ClientCompanyName;
                    cmd.Parameters.Add(new SqlParameter("@ClientContactName", SqlDbType.VarChar)).Value = client.ClientContactName;
                    
                    if(client.ClientCNPJ == "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@ClientCNPJ", SqlDbType.VarChar)).Value = DBNull.Value;
                    } else
                    {
                        cmd.Parameters.Add(new SqlParameter("@ClientCNPJ", SqlDbType.VarChar)).Value = client.ClientCNPJ;
                    }
                    
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Client created succesfully!" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = e.Message });
            }
        }
    }
}
