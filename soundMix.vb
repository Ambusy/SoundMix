Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.IO
Imports System.Threading
Imports CSCore
Imports CSCore.Codecs
Imports CSCore.Codecs.WAV
Imports CSCore.SoundOut
Imports CSCore.Streams
Public Class SoundMix
    Friend Panels(2) As Paneel
    Friend WaveFiles As New List(Of WaveFile)
    Friend PlayList As New List(Of PlayListEntry)
    Friend totalPulsesInList As Integer = 0
    Dim actPanel As Integer
    ReadOnly nrOfPanelsM1 As Integer = Panels.Length - 1
    Dim FormShown As Boolean = False
    Dim workRestoreFileName As String

    Dim continuousSoundPlayer As WasapiOut
    Friend BackgroundPlayer As PlaySoundBuffer
    Friend StopPlayingLoop As Boolean = False
    Friend WavPlayer As PlayWavSoundBuffer
    Friend Playing As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
    End Sub
    Private Sub CPictureBox_MouseDown(sender As Object, e As MouseEventArgs)
        actPanel = CInt(CType(sender, PictureBox).Name.Substring(1, 1))
        setChecked(actPanel)
        MdStartX = e.X
        MdButton = MouseButtons
        If MdButton = MouseButtons.Right Then
            ContextMenuStrip1.Cursor = Cursors.Arrow 'this is the cursor used 
            ContextMenuStrip1 = New ContextMenuStrip
            ContextMenuStrip1.Items.Add("Load", Nothing, AddressOf LoadSoundfileInView)
            ContextMenuStrip1.Items.Add("Add to playlist", Nothing, AddressOf Panels(actPanel).AddPlaylist)
            ContextMenuStrip1.Items.Add("------")
            ContextMenuStrip1.Items.Add("Set view start", Nothing, AddressOf ViewStart)
            ContextMenuStrip1.Items.Add("Set view end", Nothing, AddressOf ViewEnd)
            ContextMenuStrip1.Items.Add("------")
            ContextMenuStrip1.Items.Add("set select start", Nothing, AddressOf SelectStart)
            ContextMenuStrip1.Items.Add("set select end", Nothing, AddressOf SelectEnd)
            ContextMenuStrip1.Items.Add("set select all - CTRLa", Nothing, AddressOf SelectAll)
            ContextMenuStrip1.Items.Add("------")
            ContextMenuStrip1.Items.Add("Zoom in - CTRL+", Nothing, AddressOf ZoomIn)
            ContextMenuStrip1.Items.Add("Zoom out - CTRL-", Nothing, AddressOf ZoomOut)
            ContextMenuStrip1.Items.Add("Zoom all - CTRL0", Nothing, AddressOf ZoomOutall)
            ContextMenuStrip1.Items.Add("------")
            ContextMenuStrip1.Items.Add("Left  - <", Nothing, AddressOf ShiftLeft)
            ContextMenuStrip1.Items.Add("Right - >", Nothing, AddressOf ShiftRight)
            CType(sender, PictureBox).ContextMenuStrip = ContextMenuStrip1
        Else
            Dim bitmap As Bitmap = New Bitmap(Panels(actPanel).wavPicture.Width, Panels(actPanel).wavPicture.Height)
            If Not (Panels(actPanel).wavPicture.Image Is Nothing) Then
                bitmap = CType(Panels(actPanel).wavPicture.Image, Bitmap)
                Dim Graphics As Graphics = Graphics.FromImage(bitmap)
                Dim Pen As Pen = New Pen(New SolidBrush(Color.Gray), 1)
                Graphics.DrawLine(Pen, New PointF(MdStartX, 0), New PointF(MdStartX, Panels(actPanel).wavPicture.Height - 1))
                Panels(actPanel).wavPicture.Image = bitmap
            End If
        End If
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        FormShown = True
        Panels(0) = New Paneel(c0Sel, c0PictureBox, c0head, ProgressBar1)
        Panels(1) = New Paneel(c1Sel, c1PictureBox, c1head, ProgressBar1)
        Panels(2) = New Paneel(c2Sel, c2PictureBox, c2head, ProgressBar1)
        For i = 0 To nrOfPanelsM1
            AddHandler Panels(i).wavPicture.MouseUp, AddressOf CPictureBox_MouseUp
            AddHandler Panels(i).wavPicture.MouseDown, AddressOf CPictureBox_MouseDown
        Next
        StartToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.Q ' play view
        StopToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.S  'stop playing
        SecsToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.D5  'play last 5 secs of playlist
        SecsToolStripMenuItem1.ShortcutKeys = Keys.Alt Or Keys.D1 ' last 10 secs
        actPanel = 0
        PlacePanelsOnScreen()
        workRestoreFileName = Path.GetTempPath() & "mixer.plt"
        If File.Exists(workRestoreFileName) Then
            RestoreWork()
        End If
        'start sound player
        continuousSoundPlayer = New WasapiOut(False, CoreAudioAPI.AudioClientShareMode.Shared, 100)
        BackgroundPlayer = New PlaySoundBuffer()
        continuousSoundPlayer.Initialize(CType(BackgroundPlayer, IWaveSource))
        While Not StopPlayingLoop '// in FormClosing
            continuousSoundPlayer.Play()
            Application.DoEvents()
        End While
        continuousSoundPlayer.Stop()
        Me.Close()
        End
    End Sub
    Sub PlacePanelsOnScreen()
        Me.Cursor = Cursors.WaitCursor
        Dim wh As Integer = ClientRectangle.Size.Height - 30 '// -Menu
        Dim ww As Integer = ClientRectangle.Size.Width - 5 '// -bord
        Dim pw As Integer = ww - Panels(0).wavCheck.Width - 10
        Dim ph As Integer = CInt(wh / Panels.Count) + 5
        For i = 0 To nrOfPanelsM1
            Panels(i).wavHead.Top = i * ph + 22
            Panels(i).wavHead.Left = Panels(0).wavCheck.Width + 5
            Panels(i).wavHead.Visible = True
            Panels(i).wavHead.SendToBack()
            Panels(i).wavPicture.Top = Panels(i).wavHead.Top + Panels(i).wavHead.Height
            Panels(i).wavPicture.Left = Panels(i).wavHead.Left
            Panels(i).wavPicture.Width = pw
            Panels(i).wavPicture.Height = ph - 10 - Panels(i).wavHead.Height
            Panels(i).wavPicture.Visible = True
            Panels(i).wavCheck.Top = Panels(i).wavPicture.Top
            Panels(i).wavCheck.Left = Panels(i).wavPicture.Left - Panels(i).wavCheck.Width - 2
            Panels(i).wavCheck.Visible = True
            If Panels(i).wavFileIx > -1 Then ' Show sound in window
                Panels(i).CalcGraph(WaveFiles(Panels(i).wavFileIx))
                Panels(i).ShowBitmap()
            End If
        Next
        ProgressBar1.Top = Panels(0).wavHead.Top
        ProgressBar1.BringToFront()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub SoundMix_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If FormShown Then
            Me.Cursor = Cursors.WaitCursor
            PlacePanelsOnScreen()
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub StartPlayView_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        If Playing Then
            Exit Sub
        End If
        ' play visible part of waves
        Me.Cursor = Cursors.WaitCursor
        Dim pulsesPerPixel(2) As Single
        Dim pulsSelStart(2) As Integer      ' pulsnr in buffer 
        Dim startPuls As Integer
        Dim maxPuls As Integer
        Dim pulsSelEnd(2) As Integer
        Dim nPuls(2) As Integer
        Dim nViews As Short = 0
        For i As Integer = 0 To nrOfPanelsM1 ' find selected sounds on screen
            If Panels(i).wavCheck.Checked AndAlso Panels(i).viewEnd - Panels(i).viewStart > 0 Then
                nViews += 1S
                pulsesPerPixel(i) = CSng(Panels(i).viewEnd - Panels(i).viewStart + 1) / CSng(Panels(i).wavPicture.Width)
                pulsSelStart(i) = CInt(Math.Max(Panels(i).selectStart, Panels(i).viewStart))
                pulsSelEnd(i) = CInt(Math.Min(Panels(i).selectEnd, Panels(i).viewEnd))
                nPuls(i) = pulsSelEnd(i) - pulsSelStart(i)
                If nViews = 1 OrElse maxPuls < nPuls(i) Then
                    maxPuls = nPuls(i)
                End If
                If nViews = 1 OrElse startPuls > pulsSelStart(i) Then
                    startPuls = pulsSelStart(i)
                End If
            End If
        Next
        If nViews = 0 Then
            MsgBox("No box selected")
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        Dim bytesBuffer As Byte() = New Byte(maxPuls * 4 - 1) {}
        Dim bytes4(3) As Byte
        For i As Integer = 0 To maxPuls - 1 ' sum 1 to 3 sounds into one
            Dim Samples1(1) As Short
            Dim Samples2(1) As Short
            Dim VwDivide As Short = 0
            For j As Integer = 0 To nrOfPanelsM1
                If Panels(j).wavCheck.Checked Then
                    If pulsSelStart(j) + i <= pulsSelEnd(j) Then
                        VwDivide += 1S
                        Dim mp As Integer = WaveFiles(Panels(j).wavFileIx).NrPulses - 1
                        bytes4 = WaveFiles(Panels(j).wavFileIx).GetBufPulses(pulsSelStart(j) + i, 1)
                        Buffer.BlockCopy(bytes4, 0, Samples2, 0, 4)
                        Samples1(0) = CShort(CInt(Samples1(0) * (VwDivide - 1) \ VwDivide) + CInt(Samples2(0) \ VwDivide))
                        Samples1(1) = CShort(CInt(Samples1(1) * (VwDivide - 1) \ VwDivide) + CInt(Samples2(1) \ VwDivide))
                        Buffer.BlockCopy(Samples1, 0, bytes4, 0, 4)
                    End If
                End If
            Next
            Buffer.BlockCopy(bytes4, 0, bytesBuffer, i * 4, 4)
        Next
        WavPlayer = New PlayWavSoundBuffer(bytesBuffer, 0, bytesBuffer.Length - 1)
        WavPlayer.PlayWav(True)
        Me.Cursor = Cursors.Default
    End Sub
    Friend Sub MarkPlayProgress(puls As Integer)
        For i As Integer = 0 To nrOfPanelsM1 ' draw yellow line
            If Panels(i) IsNot Nothing AndAlso Panels(i).wavCheck.Checked AndAlso Panels(i).viewEnd - Panels(i).viewStart > 0 Then
                Panels(i).DrawMarkerLine(puls)
            End If
        Next
    End Sub
    Private Sub StopPlayViewOrList(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        If Playing Then
            WavPlayer.StopWav()
            MarkPlayProgress(-1)
        End If
    End Sub
    Private Sub SoundMix_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ListBox1.Visible Then
            e.Cancel = True
        Else
            StopPlayingLoop = True
        End If
    End Sub
    Private Sub LoadSoundfileInView(sender As Object, e As EventArgs)
        ' ask file and load it
        If Playing Then
            Exit Sub
        End If
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.ShowHelp = False
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileNames(0) <> "" Then
            Me.Cursor = Cursors.WaitCursor
            Dim ix As Integer = -1
            For i As Integer = 0 To WaveFiles.Count - 1
                Dim w As WaveFile = WaveFiles(i)
                If w.FileName = OpenFileDialog1.FileNames(0) Then
                    ix = i
                    Panels(actPanel).wavFileIx = ix
                End If
            Next
            If ix = -1 Then
                WaveFiles.Add(Panels(actPanel).OpenWaveFile(OpenFileDialog1.FileNames(0), WaveFiles.Count))
            End If
            ZoomOutall(sender, Nothing)
            Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
            Panels(actPanel).ShowBitmap()
            SaveList()
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Friend Sub setChecked(p As Integer)
        For i As Integer = 0 To nrOfPanelsM1
            Panels(i).wavCheck.Checked = (i = p)
        Next
    End Sub
    Dim MdButton As MouseButtons, MdStartX As Integer
    Private Sub ChangeView(start As Integer, ends As Integer) ' end after start; minimun line width is 1 puls
        If ends > start AndAlso ends > 0 AndAlso ends - start > Panels(actPanel).wavPicture.Width AndAlso start < WaveFiles(Panels(actPanel).wavFileIx).NrPulses Then ' new view correct
            Panels(actPanel).viewStart = start
            Panels(actPanel).viewEnd = ends
            Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
            Panels(actPanel).ShowBitmap()
        End If
    End Sub

    Private Sub CPictureBox_MouseUp(sender As Object, e As MouseEventArgs) ' select view by moving mouse between down and up
        If e.Button = MouseButtons.Left Then
            Dim MdEndX As Integer = e.X
            Dim MdEndY As Integer = e.Y
            If MdEndY < 0 OrElse MdEndY > CType(sender, PictureBox).Height Then ' abort selection
                Exit Sub
            End If
            If MdEndX < MdStartX Then
                Dim sv As Integer = MdEndX
                MdEndX = MdStartX
                MdStartX = sv
            End If
            If MdStartX < 0 Then ' mouse left picturebox
                MdStartX = 0
            End If
            If MdEndX > Panels(actPanel).wavPicture.Width Then ' mouse left picturebox
                MdEndX = Panels(actPanel).wavPicture.Width
            End If
            Dim pcs As Double = (MdStartX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
            Dim pce As Double = (MdEndX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
            ChangeView(CInt(pcs), CInt(pce))
        End If
    End Sub
    Sub ViewStart(sender As Object, e As EventArgs)
        Dim pc As Double = (MdStartX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
        ChangeView(CInt(pc), Panels(actPanel).viewEnd)
    End Sub
    Sub ViewEnd(sender As Object, e As EventArgs)
        Dim pc As Double = (MdStartX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
        ChangeView(Panels(actPanel).viewStart, CInt(pc))
    End Sub
    Sub SelectStart(sender As Object, e As EventArgs)
        Dim pc As Double = (MdStartX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
        Panels(actPanel).selectStart = CInt(Math.Min(CInt(Math.Max(0, pc)), Panels(actPanel).selectEnd))
        Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
        Panels(actPanel).ShowBitmap()
    End Sub
    Sub SelectEnd(sender As Object, e As EventArgs)
        Dim pc As Double = (MdStartX / Panels(actPanel).wavPicture.Width) * (Panels(actPanel).viewEnd - Panels(actPanel).viewStart) + Panels(actPanel).viewStart
        Panels(actPanel).selectEnd = CInt(Math.Max(CInt(Math.Min(pc, WaveFiles(Panels(actPanel).wavFileIx).NrPulses)), Panels(actPanel).selectStart))
        Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
        Panels(actPanel).ShowBitmap()
    End Sub
    Sub SelectAll(sender As Object, e As EventArgs)
        Panels(actPanel).selectStart = 0
        Panels(actPanel).selectEnd = WaveFiles(Panels(actPanel).wavFileIx).NrPulses
        Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
        Panels(actPanel).ShowBitmap()
    End Sub
    Sub ZoomOut(sender As Object, e As EventArgs)
        Dim np As Integer = Panels(actPanel).viewEnd - Panels(actPanel).viewStart
        Dim vs As Integer = Math.Max(0, Panels(actPanel).viewStart - np \ 2)
        Dim ve As Integer = Math.Min(WaveFiles(Panels(actPanel).wavFileIx).NrPulses, Panels(actPanel).viewEnd + np \ 2)
        ChangeView(vs, ve)
    End Sub
    Sub ZoomIn(sender As Object, e As EventArgs)
        Dim np As Integer = (Panels(actPanel).viewEnd - Panels(actPanel).viewStart)
        Dim vs As Integer = Panels(actPanel).viewStart + np \ 4
        Dim ve As Integer = Panels(actPanel).viewEnd - np \ 4
        ChangeView(vs, ve)
    End Sub
    Sub ZoomOutall(sender As Object, e As EventArgs)
        ChangeView(0, WaveFiles(Panels(actPanel).wavFileIx).NrPulses)
    End Sub
    Sub ShiftRight(sender As Object, e As EventArgs, Optional ic As Integer = 0)
        Dim np As Integer = (Panels(actPanel).viewEnd - Panels(actPanel).viewStart)
        If Panels(actPanel).viewEnd + np >= WaveFiles(Panels(actPanel).wavFileIx).NrPulses Then
            np = WaveFiles(Panels(actPanel).wavFileIx).NrPulses - Panels(actPanel).viewEnd - 1
        End If
        If ic = 1 Then np \= 2 ' shift arrow moves only half a view
        Dim vs As Integer = Panels(actPanel).viewStart + np
        Dim ve As Integer = Panels(actPanel).viewEnd + np
        ChangeView(vs, ve)
    End Sub
    Sub ShiftLeft(sender As Object, e As EventArgs, Optional ic As Integer = 0)
        Dim np As Integer = (Panels(actPanel).viewEnd - Panels(actPanel).viewStart)
        If Panels(actPanel).viewStart - np < 0 Then
            np = Panels(actPanel).viewStart
        End If
        If ic = 1 Then np \= 2
        Dim vs As Integer = Panels(actPanel).viewStart - np
        Dim ve As Integer = Panels(actPanel).viewEnd - np
        ChangeView(vs, ve)
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Dim i As Integer = CType(keyData, Integer)
        Dim ic As Short = CShort(i \ 65536)
        Dim ik As Short = CShort(i Mod 65536)
        If ic = 2 AndAlso ik = Keys.D0 Then
            ZoomOutall(Nothing, Nothing)
        ElseIf ic = 2 AndAlso (ik = Keys.OemMinus OrElse ik = Keys.Subtract) Then
            ZoomOut(Nothing, Nothing)
        ElseIf ic = 2 AndAlso (ik = Keys.Oemplus OrElse ik = Keys.Add) Then
            ZoomIn(Nothing, Nothing)
        ElseIf ic < 2 AndAlso ik = Keys.Right Then
            ShiftRight(Nothing, Nothing, ic)
        ElseIf ic < 2 AndAlso ik = Keys.Left Then
            ShiftLeft(Nothing, Nothing, ic)
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Friend Function PulsToDatTime(ix As Single) As DateTime
        Dim h As Integer = 0
        Dim m As Integer = 0
        Dim s As Integer = CInt(Math.Floor(ix))
        Dim ms As Integer = CInt((ix - CSng(s)) * 1000)
        If ms = 1000 Then
            ms = 0
            s += 1
        End If
        If s >= 60 Then
            m = s \ 60
            s = s Mod 60
        End If
        If m >= 60 Then
            h = m \ 60
            m = s Mod 60
        End If
        Return New DateTime(1, 1, 1, h, m, s, ms)
    End Function
    Friend Sub MarkProgressBar(v As Integer)
        If v = -1 Then
            ProgressBar1.Visible = False
        Else
            ProgressBar1.Value = v
        End If
    End Sub
    Private Sub MixPlay5secs(sender As Object, e As EventArgs) Handles SecsToolStripMenuItem.Click
        Dim startpuls As Integer = totalPulsesInList - 5 * 44100
        PlayfList(Math.Max(0, startpuls))
    End Sub
    Private Sub MixPlay10secs(sender As Object, e As EventArgs) Handles SecsToolStripMenuItem1.Click
        Dim startpuls As Integer = totalPulsesInList - 10 * 44100
        PlayfList(Math.Max(0, startpuls))
    End Sub
    Private Sub SecsToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SecsToolStripMenuItem2.Click
        Dim startpuls As Integer, tm As String
        Try
            tm = InputBox("Last 'n' seconds  or  'min.sec' minutes", "Nr in minutes or seconds in playlist to play", "1.00")
            Dim i As Integer = tm.IndexOf(".")
            If i = -1 Then
                startpuls = totalPulsesInList - CInt(tm) * 44100
            Else
                Dim m As Integer = CInt(tm.Substring(0, i))
                Dim s As Integer = CInt(tm.Substring(i + 1))
                startpuls = totalPulsesInList - (m * 60 + s) * 44100
            End If
        Catch ex As Exception
            Exit Sub
            MsgBox("Invalid duration " & tm & " , use only ciphers")
        End Try
        PlayfList(Math.Max(0, startpuls))
    End Sub
    Private Sub SaveAndExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAndExitToolStripMenuItem.Click
        SaveList()
        StopPlayingLoop = True
        Thread.Sleep(200)
        Me.Close()
        End
    End Sub
    Private Sub FinalizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FinalizeToolStripMenuItem.Click
        Dim Wwriter As IWriteable
        Dim AddorMultiplySounds As Boolean = True ' Add
        SaveFileDialog1.FileName = "full.program.wav"
        SaveFileDialog1.Filter = "Wav files (*.wav)|*.wav|All files (*.*)|*.*"
        SaveFileDialog1.CheckFileExists = False
        SaveFileDialog1.ShowHelp = False
        SaveFileDialog1.Title = "Name of file that will contain the final program."
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileNames(0) <> "" Then
            Dim oFn As String = SaveFileDialog1.FileNames(0)
            Dim wavStream = New FileStream(oFn, FileMode.Create)
            Dim waveFormat As WaveFormat
            waveFormat = New WaveFormat(44100, 16, 2)
            Wwriter = New WaveWriter(wavStream, waveFormat)
            For i As Integer = 0 To totalPulsesInList - 1
                Dim bytes4 As Byte() = FormOneOutputPuls(i, AddorMultiplySounds)
                Wwriter.Write(bytes4, 0, 4)
            Next
            CType(Wwriter, IDisposable).Dispose()
            wavStream.Dispose()
            wavStream.Close()
            File.Delete(workRestoreFileName)
        End If
    End Sub
    Function FormOneOutputPuls(pulsNr As Integer, AddorMultiplySounds As Boolean) As Byte()
        Dim bytes4(3) As Byte
        Dim Samples1(1) As Short
        Dim Samples2(1) As Short
        Dim VwDivide As Short = 0
        For Each p As PlayListEntry In PlayList
            If pulsNr >= p.startPulsList AndAlso pulsNr < p.startPulsList + p.nPulses Then
                bytes4 = WaveFiles(p.fileix).GetBufPulses(pulsNr - p.startPulsList + p.startPuls, 1)
                Buffer.BlockCopy(bytes4, 0, Samples2, 0, 4)
                If AddorMultiplySounds Then
                    Dim sumOfSamples As Integer = CInt(Samples1(0)) + Samples2(0)
                    If sumOfSamples > Short.MaxValue Then sumOfSamples = Short.MaxValue - 1
                    If sumOfSamples < -Short.MaxValue Then sumOfSamples = -Short.MaxValue + 1
                    Samples1(0) = CShort(sumOfSamples)
                    sumOfSamples = CInt(Samples1(1)) + Samples2(1)
                    If sumOfSamples > Short.MaxValue Then sumOfSamples = Short.MaxValue - 1
                    If sumOfSamples < -Short.MaxValue Then sumOfSamples = -Short.MaxValue + 1
                    Samples1(1) = CShort(sumOfSamples)
                Else
                    VwDivide += 1S
                    Samples1(0) = CShort(CInt(Samples1(0) * (VwDivide - 1) \ VwDivide) + CInt(Samples2(0) \ VwDivide))
                    Samples1(1) = CShort(CInt(Samples1(1) * (VwDivide - 1) \ VwDivide) + CInt(Samples2(1) \ VwDivide))
                End If
                Buffer.BlockCopy(Samples1, 0, bytes4, 0, 4)
            End If
        Next
        Return bytes4
    End Function
    Sub PlayfList(startPuls As Integer)
        Dim maxpuls As Integer = totalPulsesInList - startPuls
        Dim bytesBuffer As Byte() = New Byte(maxpuls * 4 - 1) {}
        For i As Integer = startPuls To totalPulsesInList - 1
            Dim bytes4 As Byte() = FormOneOutputPuls(i, False)
            Buffer.BlockCopy(bytes4, 0, bytesBuffer, (i - startPuls) * 4, 4)
        Next
        WavPlayer = New PlayWavSoundBuffer(bytesBuffer, 0, bytesBuffer.Length - 1)
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = bytesBuffer.Length - 1
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        WavPlayer.PlayWav(False)
    End Sub
    Private Sub UndoAddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoAddToolStripMenuItem.Click
        Dim ple As PlayListEntry = PlayList(PlayList.Count - 1)
        totalPulsesInList -= ple.pulsesAdded
        PlayList.RemoveAt(PlayList.Count - 1)
        SaveList()
    End Sub
    Private Sub SummaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SummaryToolStripMenuItem.Click
        ListBox1.Visible = True
        ListBox1.Top = 0
        ListBox1.Left = 0
        ListBox1.Width = Me.Width
        ListBox1.Height = Me.Height
        ListBox1.Items.Clear()
        ListBox1.Items.Add("Click HERE to close the summary")
        Dim np As Integer = 0
        For Each pe As PlayListEntry In PlayList
            Dim sndTitle As String = WaveFiles(pe.fileix).FileName
            Dim i As Integer = sndTitle.LastIndexOf(".")
            If i > -1 Then sndTitle = sndTitle.Substring(0, i)
            i = sndTitle.LastIndexOf("\")
            If i > -1 Then sndTitle = sndTitle.Substring(i + 1)
            While sndTitle.Length < 35
                sndTitle += " "
            End While
            ListBox1.Items.Add(sndTitle.Substring(0, 35))
            Dim s As String = ""
            s &= "At: " & PulsToSecs(pe.startPulsList)
            s &= " for: " & PulsToSecs(pe.nPulses)
            s &= " dup: " & PulsToSecs(pe.nPulses - pe.pulsesAdded)
            s &= " " & StrDup(pe.nPulses \ 44100, ".")
            ListBox1.Items.Add(s)
            np = pe.startPulsList + pe.nPulses
        Next
        ListBox1.Items.Add("Total time: " & PulsToSecs(np))
        ListBox1.BringToFront()
    End Sub
    Function PulsToSecs(puls As Integer) As String
        Dim sc As Single = puls / 44100.0!
        Dim s As String
        If sc > 60 Then
            Dim m As Single = CInt(sc) \ 60
            sc -= m * 60
            s = Format(m, "N0") & "." & Format(sc, "N2")
        Else
            s = Format(sc, "N2")
        End If
        While s.Length < 9
            s = " " + s
        End While
        Return s
    End Function
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.Click
        If ListBox1.SelectedIndex = 0 Then
            ListBox1.Visible = False
        End If
    End Sub
    Friend Sub SaveList()
        Using wr As New BinaryWriter(File.Open(workRestoreFileName, FileMode.Create))
            Dim i As Integer = PlayList.Count
            wr.Write(i)
            wr.Write(totalPulsesInList)
            For Each p As PlayListEntry In PlayList
                wr.Write(p.fileix)
                wr.Write(p.nPulses)
                wr.Write(p.pulsesAdded)
                wr.Write(p.startPuls)
                wr.Write(p.startPulsList)
            Next
            i = WaveFiles.Count
            wr.Write(i)
            For Each f As WaveFile In WaveFiles
                wr.Write(f.FileName)
            Next
            For Each p As Paneel In Panels
                wr.Write(p.wavFileIx)
                If p.wavFileIx > -1 Then
                    wr.Write(p.selectStart)
                    wr.Write(p.selectEnd)
                    wr.Write(p.viewStart)
                    wr.Write(p.viewEnd)
                End If
            Next
        End Using
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim a = New AboutBox1
        a.ShowDialog()
    End Sub

    Sub RestoreWork()
        Dim cPanel As Integer = 0
        If MessageBox.Show("Found work in progress. Restore?", "Restore", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Using wr As New BinaryReader(File.Open(workRestoreFileName, FileMode.Open, FileAccess.Read))
                PlayList.Clear()
                Dim n As Integer = wr.ReadInt32 'PlayList.Count
                totalPulsesInList = wr.ReadInt32
                For i As Integer = 0 To n - 1 'p As PlayListEntry In PlayList
                    Dim p As PlayListEntry = New PlayListEntry With {
                        .fileix = wr.ReadInt32,
                        .nPulses = wr.ReadInt32,
                        .pulsesAdded = wr.ReadInt32,
                        .startPuls = wr.ReadInt32,
                        .startPulsList = wr.ReadInt32
                    }
                    PlayList.Add(p)
                Next
                WaveFiles.Clear()
                n = wr.ReadInt32 ' Files.count
                For i As Integer = 0 To n - 1
                    Dim fn As String = wr.ReadString
                    Dim originalWavFile As IWaveSource
                    originalWavFile = CodecFactory.Instance.GetCodec(fn)
                    If originalWavFile.WaveFormat.SampleRate <> 44100 Then
                        originalWavFile = originalWavFile.ChangeSampleRate(44100)
                    End If
                    If originalWavFile.WaveFormat.Channels = 1 Then
                        originalWavFile = originalWavFile.ToStereo
                    End If
                    Dim bytesSize As Integer = originalWavFile.WaveFormat.SampleRate * 4 '// each sample = 2 short integers = 4 bytes 
                    WaveFiles.Add(Panels(0).OpenWaveFile(fn, bytesSize))
                Next
                actPanel = 0
                For Each p As Paneel In Panels
                    p.wavFileIx = wr.ReadInt32
                    If p.wavFileIx > -1 Then
                        cPanel = actPanel
                        Panels(actPanel).selectStart = wr.ReadInt32
                        Panels(actPanel).selectEnd = wr.ReadInt32
                        Panels(actPanel).viewStart = wr.ReadInt32
                        Panels(actPanel).viewEnd = wr.ReadInt32
                        Panels(actPanel).CalcGraph(WaveFiles(Panels(actPanel).wavFileIx))
                        Panels(actPanel).ShowBitmap()
                    End If
                    actPanel += 1
                Next
            End Using
        Else
            File.Delete(workRestoreFileName)
        End If
        actPanel = cPanel
    End Sub
End Class
Public Class Paneel
    Friend wavPicture As PictureBox
    Friend wavCheck As CheckBox
    Friend wavHead As Label
    Private ReadOnly readProgress As ProgressBar
    Friend wavFileIx As Integer ' in List "Files"
    Private sndTitle As String
    Private sndSelected As String
    Friend savedImageLine() As Color
    Friend savedImageLineNr As Integer
    Friend bitmap As Bitmap
    Friend selectStart As Integer ' 1st byte to select
    Friend selectEnd As Integer ' last byte to select
    Friend viewStart As Integer ' first byte to screen
    Friend viewEnd As Integer ' last byte to screen
    Private graphRightP(1) As Single
    Private graphLeftP(1) As Single
    Private graphRightN(1) As Single
    Private graphLeftN(1) As Single
    Private pulsesPerPixel As Single
    Private pulsesInterval As Integer
    Private QuartSize As Integer
    Sub New(ch As CheckBox, pc As PictureBox, lb As Label, pb As ProgressBar)
        wavPicture = pc
        wavCheck = ch
        wavHead = lb
        readProgress = pb
        sndTitle = ""
        sndSelected = ""
        wavFileIx = -1
    End Sub
    Function OpenWaveFile(fn As String, ixInFiles As Integer) As WaveFile
        sndTitle = fn
        Dim i As Integer = sndTitle.LastIndexOf(".")
        If i > -1 Then sndTitle = sndTitle.Substring(0, i)
        i = sndTitle.LastIndexOf("\")
        If i > -1 Then sndTitle = sndTitle.Substring(i + 1)
        wavHead.Text = "Loading file ..."
        Dim originalWavFile As IWaveSource
        '// open the wave file 
        originalWavFile = CodecFactory.Instance.GetCodec(fn)
        If originalWavFile.WaveFormat.SampleRate <> 44100 Then
            originalWavFile = originalWavFile.ChangeSampleRate(44100)
        End If
        If originalWavFile.WaveFormat.Channels = 1 Then
            originalWavFile = originalWavFile.ToStereo
        End If
        readProgress.Minimum = 0
        readProgress.Maximum = CInt(originalWavFile.Length)
        readProgress.Value = 0
        readProgress.Visible = True
        Dim bytesSize As Integer = originalWavFile.WaveFormat.SampleRate * 4 '// each sample = 2 short integers = 4 bytes 
        Dim wf As New WaveFile(fn, bytesSize)
        Dim NrBytes As Integer = 0
        Dim read As Integer = bytesSize
        While read = bytesSize
            Dim nby As Long = originalWavFile.Length - originalWavFile.Position
            If nby > bytesSize Then
                nby = bytesSize
            End If
            readProgress.Value = CInt(originalWavFile.Position)
            Dim bytesBuffer() As Byte = New Byte(bytesSize - 1) {}
            read = originalWavFile.Read(bytesBuffer, 0, CInt(nby))
            If read = bytesSize Then
                wf.sampleBlocks.Add(bytesBuffer)
            Else
                Dim lastBuffer() As Byte = New Byte(read - 1) {}
                Buffer.BlockCopy(bytesBuffer, 0, lastBuffer, 0, read)
                wf.sampleBlocks.Add(lastBuffer)
            End If
            NrBytes += CInt(read)
        End While
        wf.NrPulses = NrBytes \ 4
        readProgress.Visible = False
        originalWavFile.Dispose()
        wavFileIx = ixInFiles
        wavHead.Text = sndTitle
        viewStart = 0 ' puls on screen 
        viewEnd = wf.NrPulses - 1
        selectStart = 0 ' puls to output
        selectEnd = wf.NrPulses - 1
        Return wf
    End Function
    Sub CalcGraph(wf As WaveFile)
        ReDim graphRightP(wavPicture.Width - 1)
        ReDim graphLeftP(wavPicture.Width - 1)
        ReDim graphRightN(wavPicture.Width - 1)
        ReDim graphLeftN(wavPicture.Width - 1)
        pulsesPerPixel = CSng(viewEnd - viewStart) / CSng(wavPicture.Width)
        pulsesInterval = CInt(Math.Truncate(pulsesPerPixel))
        Dim maxPulsIx As Integer = SoundMix.WaveFiles(wavFileIx).NrPulses - 1
        Dim textSpace As Integer = 15
        QuartSize = (wavPicture.Height - textSpace) \ 4 ' stereo, positive half, negative half, room for times
        For i As Integer = 0 To wavPicture.Width - 1
            Dim firstPuls As Integer = CInt(i * pulsesPerPixel) + viewStart
            Dim lastPuls As Integer = CInt((i + 1) * pulsesPerPixel) + viewStart - 1
            Dim Bytes() As Byte
            If firstPuls > maxPulsIx OrElse lastPuls < 1 Then
                Bytes = New Byte(pulsesInterval * 4 - 1) {}                 ' empty buffer
            ElseIf lastPuls > maxPulsIx Then ' partial last block of wav data
                Dim exist As Integer = maxPulsIx - firstPuls
                Bytes = New Byte(pulsesInterval * 4 - 1) {} ' all zero
                If exist > 0 Then
                    Dim partBytes() As Byte = wf.GetBufPulses(firstPuls, exist)
                    Buffer.BlockCopy(partBytes, 0, Bytes, 0, exist * 4)
                End If
            ElseIf firstPuls < 0 Then ' partial first block of wav data
                Dim nexist As Integer = pulsesInterval + firstPuls
                Bytes = New Byte(pulsesInterval * 4 - 1) {} ' all zero
                If nexist > 0 Then
                    Dim partBytes() As Byte = wf.GetBufPulses(0, nexist)
                    Buffer.BlockCopy(partBytes, 0, Bytes, (pulsesInterval - nexist) * 4, nexist * 4)
                End If
            Else
                Bytes = wf.GetBufPulses(firstPuls, pulsesInterval) ' all data exists in Wav data
            End If
            Dim Samples() As Short = New Short(1) {}
            graphLeftP(i) = 0
            graphRightP(i) = 0
            graphLeftN(i) = 0
            graphRightN(i) = 0
            Dim lmp As Short = 0, lmn As Short = 0, rmp As Short = 0, rmn As Short = 0
            For j As Integer = 0 To pulsesInterval - 1
                Buffer.BlockCopy(Bytes, j * 4, Samples, 0, 4)
                If Samples(0) > 0 Then
                    If Samples(0) > lmp Then
                        lmp = Samples(0)
                    End If
                Else
                    If Samples(0) < lmn Then
                        lmn = Samples(0)
                    End If
                End If
                If Samples(1) > 0 Then
                    If Samples(1) > rmp Then
                        rmp = Samples(1)
                    End If
                Else
                    If Samples(1) < rmn Then
                        rmn = Samples(1)
                    End If
                End If
            Next
            graphLeftP(i) = 0 * QuartSize + CSng(Short.MaxValue - lmp) / CSng(Short.MaxValue) * QuartSize
            graphLeftN(i) = 1 * QuartSize + (-CSng(lmn)) / CSng(Short.MaxValue) * QuartSize
            graphRightP(i) = 2 * QuartSize + CSng(Short.MaxValue - rmp) / CSng(Short.MaxValue) * QuartSize
            graphRightN(i) = 3 * QuartSize + (-CSng(rmn)) / CSng(Short.MaxValue) * QuartSize
        Next
    End Sub
    Function PulsToTime(puls As Integer) As String
        Dim timessmm As Integer = CInt(puls / 441.0!)
        Dim s As Single = puls / 44100.0!
        Return (Format(s, "N2"))
    End Function
    Sub ShowBitmap()
        sndSelected = "Selection: " & PulsToTime(selectStart) & " - " & PulsToTime(selectEnd) & ":  " & PulsToTime(selectEnd - selectStart) & " secs"
        wavHead.Text = sndTitle & " " & sndSelected
        bitmap = New Bitmap(wavPicture.Width, wavPicture.Height)
        savedImageLine = New Color(wavPicture.Height) {}
        savedImageLineNr = -1
        Dim Graphics As Graphics = Graphics.FromImage(bitmap)
        Graphics.SmoothingMode = SmoothingMode.HighSpeed
        Graphics.CompositingQuality = CompositingQuality.HighSpeed
        Graphics.PixelOffsetMode = PixelOffsetMode.None
        Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit
        Graphics.Clear(Color.Black)
        '    time scale
        Dim Pen As Pen = New Pen(New SolidBrush(Color.White), 1)
        Dim textSpace As Integer = 15
        Dim seconds As Single = CSng((viewEnd - viewStart) / 44100)
        Dim firstsec As Single = CSng(viewStart / 44100)
        Dim intr As Single = seconds / 10

        Dim ist As Integer = 0
        For i As Single = 0 To seconds Step intr
            Dim iv As Single = i + firstsec
            If iv >= 0 Then
                Dim dt As DateTime = SoundMix.PulsToDatTime(iv)
                Dim stringToPrint As String = dt.ToString("MM/dd/yyyy HH:mm:ss.fff")
                stringToPrint = stringToPrint.Substring(14, 9)
                Dim printFont As Font = New Font("Courier New", 8)
                Dim textSize As Size = System.Windows.Forms.TextRenderer.MeasureText(stringToPrint, printFont)
                Dim stRect As Integer = ist * wavPicture.Width \ 10
                Dim aRectangle As Rectangle = New Rectangle(New Point(stRect, wavPicture.Height - textSpace), textSize)
                Graphics.FillRectangle(Brushes.Black, aRectangle)
                System.Windows.Forms.TextRenderer.DrawText(Graphics, stringToPrint, printFont, aRectangle, Color.White)
                If ist < 10 Then
                    Graphics.DrawLine(Pen, New PointF(stRect, 0), New PointF(stRect, wavPicture.Height - 1)) ' will be partly overwritten
                End If
            End If
            ist += 1
        Next
        ' wave
        Dim brightStart As Integer = 0
        Dim brightEnd As Integer = wavPicture.Width - 1
        If selectStart > viewEnd Then
            brightStart = viewEnd + 1
        ElseIf selectStart > viewStart AndAlso selectStart < viewEnd Then
            brightStart = CInt((selectStart - viewStart) / (viewEnd - viewStart) * wavPicture.Width)
        End If
        If selectEnd < viewStart Then
            brightEnd = 0
        ElseIf selectEnd > viewStart AndAlso selectStart < viewEnd AndAlso selectEnd < viewEnd Then
            brightEnd = CInt((selectEnd - viewStart) / (viewEnd - viewStart) * wavPicture.Width)
        End If
        Dim Image As Image = wavPicture.Image
        Dim PenL As Pen, PenR As Pen
        Dim PenLH = New Pen(New SolidBrush(Color.Green), 1)
        Dim PenLD = New Pen(New SolidBrush(Color.DarkOliveGreen), 1)
        Dim PenRH = New Pen(New SolidBrush(Color.Red), 1)
        Dim PenRD = New Pen(New SolidBrush(Color.DarkRed), 1)
        Dim pbase As Integer, ptop As Integer
        Dim p1 As PointF = New PointF(0, QuartSize)
        Dim p2 As PointF = New PointF(brightStart, QuartSize)
        PenL = PenLD
        Graphics.DrawLine(PenL, p1, p2)
        p1 = New PointF(brightStart, QuartSize)
        p2 = New PointF(brightEnd, QuartSize)
        PenL = PenLH
        Graphics.DrawLine(PenL, p1, p2)
        p1 = New PointF(brightEnd, QuartSize)
        p2 = New PointF(wavPicture.Width - 1, QuartSize)
        PenL = PenLD
        Graphics.DrawLine(PenL, p1, p2)

        p1 = New PointF(0, QuartSize * 3)
        p2 = New PointF(brightStart, QuartSize * 3)
        PenL = PenRD
        Graphics.DrawLine(PenL, p1, p2)
        p1 = New PointF(brightStart, QuartSize * 3)
        p2 = New PointF(brightEnd, QuartSize * 3)
        PenL = PenRH
        Graphics.DrawLine(PenL, p1, p2)
        p1 = New PointF(brightEnd, QuartSize * 3)
        p2 = New PointF(wavPicture.Width - 1, QuartSize * 3)
        PenL = PenRD
        Graphics.DrawLine(PenL, p1, p2)

        For i As Integer = 0 To wavPicture.Width - 1
            If i < brightStart OrElse i > brightEnd Then
                PenL = PenLD
                PenR = PenRD
            Else
                PenL = PenLH
                PenR = PenRH
            End If
            pbase = QuartSize
            ptop = CInt(graphLeftP(i))
            p1 = New PointF(i, ptop)
            p2 = New PointF(i, pbase)
            Graphics.DrawLine(PenL, p1, p2)
            ptop = CInt(graphLeftN(i))
            p1 = New PointF(i, pbase)
            p2 = New PointF(i, ptop)
            Graphics.DrawLine(PenL, p1, p2)
            pbase = (QuartSize * 3)
            ptop = CInt(graphRightP(i))
            p1 = New PointF(i, ptop)
            p2 = New PointF(i, pbase)
            Graphics.DrawLine(PenR, p1, p2)
            ptop = CInt(graphRightN(i))
            p1 = New PointF(i, pbase)
            p2 = New PointF(i, ptop)
            Graphics.DrawLine(PenR, p1, p2)
        Next
        wavPicture.Image = bitmap
    End Sub
    Friend Sub DrawMarkerLine(puls As Integer)
        Dim l As Integer = -2
        If puls >= 0 Then ' closing Draw has -puls
            puls += Math.Max(viewStart, selectStart)
            l = PulsToLine(puls)
        End If
        If l <> savedImageLineNr Then
            If savedImageLineNr > -1 Then
                For i As Integer = 0 To wavPicture.Height - 1
                    bitmap.SetPixel(savedImageLineNr, i, savedImageLine(i))
                Next
            End If
            If puls >= selectStart AndAlso puls < selectEnd Then
                Dim c As Color = Color.Yellow
                For i As Integer = 0 To wavPicture.Height - 1
                    savedImageLine(i) = bitmap.GetPixel(l, i)
                    savedImageLineNr = l
                    bitmap.SetPixel(l, i, c)
                Next
            Else
                savedImageLineNr = -1
            End If
            wavPicture.Image = bitmap
        End If
    End Sub
    Sub AddPlaylist(sender As Object, e As EventArgs)
        Dim err As Boolean = True
        Dim secs As Single, sg As Integer
        While err
            err = False
            Dim tm As String = ""
            Try
                tm = InputBox("Offset ±ss.mmm", "Offset in seconds, -=overlay, +=silence", "0.00")
                If tm <> "" Then
                    sg = 1
                    If tm(0) = "+" OrElse tm(0) = "-" Then
                        If tm(0) = "-" Then sg = -1
                        tm = tm.Substring(1)
                    End If
                    tm = "0" & tm
                    Dim s As Integer, m As Integer
                    Dim i As Integer = tm.IndexOf(".")
                    If i > -1 Then
                        s = CInt(tm.Substring(0, i))
                        m = CInt((tm.Substring(i + 1) & "000").Substring(0, 3))
                        secs = CSng(s + m / 1000.0!)
                    Else
                        secs = CSng(tm)
                    End If
                Else
                    Exit Sub
                End If
            Catch ex As Exception
                err = True
                MsgBox("Invalid duration " & tm & " , use only ciphers and at most one decimal point")
            End Try
        End While
        Dim pulsesOffset As Integer = CInt(secs * 44100) * sg
        Dim ple As PlayListEntry = New PlayListEntry With {
            .fileix = wavFileIx,
            .startPuls = selectStart,
            .nPulses = selectEnd - selectStart,
            .startPulsList = SoundMix.totalPulsesInList + pulsesOffset,
            .pulsesAdded = SoundMix.totalPulsesInList
        }
        SoundMix.totalPulsesInList += ple.nPulses + pulsesOffset
        ple.pulsesAdded = SoundMix.totalPulsesInList - ple.pulsesAdded
        SoundMix.PlayList.Add(ple)
        SoundMix.SaveList()
    End Sub
    Friend Function PulsToLine(puls As Integer) As Integer
        Dim pulsesPerPixel As Single = CSng(viewEnd - viewStart + 1) / CSng(wavPicture.Width)
        Return Math.Min(wavPicture.Width - 1, CInt(Math.Truncate((puls - viewStart - 1) / pulsesPerPixel)))
    End Function
End Class
Friend Class PlayWavSoundBuffer
    'Dim Wwriter As IWriteable
    'Dim wavStream As MemoryStream
    'Dim waveFormat As WaveFormat
    ReadOnly _pos As Integer
    ReadOnly _bytes As Integer
    ReadOnly _buffer() As Byte
    Friend soundPlayer As System.Media.SoundPlayer
    Friend Sub New(bytesBuffer() As Byte, bPos As Integer, nBytes As Integer)
        ' to verify a wav by just playing it:
        'wavStream = New MemoryStream(CInt(nBytes + 44))
        'waveFormat = New WaveFormat(44100, 16, 2)
        'Wwriter = New WaveWriter(wavStream, waveFormat)
        'Wwriter.Write(bytesBuffer, bPos, nBytes)
        'CType(Wwriter, IDisposable).Dispose()
        'wavStream.Position = 0 ' like closing, so read starts at the beginning
        'soundPlayer = New System.Media.SoundPlayer(wavStream)
        'soundPlayer.Play()
        'soundPlayer.Dispose()
        'soundPlayer = Nothing
        'wavStream.Dispose()
        'wavStream.Close()
        SoundMix.Playing = True
        _pos = bPos
        _bytes = nBytes
        _buffer = bytesBuffer
    End Sub
    Friend Sub PlayWav(ViewOrList As Boolean) ' true: update views. false: update progressbar
        Dim buffersStacked As Integer = 0
        Dim bufferLength As Integer = 4 * 4410
        Dim playStart As Integer = _pos
        Dim playEnd As Integer = _pos + _bytes
        While (playStart <= playEnd And SoundMix.Playing)
            If SoundMix.BackgroundPlayer.BuffersWaiting < 3 Then
                Dim playBuf() As Byte = New Byte(bufferLength - 1) {} '// circa 0.1 sec stereo rate 44100
                Dim nPlay As Integer = playBuf.Length
                If playStart + nPlay >= _buffer.Length Then
                    nPlay = _buffer.Length - playStart
                    playBuf = New Byte(nPlay - 1) {}
                End If
                Buffer.BlockCopy(_buffer, playStart, playBuf, 0, nPlay)
                SoundMix.BackgroundPlayer.Buffer(playBuf)
                playStart += nPlay
                buffersStacked += 1
            Else
                If ViewOrList Then
                    SoundMix.MarkPlayProgress(CInt((buffersStacked - 2) * bufferLength / 4))
                Else
                    SoundMix.MarkProgressBar(playStart - _pos)
                End If
                Thread.Sleep(95) ' // one buffer = 100 ms
            End If
            Application.DoEvents()
        End While
        If ViewOrList Then
            SoundMix.MarkPlayProgress(-1) ' restore last line
        Else
            SoundMix.MarkProgressBar(-1)
        End If
        SoundMix.Playing = False
    End Sub
    Friend Sub StopWav()
        SoundMix.Playing = False
    End Sub
End Class
Public Class WaveFile
    Friend FileName As String
    Friend pulsBufferSize As Integer
    Friend NrPulses As Integer
    Friend sampleBlocks As List(Of Byte())
    Sub New(_fn As String, _bufferSz As Integer)
        FileName = _fn
        pulsBufferSize = CInt(_bufferSz / 4) ' nr of pulses, 4 bytes (2 short samples) per puls
        sampleBlocks = New List(Of Byte())
    End Sub
    Friend Function GetBufPulses(startPos As Integer, nPulses As Integer) As Byte()
        Dim bytesBuffer() As Byte = New Byte(nPulses * 4 - 1) {}
        Dim firstBlock As Integer = startPos \ pulsBufferSize ' 1st block 
        Dim firstPuls As Integer = startPos Mod pulsBufferSize ' first sample in 1st block
        Dim pulsInFirstBlock As Integer = pulsBufferSize - firstPuls ' nr of bytes remaining in block 1
        Dim lastBlock As Integer = (startPos + nPulses - 1) \ pulsBufferSize ' last block
        If firstBlock = lastBlock Then
            Buffer.BlockCopy(sampleBlocks(firstBlock), firstPuls * 4, bytesBuffer, 0, nPulses * 4) ' copy all data from 1st partial block 
        Else
            Dim pulsInLastBlock As Integer = (startPos + nPulses - 1) Mod pulsBufferSize ' number of samples from first / last block 
            Dim nrOfFullBlocks As Integer = CInt((nPulses - pulsInFirstBlock - pulsInLastBlock) / pulsBufferSize)
            Buffer.BlockCopy(sampleBlocks(firstBlock), firstPuls * 4, bytesBuffer, 0, pulsInFirstBlock * 4) ' copy first block from startpos
            Dim offsetInBuffer As Integer = pulsInFirstBlock
            For i = 1 To nrOfFullBlocks ' buffer contains Byte, sampleBlocs contains pulses (of 2 short integers)
                Buffer.BlockCopy(sampleBlocks(firstBlock + i), 0, bytesBuffer, offsetInBuffer * 4, pulsBufferSize * 4)
                offsetInBuffer += pulsBufferSize
            Next
            Buffer.BlockCopy(sampleBlocks(lastBlock), 0, bytesBuffer, offsetInBuffer * 4, pulsInLastBlock * 4) ' copy last block, upto endpos
        End If
        Return bytesBuffer
    End Function
End Class
Class PlaySoundBuffer
    Implements IWaveSource
    Private ReadOnly _waveFormat As WaveFormat
    Private _buffer As Byte()
    Private _offset As Integer
    Private _dataAvailable As Integer
    Friend _avail As List(Of Byte())
    Private ReadOnly sin As Byte()
    Private _soffs As Integer
    Private ReadOnly sing As IWaveSource
    Sub New()
        _waveFormat = New WaveFormat(44100, 16, 2, AudioEncoding.Pcm)
        _buffer = Nothing
        _dataAvailable = 0
        _avail = New List(Of Byte())
        '// this wave will sound when nothing Is available
        sing = New SineGenerator(400, 0.00, 0).ToWaveSource(16) '// one 400hz sine with volume 0, repeated each 1764 samples (0 to 1763)  
        sin = New Byte(1764) {}
        sing.Read(sin, 0, sin.Length)
        _soffs = 0
    End Sub
    Public Sub Buffer(aBuffer As Byte())
        If (aBuffer.Length > 0) Then
            _avail.Add(aBuffer)
        End If
    End Sub
    Public Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer Implements IReadableAudioSource(Of Byte).Read
        If IsNothing(_buffer) Then ' not yet playing at all, 
            If (_avail.Count > 0) Then ' use first buffer, if offered
                _buffer = _avail(0)
                _avail.RemoveAt(0)
                _offset = 0
                _dataAvailable = _buffer.Length
            End If
        End If
        For i = 0 To count - 1 ' for each puls,  
            If (_dataAvailable = 0) Then ' previous buffer exhausted, perhaps there are other buffers waiting
                If (_avail.Count > 0) Then ' take from next buffer, if present
                    _buffer = _avail(0)
                    _avail.RemoveAt(0)
                    _offset = 0
                    _dataAvailable = _buffer.Length
                End If
            End If
            If (_dataAvailable > 0) Then ' if puls present in (previous or new) buffer, use it
                buffer(offset + i) = _buffer(_offset)
                _dataAvailable -= 1
                _offset += 1
            Else
                buffer(offset + i) = sin(_soffs) ' if not, use the silence buffer
                _soffs += 1
                If (_soffs > sin.Length - 1) Then ' silence exhausted, start al over
                    _soffs = 0
                End If
            End If
        Next
        Return count
    End Function
    Public ReadOnly Property WaveFormat As WaveFormat Implements IAudioSource.WaveFormat
        Get
            WaveFormat = _waveFormat
        End Get
    End Property
    Public ReadOnly Property BuffersWaiting As Integer
        Get
            Return _avail.Count
        End Get
    End Property
    Public Property Position As Long Implements IAudioSource.Position
        Get
            Position = 0
        End Get
        Set(value As Long)
        End Set
    End Property
    Public ReadOnly Property Length As Long Implements IAudioSource.Length
        Get
            Length = 0
        End Get
    End Property
    Public ReadOnly Property CanSeek As Boolean Implements IAudioSource.CanSeek
        Get
            CanSeek = False
        End Get
    End Property
    Public Sub Dispose() Implements IDisposable.Dispose
    End Sub
End Class
Friend Class PlayListEntry
    Friend fileix As Integer
    Friend startPulsList As Integer ' alignment with other items
    Friend pulsesAdded As Integer ' for undo
    Friend startPuls As Integer ' start in file
    Friend nPulses As Integer ' length in file and list
End Class