Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO.Ports
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.IO
Module Module1

    'Public conn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Test\PIPE_LEAK_TESTING_2204_DB.mdb;pwd=jaihons")
    Public conn As New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Test\PIPE_LEAK_TESTING_2204_DB.mdb;Jet OLEDB:Database Password=jaihons;")

    Public Port1 As String
    Public HEADING As String = "LEAKAGE TESTING MACHINE DATA LOGGER"

    Public VLR As String
    Public PLR As String
    Public Date1 As String
    Public Time1 As String
    Public Result As String
    Public FixNo As String
    Public ModelName As String
    Public UAccess As Integer

    Public Function PortConnect()

        Try
            Form1.SerialPort1.PortName = Port1
            Form1.SerialPort1.BaudRate = "9600"
            Form1.SerialPort1.DataBits = 8
            Form1.SerialPort1.StopBits = IO.Ports.StopBits.One
            Form1.SerialPort1.Parity = IO.Ports.Parity.None
            Form1.SerialPort1.Handshake = Handshake.None

            Form1.SerialPort1.Open()

        Catch ex As Exception
            'MsgBox(ErrorToString, MsgBoxStyle.Information, Title:=Port1 & " Commucation Alert")
            Form1.ToolStripStatusLabel4.Text = "Connection Error " & Port1
        End Try

        Return Nothing

    End Function

    'create new DataTable object to hold results
    Public Function GetData(ByVal query As String) As DataTable
        Dim cmd As OleDb.OleDbCommand = New OleDb.OleDbCommand(query)
        Using sda As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter()
            cmd.Connection = conn
            sda.SelectCommand = cmd
            Using dt As DataTable = New DataTable()
                sda.Fill(dt)
                Return dt
            End Using
        End Using
    End Function

    'insert funcation
    Public Sub InsertQuery(ByVal SQCommand As String)
        Dim SQLCMD As New OleDb.OleDbCommand(SQCommand, conn)
        conn.Open()
        Application.DoEvents()
        SQLCMD.ExecuteNonQuery()
        Application.DoEvents()
        conn.Close()
    End Sub


    


End Module
