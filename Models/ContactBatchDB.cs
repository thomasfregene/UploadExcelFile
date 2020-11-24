using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UploadExcelFile.Models
{
    public class ContactBatchDB
    {
        public static int GetBatchID(ContactBatch contactBatch)
        {
            string conString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            int batchID;
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand("AddBatchReturnIDWithOutput", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramBatchName = new SqlParameter
                    {
                        ParameterName = "@BatchName",
                        Value = contactBatch.BatchName
                    };
                    cmd.Parameters.Add(paramBatchName);

                    SqlParameter paramDateCreated = new SqlParameter
                    {
                        ParameterName = "@DateCreated",
                        Value = contactBatch.DateCreated
                    };
                    cmd.Parameters.Add(paramDateCreated);

                    SqlParameter paramCreatedBy = new SqlParameter
                    {
                        ParameterName = "@CreatedBy",
                        Value = contactBatch.CreatedBy,
                        //SqlDbType = SqlDbType.DateTime,
                    };
                    cmd.Parameters.Add(paramCreatedBy);

                    SqlParameter paramDateModified = new SqlParameter
                    {
                        ParameterName = "@DateModified",
                        Value = contactBatch.DateModified
                    };
                    cmd.Parameters.Add(paramDateModified);

                    SqlParameter paramStatus = new SqlParameter
                    {
                        ParameterName = "@Status",
                        Value = contactBatch.Status
                    };
                    cmd.Parameters.Add(paramStatus);

                    //output param(New Code)
                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@BatchID";
                    outputParam.Direction = ParameterDirection.Output;
                    outputParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(outputParam);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    batchID = Convert.ToInt32(cmd.Parameters["@BatchID"].Value);
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return batchID;
        }

        public static List<ContactBatch> GetAllBatches()
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<ContactBatch> contactBatches = new List<ContactBatch>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllBatches", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        contactBatches.Add(new ContactBatch
                        {
                            BatchID = Convert.ToInt32(sdr["BatchID"]),
                            BatchName = sdr["BatchName"].ToString(),
                            DateCreated = Convert.ToDateTime(sdr["DateCreated"]),
                            CreatedBy = sdr["CreatedBy"].ToString()
                        });
                    }
                }
            }
            return contactBatches;
        }

        //Edit
        public static List<ContactVM> EditContactsByBatchId(int id)
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
                            CompanyID = Convert.ToInt32(sdr["CompanyID"]),
                            BatchID = Convert.ToInt32(sdr["BatchID"])
                        });
                    }
                }
            }
            return contactVM;
        }

        public static List<ContactVM> DeleteContactByBatchId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<ContactVM> contactVM = new List<ContactVM>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
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
                                CompanyID = Convert.ToInt32(sdr["CompanyID"]),
                                BatchID = Convert.ToInt32(sdr["BatchID"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return contactVM;
        }

        public static void DeleteFileByBatchId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spDeleteFileByBatchId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter paramBatchId = new SqlParameter
                    {
                        ParameterName = "@BatchID",
                        Value = id
                    };
                    cmd.Parameters.Add(paramBatchId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

        }

        //Delete Existing records before Updating by Batch Id
        private static void DeleteByBatchId(int batchId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spDeleteBeforeInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramBatchID = new SqlParameter
                    {
                        ParameterName = "@BatchID",
                        Value = batchId
                    };
                    cmd.Parameters.Add(paramBatchID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public static void UpdateContactByBatchId(List<ContactVM> contacts, int batchId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                
                try
                {
                    DeleteByBatchId(batchId);
                    foreach (ContactVM contact in contacts)
                    {

                        SqlCommand cmd = new SqlCommand("spUpdateContactByBatchId", con);
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
                    //cmdDelete.ExecuteNonQuery();
                
                    //transaction.Commit();
                }
                catch (Exception)
                {

                    throw; //transaction.Rollback();
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