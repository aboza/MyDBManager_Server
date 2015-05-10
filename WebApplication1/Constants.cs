using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyDBManager
{
    public static class Constants
    {
        //MSSQL CONSTANTS

        //ORACLE CONSTANTS
        public static string ORACLE_CONNECTION_STRING = "Data Source=localhost:1521/{0};User ID= {1};Password= {2};";
        public static string ORACLE_QUERY = "query";
        public static string ORACLE_SOURCE = "source";
        public static string ORACLE_DDL = "ddl";
        public static string ORACLE_ERROR = "error";
        public static string ORACLE_MESSAGE = "message";
        public static string ORACLE_COLUMNS = "columns";
        public static string ORACLE_ROW = "row";
        public static string ORACLE_NAME = "name";
        public static string ORACLE_DBA_PRIVILEGE_SYSDBA = "DBA Privilege=SYSDBA;";
        public static string ORACLE_SYS_USER = "SYS";
        public static string ORACLE_SYSTEM_USER = "SYSTEM";
        //consultas
        public static string ORACLE_SELECT_DUAL = "SELECT * FROM DUAL";
        public static string ORACLE_EXPLAIN_PLAN = "EXPLAIN PLAN FOR {0}";
        public static string ORACLE_SELECT_EXPLAIN_PLAN_INFO = "SELECT id, operation, cardinality, bytes, cost, time, object_owner" +
            "from plan_table WHERE plan_id = ( SELECT MAX(plan_id) FROM plan_table )";
        public static string ORACLE_USER_TABLES = "SELECT * FROM USER_TABLES";
        public static string ORACLE_SELECT_METADATA = "SELECT dbms_metadata.get_ddl({0},{1},{2}) FROM dual";
        public static string ORACLE_SELECT_FUNCTIONS = "SELECT * FROM user_objects WHERE object_type = 'FUNCTION'";
        public static string ORACLE_SELECT_PROCEDURES = "SELECT * FROM USER_PROCEDURES";
        public static string ORACLE_SELECT_SYNONYMS = "SELECT * FROM USER_SYNONYMS";
        public static string ORACLE_SELECT_VIEWS = "SELECT * FROM USER_VIEWS";
        public static string ORACLE_SELECT_INDEXES = "SELECT * FROM USER_INDEXES";
        public static string ORACLE_SELECT_TRIGGERS = "SELECT * FROM USER_TRIGGERS";
        public static string ORACLE_SELECT_TABLESPACES = "select * from USER_TABLESPACES";
        public static string ORACLE_SELECT_PACKAGES = "SELECT * FROM user_objects WHERE object_type = 'PACKAGE'";
        public static string ORACLE_SELECT_SESSION_INFO = "SELECT sid, serial#, status, username, terminal, command, schemaname FROM v$session";
    }
}