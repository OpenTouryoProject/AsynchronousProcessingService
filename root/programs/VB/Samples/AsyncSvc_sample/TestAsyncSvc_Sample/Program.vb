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
'*  2018/08/24  西野 大介         SampleをBinaryからJSONのSerializeへ変更。
'**********************************************************************************

Imports Newtonsoft.Json

Imports Touryo.Infrastructure.Business.AsyncProcessingService
Imports Touryo.Infrastructure.Business.Util
Imports Touryo.Infrastructure.Framework.AsyncProcessingService
Imports Touryo.Infrastructure.Public.Db

''' <summary>
''' Program class for test user code
''' </summary>
Public Class Program
    ''' <summary>This is the main entry point for the application.</summary>
    Friend Shared Sub Main(args As String())
        Dim program As New Program()
        program.InsertData()
    End Sub

#Region "非同期タスクの投入"

    ''' <summary>
    ''' Inserts asynchronous task information to the database
    ''' </summary>
    ''' <returns>ApsParameterValue</returns>
    Public Function InsertData() As ApsParameterValue
        ' Create list data to json serilize.
        Dim listData As New List(Of Integer)() From {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9
        }

        ' Sets parameters of ApsParameterValue to insert asynchronous task information.
        Dim parameterValue As New ApsParameterValue("AsyncProcessingService", "InsertTask", "InsertTask", "SQL", New MyUserInfo("AsyncProcessingService", "AsyncProcessingService"))

        parameterValue.UserId = "A"
        parameterValue.ProcessName = "AAA"
        parameterValue.Data = JsonConvert.SerializeObject(listData)
        parameterValue.ExecutionStartDateTime = DateTime.Now
        parameterValue.RegistrationDateTime = DateTime.Now
        parameterValue.NumberOfRetries = 0
        parameterValue.ProgressRate = 0
        parameterValue.CompletionDateTime = DateTime.Now
        parameterValue.StatusId = CInt(AsyncStatus.Register)
        parameterValue.CommandId = 0
        parameterValue.ReservedArea = "xxxxxx"

        Dim layerB As New ApsLayerB()
        Dim returnValue As ApsReturnValue = DirectCast(layerB.DoBusinessLogic(DirectCast(parameterValue, ApsParameterValue), DbEnum.IsolationLevelEnum.DefaultTransaction), ApsReturnValue)

        Return parameterValue
    End Function

#End Region
End Class
