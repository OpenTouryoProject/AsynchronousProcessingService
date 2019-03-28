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
//* クラス名        ：LayerB
//* クラス日本語名  ：LayerB
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014  Supragyan         Created LayerB class for AsyncProcessing Service
//*  11/28/2014  Supragyan         Created Insert,Update,Select method for AsyncProcessing Service
//*  04/15/2015  Sandeep           Did code modification of insert, update and select for AsyncProcessing Service
//*  06/09/2015  Sandeep           Implemented code to update stop command to all the running asynchronous task
//*                                Modified code to reset Exception information, before starting asynchronous task 
//*  06/26/2015  Sandeep           Implemented code to get commandID in the SelectTask method,
//*                                to resolve unstable "Register" state, when you invoke [Abort] to AsyncTask, at this "Register" state
//*  06/01/2016  Sandeep           Implemented method to test the connection of specified database
//*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
//**********************************************************************************

using System;
using System.Data;

using Touryo.Infrastructure.Business.Business;

namespace Touryo.Infrastructure.Business.AsyncProcessingService
{
    /// <summary>
    /// LayerB class for AsyncProcessing Service
    /// </summary>
    public class ApsLayerB : MyFcBaseLogic
    {
        #region Insert

        /// <summary>
        /// Inserts Async Parameter values to Database through LayerD 
        /// </summary>
        /// <param name="parameterValue"></param>
        public void UOC_InsertTask(ApsParameterValue parameterValue)
        {
            // 戻り値クラスを生成して、事前に戻り値に設定しておく。
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.InsertTask(parameterValue, returnValue);
        }

        #endregion

        #region Update

        #region UpdateTaskStart 

        /// <summary>
        ///  Updates information in the database that the asynchronous task is started
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskStart(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskStart(parameterValue, returnValue);
        }

        #endregion

        #region UpdateTaskRetry

        /// <summary>
        ///  Updates information in the database that the asynchronous task is failed and can be retried later
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskRetry(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskRetry(parameterValue, returnValue);
        }

        #endregion

        #region UpdateTaskFail

        /// <summary>
        ///  Updates information in the database that the asynchronous task is failed and abort this task [status=Abort] 
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskFail(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskFail(parameterValue, returnValue);
        }

        #endregion

        #region UpdateTaskSuccess

        /// <summary>
        ///  Updates information in the database that the asynchronous task is completed
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskSuccess(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskSuccess(parameterValue, returnValue);
        }

        #endregion

        #region UpdateTaskProgress

        /// <summary>
        ///  Updates progress rate of the asynchronous task in the database.
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskProgress(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskProgress(parameterValue, returnValue);
        }

        #endregion

        #region UpdateTaskCommand

        /// <summary>
        ///  Updates command value information of a selected asynchronous task
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_UpdateTaskCommand(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.UpdateTaskCommand(parameterValue, returnValue);
        }

        #endregion

        #region StopAllTask

        /// <summary>
        ///  Set stop command for all running asynchronous task
        /// </summary>
        /// <param name="parameterValue">Asynchronous Parameter Values</param>
        private void UOC_StopAllTask(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.StopAllTask(parameterValue, returnValue);
        }

        #endregion

        #endregion        

        #region Select

        #region SelectCommand

        /// <summary>
        /// Selects user command from Database through LayerD 
        /// </summary>
        /// <param name="parameterValue"></param>
        private void UOC_SelectCommand(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.SelectCommand(parameterValue, returnValue);
        }

        #endregion

        #region SelectTask

        /// <summary>
        /// Selects Asynchronous task from LayerD 
        /// </summary>
        /// <param name="parameterValue">Async Parameter Value</param>
        private void UOC_SelectTask(ApsParameterValue parameterValue)
        {
            ApsReturnValue returnValue = new ApsReturnValue();
            this.ReturnValue = returnValue;

            ApsLayerD layerD = new ApsLayerD(this.GetDam());
            layerD.SelectTask(parameterValue, returnValue);

            DataTable dt = (DataTable)returnValue.Obj;
            returnValue.Obj = null;

            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    returnValue.TaskId = Convert.ToInt32(dt.Rows[0]["Id"]);
                    returnValue.UserId = dt.Rows[0]["UserId"].ToString();
                    returnValue.ProcessName = dt.Rows[0]["ProcessName"].ToString();
                    returnValue.Data = dt.Rows[0]["Data"].ToString();
                    returnValue.NumberOfRetries = Convert.ToInt32(dt.Rows[0]["NumberOfRetries"]);
                    returnValue.ReservedArea = dt.Rows[0]["ReservedArea"].ToString();
                    returnValue.CommandId = Convert.ToInt32(dt.Rows[0]["CommandId"]);
                }
            }
        }

        #endregion

        #endregion

        #region TestConnection

        /// <summary>
        /// Tests the connection with the specified database
        /// </summary>
        /// <param name="parameterValue">Async Parameter Value</param>
        private void UOC_TestConnection(ApsParameterValue parameterValue)
        {
            ApsLayerD layerD = new ApsLayerD(this.GetDam());
        }

        #endregion
    }
}
