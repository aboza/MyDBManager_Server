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

        string vConnectionString;
        string vCommandString;

        //WEBMETHODS
        [WebMethod]
        public XmlDocument execCommand(string aUser, string aDataBaseSID, string aPassword, string aCommand)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);

            OracleConnection connection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(aCommand, connection);
                myReader = myCommand.ExecuteReader();

            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
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
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = myReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (myReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
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
        public XmlDocument execPlan(string aUser, string aDataBaseSID, string aPassword, string aCommand)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);

            OracleConnection connection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand myCommand;
            OracleCommand runPlan;
            OracleDataReader myReader;

            try
            {
                connection.Open();
                myCommand = new OracleCommand(string.Format(MyDBManager.Constants.ORACLE_EXPLAIN_PLAN, aCommand), connection);
                myCommand.ExecuteNonQuery();

                runPlan = new OracleCommand(MyDBManager.Constants.ORACLE_SELECT_EXPLAIN_PLAN_INFO, connection);
                myReader = runPlan.ExecuteReader();

            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
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
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
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
        public XmlDocument getMetadata(string aUser, string aDataBaseSID, string aPassword, string aMetaDataType, string aTableName)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);

            OracleConnection connection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                connection.Open();
                vOracleCommand = new OracleCommand(string.Format(MyDBManager.Constants.ORACLE_SELECT_METADATA,aMetaDataType,aTableName,aUser), connection);
                vOracleDataReader = vOracleCommand.ExecuteReader();

            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(MyDBManager.Constants.ORACLE_DDL);
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
                                break;
                            default:
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetValue(i).ToString();
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getFunctions(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_FUNCTIONS;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getProcedures(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_PROCEDURES;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getSynonyms(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_SYNONYMS;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getViews(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_VIEWS;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getIndexes(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_INDEXES;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getTriggers(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_TRIGGERS;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getTables(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_USER_TABLES;

            OracleConnection connection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                connection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, connection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getTablespaces(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_TABLESPACES;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getPackages(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_PACKAGES;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getInfoSesion(string aUser, string aDataBaseSID, string aPassword)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            string vOracleCommandString = MyDBManager.Constants.ORACLE_SELECT_SESSION_INFO;

            OracleConnection vOracleConnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDataReader;

            try
            {
                vOracleConnection.Open();
                vOracleCommand = new OracleCommand(vOracleCommandString, vOracleConnection);
                vOracleDataReader = vOracleCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }

            try
            {
                if (vOracleDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vOracleDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vOracleDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vOracleDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vOracleDataReader.GetName(i));
                        string field = "";
                        string nameType = vOracleDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vOracleDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vOracleDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vOracleDataReader.GetChar(i);
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
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            catch (Exception ex)
            {
                XmlNode error = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_SOURCE);
                source.InnerText = ex.Source;

                XmlNode message = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_MESSAGE);
                source.InnerText = ex.Message;

                error.AppendChild(source);
                error.AppendChild(message);

                rootNode.AppendChild(error);

                return xmlDoc;
            }
            finally
            {
                vOracleDataReader.Close();
                vOracleConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public bool isLogin(string aUser, string aPassword, string aDataBaseSID)
        {
            prepareOracleConnectionString(aUser, aPassword, aDataBaseSID);
            vCommandString = MyDBManager.Constants.ORACLE_SELECT_DUAL;

            OracleConnection vOracleconnection = new OracleConnection(vConnectionString);

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            OracleCommand vOracleCommand;
            OracleDataReader vOracleDatReader;

            try
            {
                vOracleconnection.Open();
                vOracleCommand = new OracleCommand(vCommandString, vOracleconnection);
                vOracleDatReader = vOracleCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //CLASSMETHODS
        public void prepareOracleConnectionString(string aUser, string aPassword, string aDataBaseSID)
        {

            vConnectionString = String.Format(MyDBManager.Constants.ORACLE_CONNECTION_STRING, aDataBaseSID, aUser, aPassword);
            //SYS AND SYSTEM USER NEED SYSDBA PRIVILIGES
            if ((aUser == MyDBManager.Constants.ORACLE_SYS_USER) || (aUser == MyDBManager.Constants.ORACLE_SYSTEM_USER))
            {
                vConnectionString = string.Concat(vConnectionString, MyDBManager.Constants.ORACLE_DBA_PRIVILEGE_SYSDBA);
            }
        }
    }
}
