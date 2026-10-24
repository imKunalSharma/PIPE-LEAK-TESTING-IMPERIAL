Public Class Login

    Private Sub RectangleShape1_Click(sender As System.Object, e As System.EventArgs) Handles RectangleShape1.Click

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

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim dt As DataTable = GetData("SELECT * FROM `tabUserSet` WHERE `UserName` = '" & TextBox1.Text & "' and `UPassword` = '" & TextBox2.Text & "'")
        If dt.Rows.Count() > 0 Then
            uaccess = dt.Rows(0).Item("UAccess")
            If UAccess = 0 Then
                Form2.Show()
                Me.Hide()
            ElseIf UAccess = 1 Then
                Form2.Show()
                Me.Hide()
            ElseIf UAccess = 2 Then
                Form2.Show()
                Me.Hide()
            End If
        Else
            MsgBox("Please Enter Username and Password..", MsgBoxStyle.Information)
        End If

    End Sub
End Class