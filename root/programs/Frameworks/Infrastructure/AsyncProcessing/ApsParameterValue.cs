﻿//**********************************************************************************
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
//* クラス名        ：ApsParameterValue
//* クラス日本語名  ：ApsParameterValue
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014  Supragyan         Paramter Value class for Asynchronous Processing Service
//*  04/15/2015  Sandeep           Changed datatype of ProgressRate to decimal.
//*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
//*  2018/08/24  西野 大介         列挙型と列挙型処理クラスの移動
//**********************************************************************************

using System;
using System.Reflection;

using Touryo.Infrastructure.Business.Common;
using Touryo.Infrastructure.Business.Util;

namespace Touryo.Infrastructure.Business.AsyncProcessingService
{
    /// <summary>
    /// Paramter Value class for Asynchronous Processing Service
    /// </summary>
    public class ApsParameterValue : MyParameterValue
    {
        /// <summary>汎用エリア</summary>
        public object Obj;

        /// <summary>TaskId</summary>
        public int TaskId;

        /// <summary>UserId</summary>
        public string UserId;

        /// <summary>ProcessName</summary>
        public string ProcessName;

        /// <summary>Data</summary>
        public string Data;

        /// <summary>RegistrationDateTime</summary>
        public DateTime RegistrationDateTime;

        /// <summary>ExecutionStartDateTime</summary>
        public DateTime ExecutionStartDateTime;

        /// <summary>NumberOfRetries</summary>
        public int NumberOfRetries;

        /// <summary>ProgressRate</summary>
        public decimal ProgressRate;

        /// <summary>Status</summary>
        public int StatusId;

        /// <summary>CompletionDateTime</summary>
        public DateTime CompletionDateTime;

        /// <summary>CommandId</summary>
        public int CommandId;

        /// <summary>ReservedArea</summary>
        public string ReservedArea;

        /// <summary>ExceptionInfo</summary>
        public string ExceptionInfo;

        #region コンストラクタ

        /// <summary>コンストラクタ</summary>
        public ApsParameterValue(
            string screenId, string controlId, string methodName, string actionType, MyUserInfo user)
            : base(screenId, controlId, methodName, actionType, user)
        {
            // Baseのコンストラクタに引数を渡すために必要。
        }

        #endregion
    }
}
