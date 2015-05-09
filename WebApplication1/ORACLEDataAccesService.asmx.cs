using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

namespace MyDBManager
{
    /// <summary>
    /// Summary description for ORACLEDataAccesService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ORACLEDataAccesService : System.Web.Services.WebService
    {
        [WebMethod]
        public XmlDocument execCommand(string user, string database, string password, string command)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();

            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        public XmlDocument execPlan(string user, string database, string password, string command)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleCommand runPlan;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand("explain plan for " + command, connection);
                myCommand.ExecuteNonQuery();

                runPlan = new OracleCommand("select id, operation, cardinality, bytes, cost, time, object_owner from plan_table where plan_id = ( select max(plan_id) from plan_table )", connection);
                myReader = runPlan.ExecuteReader();

            }
            catch (OracleException ex)
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
                            default:
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetValue(i).ToString();
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (OracleException ex)
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
        public XmlDocument getMetadata(string user, string database, string password, string type, string tablename)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand("select dbms_metadata.get_ddl('" + type + "','" + tablename + "','" + user + "') from dual", connection);
                myReader = myCommand.ExecuteReader();

            }
            catch (OracleException ex)
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
                        XmlNode grid = xmlDoc.CreateElement("ddl");
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
                            default:
                                if (myReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + myReader.GetValue(i).ToString();
                                break;
                        }
                        grid.InnerText = field;
                        row.AppendChild(grid);
                        rootNode.AppendChild(row);
                    }
                }

            }
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from user_objects where object_type = 'FUNCTION'";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_PROCEDURES";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_SYNONYMS";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_VIEWS";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_INDEXES";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_TRIGGERS";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_TABLES";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        public XmlDocument getTablespaces(string user, string database, string password)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from USER_TABLESPACES";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        public XmlDocument getPackages(string user, string database, string password)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from user_objects where object_type = 'PACKAGE'";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        public XmlDocument getInfoSesion(string user, string database, string password)
        {
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select sid, serial#, status, username, terminal, command, schemaname from v$session";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            string connectionString = "Data Source=" + database + ";User Id=" + user + ";Password=" + password + ";";
            string command = "select * from dual";

            OracleConnection connection = new OracleConnection(connectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(command, connection);
                myReader = myCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
