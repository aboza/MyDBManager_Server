using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Xml;
using Oracle.DataAccess.Client;

namespace MyDBManager
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MSSQLDataAccesService : System.Web.Services.WebService
    {

        [WebMethod]
        public XmlDocument execCommand(string user, string database, string password, string command)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getFunctions(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.objects WHERE type_desc LIKE '%FUNCTION%';";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTableSpaces(string user, string database, string password)
        {
            string command = "select * from sys.databases;";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getProcedures(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.procedures";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getSynonyms(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.SYNONYMS";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getViews(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.views";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getIndexes(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.indexes";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTriggers(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.triggers";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTables(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.tables";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public string getTableDDL(string user, string database, string password, string tablename)
        {
            List<string> Columnas = new List<string>();
            List<string> FKS = new List<string>();
            List<string> PKS = new List<string>();
            List<string> Types = new List<string>();
            string command = "select * from " + tablename + ";";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string response = "CREATE TABLE " + tablename + " ( ";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand myCommand;
            SqlDataReader myReader;
            connection.Open();
            myCommand = new SqlCommand(command, connection);
            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
            {
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    Columnas.Add(myReader.GetName(i).ToString());
                }
                connection.Close();
                for (int i = 0; i < Columnas.Count; i++)
                {
                    connection.Open();
                    myCommand = new SqlCommand("select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as s where s.CONSTRAINT_NAME like '%FK_%' and s.TABLE_NAME = '" + tablename + "' and s.COLUMN_NAME = '" + Columnas[i] + "';", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader["COLUMN_NAME"].ToString() != "")
                        {
                            FKS.Add(myReader["COLUMN_NAME"].ToString());
                        }
                    }
                    connection.Close();
                }
                for (int i = 0; i < Columnas.Count; i++)
                {
                    connection.Open();
                    myCommand = new SqlCommand("select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as s where s.CONSTRAINT_NAME like '%PK_%' and s.TABLE_NAME = '" + tablename + "' and s.COLUMN_NAME = '" + Columnas[i] + "';", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader["COLUMN_NAME"].ToString() != "")
                        {
                            PKS.Add(myReader["COLUMN_NAME"].ToString());
                        }
                    }
                    connection.Close();
                }
                connection.Open();
                myCommand = new SqlCommand("select '  ['+column_name+'] ' + data_type + coalesce('('+cast(character_maximum_length as varchar)+')','') + ' ' + case when exists ( select id from syscolumns where object_name(id)='" + tablename + "' and name=column_name and columnproperty(id,name,'IsIdentity') = 1 ) then 'IDENTITY(' + cast(ident_seed('" + tablename + "') as varchar) + ',' + cast(ident_incr('" + tablename + "') as varchar) + ')' else '' end  + ' ' + ( case when IS_NULLABLE = 'No' then 'NOT ' else '' end ) + 'NULL ' + coalesce('DEFAULT '+COLUMN_DEFAULT,'') + ',' as col from information_schema.columns where table_name = '" + tablename + "' order by ordinal_position;", connection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["col"].ToString() != "")
                    {
                        Types.Add(myReader["col"].ToString());
                    }
                }
                connection.Close();
                for (int i = 0; i < Types.Count; i++)
                {
                    response += Types[i].ToString() + " ";
                }
                for (int i = 0; i < PKS.Count; i++)
                {
                    response += "PRIMARY KEY (" + PKS[i].ToString() + ") ";
                }
                for (int i = 0; i < FKS.Count; i++)
                {
                    connection.Open();
                    myCommand = new SqlCommand("select 'CONSTRAINT ['+ c1.CONSTRAINT_NAME +'] FOREING KEY (['+c1.COLUMN_NAME+']) REFERENCES ['+ c2.TABLE_SCHEMA+'].['+c2.TABLE_NAME+'] (['+c2.COLUMN_NAME+'])' as Cons from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as c1,INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE as c2, INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as r where c1.CONSTRAINT_NAME = r.CONSTRAINT_NAME and r.UNIQUE_CONSTRAINT_NAME = c2.CONSTRAINT_NAME and c1.COLUMN_NAME = '" + FKS[i].ToString() + "';", connection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader["Cons"].ToString() != "")
                        {
                            response += (myReader["Cons"].ToString()) + "";
                        }
                    }
                    connection.Close();
                }
            }
            return response;

        }
        [WebMethod]

        public string getViewDDL(string user, string database, string password, string viewName)
        {
            string command = "select m.definition from sys.sql_modules as m, sys.objects as o where o.object_id = m.object_id and o.name = '" + viewName + "';";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string response = "";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand myCommand;
            SqlDataReader myReader;
            connection.Open();
            myCommand = new SqlCommand(command, connection);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                response += myReader["definition"].ToString();
            }
            return response;

        }
        [WebMethod]
        public string getExecPlan(string user, string database, string password, string script)
        {
            string command = "set SHOWPLAN_TEXT on";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string response = "";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand myCommand;
            SqlDataReader myReader;
            connection.Open();
            myCommand = new SqlCommand(command, connection);
            myCommand.ExecuteNonQuery();
            myCommand = new SqlCommand("" + script + ";", connection);
            myReader = myCommand.ExecuteReader();
            myReader.NextResult();
            while (myReader.Read())
            {
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    response += myReader[i].ToString();
                }
            }
            connection.Close();
            connection.Open();
            myCommand = new SqlCommand("set SHOWPLAN_TEXT off", connection);
            myCommand.ExecuteNonQuery();
            connection.Close();

            return response;
        }

        [WebMethod]
        public string getDBFile(string user, string database, string password, string DBname)
        {
            string command = "select  f.physical_name as URL from sys.database_files f where  f.name = '" + DBname + "'";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string response = "";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand myCommand;
            SqlDataReader myReader;
            connection.Open();
            myCommand = new SqlCommand(command, connection);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                response = myReader["URL"].ToString();
            }
            connection.Close();
            return response;
        }
        [WebMethod]
        public List<string> getSchemaData(string user, string database, string password)
        {
            string command = "EXEC sp_spaceused null, false";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand myCommand;
            SqlDataReader myReader;
            connection.Open();
            myCommand = new SqlCommand(command, connection);
            myReader = myCommand.ExecuteReader();
            List<string> response = new List<string>();
            string fila1 = "";
            string fila2 = "";
            while (myReader.Read())
            {
                fila1 += myReader["database_name"] + " ";
                fila1 += myReader["database_size"] + " ";
                fila1 += myReader[2] + " ";
            }
            myReader.NextResult();
            while (myReader.Read())
            {
                fila2 += myReader["reserved"] + " ";
                fila2 += myReader["data"] + " ";
                fila2 += myReader["index_size"] + " ";
                fila2 += myReader["unused"] + " ";
            }
            response.Add(fila1);
            response.Add(fila2);
            return response;

        }

        [WebMethod]
        public XmlDocument getInfoSesion(string user, string database, string password)
        {
            string command = "select * from sys.dm_exec_sessions;";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (myReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement("columns");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement("name");
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement("row");
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(myReader.GetName(i));
                        string field = "";
                        string nameType = myReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetInt64(i);
                                break;
                            case "String":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = myReader.GetString(i);
                                break;
                            case "Boolean":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetChar(i);
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (SqlException ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement("error");

                XmlNode source = xmlDoc.CreateElement("source");
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement("message");
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                myReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public bool isLogin(string user, string database, string password)
        {
            string command = "SELECT * FROM sys.tables";
            string connectionString = "Data Source=localhost;Initial Catalog=" + database + ";User Id=" + user + ";Password=" + password + ";";
           


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            SqlCommand myCommand;
            SqlDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new SqlCommand(command, connection);
                myReader = myCommand.ExecuteReader();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
    }
}
