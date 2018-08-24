﻿'**********************************************************************************
'* Copyright (C) 2007,2016 Hitachi Solutions,Ltd.
'**********************************************************************************

#Region "Apache License"
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License. 
' You may obtain a copy of the License at
'
' http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
'
#End Region

'**********************************************************************************
'* クラス名        ：ApsReturnValue
'* クラス日本語名  ：ApsReturnValue
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  11/28/2014  Supragyan         Paramter Return Value class for Asynchronous Processing Service
'*  04/15/2015  Sandeep           Changed datatype of ProgressRate to decimal.
'*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
'**********************************************************************************

Imports System
Imports Touryo.Infrastructure.Business.Common

Namespace Touryo.Infrastructure.Business.AsyncProcessingService
	''' <summary>
	''' Paramter Return Value class for Asynchronous Processing Service
	''' </summary>
	Public Class ApsReturnValue
		Inherits MyReturnValue
		''' <summary>汎用エリア</summary>
		Public Obj As Object

		''' <summary>TaskId</summary>
		Public TaskId As Integer

		''' <summary>UserId</summary>
		Public UserId As String

		''' <summary>ProcessName</summary>
		Public ProcessName As String

		''' <summary>Data</summary>
		Public Data As String

		''' <summary>RegistrationDateTime</summary>
		Public RegistrationDateTime As DateTime

		''' <summary> ExecutionStartDateTime;</summary>
		Public ExecutionStartDateTime As DateTime

		''' <summary> NumberOfRetries;</summary>
		Public NumberOfRetries As Integer

		''' <summary>ProgressRate</summary>
		Public ProgressRate As Decimal

		''' <summary> StatusId;</summary>
		Public StatusId As Integer

		''' <summary>CompletionDateTime</summary>
		Public CompletionDateTime As DateTime

		''' <summary> CommandId;</summary>
		Public CommandId As Integer

		''' <summary>ReservedArea</summary>
		Public ReservedArea As String

		''' <summary>ExceptionInfo</summary>
		Public ExceptionInfo As String
	End Class
End Namespace
