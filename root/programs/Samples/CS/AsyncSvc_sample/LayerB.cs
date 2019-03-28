//**********************************************************************************
//* 非同期処理サービス・サンプル アプリ
//**********************************************************************************

// テスト用サンプルなので、必要に応じて流用 or 削除して下さい。

//**********************************************************************************
//* クラス名        ：LayerB
//* クラス日本語名  ：非同期タスクのシミュレーション実行
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014   Supragyan        LayerB class for AsyncProcessing Service.
//*  17/08/2015   Sandeep          Implemented serialization and deserialization methods.
//*                                Modified the code to start and update asynchronous task.
//*                                Implemented code to get command value and update progress rate.
//*                                Implemented code to declare and initialize the member variable.
//*                                Implemented code to handle abnormal termination, while updating the asynchronous process.
//*                                Implemented code to resume asynchronous process in the middle of the processing.
//*  21/08/2015   Sandeep          Modified code to call layerD of AsynProcessingService instead of do business logic.
//*  28/08/2015   Sandeep          Resolved transaction timeout issue by using DamKeyForABT and DamKeyForAMT properties.
//*  07/06/2016   Sandeep          Implemented code that respond to various test cases, other than success state.
//*  08/06/2016   Sandeep          Implemented method to update the command of selected task.
//*  2018/08/24  西野 大介         SampleをBinaryからJSONのSerializeへ変更。
//*  2018/08/24  西野 大介         Utilityメソッドを部品化と確率計算の修正。
//**********************************************************************************

using System;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;

using Newtonsoft.Json;

using Touryo.Infrastructure.Business.AsyncProcessingService;
using Touryo.Infrastructure.Framework.AsyncProcessingService;
using Touryo.Infrastructure.Framework.Exceptions;
using Touryo.Infrastructure.Framework.Util;
using Touryo.Infrastructure.Public.Util;

namespace AsyncSvc_sample
{
    /// <summary>
    /// LayerB class for AsyncProcessing service sample
    /// </summary>
    public class LayerB : MyApsBaseLogic
    {
        #region Member declartion

        /// <summary>Constant values</summary>
        const uint SUCCESS_STATE = 100;

        /// <summary>Task progress rate</summary>
        private uint ProgressRate;

        #region SampleのSimulation

        /// <summary>Number of seconds</summary>
        private int NumberOfSeconds;

        /// <summary>Max progress rate</summary>
        private uint MaxProgressRate;

        /// <summary>Stop probability</summary>
        private uint StopPercentage;

        /// <summary>Abort probability</summary>
        private uint AbortPercentage;

        #endregion

        #endregion

        #region Member initialization

        /// <summary>Constructor</summary>
        public LayerB()
        {
            #region SampleのSimulation
            
            // Number of seconds to sleep the thread.
            string numberOfSeconds = GetConfigParameter.GetConfigValue("FxSleepUserProcess");
            if (!string.IsNullOrEmpty(numberOfSeconds))
            {
                this.NumberOfSeconds = int.Parse(numberOfSeconds);
            }
            else
            {
                this.NumberOfSeconds = 5;
            }

            // Max progress rate
            string maxProgressRate = GetConfigParameter.GetConfigValue("FxMaxProgressRate");
            if (!string.IsNullOrEmpty(maxProgressRate))
            {
                this.MaxProgressRate = uint.Parse(maxProgressRate);
            }
            else
            {
                this.MaxProgressRate = 30;
            }

            // Stop probability.
            string stopPercentage = GetConfigParameter.GetConfigValue("FxStopPercentage");
            if (!string.IsNullOrEmpty(stopPercentage))
            {
                this.StopPercentage = uint.Parse(stopPercentage);
            }
            else
            {
                this.StopPercentage = 3;
            }

            // Abort probability.
            string abortPercentage = GetConfigParameter.GetConfigValue("FxAbortPercentage");
            if (!string.IsNullOrEmpty(abortPercentage))
            {
                this.AbortPercentage = uint.Parse(abortPercentage);
            }
            else
            {
                this.AbortPercentage = 1;
            }

            #endregion
        }

        #endregion

        #region Member methods

        #region 非同期タスクの実行

