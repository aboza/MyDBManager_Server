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

        string vConnectionString;
        string vCommandString;
        //WebMethods

        [WebMethod]
        public XmlDocument getUsers (string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_USERS, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument execCommand(string aUser, string aDataBase, string aPassword, string aCommand)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(aCommand, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();

            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getFunctions(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_FUNCTIONS, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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

                XmlNode source = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ERROR);
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTableSpaces(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_TABLESPACES, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getProcedures(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_PROCEDURES, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getSynonyms(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_SYNONYMS, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getViews(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                connection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_VIEWS, connection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                connection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getIndexes(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_INDEXES, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTriggers(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_TRIGGERS, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }
        [WebMethod]
        public XmlDocument getTables(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_TABLES, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public string getTableDDL(string aUser, string aDataBase, string aPassword, string aTablename)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);
            List<string> vTableColumns = new List<string>();
            List<string> vTableFKs = new List<string>();
            List<string> vTablePKs = new List<string>();
            List<string> vTableTypes = new List<string>();

            string vDDLResult = "CREATE TABLE " + aTablename + " ( ";
            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_ALL_FROM, aTablename), vMSSQLConnection);
            vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            if (vMSSQLDataReader.HasRows)
            {
                for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                {
                    vTableColumns.Add(vMSSQLDataReader.GetName(i).ToString());
                }
                vMSSQLConnection.Close();
                for (int vColumnIndex = 0; vColumnIndex < vTableColumns.Count; vColumnIndex++)
                {
                    vMSSQLConnection.Open();
                    vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_SCHEMA_FKS, aTablename, vTableColumns[vColumnIndex]), vMSSQLConnection);
                    vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
                    while (vMSSQLDataReader.Read())
                    {
                        if (vMSSQLDataReader["COLUMN_NAME"].ToString() != "")
                        {
                            vTableFKs.Add(vMSSQLDataReader["COLUMN_NAME"].ToString());
                        }
                    }
                    vMSSQLConnection.Close();
                }
                for (int vColumnIndex = 0; vColumnIndex < vTableColumns.Count; vColumnIndex++)
                {
                    vMSSQLConnection.Open();
                    vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_SCHEMA_PKS, aTablename, vTableColumns[vColumnIndex]), vMSSQLConnection);
                    vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
                    while (vMSSQLDataReader.Read())
                    {
                        if (vMSSQLDataReader["COLUMN_NAME"].ToString() != "")
                        {
                            vTablePKs.Add(vMSSQLDataReader["COLUMN_NAME"].ToString());
                        }
                    }
                    vMSSQLConnection.Close();
                }
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_SCHEMA_TYPES, aTablename), vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
                while (vMSSQLDataReader.Read())
                {
                    if (vMSSQLDataReader["col"].ToString() != "")
                    {
                        vTableTypes.Add(vMSSQLDataReader["col"].ToString());
                    }
                }
                vMSSQLConnection.Close();
                for (int i = 0; i < vTableTypes.Count; i++)
                {
                    vDDLResult += vTableTypes[i].ToString() + " ";
                }
                for (int i = 0; i < vTablePKs.Count; i++)
                {
                    vDDLResult += "PRIMARY KEY (" + vTablePKs[i].ToString() + ") ";
                }
                for (int i = 0; i < vTableFKs.Count; i++)
                {
                    vMSSQLConnection.Open();
                    vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_SCHEMA_CONSTRAINTS, vTableFKs[i].ToString()), vMSSQLConnection);
                    vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
                    while (vMSSQLDataReader.Read())
                    {
                        if (vMSSQLDataReader["Cons"].ToString() != "")
                        {
                            vDDLResult += (vMSSQLDataReader["Cons"].ToString()) + "";
                        }
                    }
                    vMSSQLConnection.Close();
                }
            }
            return vDDLResult;

        }
        [WebMethod]

        public string getViewDDL(string aUser, string aDataBase, string aPassword, string aViewName)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            string vDDLResult = "";
            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_VIEW_DDL, aViewName), vMSSQLConnection);
            vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            while (vMSSQLDataReader.Read())
            {
                vDDLResult += vMSSQLDataReader["definition"].ToString();
            }
            return vDDLResult;

        }
        [WebMethod]
        public string getExecPlan(string aUser, string aDataBase, string aPassword, string aCommand)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            string vDDLResponse = "";
            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SHOW_PLAN_ON, vMSSQLConnection);
            vMSSQLCommand.ExecuteNonQuery();
            vMSSQLCommand = new SqlCommand("" + aCommand + ";", vMSSQLConnection);
            vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            vMSSQLDataReader.NextResult();
            while (vMSSQLDataReader.Read())
            {
                for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                {
                    vDDLResponse += vMSSQLDataReader[i].ToString();
                }
            }
            vMSSQLConnection.Close();
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SHOW_PLAN_OFF, vMSSQLConnection);
            vMSSQLCommand.ExecuteNonQuery();
            vMSSQLConnection.Close();

            return vDDLResponse;
        }

        [WebMethod]
        public string getDBFile(string aUser, string aDataBase, string aPassword, string aDBName)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            string vDBFile = "";
            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(string.Format(MyDBManager.Constants.MSSQL_SELECT_DB_FILE, aDBName), vMSSQLConnection);
            vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            while (vMSSQLDataReader.Read())
            {
                vDBFile = vMSSQLDataReader["URL"].ToString();
            }
            vMSSQLConnection.Close();
            return vDBFile;
        }
        [WebMethod]
        public List<string> getSchemaData(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);

            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;
            vMSSQLConnection.Open();
            vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SCHEMA_DATA, vMSSQLConnection);
            vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            List<string> vSchemaData = new List<string>();
            string vStringRow = "";
            string vStringRow2 = "";
            while (vMSSQLDataReader.Read())
            {
                vStringRow += vMSSQLDataReader["database_name"] + " ";
                vStringRow += vMSSQLDataReader["database_size"] + " ";
                vStringRow += vMSSQLDataReader[2] + " ";
            }
            vMSSQLDataReader.NextResult();
            while (vMSSQLDataReader.Read())
            {
                vStringRow2 += vMSSQLDataReader["reserved"] + " ";
                vStringRow2 += vMSSQLDataReader["data"] + " ";
                vStringRow2 += vMSSQLDataReader["index_size"] + " ";
                vStringRow2 += vMSSQLDataReader["unused"] + " ";
            }
            vSchemaData.Add(vStringRow);
            vSchemaData.Add(vStringRow2);
            return vSchemaData;

        }

        [WebMethod]
        public XmlDocument getInfoSesion(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);
            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_SESSION_INFO, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
            }
            catch (SqlException ex)
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
                if (vMSSQLDataReader.HasRows)
                {
                    XmlNode colNames = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_COLUMNS);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode colName = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_NAME);
                        colName.InnerText = vMSSQLDataReader.GetName(i);
                        colNames.AppendChild(colName);
                    }
                    rootNode.AppendChild(colNames);
                }
                while (vMSSQLDataReader.Read())
                {
                    XmlNode row = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_ROW);
                    for (int i = 0; i < vMSSQLDataReader.FieldCount; i++)
                    {
                        XmlNode grid = xmlDoc.CreateElement(vMSSQLDataReader.GetName(i));
                        string field = "";
                        string nameType = vMSSQLDataReader.GetFieldType(i).Name;
                        switch (nameType)
                        {
                            case "Int16":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt16(i);
                                break;
                            case "Int32":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt32(i);
                                break;
                            case "Int64":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetInt64(i);
                                break;
                            case "String":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = vMSSQLDataReader.GetString(i);
                                break;
                            case "Boolean":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetBoolean(i);
                                break;
                            case "Byte":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetByte(i);
                                break;
                            case "DateTime":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetDateTime(i);
                                break;
                            case "Char":
                                if (vMSSQLDataReader.IsDBNull(i))
                                    field = "NULL";
                                else
                                    field = "" + vMSSQLDataReader.GetChar(i);
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
                vMSSQLDataReader.Close();
                vMSSQLConnection.Close();
            }
            return xmlDoc;
        }

        [WebMethod]
        public bool isLogin(string aUser, string aDataBase, string aPassword)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);


            SqlConnection vMSSQLConnection = new SqlConnection();
            vMSSQLConnection.ConnectionString = vConnectionString;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(MyDBManager.Constants.DATABASE_QUERY);
            xmlDoc.AppendChild(rootNode);
            SqlCommand vMSSQLCommand;
            SqlDataReader vMSSQLDataReader;

            try
            {
                vMSSQLConnection.Open();
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MSSQL_SELECT_SYSTABLES, vMSSQLConnection);
                vMSSQLDataReader = vMSSQLCommand.ExecuteReader();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //CLASSMETHODS
        public void prepareMSSQLConnectionString(string aUser, string aPassword, string aDataBase)
        {
            vConnectionString = String.Format(MyDBManager.Constants.MSSQL_CONNECTION_STRING, aDataBase, aUser, aPassword);
        }
    }
}
