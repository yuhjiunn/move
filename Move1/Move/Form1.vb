Imports TwinCAT.Ads
Imports System.IO
Imports System
Imports System.Text
Imports System.Collections


Public Class Form1

    Dim tcClient As New TcAdsClient
    Dim dataStream As New AdsStream(2)
    Dim binReader As New BinaryReader(dataStream)
    Dim tcClientState As TwinCAT.Ads.StateInfo
    Dim Xray As Integer
    Dim automode As Integer
    Dim passS11 As Integer
    Dim OperationDuration As Integer
    Dim xSpeed As Integer
    Dim myfolder As String
    Dim newfolder As String
    Dim counter As Integer
    Dim savePath, savePath2 As String
    Dim hh, mm, ss As Integer
    Dim filename As String = "C:\Move\config_v1.txt"
    Dim OpenCMD

    Public Function ReadALine(ByVal File_Path As String, ByVal TotalLine As Integer, ByVal Line2Read As Integer) As String
        Dim Buffer As Array
        Dim Line As String
        If TotalLine <= Line2Read Then
            Return "No Such Line"
        End If
        Buffer = File.ReadAllLines(File_Path)
        Line = Buffer(Line2Read)
        Return Line
    End Function

    Public Function GetNumberOfLines(ByVal file_path As String) As Integer
        Dim sr As New StreamReader(file_path)
        Dim NumberOfLines As Integer
        Do While sr.Peek >= 0
            sr.ReadLine()
            NumberOfLines += 1
        Loop
        Return NumberOfLines
        sr.Close()
        sr.Dispose()
    End Function



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox1.Text = (ReadALine(filename, GetNumberOfLines(filename), 0)) 'OCR'
        TextBox2.Text = (ReadALine(filename, GetNumberOfLines(filename), 1)) 'X-ray'
        TextBox3.Text = (ReadALine(filename, GetNumberOfLines(filename), 2)) 'RPM-object'
        TextBox4.Text = (ReadALine(filename, GetNumberOfLines(filename), 3)) 'RPM-alarm'
        TextBox5.Text = (ReadALine(filename, GetNumberOfLines(filename), 4)) 'WIM'
        TextBox6.Text = (ReadALine(filename, GetNumberOfLines(filename), 5)) 'reserve'
        TextBox7.Text = (ReadALine(filename, GetNumberOfLines(filename), 6)) 'server'
        TextBox8.Text = (ReadALine(filename, GetNumberOfLines(filename), 7)) '0101/0102/0103'
        TextBox9.Text = (ReadALine(filename, GetNumberOfLines(filename), 8)) 'AMS NetID'
        Button1.PerformClick()
        'OpenCMD = CreateObject("wscript.shell")
        'OpenCMD.run((ReadALine(filename, GetNumberOfLines(filename), 8)))
        'OpenCMD.run((ReadALine(filename, GetNumberOfLines(filename), 9)))
        'OpenCMD.run((ReadALine(filename, GetNumberOfLines(filename), 10)))
        'OpenCMD.run((ReadALine(filename, GetNumberOfLines(filename), 11)))
        'OpenCMD.run((ReadALine(filename, GetNumberOfLines(filename), 12)))

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
           
            tcClient.Read(automode, dataStream)
            Label7.Text = binReader.ReadBoolean.ToString
            Label5.Text = tcClient.ReadAny(passS11, GetType(Boolean)).ToString
            Label6.Text = tcClient.ReadAny(OperationDuration, GetType(String), New Integer() {80}).ToString
            dataStream.Position = 0
            Label22.Text = tcClient.ReadAny(Xray, GetType(Boolean)).ToString
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
            'Application.Exit()
        End Try

        ' X-ray
        Dim xray_count = My.Computer.FileSystem.GetFiles(TextBox2.Text)
        Label19.Text = CStr(xray_count.Count)   ' get the file count in xray png folder

        'If Label7.Text = "True" And Label5.Text = "True" Then    ' automode is ON and passS11
        '    'OperationDuration > 20000
        '    'Or(OperationDuration > ReadALine(filename, GetNumberOfLines(filename), 13) And Label5.Text = "True")
        '    ' If CStr(xray_count.Count) > ReadALine(filename, GetNumberOfLines(filename), 9) Then
        '    Threading.Thread.Sleep((ReadALine(filename, GetNumberOfLines(filename), 10)))
        '    Button4.PerformClick()
        'End If

        If Label7.Text = "True" And CStr(xray_count.Count) > ReadALine(filename, GetNumberOfLines(filename), 9) Then    ' automode is ON and more than file count
            'Or(OperationDuration > ReadALine(filename, GetNumberOfLines(filename), 13) And Label5.Text = "True")
            ' If CStr(xray_count.Count) > ReadALine(filename, GetNumberOfLines(filename), 9) Then
            Threading.Thread.Sleep((ReadALine(filename, GetNumberOfLines(filename), 10)))
            Button4.PerformClick()
        End If


    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        myfolder = TextBox8.Text + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
        Label10.Text = myfolder
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        counter = counter + 1
        Label13.Text = counter
        savePath = TextBox7.Text + myfolder + counter.ToString("0000")
        IO.Directory.CreateDirectory(savePath)


        'X-ray
            Dim filelist1 = My.Computer.FileSystem.GetFiles(TextBox2.Text$)
            For Each foundfile In filelist1
                My.Computer.FileSystem.MoveFile(foundfile, savePath & "\" & My.Computer.FileSystem.GetFileInfo(foundfile).Name)
            Next


        ' OCR
        Dim filelist = My.Computer.FileSystem.GetFiles(TextBox1.Text$)
        For Each foundfile In filelist
            My.Computer.FileSystem.MoveFile(foundfile, savePath & "\" & My.Computer.FileSystem.GetFileInfo(foundfile).Name)
        Next


        'WIM
        Dim filelist4 = My.Computer.FileSystem.GetFiles(TextBox5.Text$)
        For Each foundfile In filelist4
            My.Computer.FileSystem.MoveFile(foundfile, savePath & "\" & My.Computer.FileSystem.GetFileInfo(foundfile).Name)
        Next


        'reserve
        Dim filelist5 = My.Computer.FileSystem.GetFiles(TextBox6.Text$)
        For Each foundfile In filelist5
            My.Computer.FileSystem.MoveFile(foundfile, savePath & "\" & My.Computer.FileSystem.GetFileInfo(foundfile).Name)
        Next


    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        counter = TextBox10.Text
        Label13.Text = counter
    End Sub



    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        hh = DateTime.Now.Hour.ToString()
        mm = DateTime.Now.Minute.ToString()
        ss = DateTime.Now.Second.ToString()


        If (hh = "00") And (mm = "00") And ((ss = "00") Or (ss = "01") Or (ss = "02")) Then
            counter = 0
            Label13.Text = counter
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        counter = TextBox10.Text
        tcClient = New TcAdsClient
        tcClientState = New TwinCAT.Ads.StateInfo
        Try

            tcClient.Connect(TextBox9.Text, 801)
            tcClientState = tcClient.ReadState()

            If tcClientState.AdsState = 5 Then
                MsgBox("successfully connected to plc", MsgBoxStyle.Information, "info")
            Else
                MsgBox("connection fail to plc", MsgBoxStyle.Critical, "error")
                'If MsgBoxResult.Ok Then
                '    Application.Exit()
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Try
            Xray = tcClient.CreateVariableHandle(".outputxray")
            automode = tcClient.CreateVariableHandle(".StsOperationIsFullScanStart")
            passS11 = tcClient.CreateVariableHandle(".StsVehicleDetectPassS11")
            OperationDuration = tcClient.CreateVariableHandle(".operationdurationcrtstr")
            xSpeed = tcClient.CreateVariableHandle(".stsspeedatobstr")
            Timer1.Start()
        Catch ex As Exception
            MsgBox("fail to link to twincat variable")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick

        savePath2 = (ReadALine(filename, GetNumberOfLines(filename), 12))
        Dim RPMtag = (ReadALine(filename, GetNumberOfLines(filename), 11))

        'object RPM

        Dim filelist2 = My.Computer.FileSystem.GetFiles(TextBox3.Text$)
        Dim newpath As String

        For Each foundfile In filelist2
            newpath = Path.GetFileNameWithoutExtension(foundfile) + RPMtag
            My.Computer.FileSystem.RenameFile(foundfile, newpath)
            Threading.Thread.Sleep(1000)
            File.Move(TextBox3.Text + Path.GetFileName(newpath), savePath2 + Path.GetFileName(newpath))
        Next


        'alarm RPM
        Dim filelist3 = My.Computer.FileSystem.GetFiles(TextBox4.Text$)
        Dim newpath_alarm As String

        For Each foundfile In filelist3
            newpath_alarm = Path.GetFileNameWithoutExtension(foundfile) + RPMtag
            My.Computer.FileSystem.RenameFile(foundfile, newpath_alarm)
            Threading.Thread.Sleep(1000)
            File.Move(TextBox4.Text + Path.GetFileName(newpath_alarm), savePath2 + Path.GetFileName(newpath_alarm))
        Next


    End Sub

  
End Class

    