        /// <summary>
        /// Initiate the processing of asynchronous task.
        /// </summary>
        /// <param name="parameterValue">asynchronous parameter values</param>
        public void UOC_Start(ApsParameterValue parameterValue)
        {
            // Generates a return value class.
            // 戻り値クラスを生成する。
            ApsReturnValue returnValue = new ApsReturnValue();

            this.ReturnValue = returnValue;

            // Get array data from serialized json string.
            List<int> listData = JsonConvert.DeserializeObject<List<int>>(parameterValue.Data);

            // Get command information from database to check for retry.
            // データベースからコマンド情報を取得して確認する。
            ApsUtility.GetCommandValue(parameterValue.TaskId, returnValue, this.GetDam(this.DamKeyforAMT));

            if (returnValue.CommandId == (int)AsyncCommand.Stop)
            {
                // Retry task: to resume asynchronous process in the middle of the processing.
                // 再試行タスク：処理の途中で停止された非同期処理を再開する。
                ApsUtility.ResumeProcessing(parameterValue.TaskId, returnValue, this.GetDam(this.DamKeyforAMT));

                // Updated progress rate will be taken as random number.
                // 進捗率をインクリメントする。
                ProgressRate = this.GenerateProgressRate(ProgressRate);
            }
            else
            {
                // Otherwise, implement code to initiating a new task. 
                // それ以外の場合は、新しいタスクを開始するコードを実装する。
                //...
                // Hence, initializing progress rate to zero.
                // したがって、進捗率をゼロに設定する。
                ProgressRate = 0;
            }

            // Updates the progress rate and handles abnormal termination of the process.
            // 進捗率をインクリメントしたり、プロセスの異常終了を処理したり。
            this.Update(parameterValue.TaskId, returnValue);
        }

        /// <summary>
        ///  Updates the progress rate and handles abnormal termination of the process.
        /// </summary>
        /// <param name="taskID">Task ID</param>
        /// <param name="returnValue">user parameter value</param>
        private void Update(int taskID, ApsReturnValue returnValue)
        {
            // Place the following statements in the loop, till the completion of task.
            // タスクが完了するまで、ループ内の処理を実行する。

            while (true)
            {
                // Get command information from database to check for retry.
                // データベースからコマンド情報を取得して、CommandIdを確認。
                ApsUtility.GetCommandValue(taskID, returnValue, this.GetDam(this.DamKeyforAMT));

                switch (returnValue.CommandId)
                {
                    case (int)AsyncCommand.Stop:

                        // If you want to retry, then throw the following exception.
                        // 処理の途中で停止する場合は、次の例外をスロー。

                        throw new BusinessApplicationException(
                            AsyncErrorMessageID.APSStopCommand.ToString(),
                            GetMessage.GetMessageDescription("CTE0003"), "");

                    case (int)AsyncCommand.Abort:

                        // Implement code to forcefully Abort the task.
                        // If the task is abnormal terminated, then throw the exception.
                        // 強制的にタスクを中断する場合は、例外をスロー。

                        throw new BusinessSystemException(
                            AsyncErrorMessageID.APSAbortCommand.ToString(),
                            GetMessage.GetMessageDescription("CTE0004"));

                    default:
                        // Generates new progress rate of the task.
                        // 進捗率をインクリメント
                        ProgressRate = this.GenerateProgressRate(ProgressRate);

                        // Update the progress rate in database.
                        // データベースの進捗率を更新。
                        ApsUtility.UpdateProgressRate(
                            taskID, returnValue, ProgressRate, this.GetDam(this.DamKeyforAMT));

                        // 非同期タスクのシミュレーション
                        if (this.Fortune(this.AbortPercentage))
                        {
                            // Update ABORT command to database
                            // データベースのコマンドをABORTに更新。
                            ApsUtility.UpdateTaskCommand(
                                taskID, (int)AsyncCommand.Abort,
                                returnValue, this.GetDam(this.DamKeyforAMT));
                        }
                        else if (this.Fortune(this.StopPercentage))
                        {
                            // Update STOP command to database
                            // データベースのコマンドをSTOPに更新。
                            ApsUtility.UpdateTaskCommand(
                                taskID, (int)AsyncCommand.Stop,
                                returnValue, this.GetDam(this.DamKeyforAMT));
                        }
                        else if (SUCCESS_STATE <=ProgressRate)
                        {
                            // Task is completed sucessfully.
                            // タスクは正常に完了
                            return;
                        }
                        else
                        {
                            // タスクは継続する。
                            Thread.Sleep(this.NumberOfSeconds * 1000);
                        }

                        break;
                }
            }
        }

        #region SampleのSimulation

        /// <summary>
        ///  Generates new progress rate for the task based on last progress rate in increasing order.
        /// </summary>
        /// <param name="lastProgressRate">Last progress rate</param>
        /// <returns>New progress rate</returns>
        private uint GenerateProgressRate(uint lastProgressRate)
        {            
            // 乱数の30の剰余を足し込む。
            lastProgressRate += (RandomValueGenerator.GenerateRandomUint() % 30);

            if (SUCCESS_STATE <= lastProgressRate)
            {
                // 100%以上の場合、100%
                return SUCCESS_STATE;
            }
            else
            {
                // 100%未満の場合、その値
                return lastProgressRate;
            }
        }

        /// <summary>真偽の占い</summary>
        /// <param name="percentage">確率</param>
        /// <returns>真偽</returns>
        private bool Fortune(uint percentage)
        {
            return ((RandomValueGenerator.GenerateRandomUint() % 100) < percentage);
        }

        #endregion

        #endregion

        #endregion
    }
}