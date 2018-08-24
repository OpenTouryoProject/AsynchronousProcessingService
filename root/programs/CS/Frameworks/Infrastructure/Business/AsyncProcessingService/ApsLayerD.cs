//**********************************************************************************
//* Copyright (C) 2007,2016 Hitachi Solutions,Ltd.
//**********************************************************************************

#region Apache License
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

//**********************************************************************************
//* クラス名        ：LayerD
//* クラス日本語名  ：LayerD
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014  Supragyan         Created LayerD class for AsyncProcessing Service
//*  11/28/2014  Supragyan         Created Insert,Update,Select method for AsyncProcessing Service
//*  04/14/2015  Sandeep           Did code modification of update and select asynchronous task 
//*  04/14/2015  Sandeep           Did code implementation of SetSqlByFile3 to access the SQL from embedded resource
//*  05/28/2015  Sandeep           Did code implementation to update Exception information to the database
//*  06/09/2015  Sandeep           Implemented code to update stop command to all the running asynchronous task
//*                                Modified code to reset Exception information, before starting asynchronous task 
//*  06/26/2015  Sandeep           Removed the where condition command <> 'Abort' from the SelectTask asynchronous task,
//*                                to resolve unstable "Register" state, when you invoke [Abort] to AsyncTask, at this "Register" state
//*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
//**********************************************************************************

using System;
using System.Data;

using Touryo.Infrastructure.Business.Dao;
using Touryo.Infrastructure.Framework.AsyncProcessingService;
using Touryo.Infrastructure.Public.Db;

namespace Touryo.Infrastructure.Business.AsyncProcessingService
{
    /// <summary>
    /// LayerD class for AsyncProcessing Service
    /// </summary>
    public class ApsLayerD : MyBaseDao
    {
        /// <summary>AsyncProcessingService用B層</summary>
        /// <param name="dam">dam</param>
        public ApsLayerD(BaseDam dam) : base(dam) { }

        #region Insert

        /// <summary>
        /// Inserts async parameter values to database
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="returnValue"></param>
        public void InsertTask(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "AsyncProcessingServiceInsert.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P2", parameterValue.UserId);
            this.SetParameter("P3", parameterValue.ProcessName);
            this.SetParameter("P4", parameterValue.Data);
            this.SetParameter("P5", parameterValue.RegistrationDateTime);
            this.SetParameter("P6", DBNull.Value);
            this.SetParameter("P7", parameterValue.NumberOfRetries);
            this.SetParameter("P8", DBNull.Value);
            this.SetParameter("P9", parameterValue.StatusId);
            this.SetParameter("P10", parameterValue.ProgressRate);
            this.SetParameter("P11", parameterValue.CommandId);
            this.SetParameter("P12", parameterValue.ReservedArea);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region Update

        #region UpdateTaskStart

        /// <summary>
        ///  Updates information in the database that the asynchronous task is started
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskStart(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskStart.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.ExecutionStartDateTime);
            this.SetParameter("P3", parameterValue.StatusId);
            this.SetParameter("P4", DBNull.Value);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region UpdateTaskRetry

        /// <summary>
        ///  Updates information in the database that the asynchronous task is failed and can be retried later
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskRetry(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskRetry.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.NumberOfRetries);
            this.SetParameter("P3", parameterValue.CompletionDateTime);
            this.SetParameter("P4", parameterValue.StatusId);
            this.SetParameter("P5", parameterValue.ExceptionInfo);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region UpdateTaskFail

        /// <summary>
        ///  Updates information in the database that the asynchronous task is failed and abort this task [status=Abort]
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskFail(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskFail.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.CompletionDateTime);
            this.SetParameter("P3", parameterValue.StatusId);
            this.SetParameter("P4", parameterValue.ExceptionInfo);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region UpdateTaskSuccess

        /// <summary>
        ///  Updates information in the database that the asynchronous task is completed
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskSuccess(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskSuccess.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.CompletionDateTime);
            this.SetParameter("P3", parameterValue.ProgressRate);
            this.SetParameter("P4", parameterValue.StatusId);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region UpdateTaskProgress

        /// <summary>
        ///  Updates progress rate of the asynchronous task in the database.
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskProgress(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskProgress.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.ProgressRate);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region UpdateTaskCommand

        /// <summary>
        ///  Updates command value information of a selected asynchronous task
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void UpdateTaskCommand(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "UpdateTaskCommand.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);
            this.SetParameter("P2", parameterValue.CommandId);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #region StopAllTask

        /// <summary>
        ///  Set stop command for all running asynchronous task.
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void StopAllTask(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "StopAllTask.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.StatusId);
            this.SetParameter("P2", parameterValue.CommandId);

            // Execute SQL query
            returnValue.Obj = this.ExecInsUpDel_NonQuery();
        }

        #endregion

        #endregion

        #region Select

        #region SelectCommand

        /// <summary>
        ///  Selects user command from database
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        /// <param name="returnValue">Asynchronous Return Values</param>
        public void SelectCommand(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "SelectCommand.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.TaskId);

            // Execute SQL query
            returnValue.Obj = this.ExecSelectScalar();
        }

        #endregion

        #region SelectTask

        /// <summary>
        ///  To get Asynchronous Task from the database
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="returnValue"></param>
        public void SelectTask(ApsParameterValue parameterValue, ApsReturnValue returnValue)
        {
            string filename = string.Empty;
            filename = "SelectTask.sql";

            // Get SQL query from file.
            this.SetSqlByFile2(filename);

            // Set SQL parameter values
            this.SetParameter("P1", parameterValue.RegistrationDateTime);
            this.SetParameter("P2", parameterValue.NumberOfRetries);
            this.SetParameter("P3", (int)AsyncStatus.Register);
            this.SetParameter("P4", (int)AsyncStatus.AbnormalEnd);
            this.SetParameter("P7", parameterValue.CompletionDateTime);

            DataTable dt = new DataTable();

            // Get Asynchronous Task from the database
            this.ExecSelectFill_DT(dt);
            returnValue.Obj = dt;
        }

        #endregion

        #endregion        
    }
}
