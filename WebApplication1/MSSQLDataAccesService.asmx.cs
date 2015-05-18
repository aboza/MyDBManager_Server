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

        /*getUsers: Obtiene la informacion de los usuarios de la instancia de Base de Datos
         * retornando la informacion en forma de un XML
         */
        [WebMethod]

        public XmlDocument getPrivilege(string aUser, string aDataBase, string aPassword)
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
                vMSSQLCommand = new SqlCommand(MyDBManager.Constants.MDSQL_PRIVILEGE, vMSSQLConnection);
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
        public XmlDocument getUsers (string aUser, string aDataBase, string aPassword)
=======
        public XmlDocument getUsers(string aUser, string aDataBase, string aPassword)
>>>>>>> origin/master
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

        /*ExecCommand: Ejecuta un comando o script SQL ya sea DDL o DML retornando el resultado
         * de la consulta por medio de un XML
         */
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

        /*getFunctions: Obtiene las funciones presentes para la instancia de Base de Datos 
         * seleccionada.
         */
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

        /*getTableSpaces: Obtiene la informacion de las bases de datos
         * retornando el resultado en forma de XML
         */
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

        /*getProcedure:Obtiene la informacion de los procedimientos presentes para 
         * la instancia de Base de Datos seleccionada
         */
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

        /*getSynonyms: Obtiene la informacion de los sinonimos presentes para la instancia de Base de Datos
         * seleccionada
         */
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

        /*getViews: Obtiene la informacion de las vistas presentes para la instancia
         * de Base de Datos seleccionada
         */
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

        /*getIndexes: Obtiene la informacion de los indices presentes para la
         * instancia de Base de Datos seleccionada
         */
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

        /*getTriggers: Obtiene la informacion de los triggers presentes para la instancia
         * de Base de Datos seleccionada
         */
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

        /*getTables: Obtiene la informacion de las tablas presentes para
         * la instancia de Base de Datos seleccionada
         */
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

        /*getTableDDL: Mediante 3 consultas creamos un string con la informacion necesaria para
         * generar el DDL para una tabla especifica, el proceso concatena una serie de resultados
         * para finalizar con el DDL resultado.
         */
        [WebMethod]
        public string getTableDDL(string aUser, string aDataBase, string aPassword, string aTablename)
        {
            prepareMSSQLConnectionString(aUser, aPassword, aDataBase);
            List<string> vTableColumns = new List<string>();
            List<string> vTableFKs = new List<string>();
            List<string> vTablePKs = new List<string>();
            List<string> vTableTypes = new List<string>();

            string vDDLResult = "CREATE TABLE " + aTablename + " ( ";//Inicio del DDL
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
                for (int vColumnIndex = 0; vColumnIndex < vTableColumns.Count; vColumnIndex++)//Obtenemos los FKs
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
                for (int vColumnIndex = 0; vColumnIndex < vTableColumns.Count; vColumnIndex++)//Obtenemos los PKs
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
                        vTableTypes.Add(vMSSQLDataReader["col"].ToString());//agregamos las columnas
                    }
                }
                vMSSQLConnection.Close();
                for (int i = 0; i < vTableTypes.Count; i++)
                {
                    vDDLResult += vTableTypes[i].ToString() + " ";
                }
                for (int i = 0; i < vTablePKs.Count; i++)
                {
                    vDDLResult += "PRIMARY KEY (" + vTablePKs[i].ToString() + ") ";//creamos el DDL para el primary Key
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
                            vDDLResult += (vMSSQLDataReader["Cons"].ToString()) + "";//agregamos los constraints
                        }
                    }
                    vMSSQLConnection.Close();
                }
            }
            return vDDLResult;// al final tenemos un string con el DDL completo

        }

        /*getViewDDL: Obtenemos el DDL para una vista presente en la instancia
         * de Base de Datos seleccionada.
         */
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

        /*getExecPlan: MSSQL cuenta con un modo de texto por medio del cual podemos obtener
         * el plan de ejecucion de una consulta, recordemos que ademas de esto, MSSQL cuenta con
         * un modulo bastante interactivo y visual para ver esta informacion.
         */
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

        /*getDBFile: Obtenemos la direccion de los archivos de Base de Datos
         * para una Base de Datos seleccionada (debe estar presente en sys.dataBase_files)
         */
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

        /*getSchemaData: Obtenemos la informacion relevante para un Schema/Base de Datos
         * nombre, tamaño, espacio en disco, etc...
         */
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

        /*getInfoSession: Obtiene la informacion de las sesiones activas en la instancia
         * de Base de Datos seleccionada
         */
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

        /*isLogin: Verifica la autenticidad de un usario para acceder a la instancia
         * de Base de Datos
        */
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
        /*prepareMSSQLConnectionString: prepara el string de conexion para una Base de Datos MSSQL
         */
        public void prepareMSSQLConnectionString(string aUser, string aPassword, string aDataBase)
        {
            vConnectionString = String.Format(MyDBManager.Constants.MSSQL_CONNECTION_STRING, aDataBase, aUser, aPassword);
        }
    }
}
