Imports System.Drawing.Text
Imports MessagingToolkit.QRCode.Codec
Public Class Form3
    Dim Bstr As String = "68626555AA098570056800554100001006189500000"
    Dim P1 As String
    Dim SR As String
    Dim SC As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'ModelName = "XYZ"
        'Time1 = "safasdfds"

       

    End Sub
    Public QRcode_Image As Image
    Public Function qrcodeGen(ByVal QRCode_Text As String)

        Try
            Dim qrCode As New QRCodeEncoder
            qrCode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE
            qrCode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
            'Form1.PictureBox2.Image = qrCode.Encode(QRCode_Text, System.Text.Encoding.UTF8)

            QRcode_Image = qrCode.Encode(QRCode_Text, System.Text.Encoding.UTF8)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        Return QRcode_Image
    End Function

    Public Sub FullImage()

        If P1 <> "" And SR <> "" Then
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


            graphics.DrawImage(qrcodeGen(Bstr), 100, 65, 200, 200)
            graphics.DrawImage(Image.FromFile("C:\Test\mopar.jpg"), 400, 65, 200, 200)
            graphics.DrawString(P1, New Font("Arial", 15, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(700, 50))
            graphics.DrawString(SR, New Font("Arial", 15, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(700, 125))
            graphics.DrawString(SC, New Font("Arial", 18, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(700, 200))
            PictureBox1.BackgroundImage = bitmap
            PictureBox1.BackgroundImage.Save("C:\C_Barcode\ Label Print" & P1 & " " & SR & ".Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg)
            'Barcode_print()  'print
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

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        P1 = Bstr.Substring(0, 10)
        SR = Bstr.Substring(21, 10)
        SC = Bstr.Substring(31, 7)

        FullImage()
    End Sub
End Class