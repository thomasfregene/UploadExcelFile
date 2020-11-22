using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UploadExcelFile.Models
{
    public static class ContactDb
    {
        public static List<ContactVM> GetContactsByBatchId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<ContactVM> contactVM = new List<ContactVM>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("spGetContactByBatches", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramBatchId = new SqlParameter
                {
                    ParameterName = "@BatchID",
                    Value = id
                };
                cmd.Parameters.Add(paramBatchId);

                con.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        contactVM.Add(new ContactVM
                        {
                            FirstName = sdr["FirstName"].ToString(),
                            LastName = sdr["LastName"].ToString(),
                            Email = sdr["Email"].ToString(),
                            Telephone = sdr["Telephone"].ToString(),
                            Mobile = sdr["Mobile"].ToString(),
                            CompanyID = Convert.ToInt32(sdr["CompanyID"])
                        });
                    }
                }
            }
            return contactVM;
        }
        public static void PostToDatabase(List<ContactVM> contacts, int batchId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                foreach (var contact in contacts)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spCreateContact", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter paramFirstName = new SqlParameter
                        {
                            ParameterName = "@FirstName",
                            Value = contact.FirstName
                        };
                        cmd.Parameters.Add(paramFirstName);

                        SqlParameter paramLastName = new SqlParameter
                        {
                            ParameterName = "@LastName",
                            Value = contact.LastName
                        };
                        cmd.Parameters.Add(paramLastName);

                        SqlParameter paramEmail = new SqlParameter
                        {
                            ParameterName = "@Email",
                            Value = contact.Email
                        };
                        cmd.Parameters.Add(paramEmail);

                        SqlParameter paramTelephone = new SqlParameter
                        {
                            ParameterName = "@Telephone",
                            Value = contact.Telephone
                        };
                        cmd.Parameters.Add(paramTelephone);

                        SqlParameter paramMobile = new SqlParameter
                        {
                            ParameterName = "@Mobile",
                            Value = contact.Mobile
                        };
                        cmd.Parameters.Add(paramMobile);

                        SqlParameter paramCompanyID = new SqlParameter
                        {
                            ParameterName = "@CompanyID",
                            Value = contact.CompanyID
                        };
                        cmd.Parameters.Add(paramCompanyID);

                        SqlParameter paramBatchID = new SqlParameter
                        {
                            ParameterName = "@BatchID",
                            Value = batchId
                        };
                        cmd.Parameters.Add(paramBatchID);


                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                }
               
                
                
            }
            
        }
    }
}