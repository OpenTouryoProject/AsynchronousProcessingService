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
'* クラス名        ：ApsUtility
'* クラス日本語名  ：ApsUtility
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  2018/08/24  西野 大介         新規作成（Utilityメソッドを部品化）
'**********************************************************************************

Imports Touryo.Infrastructure.Business.Util
Imports Touryo.Infrastructure.Public.Db

Namespace Touryo.Infrastructure.Business.AsyncProcessingService
	''' <summary>ApsUtility</summary>
	Public Class ApsUtility
		''' <summary>Get command information from database. </summary>
		''' <param name="taskID">asynchronous task id</param>
		''' <param name="returnValue">asynchronous return value</param>
		''' <param name="dam">BaseDam</param>
		Public Shared Sub GetCommandValue(taskID As Integer, returnValue As ApsReturnValue, dam As BaseDam)
			' Sets parameters of AsyncProcessingServiceParameterValue to get command value.
			Dim parameterValue As New ApsParameterValue("AsyncProcessingService", "SelectCommand", "SelectCommand", "SQL", New MyUserInfo("AsyncProcessingService", "AsyncProcessingService"))

			parameterValue.TaskId = taskID

			' Calls data access part of asynchronous processing service.
			Dim layerD As New ApsLayerD(dam)
			layerD.SelectCommand(parameterValue, returnValue)
			returnValue.CommandId = CInt(returnValue.Obj)
		End Sub

		''' <summary>
		'''  Resumes asynchronous process in the middle of the processing.
		''' </summary>
		''' <param name="taskID">Task ID</param>
		''' <param name="returnValue">asynchronous return value</param>
		''' <param name="dam">BaseDam</param>
		Public Shared Sub ResumeProcessing(taskID As Integer, returnValue As ApsReturnValue, dam As BaseDam)
			' Reset the command of selected task.
			ApsUtility.UpdateTaskCommand(taskID, 0, returnValue, dam)
		End Sub

		''' <summary>
		'''  Updates the progress rate in the database. 
		''' </summary>
		''' <param name="taskID">asynchronous task id</param>
		''' <param name="returnValue">ApsReturnValue</param>
		''' <param name="progressRate">progress rate</param>
		''' <param name="dam">BaseDam</param>
		Public Shared Sub UpdateProgressRate(taskID As Integer, returnValue As ApsReturnValue, progressRate As Decimal, dam As BaseDam)
			' Sets parameters of AsyncProcessingServiceParameterValue to Update progress rate
			Dim parameterValue As New ApsParameterValue("AsyncProcessingService", "UpdateTaskProgress", "UpdateTaskProgress", "SQL", New MyUserInfo("AsyncProcessingService", "AsyncProcessingService"))

			parameterValue.TaskId = taskID
			parameterValue.ProgressRate = progressRate

			' Calls data access part of asynchronous processing service.
			Dim layerD As New ApsLayerD(dam)
			layerD.UpdateTaskProgress(parameterValue, returnValue)
		End Sub

		''' <summary>
		'''  Updates the command of selected task
		''' </summary>
		''' <param name="taskID">Task ID</param>
		''' <param name="commandId">Command ID</param>
		''' <param name="returnValue">user parameter value</param>
		''' <param name="dam">BaseDam</param>
		Public Shared Sub UpdateTaskCommand(taskID As Integer, commandId As Integer, returnValue As ApsReturnValue, dam As BaseDam)
			' Sets parameters of AsyncProcessingServiceParameterValue to update the command of selected task.
			Dim parameterValue As New ApsParameterValue("AsyncProcessingService", "UpdateTaskCommand", "UpdateTaskCommand", "SQL", New MyUserInfo("AsyncProcessingService", "AsyncProcessingService"))

			parameterValue.TaskId = taskID
			parameterValue.CommandId = commandId

			' Calls data access part of asynchronous processing service.
			Dim layerD As New ApsLayerD(dam)
			layerD.UpdateTaskCommand(parameterValue, returnValue)
		End Sub


	End Class
End Namespace
