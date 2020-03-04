using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KPIT_K_Foundation.Library
{
    public class WorkflowHelperClass
    {
        private string _stWorkflowName;
        private int _inNumberOfWorkflow;
        private int _inNumberOfSteps;
        private int _inEscalationTime;
        private string _table;
        private string _createdBy;
        private string _modifiedBy;
        private DateTime _createdOn;
        private DateTime _modifiedOn;
        private string _connectionString;
        private bool _status;
        private int _fromStep;
        private int _toStep;

        public WorkflowHelperClass()
        {

        }
        public WorkflowHelperClass(string table, string stWorkflowName, int inNumberOfWorkflows, int inNumberOfSteps, int inEscalationTime, string createdBy, 
            string modifiedBy, DateTime createdOn, DateTime modifiedOn, string connectionString, bool status, int fromStep = 0, int toStep = 0)
        {
            this._table = table;
            this._stWorkflowName = stWorkflowName;
            this._inNumberOfWorkflow = inNumberOfWorkflows;
            this._inNumberOfSteps = inNumberOfSteps;
            this._inEscalationTime = inEscalationTime;
            this._createdBy = createdBy;
            this._createdOn = createdOn;
            this._modifiedBy = modifiedBy;
            this._modifiedOn = modifiedOn;
            this._connectionString = connectionString;
            this._status = status;
            this._fromStep = fromStep;
            this._toStep = toStep;
            this.CreateTable();

        }

        public void CreateTable()
        {
            Dbase dbase = new Dbase();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query =
            @" IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
              WHERE TABLE_NAME='WorkflowMaster') 

              BEGIN

              CREATE TABLE [dbo].[WorkflowMaster](
              [WorkflowId] [int] PRIMARY KEY IDENTITY(1, 1),
              [WorkflowName] [nvarchar](50) NOT NULL,
              [NumberOfSteps] [int] ,
              [EscalationTime] [int],
              [Status] [BIT] ,
              [CreatedBy] [varchar](50) NULL,
              [CreatedOn] [DateTime] NOT NULL,
              [ModifiedBy] [nvarchar](50) NULL,
              [ModifiedOn] [DateTime] NOT NULL);

              END  ";
            query = query + @" 
              IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
              WHERE TABLE_NAME='WorkflowStepsMaster') 

              BEGIN

              CREATE TABLE [dbo].[WorkflowStepsMaster](
              StepId int PRIMARY KEY IDENTITY (1, 1),
              WorKflowId int FOREIGN KEY REFERENCES WorkflowMaster(WorkflowId),
              StepTitle nvarchar(50),
              OnRejectSendTo nvarchar(50),
              WorkflowStepSequence nvarchar(10),
              [Status] bit,
              CreatedOn DateTime NOT NULL,
              CreatedBy nvarchar(50) not null,
              ModifiedOn DateTime  NULL,
              ModifiedBy nvarchar(50) null);

              END  
              ";
            query = query + @" IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
              WHERE TABLE_NAME='UserMaster') 

              BEGIN
              CREATE TABLE [dbo].[UserMaster](
              UserId int PRIMARY KEY IDENTITY (1, 1),
              UserName nvarchar(50),
              [Password] nvarchar(50),
              Email nvarchar(50),
              CreatedOn DateTime NOT NULL,
              CreatedBy nvarchar(50) not null,
              ModifiedOn DateTime  NULL,
              ModifiedBy nvarchar(50) null
              ); 
              INSERT INTO UserMaster(UserName,[Password],Email,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy )
              Values('Sristi Annu','1234','sristia@birlasoft.com',getdate(),'System',null,null),
	                ('Ajay Kedar','Ask123','ajayk5@birlasoft.com',getdate(),'System',null,null),
                    ('Ankit Gupta','1234','ankitg10@birlasoft.com',getdate(),'System',null,null);
              END  ";


            query = query + @" IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
              WHERE TABLE_NAME='RoleMaster') 

              BEGIN
               CREATE TABLE RoleMaster (
               RoleId int PRIMARY KEY IDENTITY (1, 1),
               RoleDescription nvarchar(25),
               CreatedOn DateTime NOT NULL,
               CreatedBy nvarchar(50) not null,
               ModifiedOn DateTime  NULL,
               ModifiedBy nvarchar(50) null
               ); 
               INSERT INTO RoleMaster(RoleDescription,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy )
               VALUES('Administrator',getdate(),'System',null,null),
               	('HO Administrator',getdate(),'System',null,null),
               	('Sys Administrator',getdate(),'System',null,null),
               	('Reviewer',getdate(),'System',null,null),
               	('Developer',getdate(),'System',null,null),
               	('Approver',getdate(),'System',null,null);
              END  ";
            query = query + @" IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
                  WHERE TABLE_NAME='UserRoles') 

              BEGIN
                CREATE TABLE UserRoles(
                 UserRoleId int PRIMARY KEY IDENTITY(1, 1),
                 UserId int FOREIGN KEY REFERENCES UserMaster(UserId),
                 RoleId int FOREIGN KEY REFERENCES RoleMaster(RoleId),
                 [Status] bit
                 );
                
                INSERT INTO UserRoles(UserId, RoleId, Status)
                 VALUES(2, 1, 1),
	                   (3, 1, 1),
                       (1, 2, 1);
              END  ";

            if (_fromStep > 0 && _toStep > 0)
            {
                for (int i = 1; i <= _inNumberOfWorkflow; i++)
                {
                    query = query + @"INSERT INTO[dbo].[WorkflowMaster] ( WorkflowName, NumberOfSteps, EscalationTime, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)" +
                        " VALUES('" + _stWorkflowName + i + "'," + _inNumberOfSteps + "," + _inEscalationTime + "," + (_status ? "1" : "0") + ",'" + _createdBy + "',getDate(),'" + null + "','');" +
                        " Declare @id" + i + " int;" +
                        "Set @id" + i + " = (SELECT SCOPE_IDENTITY()); ";

                    for (int j = _fromStep; _fromStep >= 1; --_fromStep)
                    {
                        string level = string.Empty;
                        for (int k = _toStep; _toStep >= 1; --_toStep)
                        {
                            level = (Convert.ToInt32(_toStep)).ToString();
                            if (level == 0.ToString())
                                level = "''";
                            else
                                level = "'Level-" + (Convert.ToInt32(_toStep)).ToString() + "'";
                            query = query + "INSERT INTO [dbo].[WorkflowStepsMaster] ( WorkflowId, StepTitle, OnRejectSendTo,WorkflowStepSequence, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                                 "VALUES( @id" + i + ",'" + _stWorkflowName + i + " Level-" + j + "'" + "," + level + "," + j + "," + (_status ? "1" : "0") + ",'" + _createdBy + "',getDate(),'" + null + "','');";
                            j = _toStep;
                        }
                        if (level != 0.ToString() && level != "")
                        {
                            string level1 = (Convert.ToInt32(j) - 1).ToString();
                            if (level1 == 0.ToString())
                                level1 = "''";
                            else                                
                                level1 = "'Level-" + (Convert.ToInt32(_toStep)).ToString() + "'";
                            query = query + "INSERT INTO [dbo].[WorkflowStepsMaster] ( WorkflowId, StepTitle, OnRejectSendTo,WorkflowStepSequence, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                                 "VALUES( @id" + i + ",'" + _stWorkflowName + i + " Level-" + j + "'" + "," + level1 + "," + j + "," + (_status ? "1" : "0") + ",'" + _createdBy + "',getDate(),'" + null + "','');";
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= _inNumberOfWorkflow; i++)
                {
                    query = query + @"INSERT INTO[dbo].[WorkflowMaster] ( WorkflowName, NumberOfSteps, EscalationTime, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)" +
                        " VALUES('" + _stWorkflowName + i + "'," + _inNumberOfSteps + "," + _inEscalationTime + "," + (_status ? "1" : "0") + ",'" + _createdBy + "',getDate(),'" + null + "','');" +
                        " Declare @id" + i + " int;" +
                        "Set @id" + i + " = (SELECT SCOPE_IDENTITY()); ";

                    for (int j = _inNumberOfSteps; j >= 1; j--)
                    {
                        string level = (Convert.ToInt32(j) - 1).ToString();
                        if (level == 0.ToString())
                            level = "''";
                        else
                            level = "'Level-" + (Convert.ToInt32(j) - 1).ToString() + "'";
                        query = query + "INSERT INTO [dbo].[WorkflowStepsMaster] ( WorkflowId, StepTitle, OnRejectSendTo,WorkflowStepSequence, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                             "VALUES( @id" + i + ",'" + _stWorkflowName + i + " Level-" + j + "'" + "," + level + "," + j + "," + (_status ? "1" : "0") + ",'" + _createdBy + "',getDate(),'" + null + "','');";
                    }
                }
            }
            query = query.Remove(query.Length - 1, 1) + ";";

            SqlCommand sqlCommand = new SqlCommand(query, connection);
            bool success = Convert.ToBoolean(sqlCommand.ExecuteNonQuery());
            if (success)
            {
                MessageBox.Show("Records saved successfully!");

            }
            else
            {
                MessageBox.Show("Error while inserting records!");
            }
            connection.Close();

        }



    }
}
