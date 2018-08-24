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
//* クラス名        ：ApsUtility
//* クラス日本語名  ：ApsUtility
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  2018/08/24  西野 大介         新規作成（Utilityメソッドを部品化）
//**********************************************************************************

using Touryo.Infrastructure.Business.Util;
using Touryo.Infrastructure.Public.Db;

namespace Touryo.Infrastructure.Business.AsyncProcessingService
{
    /// <summary>ApsUtility</summary>
    public class ApsUtility
    {
        /// <summary>Get command information from database. </summary>
        /// <param name="taskID">asynchronous task id</param>
        /// <param name="returnValue">asynchronous return value</param>
        /// <param name="dam">BaseDam</param>
        public static void GetCommandValue(int taskID, ApsReturnValue returnValue, BaseDam dam)
        {
            // Sets parameters of AsyncProcessingServiceParameterValue to get command value.
            ApsParameterValue parameterValue = new ApsParameterValue(
                "AsyncProcessingService", "SelectCommand", "SelectCommand", "SQL",
                new MyUserInfo("AsyncProcessingService", "AsyncProcessingService"));

            parameterValue.TaskId = taskID;

            // Calls data access part of asynchronous processing service.
            ApsLayerD layerD = new ApsLayerD(dam);
            layerD.SelectCommand(parameterValue, returnValue);
            returnValue.CommandId = (int)returnValue.Obj;
        }

        /// <summary>
        ///  Resumes asynchronous process in the middle of the processing.
        /// </summary>
        /// <param name="taskID">Task ID</param>
        /// <param name="returnValue">asynchronous return value</param>
        /// <param name="dam">BaseDam</param>
        public static void ResumeProcessing(int taskID, ApsReturnValue returnValue, BaseDam dam)
        {
            // Reset the command of selected task.
            ApsUtility.UpdateTaskCommand(taskID, 0, returnValue, dam);
        }

        /// <summary>
        ///  Updates the progress rate in the database. 
        /// </summary>
        /// <param name="taskID">asynchronous task id</param>
        /// <param name="returnValue">ApsReturnValue</param>
        /// <param name="progressRate">progress rate</param>
        /// <param name="dam">BaseDam</param>
        public static void UpdateProgressRate(int taskID, ApsReturnValue returnValue, decimal progressRate, BaseDam dam)
        {
            // Sets parameters of AsyncProcessingServiceParameterValue to Update progress rate
            ApsParameterValue parameterValue = new ApsParameterValue(
                "AsyncProcessingService", "UpdateTaskProgress", "UpdateTaskProgress", "SQL",
                new MyUserInfo("AsyncProcessingService", "AsyncProcessingService"));

            parameterValue.TaskId = taskID;
            parameterValue.ProgressRate = progressRate;

            // Calls data access part of asynchronous processing service.
            ApsLayerD layerD = new ApsLayerD(dam);
            layerD.UpdateTaskProgress(parameterValue, returnValue);
        }

        /// <summary>
        ///  Updates the command of selected task
        /// </summary>
        /// <param name="taskID">Task ID</param>
        /// <param name="commandId">Command ID</param>
        /// <param name="returnValue">user parameter value</param>
        /// <param name="dam">BaseDam</param>
        public static void UpdateTaskCommand(int taskID, int commandId, ApsReturnValue returnValue, BaseDam dam)
        {
            // Sets parameters of AsyncProcessingServiceParameterValue to update the command of selected task.
            ApsParameterValue parameterValue = new ApsParameterValue(
                "AsyncProcessingService", "UpdateTaskCommand", "UpdateTaskCommand", "SQL",
                new MyUserInfo("AsyncProcessingService", "AsyncProcessingService"));

            parameterValue.TaskId = taskID;
            parameterValue.CommandId = commandId;

            // Calls data access part of asynchronous processing service.
            ApsLayerD layerD = new ApsLayerD(dam);
            layerD.UpdateTaskCommand(parameterValue, returnValue);
        }

        
    }
}
