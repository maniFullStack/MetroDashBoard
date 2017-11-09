====== Adding to a project ======
To use this class library in a website project:
1. Copy the compiled version to a folder in your web project. Typically, we use the folder ~/_DLL.
2. Add a project reference to the dll.
3. In the page you want to use it on, add the namespace WebsiteUtilities like so: using WebsiteUtilities;
4. That's it! Start using the classes.

Note: When publishing live, make sure to build a Release DLL and use that instead of the debug DLL. Also, don't publish the .pdb files.
Note 2: Be sure to check the .dll file into source control so everyone will have access to it.


====== Configuration ======

=== For SQL support ===
To set up SQL database access, you need to update the <connectionstrings> key in your web.config file.
For the default database class (SQLDatabase) to work, add a connection string with the value "DatabaseDefault".
For SQLDatabaseSMI to work, add a connection string with the name "DatabaseSMI".
For SQLDatabaseWeb to work, add a connection string with the name "DatabaseWeb".
For SQLDatabaseDebug to work, add a connection string with the name "DatabaseDebug".

=== For Excel Support ===
You need to add a connection string for it with the name "Excel" in web.config. Example keys follow.
Note: The file & path (without extension) will replace the string "{0}" in the final connection string.
Note 2: If there are double quotes in the connection string, use &quot; instead.

Excel Pre-2007 XLS (Default):
<add name="Excel" connectionString="Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}.xls;Extended Properties=&quot;Excel 8.0;HDR=YES;&quot;" providerName="System.Data.OleDb.OleDbConnectionStringBuilder"/>

Excel 2007 XLSX:
<add name="Excel" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}.xlsx;Extended Properties=&quot;Excel 12.0 Xml;HDR=YES;&quot;" providerName="System.Data.OleDb.OleDbConnectionStringBuilder"/>

=== For Error Log support ===
In order to get the error logging working, you need to update the web.config and possibly update a folder.
The error logging has three different options: Log to Database, Log to Event Log and Log to File.
You can use any combination of these three options.

In order to determine which ones to use, it will check your web.config's <appSettings> key for certain values.
To enable File logging, add: <add key="LogToFile" value="true"/>
To enable Database logging, add: <add key="LogToDatabase" value="true"/>
To enable Event Log logging, add: <add key="LogToEventLog" value="true"/>

--- Additional steps for File Logging ---
When logging to a file, naturally you need to specify a logging directory.
By default, the file logger will attempt to save to: ~/Files/Logs/
You can change this folder by adding the following key to <appSettings>: <add key="LogFileDirectory" value="~/New/Path/To/Logs/"/>
	Note: Ensure there is an ending slash on the path.

The second part of this is that you actually have to add that folder and put permissions on it.
You can add it to the project easily but don't forget to set permissions on the live web server so it can write to it.

--- Additional steps for Database Logging ---
To log to the database, of course you need to create the database table.
The database connection will use the "DatabaseDefault" (see "For SQL Support" above) connection string.

By default, logs will get saved to a table called tblSYSAppErrorLogs.
You can change this table name by adding the following key to <appSettings>: <add key="LogTableName" value="tblMyNewLogTable"/>
	Note: Do not add [ or ] to this name. They will be added automatically.

Once you have decided on a name, you'll have to run the following query to create the table.
Note: Be sure to update the table name if you decided to change it.

CREATE TABLE [dbo].[tblSYSAppErrorLogs](
	[EventPK] [int] IDENTITY(276,1) NOT NULL,
	[ApplicationSource] [varchar](100) NOT NULL,
	[EventSource] [int] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Server] [varchar](50) NULL,
	[PageURL] [varchar](250) NULL,
	[EventDate] [datetime] NOT NULL,
	[Notes] [varchar](2000) NULL,
	[LastEditBy] [varchar](50) NULL,
	[LastEditDate] [datetime] NULL,
 CONSTRAINT [PK_tblSYSAppErrorLogs] PRIMARY KEY CLUSTERED 
(
	[EventPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


====== web.config Example (With Defaults) ======
<configuration>
  <appSettings>
    <add key="LogToFile" value="false"/>
    <add key="LogToDatabase" value="false"/>
    <add key="LogToEventLog" value="false"/>
    <add key="LogFileDirectory" value="~/Files/Logs/"/>
    <add key="LogTableName" value="tblSYSAppErrorLogs"/>
  </appSettings>
  <connectionStrings>
	<add name="DatabaseDefault" connectionString="Data Source=SQL2;Initial Catalog=MShopCanada;Integrated Security=SSPI" providerName="System.Data.SqlClient"/>
    <add name="DatabaseWeb" connectionString="Data Source=SPOCK;Initial Catalog=web;Integrated Security=SSPI" providerName="System.Data.SqlClient"/>    
  </connectionStrings>
</configuration>