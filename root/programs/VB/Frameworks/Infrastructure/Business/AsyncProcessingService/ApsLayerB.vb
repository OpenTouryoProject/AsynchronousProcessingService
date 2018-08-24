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
'* クラス名        ：LayerB
'* クラス日本語名  ：LayerB
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  11/28/2014  Supragyan         Created LayerB class for AsyncProcessing Service
'*  11/28/2014  Supragyan         Created Insert,Update,Select method for AsyncProcessing Service
'*  04/15/2015  Sandeep           Did code modification of insert, update and select for AsyncProcessing Service
'*  06/09/2015  Sandeep           Implemented code to update stop command to all the running asynchronous task
'*                                Modified code to reset Exception information, before starting asynchronous task 
'*  06/26/2015  Sandeep           Implemented code to get commandID in the SelectTask method,
'*                                to resolve unstable "Register" state, when you invoke [Abort] to AsyncTask, at this "Register" state
'*  06/01/2016  Sandeep           Implemented method to test the connection of specified database
'*  2018/08/24  西野 大介         クラス名称の変更（ ---> Aps）
'**********************************************************************************

Imports System
Imports System.Data

Imports Touryo.Infrastructure.Business.Business

Namespace Touryo.Infrastructure.Business.AsyncProcessingService

	''' <summary>
	''' LayerB class for AsyncProcessing Service
	''' </summary>
	Public Class ApsLayerB
		Inherits MyFcBaseLogic
		#Region "Insert"

		''' <summary>
		''' Inserts Async Parameter values to Database through LayerD 
		''' </summary>
		''' <param name="parameterValue"></param>
		Public Sub UOC_InsertTask(parameterValue As ApsParameterValue)
			' 戻り値クラスを生成して、事前に戻り値に設定しておく。
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.InsertTask(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "Update"

		#Region "UpdateTaskStart"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is started
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskStart(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskStart(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "UpdateTaskRetry"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is failed and can be retried later
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskRetry(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskRetry(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "UpdateTaskFail"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is failed and abort this task [status=Abort] 
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskFail(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskFail(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "UpdateTaskSuccess"

		''' <summary>
		'''  Updates information in the database that the asynchronous task is completed
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskSuccess(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskSuccess(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "UpdateTaskProgress"

		''' <summary>
		'''  Updates progress rate of the asynchronous task in the database.
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskProgress(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskProgress(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "UpdateTaskCommand"

		''' <summary>
		'''  Updates command value information of a selected asynchronous task
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_UpdateTaskCommand(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.UpdateTaskCommand(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "StopAllTask"

		''' <summary>
		'''  Set stop command for all running asynchronous task
		''' </summary>
		''' <param name="parameterValue">Asynchronous Parameter Values</param>
		Private Sub UOC_StopAllTask(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.StopAllTask(parameterValue, returnValue)
		End Sub

		#End Region

		#End Region

		#Region "Select"

		#Region "SelectCommand"

		''' <summary>
		''' Selects user command from Database through LayerD 
		''' </summary>
		''' <param name="parameterValue"></param>
		Private Sub UOC_SelectCommand(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.SelectCommand(parameterValue, returnValue)
		End Sub

		#End Region

		#Region "SelectTask"

		''' <summary>
		''' Selects Asynchronous task from LayerD 
		''' </summary>
		''' <param name="parameterValue">Async Parameter Value</param>
		Private Sub UOC_SelectTask(parameterValue As ApsParameterValue)
			Dim returnValue As New ApsReturnValue()
			Me.ReturnValue = returnValue

			Dim layerD As New ApsLayerD(Me.GetDam())
			layerD.SelectTask(parameterValue, returnValue)

			Dim dt As DataTable = DirectCast(returnValue.Obj, DataTable)
			returnValue.Obj = Nothing

			If dt IsNot Nothing Then
				If dt.Rows.Count <> 0 Then
					returnValue.TaskId = Convert.ToInt32(dt.Rows(0)("Id"))
					returnValue.UserId = dt.Rows(0)("UserId").ToString()
					returnValue.ProcessName = dt.Rows(0)("ProcessName").ToString()
					returnValue.Data = dt.Rows(0)("Data").ToString()
					returnValue.NumberOfRetries = Convert.ToInt32(dt.Rows(0)("NumberOfRetries"))
					returnValue.ReservedArea = dt.Rows(0)("ReservedArea").ToString()
					returnValue.CommandId = Convert.ToInt32(dt.Rows(0)("CommandId"))
				End If
			End If
		End Sub

		#End Region

		#End Region

		#Region "TestConnection"

		''' <summary>
		''' Tests the connection with the specified database
		''' </summary>
		''' <param name="parameterValue">Async Parameter Value</param>
		Private Sub UOC_TestConnection(parameterValue As ApsParameterValue)
			Dim layerD As New ApsLayerD(Me.GetDam())
		End Sub

		#End Region
	End Class
End Namespace
