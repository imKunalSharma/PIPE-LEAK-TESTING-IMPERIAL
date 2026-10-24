Imports System.Text
Imports System.IO

Public Class Report
    Private Sub Report_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Form2.Show()
    End Sub
    Private Sub Report_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Button2.Enabled = False

    End Sub
    Dim count As Integer
    Dim listarr(9) As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Set AutoGenerateColumns False.
        DataGridView1.AutoGenerateColumns = False

        DataGridView1.ColumnCount = 9

        'Add Columns.
        DataGridView1.Columns(0).Name = "ID"
        DataGridView1.Columns(1).Name = "LoginUser"
        DataGridView1.Columns(2).Name = "Date"
        DataGridView1.Columns(3).Name = "Time"
        DataGridView1.Columns(4).Name = "Fixture Name"
        DataGridView1.Columns(5).Name = "Serial No"
        DataGridView1.Columns(6).Name = "Vaccum Test Leak Rate."
        DataGridView1.Columns(7).HeaderText = "Pressure Test Leak Rate"
        DataGridView1.Columns(8).HeaderText = "Result"

        DataGridView1.Columns(0).Width = 140
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 120
        DataGridView1.Columns(3).Width = 120
        DataGridView1.Columns(4).Width = 120
        DataGridView1.Columns(5).Width = 120
        DataGridView1.Columns(6).Width = 120
        DataGridView1.Columns(7).Width = 120
        DataGridView1.Columns(8).Width = 120
        DataGridView1.RowHeadersVisible = False


        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Trebuchet MS", 10, FontStyle.Bold)
        DataGridView1.GridColor = Color.Black


        DataGridView1.Rows.Clear()
        Dim fromdt As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim todt As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")

        'Dim query As String = "Select iif(Status='1','1') As CallTime, iif(Status='0', '0') as AckTime, DateTime1 as DiffMin from History" & items & " WHERE DateTime1 BETWEEN '" & fromdt & " 00:00:00' AND '" & todt & " 23:59:59' ORDER BY `DateTime1` DESC"
        'Dim query As String = "Select * from tabModelReport WHERE LogTime BETWEEN #" & fromdt & " 00:00:00# AND #" & todt & " 23:59:59#"
        Dim value As String = ComboBox1.SelectedItem.ToString
        Dim query As String
        If value = "All" Then
            query = "Select * from tabModelReport WHERE LogTime BETWEEN #" & fromdt & " 00:00:00# AND #" & todt & " 23:59:59# order by Id desc"
        Else
            query = "Select * from tabModelReport WHERE Result = '" & value & "' and LogTime BETWEEN #" & fromdt & " 00:00:00# AND #" & todt & " 23:59:59# order by Id desc"
        End If


        conn.Open()
        Dim cmd As New OleDb.OleDbCommand(query, conn)
        Dim rd As OleDb.OleDbDataReader
        Try
            rd = cmd.ExecuteReader

            count = 1
            While rd.Read

                listarr(0) = count
                listarr(1) = rd("LoginUser").ToString
                listarr(2) = CDate(rd("LogTime").ToString).ToString("yyyy-MM-dd")
                listarr(3) = CDate(rd("LogTime").ToString).ToString("HH:mm:ss")
                listarr(4) = rd("FixtureName").ToString
                listarr(5) = rd("SerialNo").ToString
                listarr(6) = rd("LeakRate1").ToString
                listarr(7) = rd("LeakRate2").ToString
                listarr(8) = rd("Result").ToString

                DataGridView1.Rows.Add(listarr)

                count = count + 1


            End While
            rd.Close()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
        conn.Close()
        Button2.Enabled = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
    Dim filename As String
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        filename = "Date " + DateTimePicker1.Value.ToString("MM dd yyyy") + " To " + DateTimePicker2.Value.ToString("MM dd yyyy") + "  Reports " + DateTime.Now.ToString("dd MM yyyy HHmmss") + ".CSV"
        Dim folderPath As String = "C:\Reports\"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If

        Dim headers = (From header As DataGridViewColumn In DataGridView1.Columns.Cast(Of DataGridViewColumn)() _
              Select header.HeaderText).ToArray
        Dim rows = From row As DataGridViewRow In DataGridView1.Rows.Cast(Of DataGridViewRow)() _
                   Where Not row.IsNewRow _
                   Select Array.ConvertAll(row.Cells.Cast(Of DataGridViewCell).ToArray, Function(c) If(c.Value IsNot Nothing, c.Value.ToString, ""))
        Using sw As New IO.StreamWriter(folderPath & filename)
            sw.WriteLine(String.Join(",", headers))
            For Each r In rows
                sw.WriteLine(String.Join(",", r))
            Next
        End Using
        MsgBox("File Path : " & folderPath & vbCr & " Export Successfully", MsgBoxStyle.Information, Title:="Export in CSV")
    End Sub
End Class