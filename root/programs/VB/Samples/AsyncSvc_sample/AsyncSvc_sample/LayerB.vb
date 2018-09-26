'**********************************************************************************
'* 非同期処理サービス・サンプル アプリ
'**********************************************************************************

' テスト用サンプルなので、必要に応じて流用 or 削除して下さい。

'**********************************************************************************
'* クラス名        ：LayerB
'* クラス日本語名  ：非同期タスクのシミュレーション実行
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  11/28/2014   Supragyan        LayerB class for AsyncProcessing Service.
'*  17/08/2015   Sandeep          Implemented serialization and deserialization methods.
'*                                Modified the code to start and update asynchronous task.
'*                                Implemented code to get command value and update progress rate.
'*                                Implemented code to declare and initialize the member variable.
'*                                Implemented code to handle abnormal termination, while updating the asynchronous process.
'*                                Implemented code to resume asynchronous process in the middle of the processing.
'*  21/08/2015   Sandeep          Modified code to call layerD of AsynProcessingService instead of do business logic.
'*  28/08/2015   Sandeep          Resolved transaction timeout issue by using DamKeyForABT and DamKeyForAMT properties.
'*  07/06/2016   Sandeep          Implemented code that respond to various test cases, other than success state.
'*  08/06/2016   Sandeep          Implemented method to update the command of selected task.
'*  2018/08/24  西野 大介         SampleをBinaryからJSONのSerializeへ変更。
'*  2018/08/24  西野 大介         Utilityメソッドを部品化と確率計算の修正。
'**********************************************************************************

Imports System
Imports System.Threading
Imports System.Collections.Generic
Imports System.Security.Cryptography

Imports Newtonsoft.Json

Imports Touryo.Infrastructure.Business.AsyncProcessingService
Imports Touryo.Infrastructure.Framework.AsyncProcessingService
Imports Touryo.Infrastructure.Framework.Exceptions
Imports Touryo.Infrastructure.Framework.Util
Imports Touryo.Infrastructure.Public.Util

''' <summary>
''' LayerB class for AsyncProcessing service sample
''' </summary>
Public Class LayerB
    Inherits MyApsBaseLogic
#Region "Member declartion"

    ''' <summary>Constant values</summary>
    Const SUCCESS_STATE As UInteger = 100

    ''' <summary>Task progress rate</summary>
    Private ProgressRate As UInteger

#Region "SampleのSimulation"

    ''' <summary>Number of seconds</summary>
    Private NumberOfSeconds As Integer

    ''' <summary>Max progress rate</summary>
    Private MaxProgressRate As UInteger

    ''' <summary>Stop probability</summary>
    Private StopPercentage As UInteger

    ''' <summary>Abort probability</summary>
    Private AbortPercentage As UInteger

#End Region

#End Region

#Region "Member initialization"

    ''' <summary>Constructor</summary>
    Public Sub New()
        '#Region "SampleのSimulation"

        ' Number of seconds to sleep the thread.
        Dim numberOfSeconds As String = GetConfigParameter.GetConfigValue("FxSleepUserProcess")
        If Not String.IsNullOrEmpty(numberOfSeconds) Then
            Me.NumberOfSeconds = Integer.Parse(numberOfSeconds)
        Else
            Me.NumberOfSeconds = 5
        End If

        ' Max progress rate
        Dim maxProgressRate As String = GetConfigParameter.GetConfigValue("FxMaxProgressRate")
        If Not String.IsNullOrEmpty(maxProgressRate) Then
            Me.MaxProgressRate = UInteger.Parse(maxProgressRate)
        Else
            Me.MaxProgressRate = 30
        End If

        ' Stop probability.
        Dim stopPercentage As String = GetConfigParameter.GetConfigValue("FxStopPercentage")
        If Not String.IsNullOrEmpty(stopPercentage) Then
            Me.StopPercentage = UInteger.Parse(stopPercentage)
        Else
            Me.StopPercentage = 3
        End If

        ' Abort probability.
        Dim abortPercentage As String = GetConfigParameter.GetConfigValue("FxAbortPercentage")
        If Not String.IsNullOrEmpty(abortPercentage) Then
            Me.AbortPercentage = UInteger.Parse(abortPercentage)
        Else
            Me.AbortPercentage = 1

            '#End Region
        End If
    End Sub

#End Region

#Region "Member methods"

