using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyDBManager
{
    public static class Constants
    {
        //GENERAL CONSTANTS
        public static string DATABASE_QUERY = "query";
        public static string DATABASE_ERROR = "error";
        public static string DATABASE_MESSAGE = "message";
        public static string DATABASE_COLUMNS = "columns";
        public static string DATABASE_ROW = "row";
        public static string DATABASE_NAME = "name";
        public static string DATABASE_SOURCE = "source";

        //MSSQL CONSTANTS
        public static string MSSQL_CONNECTION_STRING = "Data Source=localhost;Initial Catalog= {0};User Id= {1};Password= {2};";
        //consultas
        public static string MSSQL_SELECT_SYSTABLES = "SELECT * FROM sys.tables";
        public static string MSSQL_SELECT_FUNCTIONS = "SELECT * FROM sys.objects WHERE type_desc LIKE '%FUNCTION%';";
        public static string MSSQL_SELECT_TABLESPACES = "SELECT * FROM sys.databases;";
        public static string MSSQL_SELECT_PROCEDURES = "SELECT * FROM sys.procedures";
        public static string MSSQL_SELECT_SYNONYMS = "SELECT * FROM sys.SYNONYMS";
        public static string MSSQL_SELECT_VIEWS = "SELECT * FROM sys.views";
        public static string MSSQL_SELECT_INDEXES = "SELECT * FROM sys.indexes";
        public static string MSSQL_SELECT_TRIGGERS = "SELECT * FROM sys.triggers";
        public static string MSSQL_SELECT_TABLES = "SELECT * FROM sys.tables";
        public static string MSSQL_SELECT_ALL_FROM = "SELECT * FROM {0};";
        public static string MSSQL_SELECT_SCHEMA_FKS = "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS S WHERE S.CONSTRAINT_NAME LIKE '%FK_%' AND S.TABLE_NAME = '{0}' AND S.COLUMN_NAME = '{1}';";
        public static string MSSQL_SELECT_SCHEMA_PKS = "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS S WHERE S.CONSTRAINT_NAME LIKE '%PK_%' AND S.TABLE_NAME = '{0}' AND S.COLUMN_NAME = '{1}';";
        public static string MSSQL_SELECT_SCHEMA_TYPES = "SELECT '  ['+column_name+'] ' + data_type + COALESCE('('+CAST(character_maximum_length as varchar)+')','') + ' ' +" +
            "CASE WHEN EXISTS ( SELECT id FROM syscolumns WHERE object_name(id)='{0}' AND name=column_name AND " +
            "columnproperty(id,name,'IsIdentity') = 1 ) THEN 'IDENTITY(' + CAST(ident_seed('{0}') as varchar) + ',' +" +
            " CAST(ident_incr('{0}') as varchar) + ')' ELSE '' END  + ' ' + ( CASE WHEN IS_NULLABLE = 'No' THEN 'NOT ' else '' END ) +" +
            " 'NULL ' + COALESCE('DEFAULT '+COLUMN_DEFAULT,'') + ',' AS col FROM information_schema.columns WHERE table_name = '{0}' ORDER BY ordinal_position;";
        public static string MSSQL_SELECT_SCHEMA_CONSTRAINTS = "SELECT 'CONSTRAINT ['+ c1.CONSTRAINT_NAME +'] FOREING KEY (['+c1.COLUMN_NAME+']) REFERENCES ['+ c2.TABLE_SCHEMA+'].['+c2.TABLE_NAME+']" +
            " (['+c2.COLUMN_NAME+'])' AS Cons FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS c1,INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS c2, INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS r WHERE" +
            " c1.CONSTRAINT_NAME = r.CONSTRAINT_NAME AND r.UNIQUE_CONSTRAINT_NAME = c2.CONSTRAINT_NAME AND c1.COLUMN_NAME = '{0}';";
        public static string MSSQL_SELECT_VIEW_DDL = "SELECT m.definition FROM sys.sql_modules AS m, sys.objects AS o WHERE o.object_id = m.object_id AND o.name = '{0}';";
        public static string MSSQL_SHOW_PLAN_ON = "SET SHOWPLAN_TEXT ON";
        public static string MSSQL_SHOW_PLAN_OFF = "SET SHOWPLAN_TEXT OFF";
        public static string MSSQL_SELECT_DB_FILE = "SELECT  f.physical_name AS URL FROM sys.database_files f WHERE  f.name = '{0}'";
        public static string MSSQL_SCHEMA_DATA = "EXEC sp_spaceused null, false";
        public static string MSSQL_SELECT_SESSION_INFO = "SELECT * FROM sys.dm_exec_sessions;";

        //ORACLE CONSTANTS
        public static string ORACLE_CONNECTION_STRING = "Data Source=localhost:1521/{0};User ID= {1};Password= {2};";
        public static string ORACLE_DDL = "ddl";
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