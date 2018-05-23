//**********************************************************************************
//* 非同期処理サービス・サンプル アプリ
//**********************************************************************************

// テスト用サンプルなので、必要に応じて流用 or 削除して下さい。

//**********************************************************************************
//* クラス名        ：Program
//* クラス日本語名  ：Program
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014  Supragyan         For Inserts data to database.
//*  17/08/2015  Sandeep           Modified insert method name from 'Start' to 'InsertTask'.
//*                                Modified object of LayerB that is related to Business project,
//*                                instead of AsyncSvc_sample project. 
//**********************************************************************************

using System;

using Touryo.Infrastructure.Business.AsyncProcessingService;
using Touryo.Infrastructure.Business.Util;
using Touryo.Infrastructure.Public.Str;
using Touryo.Infrastructure.Public.Db;

namespace TestAsyncSvc_Sample
{
    /// <summary>
    /// Program class for test user code
    /// </summary>
    public class Program
    {
        /// <summary>This is the main entry point for the application.</summary>
        static void Main(string[] args)
        {
            Program program = new Program();
            program.InsertData();
        }

        #region Utilityメソッド

        /// <summary>
        ///  Converts byte array to serialized base64 string
        /// </summary>
        /// <param name="arrayData">byte array</param>
        /// <returns>base64 string</returns>
        public static string SerializeToBase64String(byte[] arrayData)
        {
            string base64String = string.Empty;
            if (arrayData != null)
            {
                CustomEncode.ToBase64String(arrayData);
            }
            return base64String;
        }

        #endregion

        #region 非同期タスクの投入

        /// <summary>
        /// Inserts asynchronous task information to the database
        /// </summary>
        /// <returns>AsyncProcessingServiceParameterValue</returns>
        public AsyncProcessingServiceParameterValue InsertData()
        {
            // Create array data to serilize.
            byte[] arrayData = { 1, 2, 3, 4, 5 };

            // Sets parameters of AsyncProcessingServiceParameterValue to insert asynchronous task information.
            AsyncProcessingServiceParameterValue asyncParameterValue = new AsyncProcessingServiceParameterValue(
                "AsyncProcessingService", "InsertTask", "InsertTask", "SQL",
                new MyUserInfo("AsyncProcessingService", "AsyncProcessingService"));

            asyncParameterValue.UserId = "A";
            asyncParameterValue.ProcessName = "AAA";
            asyncParameterValue.Data = Program.SerializeToBase64String(arrayData);
            asyncParameterValue.ExecutionStartDateTime = DateTime.Now;
            asyncParameterValue.RegistrationDateTime = DateTime.Now;
            asyncParameterValue.NumberOfRetries = 0;
            asyncParameterValue.ProgressRate = 0;
            asyncParameterValue.CompletionDateTime = DateTime.Now;
            asyncParameterValue.StatusId = (int)(AsyncProcessingServiceParameterValue.AsyncStatus.Register);
            asyncParameterValue.CommandId = 0;
            asyncParameterValue.ReservedArea = "xxxxxx";

            DbEnum.IsolationLevelEnum iso = DbEnum.IsolationLevelEnum.DefaultTransaction;
            AsyncProcessingServiceReturnValue asyncReturnValue;

            // Execute do business logic method.
            LayerB layerB = new LayerB();
            asyncReturnValue = (AsyncProcessingServiceReturnValue)layerB.DoBusinessLogic((AsyncProcessingServiceParameterValue)asyncParameterValue, iso);
            return asyncParameterValue;
        }

        #endregion
    }
}