'**********************************************************************************
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
'* クラス名        ：LayerD
'* クラス日本語名  ：LayerD
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  11/28/2014  Supragyan         Created LayerD class for AsyncProcessing Service
'*  11/28/2014  Supragyan         Created Insert,Update,Select method for AsyncProcessing Service
'*  04/14/2015  Sandeep           Did code modification of update and select asynchronous task 
'*  04/14/2015  Sandeep           Did code implementation of SetSqlByFile3 to access the SQL from embedded resource
'*  05/28/2015  Sandeep           Did code implementation to update Exception information to the database
'*  06/09/2015  Sandeep           Implemented code to update stop command to all the running asynchronous task
'*                                Modified code to reset Exception information, before starting asynchronous task 
'*  06/26/2015  Sandeep           Removed the where condition command <> 'Abort' from the SelectTask asynchronous task,
'*                                to resolve unstable "Register" state, when you invoke [Abort] to AsyncTask, at this "Register" state
'*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
'**********************************************************************************

Imports System
Imports System.Data

Imports Touryo.Infrastructure.Business.Dao
Imports Touryo.Infrastructure.Framework.AsyncProcessingService
Imports Touryo.Infrastructure.Public.Db

Namespace Touryo.Infrastructure.Business.AsyncProcessingService
	''' <summary>
	''' LayerD class for AsyncProcessing Service
	''' </summary>
	Public Class ApsLayerD
		Inherits MyBaseDao
		''' <summary>AsyncProcessingService用B層</summary>
		''' <param name="dam">dam</param>
		Public Sub New(dam As BaseDam)
			MyBase.New(dam)
		End Sub

		#Region "Insert"

		''' <summary>
		''' Inserts async parameter values to database
		''' </summary>
		''' <param name="parameterValue"></param>
		''' <param name="returnValue"></param>
		Public Sub InsertTask(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "AsyncProcessingServiceInsert.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P2", parameterValue.UserId)
			Me.SetParameter("P3", parameterValue.ProcessName)
			Me.SetParameter("P4", parameterValue.Data)
			Me.SetParameter("P5", parameterValue.RegistrationDateTime)
			Me.SetParameter("P6", DBNull.Value)
			Me.SetParameter("P7", parameterValue.NumberOfRetries)
			Me.SetParameter("P8", DBNull.Value)
			Me.SetParameter("P9", parameterValue.StatusId)
			Me.SetParameter("P10", parameterValue.ProgressRate)
			Me.SetParameter("P11", parameterValue.CommandId)
			Me.SetParameter("P12", parameterValue.ReservedArea)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "Update"

		#Region "UpdateTaskStart"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is started
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskStart(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskStart.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.ExecutionStartDateTime)
			Me.SetParameter("P3", parameterValue.StatusId)
			Me.SetParameter("P4", DBNull.Value)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "UpdateTaskRetry"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is failed and can be retried later
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskRetry(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskRetry.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.NumberOfRetries)
			Me.SetParameter("P3", parameterValue.CompletionDateTime)
			Me.SetParameter("P4", parameterValue.StatusId)
			Me.SetParameter("P5", parameterValue.ExceptionInfo)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "UpdateTaskFail"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is failed and abort this task [status=Abort]
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskFail(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskFail.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.CompletionDateTime)
			Me.SetParameter("P3", parameterValue.StatusId)
			Me.SetParameter("P4", parameterValue.ExceptionInfo)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "UpdateTaskSuccess"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is completed
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskSuccess(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskSuccess.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.CompletionDateTime)
			Me.SetParameter("P3", parameterValue.ProgressRate)
			Me.SetParameter("P4", parameterValue.StatusId)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "UpdateTaskProgress"

		''' <summary>
		'''  Updates progress rate of the asynchronous task in the database.
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskProgress(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskProgress.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.ProgressRate)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "UpdateTaskCommand"

		''' <summary>
		'''  Updates command value information of a selected asynchronous task
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub UpdateTaskCommand(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "UpdateTaskCommand.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)
			Me.SetParameter("P2", parameterValue.CommandId)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#Region "StopAllTask"

		''' <summary>
		'''  Set stop command for all running asynchronous task.
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub StopAllTask(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "StopAllTask.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.StatusId)
			Me.SetParameter("P2", parameterValue.CommandId)

			' Execute SQL query
			returnValue.Obj = Me.ExecInsUpDel_NonQuery()
		End Sub

		#End Region

		#End Region

		#Region "Select"

		#Region "SelectCommand"

		''' <summary>
		'''  Selects user command from database
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		''' <param name="returnValue">Asynchronous Return Values</param>
		Public Sub SelectCommand(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "SelectCommand.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.TaskId)

			' Execute SQL query
			returnValue.Obj = Me.ExecSelectScalar()
		End Sub

		#End Region

		#Region "SelectTask"

		''' <summary>
		'''  To get Asynchronous Task from the database
		''' </summary>
		''' <param name="parameterValue"></param>
		''' <param name="returnValue"></param>
		Public Sub SelectTask(parameterValue As ApsParameterValue, returnValue As ApsReturnValue)
			Dim filename As String = String.Empty
			filename = "SelectTask.sql"

			' Get SQL query from file.
			Me.SetSqlByFile2(filename)

			' Set SQL parameter values
			Me.SetParameter("P1", parameterValue.RegistrationDateTime)
			Me.SetParameter("P2", parameterValue.NumberOfRetries)
			Me.SetParameter("P3", CInt(AsyncStatus.Register))
			Me.SetParameter("P4", CInt(AsyncStatus.AbnormalEnd))
			Me.SetParameter("P7", parameterValue.CompletionDateTime)

			Dim dt As New DataTable()

			' Get Asynchronous Task from the database
			Me.ExecSelectFill_DT(dt)
			returnValue.Obj = dt
		End Sub

		#End Region

		#End Region
	End Class
End Namespace
