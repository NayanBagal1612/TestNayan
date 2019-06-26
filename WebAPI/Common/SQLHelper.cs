
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Common;

namespace TestWebApplication.WebApi.Common
{
    public class SQLHelper
    {
        string connectionString = string.Empty;

        SqlConnection sqlConnection;

        SqlCommand sqlCommand;

        SqlDataAdapter sqlDataAdapter;

        public Dictionary<string, object> parameters { get; set; }

        public SQLHelper()
        {
            try
            {
                parameters = new Dictionary<string, object>();
                TextLog.WriteLog("SQLHelper");
                connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.ToString();
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
               TextLog.WriteLog(ex.Message);
            }
        }


        #region LoadListData

        public List<T> LoadListData<T>(object request, string Sp, string actionName)
        {
           // TextLog.StartWriteLog("LoadListData", "", "");
            List<T> list = (List<T>)Activator.CreateInstance(typeof(List<T>));
            DataSet ds = new DataSet();
            string output = string.Empty;

            try
            {
                sqlCommand = new SqlCommand()
                {
                    Connection = sqlConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = Sp
                };

                SetInputParameters(actionName, request);

                foreach (var item in parameters)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(item.Key, item.Value));
                }

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(ds);


                foreach (DataTable table in ds.Tables)
                {
                    list = ConvertListDataTable<T>(table);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {

            }

            return list;
        }

        #endregion

        #region LoadModelData

        public T LoadModelData<T>(object request, string Sp, string actionName)
        {
            T list = (T)Activator.CreateInstance(typeof(T));
            DataSet ds = new DataSet();
            string output = string.Empty;

            try
            {
                sqlCommand = new SqlCommand()
                {
                    Connection = sqlConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = Sp
                };

                SetInputParameters(actionName, request);

                foreach (var item in parameters)
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                }

                sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    list = ConvertModelDataTable<T>(table);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {

            }

            return list;
        }

        #endregion

        private static T ConvertModelDataTable<T>(DataTable dt)
        {
            T data = (T)Activator.CreateInstance(typeof(T));
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    data = GetItem<T>(row);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return data;
        }

        private static List<T> ConvertListDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            dynamic value;
            try
            {
                List<DataColumn> col = new List<DataColumn>();
                List<PropertyInfo> prop = new List<PropertyInfo>();

                int index = 0;

                foreach (DataColumn column in dr.Table.Columns)
                {
                    col.Add(column);
                }

                foreach (PropertyInfo column in temp.GetProperties())
                {
                    prop.Add(column);
                }

                for (int i = 0; i < prop.Count; i++)
                {
                    PropertyInfo pro = prop[i];

                    for (int j = index; j < col.Count;)
                    {
                        index++;
                        var val1 = temp.GetProperties().Where(c => c.Name.ToLower() == col[j].ColumnName.ToLower()).FirstOrDefault();

                        value = dr[col[j].ColumnName];
                        Type type = val1.PropertyType;

                        if (type == typeof(int))
                        {
                            pro.SetValue(obj, ConvertToInt(dr[col[j].ColumnName]));
                            break;
                        }
                        else if (type == typeof(double))
                        {
                            pro.SetValue(obj, ConvertToDecimal(dr[col[j].ColumnName]));
                            break;
                        }
                        else if (type == typeof(DateTime))
                        {
                            pro.SetValue(obj, Convert.ToDateTime(dr[col[j].ColumnName]));
                            break;
                        }
                        else if (type == typeof(long))
                        {
                            pro.SetValue(obj, ConvertToLong(dr[col[j].ColumnName]));
                            break;
                        }
                        else
                        {
                            pro.SetValue(obj, ConvertToString(dr[col[j].ColumnName]));
                            break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return obj;
        }

        private static string ConvertToString(object value)
        {
            return Convert.ToString(HelperFunctions.ReturnEmptyIfNull(value));
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static DateTime ConvertToDateTime(object date)
        {
            return Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(date));
        }


        private void SetInputParameters(string actionName, object request)
        {
            try
            {

                switch (actionName)
                {
                    //case "ADD_STUDENT":

                    //    GetStudentRequest studentRequest = (GetStudentRequest)request;
                    //    parameters.Add("ID", studentRequest.ID);
                    //    parameters.Add("NAME", studentRequest.NAME);
                    //    parameters.Add("AGE", studentRequest.AGE);
                    //    parameters.Add("ADDRESS", studentRequest.ADDRESS);
                    //    break;

                    //case "DELETE_STUDENT":
                    //    GetStudentRequest deleteStudentRequest = (GetStudentRequest)request;
                    //    parameters.Add("ID", deleteStudentRequest.ID);
                    //    break;

                    //case "ValidateLogin":
                    //    LoginRequest loginRequest = (LoginRequest)request;
                    //    parameters.Add("USERNAME", loginRequest.USERNAME);
                    //    parameters.Add("PASSWORD", loginRequest.PASSWORD);
                    //    break;


                    default:

                        break;
                }

            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }



    }
}