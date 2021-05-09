Public NotInheritable Class AboutBox1

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = "Sound Mixer"
        Me.LabelVersion.Text = "V 1.0"
        Me.LabelCopyright.Text = My.Application.Info.Copyright & " Ambusy"
        Me.LabelCompanyName.Text = ""
        Me.TextBoxDescription.Text = "Typically composes a stream of (music) items from single items (songs)," & vbCrLf &
            "the announcer's recorded presentation and advertisements." & vbCrLf &
            "You mix songs and selected parts of announcements" & vbCrLf &
            "on a time line with overlaps, where necessary," & vbCrLf &
            "verify and produce the resulting wav file." & vbCrLf &
            "The 3 input stream panels are governed with popup menus" & vbCrLf &
            "The output stream is governed in the top menu"
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
