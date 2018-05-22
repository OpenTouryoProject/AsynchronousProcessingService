'**********************************************************************************
'* 非同期処理サービス・サンプル アプリ
'**********************************************************************************

' テスト用サンプルなので、必要に応じて流用 or 削除して下さい。

'**********************************************************************************
'* クラス名        ：Program
'* クラス日本語名  ：Program
'*
'*  日時        更新者            内容
'*  ----------  ----------------  -------------------------------------------------
'*  11/28/2014  Supragyan         For Inserts data to database.
'*  17/08/2015  Sandeep           Modified insert method name from 'Start' to 'InsertTask'.
'*                                Modified object of LayerB that is related to Business project,
'*                                instead of AsyncSvc_sample project. 
'**********************************************************************************

Imports Touryo.Infrastructure.Business.AsyncProcessingService
Imports Touryo.Infrastructure.Business.Util
Imports Touryo.Infrastructure.Public.Str
Imports Touryo.Infrastructure.Public.Db

Namespace TestAsyncSvc_Sample
	''' <summary>
	''' Program class for test user code
	''' </summary>
	Public Class Program
		''' <summary>This is the main entry point for the application.</summary>
        Friend Shared Sub Main(args As String())
            Dim program As New Program()
            program.InsertData()
        End Sub

#Region "Utilityメソッド"

        ''' <summary>
        '''  Converts byte array to serialized base64 string
        ''' </summary>
        ''' <param name="arrayData">byte array</param>
        ''' <returns>base64 string</returns>
        Public Shared Function SerializeToBase64String(arrayData As Byte()) As String
            Dim base64String As String = String.Empty
            If arrayData IsNot Nothing Then
                CustomEncode.ToBase64String(arrayData)
            End If
            Return base64String
        End Function

#End Region

#Region "非同期タスクの投入"

        ''' <summary>
        ''' Inserts asynchronous task information to the database
        ''' </summary>
        ''' <returns>AsyncProcessingServiceParameterValue</returns>
        Public Function InsertData() As AsyncProcessingServiceParameterValue
            ' Create array data to serilize.
            Dim arrayData As Byte() = {1, 2, 3, 4, 5}

            ' Sets parameters of AsyncProcessingServiceParameterValue to insert asynchronous task information.
            Dim asyncParameterValue As New AsyncProcessingServiceParameterValue(
                "AsyncProcessingService", "InsertTask", "InsertTask", "SQL",
                New MyUserInfo("AsyncProcessingService", "AsyncProcessingService"))

            asyncParameterValue.UserId = "A"
            asyncParameterValue.ProcessName = "AAA"
            asyncParameterValue.Data = Program.SerializeToBase64String(arrayData)
            asyncParameterValue.ExecutionStartDateTime = DateTime.Now
            asyncParameterValue.RegistrationDateTime = DateTime.Now
            asyncParameterValue.NumberOfRetries = 0
            asyncParameterValue.ProgressRate = 0
            asyncParameterValue.CompletionDateTime = DateTime.Now
            asyncParameterValue.StatusId = CInt(AsyncProcessingServiceParameterValue.AsyncStatus.Register)
            asyncParameterValue.CommandId = 0
            asyncParameterValue.ReservedArea = "xxxxxx"

            Dim iso As DbEnum.IsolationLevelEnum = DbEnum.IsolationLevelEnum.DefaultTransaction
            Dim asyncReturnValue As AsyncProcessingServiceReturnValue

            ' Execute do business logic method.
            Dim layerB As New LayerB()
            asyncReturnValue = DirectCast(layerB.DoBusinessLogic(DirectCast(asyncParameterValue, AsyncProcessingServiceParameterValue), iso), AsyncProcessingServiceReturnValue)
            Return asyncParameterValue
        End Function

#End Region

    End Class
End Namespace
