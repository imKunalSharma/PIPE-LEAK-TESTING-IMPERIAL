Public Class Form2
    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Form1.SerialPort1.Close()
            Login.Close()

        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If UAccess = 0 Then
            Button1.Enabled = False
            Button1.BackColor = Color.Gray
        ElseIf UAccess = 1 Then
            Button1.Enabled = False
            Button1.BackColor = Color.Gray
        ElseIf UAccess = 2 Then
            Button1.Enabled = True
        End If

        

    End Sub

    Private Sub RichTextBox2_TextChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button1_MouseHover(sender As Object, e As System.EventArgs) Handles Button1.MouseHover
        Button1.BackColor = Color.Yellow
    End Sub
    Private Sub Button1_MouseLeave(sender As Object, e As System.EventArgs) Handles Button1.MouseLeave
        Button1.BackColor = Color.White
    End Sub
    
    Private Sub Button2_MouseHover(sender As Object, e As System.EventArgs) Handles Button2.MouseHover
        Button2.BackColor = Color.Yellow
    End Sub
    Private Sub Button2_MouseLeave(sender As Object, e As System.EventArgs) Handles Button2.MouseLeave
        Button2.BackColor = Color.White
    End Sub
    
    Private Sub Button3_MouseHover(sender As Object, e As System.EventArgs) Handles Button3.MouseHover
        Button3.BackColor = Color.Yellow
    End Sub

    Private Sub Button3_MouseLeave(sender As Object, e As System.EventArgs) Handles Button3.MouseLeave
        Button3.BackColor = Color.White
    End Sub
    Private Sub Button4_MouseHover(sender As Object, e As System.EventArgs) Handles Button4.MouseHover
        Button4.BackColor = Color.Yellow
    End Sub

    Private Sub Button4_MouseLeave(sender As Object, e As System.EventArgs) Handles Button4.MouseLeave
        Button4.BackColor = Color.White
    End Sub
    Private Sub Button5_MouseHover(sender As Object, e As System.EventArgs) Handles Button5.MouseHover
        Button5.BackColor = Color.Yellow
    End Sub

    Private Sub Button5_MouseLeave(sender As Object, e As System.EventArgs) Handles Button5.MouseLeave
        Button5.BackColor = Color.White
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If MessageBox.Show("Are you sur to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Form1.SerialPort1.Close()
            Environment.Exit(0)
            Login.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        If UAccess = 0 Then
            Form1.Button3.Visible = False
            Form1.Button4.Visible = False
        ElseIf UAccess = 1 Then
            Form1.Button3.Visible = True
            Form1.Button4.Visible = True
        ElseIf UAccess = 2 Then
            Form1.Button3.Visible = True
            Form1.Button4.Visible = True
        End If

        Form1.Show()


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Visible = False
        Report.Show()
    End Sub
End Class