#Region "非同期タスクの実行"

    ''' <summary>
    ''' Initiate the processing of asynchronous task.
    ''' </summary>
    ''' <param name="parameterValue">asynchronous parameter values</param>
    Public Sub UOC_Start(parameterValue As ApsParameterValue)
        ' Generates a return value class.
        ' 戻り値クラスを生成する。
        Dim returnValue As New ApsReturnValue()

        Me.ReturnValue = returnValue

        ' Get array data from serialized json string.
        Dim listData As List(Of Integer) = JsonConvert.DeserializeObject(Of List(Of Integer))(parameterValue.Data)

        ' Get command information from database to check for retry.
        ' データベースからコマンド情報を取得して確認する。
        ApsUtility.GetCommandValue(parameterValue.TaskId, returnValue, Me.GetDam(Me.DamKeyforAMT))

        If returnValue.CommandId = CInt(AsyncCommand.[Stop]) Then
            ' Retry task: to resume asynchronous process in the middle of the processing.
            ' 再試行タスク：処理の途中で停止された非同期処理を再開する。
            ApsUtility.ResumeProcessing(parameterValue.TaskId, returnValue, Me.GetDam(Me.DamKeyforAMT))

            ' Updated progress rate will be taken as random number.
            ' 進捗率をインクリメントする。
            ProgressRate = Me.GenerateProgressRate(ProgressRate)
        Else
            ' Otherwise, implement code to initiating a new task. 
            ' それ以外の場合は、新しいタスクを開始するコードを実装する。
            '...
            ' Hence, initializing progress rate to zero.
            ' したがって、進捗率をゼロに設定する。
            ProgressRate = 0
        End If

        ' Updates the progress rate and handles abnormal termination of the process.
        ' 進捗率をインクリメントしたり、プロセスの異常終了を処理したり。
        Me.Update(parameterValue.TaskId, returnValue)
    End Sub

    ''' <summary>
    '''  Updates the progress rate and handles abnormal termination of the process.
    ''' </summary>
    ''' <param name="taskID">Task ID</param>
    ''' <param name="returnValue">user parameter value</param>
    Private Sub Update(taskID As Integer, returnValue As ApsReturnValue)
        ' Place the following statements in the loop, till the completion of task.
        ' タスクが完了するまで、ループ内の処理を実行する。

        While True
            ' Get command information from database to check for retry.
            ' データベースからコマンド情報を取得して、CommandIdを確認。
            ApsUtility.GetCommandValue(taskID, returnValue, Me.GetDam(Me.DamKeyforAMT))

            Select Case returnValue.CommandId
                Case CInt(AsyncCommand.[Stop])

                    ' If you want to retry, then throw the following exception.
                    ' 処理の途中で停止する場合は、次の例外をスロー。

                    Throw New BusinessApplicationException(AsyncErrorMessageID.APSStopCommand.ToString(), GetMessage.GetMessageDescription("CTE0003"), "")

                Case CInt(AsyncCommand.Abort)

                    ' Implement code to forcefully Abort the task.
                    ' If the task is abnormal terminated, then throw the exception.
                    ' 強制的にタスクを中断する場合は、例外をスロー。

                    Throw New BusinessSystemException(AsyncErrorMessageID.APSAbortCommand.ToString(), GetMessage.GetMessageDescription("CTE0004"))
                Case Else

                    ' Generates new progress rate of the task.
                    ' 進捗率をインクリメント
                    ProgressRate = Me.GenerateProgressRate(ProgressRate)

                    ' Update the progress rate in database.
                    ' データベースの進捗率を更新。
                    ApsUtility.UpdateProgressRate(taskID, returnValue, ProgressRate, Me.GetDam(Me.DamKeyforAMT))

                    ' 非同期タスクのシミュレーション
                    If Me.Fortune(Me.AbortPercentage) Then
                        ' Update ABORT command to database
                        ' データベースのコマンドをABORTに更新。
                        ApsUtility.UpdateTaskCommand(taskID, CInt(AsyncCommand.Abort), returnValue, Me.GetDam(Me.DamKeyforAMT))
                    ElseIf Me.Fortune(Me.StopPercentage) Then
                        ' Update STOP command to database
                        ' データベースのコマンドをSTOPに更新。
                        ApsUtility.UpdateTaskCommand(taskID, CInt(AsyncCommand.[Stop]), returnValue, Me.GetDam(Me.DamKeyforAMT))
                    ElseIf SUCCESS_STATE <= ProgressRate Then
                        ' Task is completed sucessfully.
                        ' タスクは正常に完了
                        Return
                    Else
                        ' タスクは継続する。
                        Thread.Sleep(Me.NumberOfSeconds * 1000)
                    End If

                    Exit Select
            End Select
        End While
    End Sub

#Region "SampleのSimulation"

    ''' <summary>
    '''  Generates new progress rate for the task based on last progress rate in increasing order.
    ''' </summary>
    ''' <param name="lastProgressRate">Last progress rate</param>
    ''' <returns>New progress rate</returns>
    Private Function GenerateProgressRate(lastProgressRate As UInteger) As UInteger
        '/ Sleeps the thread, to minimize the CPU utilization.
        'Thread.Sleep(this.NumberOfSeconds * 1000);

        '/ Generate new progress rate
        'Random randProgressRate = new Random();
        'return randProgressRate.Next(lastProgressRate + 1, SUCCESS_STATE + 1);

        ' 乱数の30の剰余を足し込む。
        lastProgressRate += (Me.GenerateRandomUint() Mod 30)

        If SUCCESS_STATE <= lastProgressRate Then
            ' 100%以上の場合、100%
            Return SUCCESS_STATE
        Else
            ' 100%未満の場合、その値
            Return lastProgressRate
        End If
    End Function

    ''' <summary>真偽の占い</summary>
    ''' <param name="percentage">確率</param>
    ''' <returns>真偽</returns>
    Private Function Fortune(percentage As UInteger) As Boolean
        Return ((Me.GenerateRandomUint() Mod 100) < percentage)
    End Function

    ''' <summary>GenerateRandomUint</summary>
    ''' <returns>Random uint</returns>
    Private Function GenerateRandomUint() As UInteger
        Dim bs As Byte() = New Byte(4 - 1) {}
        Dim rng As New RNGCryptoServiceProvider()
        rng.GetBytes(bs)
        rng.Dispose()
        Return BitConverter.ToUInt32(bs, 0)
    End Function

#End Region

#End Region

#End Region
End Class
