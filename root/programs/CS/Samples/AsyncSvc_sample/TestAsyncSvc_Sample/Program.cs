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
//*  2018/08/24  西野 大介         SampleをBinaryからJSONのSerializeへ変更。
//**********************************************************************************

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Touryo.Infrastructure.Business.AsyncProcessingService;
using Touryo.Infrastructure.Business.Util;
using Touryo.Infrastructure.Framework.AsyncProcessingService;
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
        
        #region 非同期タスクの投入

        /// <summary>
        /// Inserts asynchronous task information to the database
        /// </summary>
        /// <returns>ApsParameterValue</returns>
        public ApsParameterValue InsertData()
        {
            // Create list data to json serilize.
            List<int> listData = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            
            // Sets parameters of ApsParameterValue to insert asynchronous task information.
            ApsParameterValue parameterValue = new ApsParameterValue(
                "AsyncProcessingService", "InsertTask", "InsertTask", "SQL",
                new MyUserInfo("AsyncProcessingService", "AsyncProcessingService"));

            parameterValue.UserId = "A";
            parameterValue.ProcessName = "AAA";
            parameterValue.Data = JsonConvert.SerializeObject(listData);
            parameterValue.ExecutionStartDateTime = DateTime.Now;
            parameterValue.RegistrationDateTime = DateTime.Now;
            parameterValue.NumberOfRetries = 0;
            parameterValue.ProgressRate = 0;
            parameterValue.CompletionDateTime = DateTime.Now;
            parameterValue.StatusId = (int)(AsyncStatus.Register);
            parameterValue.CommandId = 0;
            parameterValue.ReservedArea = "xxxxxx";
            
            ApsLayerB layerB = new ApsLayerB();
            ApsReturnValue returnValue = (ApsReturnValue)layerB.DoBusinessLogic(
                (ApsParameterValue)parameterValue, DbEnum.IsolationLevelEnum.DefaultTransaction);

            return parameterValue;
        }

        #endregion
    }
}