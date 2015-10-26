Imports System.Text

Public Class Form1
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click

        If OpenFileDialog1.ShowDialog() <> DialogResult.OK Then Return
        TextBox1.Text = OpenFileDialog1.FileName

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If String.IsNullOrEmpty(TextBox1.Text) Then
            MsgBox("choose file")
            Return
        End If

        'array to list
        Dim list As New List(Of Location)
        Using stream As New System.IO.StreamReader(TextBox1.Text)
            stream.ReadLine() '1行目はヘッダー
            Dim line As String
            Do
                line = stream.ReadLine()
                Dim temp() As String = line.Split(",")
                Dim loc As New Location
                loc.Latitude = CDbl(temp(2).Trim())
                loc.Longitude = CDbl(temp(3).Trim())
                list.Add(loc)
                If stream.EndOfStream Then Exit Do
            Loop
        End Using

        Dim sb As New StringBuilder
        For i As Integer = 0 To list.Count - 1
            sb.AppendLine(list(i).Longitude & "," & list(i).Latitude & ",0.0")
        Next
        'go back to first point
        sb.AppendLine(list(0).Longitude & "," & list(0).Latitude & ",0.0")

        Dim xmlVal As String
        Using stream As New System.IO.StreamReader("template.kml")
            Dim templateString As String = stream.ReadToEnd
            xmlVal = String.Format(templateString, TextBox2.Text, sb.ToString())
        End Using

        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            Using stream As New System.IO.StreamWriter(SaveFileDialog1.FileName)
                stream.Write(xmlVal)
            End Using
        Else
            Exit Sub
        End If

        MsgBox("CSV converted")

    End Sub
End Class
