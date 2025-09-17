Public Class Form1
    Private isResult As Boolean = False
    Private justPressedOperator As Boolean = False
    Private lastResult As Double = 0
    Private operation As String

    Private Sub btp_Click(sender As Object, e As EventArgs) Handles btp.Click
        If Not txtdisplay.Text.Contains(".") Then
            If txtdisplay.Text = "" Then
                txtdisplay.Text = "0."
            Else
                txtdisplay.Text &= "."
            End If
        End If
    End Sub

    ' === Operator Buttons (+ - × ÷) ===
    Private Sub btnOperator_Click(sender As Object, e As EventArgs) _
        Handles bta.Click, bts.Click, btt.Click, btd.Click

        Dim btn As Button = CType(sender, Button)
        Dim op As String = btn.Text

        ' Case 1: Start with negative number
        If txtdisplay.Text = "" AndAlso op = "-" Then
            txtdisplay.Text = "-"
            isResult = False
            justPressedOperator = False
            Return
        End If

        ' Case 2: Allow negative numbers after operator
        If txtdisplay.Text.EndsWith(" ") AndAlso op = "-" Then
            txtdisplay.Text &= "-"
            isResult = False
            justPressedOperator = False
            Return
        End If

        ' Case 3: If last was a result, continue from it
        If isResult Then
            txtdisplay.Text = lastResult.ToString() & " " & op & " "
            isResult = False
            justPressedOperator = True
            Return
        End If

        ' Case 4: Replace or append operator
        If txtdisplay.Text.EndsWith(" ") Then
            txtdisplay.Text = txtdisplay.Text.Substring(0, txtdisplay.Text.Length - 3) & " " & op & " "
        ElseIf txtdisplay.Text <> "" AndAlso txtdisplay.Text <> "-" Then
            txtdisplay.Text &= " " & op & " "
        End If

        justPressedOperator = True
    End Sub

    ' === Clear Button (C) ===
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btc.Click
        txtdisplay.Clear()
        lastResult = 0
        operation = ""
        isResult = False
        justPressedOperator = False
    End Sub

    ' === Equal Button 😊) ===
    Private Sub bte_Click(sender As Object, e As EventArgs) Handles bte.Click
        Dim expr As String = txtdisplay.Text.Trim()
        If expr = "" Then Return

        expr = expr.Replace("×", "*").Replace("÷", "/")

        Try
            Dim dt As New DataTable()
            Dim result = dt.Compute(expr, Nothing)
            lastResult = Convert.ToDouble(result)

            txtdisplay.Text = lastResult.ToString()
            isResult = True
            justPressedOperator = False
        Catch ex As Exception
            MessageBox.Show("You cant put operator on hold")
        End Try
    End Sub

    ' === Backspace Button (⌫) ===
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles mali.Click
        If isResult Then Exit Sub
        If txtdisplay.Text.Length > 0 Then
            txtdisplay.Text = txtdisplay.Text.Substring(0, txtdisplay.Text.Length - 1)
        End If
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        Select Case e.KeyChar
        ' === Equal / Enter ===
            Case ChrW(Keys.Enter), "="c
                bte.PerformClick()
                e.Handled = True

        ' === Numbers 0–9 ===
            Case "0"c To "9"c
                Dim btnName As String = "bt" & e.KeyChar
                Dim btn As Button = Me.Controls.Find(btnName, True).FirstOrDefault()
                If btn IsNot Nothing Then btn.PerformClick()
                e.Handled = True

        ' === Operators (+ - × ÷) ===
            Case "+"c, "-"c, "*"c, "/"c
                Dim btnName As String = ""
                Select Case e.KeyChar
                    Case "+"c : btnName = "bta"
                    Case "-"c : btnName = "bts"
                    Case "*"c : btnName = "btt"
                    Case "/"c : btnName = "btd"
                End Select
                Dim btn As Button = Me.Controls.Find(btnName, True).FirstOrDefault()
                If btn IsNot Nothing Then btn.PerformClick()
                e.Handled = True

        ' === Decimal Point ===
            Case "."c
                btp.PerformClick()
                e.Handled = True

        ' === Backspace (⌫) ===
            Case ChrW(Keys.Back)
                mali.PerformClick()
                e.Handled = True

        ' === Clear (C or c) ===
            Case "c"c, "C"c
                btc.PerformClick()
                e.Handled = True
        End Select
    End Sub
    ' === Number Buttons (0–9) ===
    Private Sub NumberButton_Click(sender As Object, e As EventArgs) Handles bt0.Click, bt1.Click, bt2.Click, bt3.Click, bt4.Click, bt5.Click, bt6.Click, bt7.Click, bt8.Click, bt9.Click
        Dim btn As Button = CType(sender, Button)

        If isResult Then
            ' After = and typing a number → new calculation
            txtdisplay.Text = btn.Text
            isResult = False
            justPressedOperator = False
        ElseIf justPressedOperator Then
            ' After operator → append number
            txtdisplay.Text &= btn.Text
            justPressedOperator = False
        Else
            ' Normal typing
            txtdisplay.Text &= btn.Text
        End If
    End Sub
End Class