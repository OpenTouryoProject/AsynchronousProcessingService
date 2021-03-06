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
//* クラス名        ：ApsReturnValue
//* クラス日本語名  ：ApsReturnValue
//*
//*  日時        更新者            内容
//*  ----------  ----------------  -------------------------------------------------
//*  11/28/2014  Supragyan         Paramter Return Value class for Asynchronous Processing Service
//*  04/15/2015  Sandeep           Changed datatype of ProgressRate to decimal.
//*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
//**********************************************************************************

using System;
using Touryo.Infrastructure.Business.Common;

namespace Touryo.Infrastructure.Business.AsyncProcessingService
{
    /// <summary>
    /// Paramter Return Value class for Asynchronous Processing Service
    /// </summary>
    public class ApsReturnValue : MyReturnValue
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

        /// <summary> ExecutionStartDateTime;</summary>
        public DateTime ExecutionStartDateTime;

        /// <summary> NumberOfRetries;</summary>
        public int NumberOfRetries;

        /// <summary>ProgressRate</summary>
        public decimal ProgressRate;

        /// <summary> StatusId;</summary>
        public int StatusId;

        /// <summary>CompletionDateTime</summary>
        public DateTime CompletionDateTime;

        /// <summary> CommandId;</summary>
        public int CommandId;

        /// <summary>ReservedArea</summary>
        public string ReservedArea;

        /// <summary>ExceptionInfo</summary>
        public string ExceptionInfo;
    }
}
