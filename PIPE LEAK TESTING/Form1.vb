Imports System.IO.Ports
Imports System.Drawing.Text
Imports System.IO

Public Class Form1
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form2.Show()
    End Sub
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Button3.Visible = False
        Button4.Visible = False

        DATAGRID()
        Dim dt2 As DataTable = GetData("SELECT ID,ModelName FROM `tabModelSet` ")
        ComboBox1.DataSource = dt2
        ComboBox1.ValueMember = "ID"
        ComboBox1.DisplayMember = "ModelName"

        'Me.Hide()
        Dim dt As DataTable = GetData("SELECT * FROM `port` WHERE `Id` = 1")
        If dt.Rows.Count() > 0 Then
            Port1 = dt.Rows(0).Item("Port1")
            ToolStripStatusLabel4.Text = "Connected To : " & Port1
            PortConnect()   'serial Port Function
        End If

        Timer1.Start()
        Label3.Text = HEADING
        FullImage()
        'Form2.Show()

        Timer2.Enabled = True
        Timer2.Interval = 1000


        Timer3.Interval = 2000
        GroupBox3.Visible = False
        ComboBox1.Visible = False

        Dim dt3 As DataTable = GetData("SELECT * FROM `tabModelReport` order by ID desc")
        If dt3.Rows.Count() > 0 Then
            OKcount = dt3.Rows(0).Item("OKCounter")
            NGcount = dt3.Rows(0).Item("NGCounter")
            SerialNo = dt3.Rows(0).Item("SerialNo")
            TextBox1.Text = dt3.Rows(0).Item("ModelName")
            TextBox2.Text = dt3.Rows(0).Item("ModelName")

            TextBox9.Text = OKcount
            TextBox10.Text = NGcount
            TextBox11.Text = OKcount + NGcount

        End If
        GroupBox3.Visible = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        Label2.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
        Label1.Text = System.DateTime.Now.ToString("HH:mm:ss tt")
        ToolStripStatusLabel3.Text = System.DateTime.Now.ToString("HH:mm:ss")
        ToolStripStatusLabel2.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
    End Sub
    Public buff_rx As Byte
    Public Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        'SerialPort1.ReadTimeout = 20

        Do While SerialPort1.BytesToRead > 0
            Try
                buff_rx = SerialPort1.ReadByte
                If (buff_rx <> 0) Then
                    Me.Invoke(New EventHandler(AddressOf update_dat))
                End If

            Catch ex As Exception

            End Try
            ' Application.DoEvents()
        Loop

    End Sub


    Dim arr(12) As String
    Dim count1 As Integer = 1
    Dim str19 As String
    Dim dt As String = System.DateTime.Now.ToString("dd/MM/yyyy") & " " & System.DateTime.Now.ToString("HH:mm:ss tt")
    Dim joinstr As String
    Dim checkjoinstr As String
    Dim OKcount As Integer = 0
    Dim NGcount As Integer = 0
    Dim SerialNo As String
    Dim fstr As String
    Public Sub update_dat(ByVal sender As Object, ByVal e As System.EventArgs)

        If buff_rx <> 0 Then
            Dim i2 As Integer = buff_rx
            Dim x2 As String = i2.ToString("X2") + " "
            Dim str As String = Convert.ToChar(buff_rx).ToString()
            'RichTextBox1.AppendText(str)

            fstr = fstr & str
            Label17.Text = fstr.Length

            '0.059	$$.050	c$$FIX-01$$OK$$000D
        End If
        

        If fstr.Length = 32 Then
            RichTextBox2.Text = ""
            Label18.Text = fstr.Length
            RichTextBox2.AppendText(fstr & Environment.NewLine)
            'RichTextBox2.AppendText(Environment.NewLine)

            'ToolStripStatusLabel5.Text = fstr.Trim

            VLR = fstr.Substring(0, 5)
            PLR = fstr.Substring(8, 4)
            FixNo = fstr.Substring(16, 6)
            Result = fstr.Substring(24, 2)
            SerialNo = CStr(CInt("&H" & fstr.Substring(28, 4))).PadLeft(4, "0")

            Dim datetime1 As String = System.DateTime.Now.ToString("dd/MM/yyyy") & " " & System.DateTime.Now.ToString("HH:mm:ss")

            TextBox3.Text = datetime1
            TextBox4.Text = FixNo
            TextBox5.Text = SerialNo
            TextBox6.Text = VLR
            TextBox7.Text = PLR
            TextBox8.Text = Result

            Label36.Text = Result

            If Result = "OK" Then
                Label36.BackColor = Color.Green
                TextBox9.Text = CInt(TextBox9.Text) + 1
            ElseIf Result = "NG" Then
                Label36.BackColor = Color.Red
                TextBox10.Text = CInt(TextBox10.Text) + 1
            End If


            TextBox11.Text = CInt(TextBox9.Text) + CInt(TextBox10.Text)

            ModelName = TextBox1.Text
            Dim query As String = "INSERT INTO tabModelReport(Result, OKCounter, NGCounter, PLCString, FixtureName, LeakRate1, LeakRate2, SerialNo, ModelName) Values ('" & Result & "', '" & CInt(TextBox9.Text) & "', '" & CInt(TextBox10.Text) & "', '" & fstr & "', '" & FixNo & "', '" & VLR & "', '" & PLR & "', '" & SerialNo & "', '" & ModelName & "')"
            InsertQuery(query)


            'CREATE FOLDER AND CSV Andon 
            Dim folderPath As String = "C:\Reports\" + DateTime.Now.ToString("yyyy") + "\" + DateTime.Now.ToString("MM") + "\"
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            Else
                Dim Exists_File As String = folderPath + CStr(DateTime.Now.ToString("dd-MM-yyy")) + ".csv"
                If Not File.Exists(Exists_File) Then 'IF CSV FILE EXISTS THEN DATA ADD
                    'My.Computer.FileSystem.WriteAllText(Exists_File, "LogTime, Fixture Name, Serial No, Vacuum Test Leak Rate, Pressure Test Leak Rate, Result" & vbCrLf & "", False) 'add
                    My.Computer.FileSystem.WriteAllText(Exists_File, "LogTime, Fixture Name, Serial No, Vacuum Test Leak Rate, Pressure Test Leak Rate, Result" & vbCrLf & "'" & System.DateTime.Now.ToString("yyyy-MM-dd") + " " + System.DateTime.Now.ToString("HH:mm:ss") & "','" & FixNo & "','" & SerialNo & "','" & VLR & "','" & PLR & "','" & Result & "'" & "" & vbCrLf & "", False) 'add
                Else
                    File.AppendAllText(Exists_File, "'" & System.DateTime.Now.ToString("dd-MM-yyyy") + " " + System.DateTime.Now.ToString("HH:mm:ss") & "','" & FixNo & "','" & SerialNo & "','" & VLR & "','" & PLR & "','" & Result & "'" & "" & vbCrLf & "")

                End If
            End If

            datagriddataview()

            Date1 = System.DateTime.Now.ToString("ddMMyy")
            Time1 = System.DateTime.Now.ToString("HHmmss")

            FullImage()

            fstr = ""

            Timer3.Interval = 1000
            Timer3.Enabled = True
        End If

    End Sub




    Public Sub FullImage()

        If ModelName <> "" And Time1 <> "" Then
            Dim bitmap As Bitmap = New Bitmap(1181, 318, System.Drawing.Imaging.PixelFormat.Format32bppArgb)

            'Dim bitmap As Bitmap = New Bitmap(567, 612, System.Drawing.Imaging.PixelFormat.Format64bppArgb)
            bitmap.SetResolution(300, 300)
            Dim graphics As Graphics = graphics.FromImage(bitmap)


            graphics.Clear(Color.White)

            graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality


            Dim myPen As Pen

            'instantiate a new pen object using the color structure
            myPen = New Pen(Color.Black, Width = 0.1)
            graphics.DrawString("PSA " & ModelName & " AOOBYI 03", New Font("Arial", 15, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(100, 50))
            graphics.DrawString("VACUUM LEAK RATE: " & VLR & " cm3.mm", New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(150, 140))
            graphics.DrawString("PRESSURE LEAK RATE: " & PLR & " cm3/mm", New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(135, 180))
            graphics.DrawString("DATE: " & Date1, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(200, 230))
            graphics.DrawString("TIME: " & Time1, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(550, 230))
            graphics.DrawString(Result, New Font("Arial", 15, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(900, 140))
            graphics.DrawString(FixNo, New Font("Arial", 15, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(850, 200))
            PictureBox1.BackgroundImage = bitmap
            PictureBox1.BackgroundImage.Save("C:\C_Barcode\ Label Print" & ModelName & " " & Time1 & ".Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg)
            Barcode_print()  'print
            Time1 = ""

        End If


    End Sub
    Public PrintDoc As Printing.PrintDocument = New Printing.PrintDocument()
    Public pd_PrintDialog As New PrintDialog
    Public Sub PrintDocHandler(ByVal sender As Object, ByVal ev As Printing.PrintPageEventArgs)
        ev.Graphics.DrawImage(PictureBox1.BackgroundImage, ev.Graphics.VisibleClipBounds)
    End Sub
    Public Function Barcode_print()



        PrintDoc.PrintController = New System.Drawing.Printing.StandardPrintController

        AddHandler PrintDoc.PrintPage, AddressOf PrintDocHandler
        'Print
        PrintDoc.Print()

        Return Nothing
    End Function


    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim row As DataRowView = DirectCast(ComboBox1.SelectedItem, DataRowView)

        Dim dt As DataTable = GetData("SELECT * FROM `tabModelSet` WHERE ID = " & row("ID").ToString & "")
        If dt.Rows.Count() > 0 Then
            TextBox1.Text = dt.Rows(0).Item("ModelName")
            TextBox2.Text = dt.Rows(0).Item("PartNumber")

        End If
        GroupBox3.Visible = False
        ComboBox1.Visible = False
        Label16.Text = ""
    End Sub


    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click

    End Sub


    Public Function DATAGRID()
        'Set AutoGenerateColumns False.
        DataGridView1.AutoGenerateColumns = False

        DataGridView1.ColumnCount = 6

        'Add Columns.
        DataGridView1.Columns(0).Name = "LogTime"
        DataGridView1.Columns(1).Name = "Fixture Name"
        DataGridView1.Columns(2).Name = "Serial No"
        DataGridView1.Columns(3).Name = "Vaccum Test Leak Rate."
        DataGridView1.Columns(4).HeaderText = "Pressure Test Leak Rate"
        DataGridView1.Columns(5).HeaderText = "Result"

        DataGridView1.Columns(0).Width = 140
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 120
        DataGridView1.Columns(3).Width = 120
        DataGridView1.Columns(4).Width = 120
        DataGridView1.Columns(5).Width = 120
        DataGridView1.RowHeadersVisible = False


        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Trebuchet MS", 10, FontStyle.Bold)
        DataGridView1.GridColor = Color.Black


        datagriddataview()
        Return Nothing



    End Function
    Dim count As Integer
    Dim listarr(6) As String
    Public Function datagriddataview()
        DataGridView1.Rows.Clear()
        Dim currentdt As String = System.DateTime.Now.ToString("yyyy-MM-dd")

        'Dim query As String = "Select iif(Status='1','1') As CallTime, iif(Status='0', '0') as AckTime, DateTime1 as DiffMin from History" & items & " WHERE DateTime1 BETWEEN '" & fromdt & " 00:00:00' AND '" & todt & " 23:59:59' ORDER BY `DateTime1` DESC"
        'Dim query As String = "Select * from tabModelReport WHERE LogTime BETWEEN #" & fromdt & " 00:00:00# AND #" & todt & " 23:59:59#"

        Dim query As String = "Select * from tabModelReport WHERE LogTime BETWEEN #" & currentdt & " 00:00:00# AND #" & currentdt & " 23:59:59# order by Id desc"

        conn.Open()
        Dim cmd As New OleDb.OleDbCommand(query, conn)
        Dim rd As OleDb.OleDbDataReader
        Try
            rd = cmd.ExecuteReader

            count = 1
            While rd.Read

                listarr(0) = rd("LogTime").ToString
                listarr(1) = rd("FixtureName").ToString
                listarr(2) = rd("SerialNo").ToString
                listarr(3) = rd("LeakRate1").ToString
                listarr(4) = rd("LeakRate2").ToString
                listarr(5) = rd("Result").ToString

                DataGridView1.Rows.Add(listarr)

                count = count + 1
               

            End While
            rd.Close()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
        conn.Close()

        Return Nothing
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox9.Text = 0
        Button3.Visible = False
        Button4.Visible = False
        GroupBox3.Visible = False
        ComboBox1.Visible = False
        Label16.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        GroupBox3.Visible = True
        TextBox12.Clear()
        
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If TextBox12.Text = "0007" Then
            Label16.Text = "Select Model.."
            Label16.ForeColor = Color.Green
            ComboBox1.Visible = True
            TextBox12.Clear()
            Button3.Visible = True
            Button4.Visible = True
        Else
            Label16.Text = "Enter correct Password.."
            Label16.ForeColor = Color.Red
            TextBox12.Clear()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        TextBox10.Text = 0
        Button3.Visible = False
        Button4.Visible = False
        GroupBox3.Visible = False
        ComboBox1.Visible = False
        Label16.Text = ""
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub Timer3_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        Label36.BackColor = Color.Black
        Timer3.Enabled = False
    End Sub
End Class