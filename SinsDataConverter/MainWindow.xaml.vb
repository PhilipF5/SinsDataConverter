Class MainWindow
    Dim PathType As String
    Dim HasCustomEXE As Boolean = False
    Dim CustomEXEPath As String
    Dim SubfolderIndex As New System.Collections.ObjectModel.Collection(Of String)
    Dim RegSins As Microsoft.Win32.RegistryKey
    Dim RegEntrench As Microsoft.Win32.RegistryKey
    Dim RegDiplo As Microsoft.Win32.RegistryKey
    Dim RegTrinity As Microsoft.Win32.RegistryKey
    Dim RegRebel As Microsoft.Win32.RegistryKey

    Delegate Sub cleanupthreaddataDelegate()

    ' Variables to support threading
    Dim UsingSins As Boolean = False
    Dim UsingEntrench As Boolean = False
    Dim UsingDiplo As Boolean = False
    Dim UsingRebel As Boolean = False
    Dim ConversionPath As String
    Dim OutputLocation As String
    Dim UsingSubfolders As Boolean = False
    Dim ExcludedSubfolders As System.Collections.IList
    Dim ConvertToTXT As Boolean = True
    Dim ConvertToBIN As Boolean = False

    Private Sub GetRegistryPaths()
        If IntPtr.Size = 8 Then
            RegSins = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sins", False)
            RegEntrench = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinsentrench", False)
            RegDiplo = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinsdiplo", False)
            RegTrinity = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinstrinity", False)
            RegRebel = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinsrebellion", False)
        Else
            RegSins = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sins", False)
            RegEntrench = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinsentrench", False)
            RegDiplo = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinsdiplo", False)
            RegTrinity = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinstrinity", False)
            RegRebel = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinsrebellion", False)
        End If
    End Sub

    Private Function SetCustomGamePath() As Boolean
        Dim ValidPath As Boolean = False
        Do Until ValidPath = True
            Dim folders As New System.Windows.Forms.FolderBrowserDialog
            folders.Description = "Select your custom game path"
            If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ValidPath = CheckPathForGames(True, folders.SelectedPath, My.Computer.FileSystem.GetDirectories(folders.SelectedPath, FileIO.SearchOption.SearchTopLevelOnly),
                                              My.Computer.FileSystem.GetFiles(folders.SelectedPath, FileIO.SearchOption.SearchTopLevelOnly))
            End If
            If ValidPath = False Then
                If MsgBox("No valid path selected!  Try again?", MsgBoxStyle.YesNo, "Error") = MsgBoxResult.No Then
                    Exit Do
                End If
            Else
                MsgBox("Custom game path set successfully!", MsgBoxStyle.OkOnly, "Success")
                My.Settings.SavedInstall = folders.SelectedPath
            End If
        Loop
        Return ValidPath
    End Function

    Private Function CheckPathForGames(Optional ByVal Custom As Boolean = False, Optional ByVal CustomPath As String = "",
                                       Optional ByVal FoldersInPath As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing,
                                       Optional ByVal FilesInPath As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing) As Boolean
        Dim HasGame As Boolean = True
        Dim CheckPath As String
        If Custom = False Then
            Try
                CheckPath = RegRebel.GetValue("Path") + "\"
            Catch NoRebellion As NullReferenceException
                btnRebellion.IsEnabled = False
                Try
                    CheckPath = RegTrinity.GetValue("Path") + "\"
                Catch NoTrinity As NullReferenceException
                    Try
                        CheckPath = RegDiplo.GetValue("Path") + "\"
                    Catch NoDiplomacy As NullReferenceException
                        btnDiplomacy.IsEnabled = False
                        Try
                            CheckPath = RegEntrench.GetValue("Path") + "\"
                        Catch NoEntrenchment As NullReferenceException
                            btnEntrenchment.IsEnabled = False
                            Try
                                CheckPath = RegSins.GetValue("Path") + "\"
                            Catch NoSins As NullReferenceException
                                btnSins.IsEnabled = False
                                HasGame = False
                            End Try
                        End Try
                    End Try
                End Try
            End Try
            If btnRebellion.IsEnabled Then
                btnDiplomacy.IsEnabled = False
                btnEntrenchment.IsEnabled = False
                btnSins.IsEnabled = False
            End If
        Else
            If Not CustomPath.Contains("Rebellion") Then
                btnRebellion.IsEnabled = False
                If Not FoldersInPath.Contains("Diplomacy") Then
                    btnDiplomacy.IsEnabled = False
                    If Not FoldersInPath.Contains("Entrenchment") Then
                        btnEntrenchment.IsEnabled = False
                        If Not FilesInPath.Contains(CustomPath + "\Sins of a Solar Empire.exe") Then
                            btnSins.IsEnabled = False
                            HasGame = False
                        Else
                            btnSins.IsEnabled = True
                        End If
                    Else
                        btnEntrenchment.IsEnabled = True
                    End If
                Else
                    btnDiplomacy.IsEnabled = True
                End If
            Else
                btnDiplomacy.IsEnabled = False
                btnEntrenchment.IsEnabled = False
                btnSins.IsEnabled = False
            End If
        End If
        Return HasGame
    End Function

    Private Sub btnFile_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnFile.Click
        Dim files As New System.Windows.Forms.OpenFileDialog
        files.Filter = "Mesh|*.mesh|Particle|*.particle|Brushes|*.brushes|Entity|*.entity"
        files.FilterIndex = 4
        files.Title = "Select a file to convert..."
        files.InitialDirectory = "Desktop"
        If files.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AcceptSourceFile(sender, e, files.FileName)
        End If
    End Sub

    Private Sub AcceptSourceFile(ByVal sender As Object, ByVal e As RoutedEventArgs, ByVal Path As String)
        txtSource.Text = Path
        PathType = "file"
        Dim Sequence As New System.Windows.Media.Animation.Storyboard
        If btnSub.IsEnabled = True Then
            Dim NoSubfolders As New System.Windows.Media.Animation.ColorAnimation
            NoSubfolders.To = Colors.Red
            NoSubfolders.Duration = TimeSpan.FromSeconds(0.5)
            NoSubfolders.AutoReverse = True
            Dim Brush2 As New SolidColorBrush
            Brush2.Color = Colors.White
            btnSub.Background = Brush2
            btnSub.Foreground = Brush2
            Me.RegisterName("ForeColor2", Brush2)
            Animation.Storyboard.SetTargetName(NoSubfolders, "ForeColor2")
            Animation.Storyboard.SetTargetProperty(NoSubfolders, New PropertyPath(SolidColorBrush.ColorProperty))
            Sequence.Children.Add(NoSubfolders)
        End If
        btnSub.IsEnabled = False
        btnSub.IsChecked = False
        Dim SourceSelected As New System.Windows.Media.Animation.ColorAnimation
        SourceSelected.To = Colors.LimeGreen
        SourceSelected.Duration = TimeSpan.FromSeconds(0.5)
        SourceSelected.AutoReverse = True
        Dim Brush As New SolidColorBrush
        Brush.Color = Colors.White
        lblConvert.Foreground = Brush
        txtSource.Foreground = Brush
        Me.RegisterName("ForeColor", Brush)
        Animation.Storyboard.SetTargetName(SourceSelected, "ForeColor")
        Animation.Storyboard.SetTargetProperty(SourceSelected, New PropertyPath(SolidColorBrush.ColorProperty))
        Sequence.Children.Add(SourceSelected)
        Sequence.Begin(Me)
        Me.UnregisterName("ForeColor")
        Try
            Me.UnregisterName("ForeColor2")
        Catch ex As ArgumentException

        End Try
    End Sub

    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnFolder.Click
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AcceptSourceFolder(sender, e, folders.SelectedPath)
        End If
    End Sub

    Private Sub AcceptSourceFolder(ByVal sender As Object, ByVal e As RoutedEventArgs, ByVal Path As String)
        txtSource.Text = Path
        PathType = "folder"
        btnSub.IsEnabled = True
        ClearSubfolders(sender, e)
        If btnSub.IsChecked Then
            RetrieveSubfolders(sender, e)
        End If
        Dim SourceSelected As New System.Windows.Media.Animation.ColorAnimation
        SourceSelected.To = Colors.LimeGreen
        SourceSelected.Duration = TimeSpan.FromSeconds(0.5)
        SourceSelected.AutoReverse = True
        Dim Brush As New SolidColorBrush
        Brush.Color = Colors.White
        lblConvert.Foreground = Brush
        txtSource.Foreground = Brush
        btnSub.Background = Brush
        btnSub.Foreground = Brush
        Dim Sequence As New System.Windows.Media.Animation.Storyboard
        Me.RegisterName("ForeColor", Brush)
        Animation.Storyboard.SetTargetName(SourceSelected, "ForeColor")
        Animation.Storyboard.SetTargetProperty(SourceSelected, New PropertyPath(SolidColorBrush.ColorProperty))
        Sequence.Children.Add(SourceSelected)
        Sequence.Begin(Me)
        Me.UnregisterName("ForeColor")
    End Sub

    Private Sub RetrieveSubfolders(ByVal sender As Object, ByVal e As RoutedEventArgs)
        For Each Subfolder In My.Computer.FileSystem.GetDirectories(txtSource.Text, FileIO.SearchOption.SearchAllSubDirectories)
            SubfolderIndex.Add(Subfolder)
        Next
        For Each Subfolder In SubfolderIndex
            Dim SubfolderName As String = Subfolder.Replace(txtSource.Text + "\", "")
            lstSubfolders.Items.Add(SubfolderName)
        Next
    End Sub

    Private Sub ClearSubfolders(ByVal sender As Object, ByVal e As RoutedEventArgs)
        SubfolderIndex.Clear()
        lstSubfolders.Items.Clear()
    End Sub

    Private Sub btnOutput_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOutput.Click
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtOutput.Text = folders.SelectedPath
            btnInPlace.IsChecked = False
            OutputSelectedAnimation(sender, e)
        End If
    End Sub

    Private Sub OutputSelectedAnimation(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim OutputSelected As New System.Windows.Media.Animation.ColorAnimation
        OutputSelected.To = Colors.LimeGreen
        OutputSelected.Duration = TimeSpan.FromSeconds(0.5)
        OutputSelected.AutoReverse = True
        Dim Brush As New SolidColorBrush
        Brush.Color = Colors.White
        lblOutput.Foreground = Brush
        txtOutput.Foreground = Brush
        Dim Sequence As New System.Windows.Media.Animation.Storyboard
        Me.RegisterName("ForeColor", Brush)
        Animation.Storyboard.SetTargetName(OutputSelected, "ForeColor")
        Animation.Storyboard.SetTargetProperty(OutputSelected, New PropertyPath(SolidColorBrush.ColorProperty))
        Sequence.Children.Add(OutputSelected)
        Sequence.Begin(Me)
        Me.UnregisterName("ForeColor")
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnReset.Click
        txtSource.Text = Nothing
        txtOutput.Text = Nothing
        PathType = Nothing
        btnSins.IsChecked = False
        btnEntrenchment.IsChecked = False
        btnDiplomacy.IsChecked = False
        btnRebellion.IsChecked = False
        btnInPlace.IsChecked = False
        CustomOff(sender, e)
    End Sub

    Private Sub CustomOn(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs, ByVal Path As String)
        CustomEXEPath = Path
        HasCustomEXE = True
        My.Settings.SavedEXE = CustomEXEPath
        Dim CustomSelected1 As New System.Windows.Media.Animation.DoubleAnimation
        CustomSelected1.To = 0
        CustomSelected1.Duration = New Duration(TimeSpan.FromSeconds(1))
        CustomSelected1.AutoReverse = False
        Dim CustomSelected2 As New System.Windows.Media.Animation.DoubleAnimation
        CustomSelected2.To = 1
        CustomSelected2.Duration = New Duration(TimeSpan.FromSeconds(1))
        CustomSelected2.AutoReverse = False
        Dim Sequence As New System.Windows.Media.Animation.Storyboard
        Sequence.Children.Add(CustomSelected1)
        Animation.Storyboard.SetTargetName(CustomSelected1, grpVersion.Name)
        Animation.Storyboard.SetTargetProperty(CustomSelected1, New PropertyPath(StackPanel.OpacityProperty))
        Sequence.Children.Add(CustomSelected2)
        Animation.Storyboard.SetTargetName(CustomSelected2, lblCustomSelected.Name)
        Animation.Storyboard.SetTargetProperty(CustomSelected2, New PropertyPath(StackPanel.OpacityProperty))
        Sequence.Begin(Me)
    End Sub

    Private Sub CustomOff(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CustomEXEPath = Nothing
        HasCustomEXE = False
        My.Settings.SavedEXE = ""
        Dim CustomSelected1 As New System.Windows.Media.Animation.DoubleAnimation
        CustomSelected1.To = 1
        CustomSelected1.Duration = New Duration(TimeSpan.FromSeconds(1))
        CustomSelected1.AutoReverse = False
        Dim CustomSelected2 As New System.Windows.Media.Animation.DoubleAnimation
        CustomSelected2.To = 0
        CustomSelected2.Duration = New Duration(TimeSpan.FromSeconds(1))
        CustomSelected2.AutoReverse = False
        Dim Sequence As New System.Windows.Media.Animation.Storyboard
        Sequence.Children.Add(CustomSelected1)
        Animation.Storyboard.SetTargetName(CustomSelected1, grpVersion.Name)
        Animation.Storyboard.SetTargetProperty(CustomSelected1, New PropertyPath(StackPanel.OpacityProperty))
        Sequence.Children.Add(CustomSelected2)
        Animation.Storyboard.SetTargetName(CustomSelected2, lblCustomSelected.Name)
        Animation.Storyboard.SetTargetProperty(CustomSelected2, New PropertyPath(StackPanel.OpacityProperty))
        Sequence.Begin(Me)
    End Sub

    Private Sub Conversion()
        If ConversionPath = "" _
        Or OutputLocation = "" Then
            MsgBox("You must select both a source file or folder, and an output folder!", , "Error")
            Return
        End If
        If UsingSins = False _
        And UsingEntrench = False _
        And UsingDiplo = False _
        And UsingRebel = False _
        And HasCustomEXE = False Then
            MsgBox("You must select a either an installed version of Sins, or a custom version!", , "Error")
            Return
        End If
        Dim RegPath As Microsoft.Win32.RegistryKey
        Dim SinsDir As String
        If Not My.Settings.SavedInstall = "" And Not My.Settings.SavedInstall = Nothing Then
            SinsDir = My.Settings.SavedInstall
        ElseIf HasCustomEXE = False Then
            If IntPtr.Size = 8 Then
                RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinsrebellion", False)
                Try
                    SinsDir = RegPath.GetValue("Path")
                Catch ex3 As NullReferenceException
                    Try
                        RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sins", False)
                        SinsDir = RegPath.GetValue("Path")
                    Catch ex As NullReferenceException
                        Try
                            RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinstrinity", False)
                            SinsDir = RegPath.GetValue("Path")
                        Catch ex2 As NullReferenceException
                            If MsgBox("Sins is not properly installed on your computer.  Would you like to set a custom default path for the game?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
                                If SetCustomGamePath() = True Then
                                    SinsDir = My.Settings.SavedInstall
                                Else
                                    MsgBox("Conversion failed!", MsgBoxStyle.OkOnly, "Error")
                                    Return
                                End If
                            Else
                                MsgBox("Conversion failed!", MsgBoxStyle.OkOnly, "Error")
                                Return
                            End If
                        End Try
                    End Try
                End Try
            Else
                RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinsrebellion", False)
                Try
                    SinsDir = RegPath.GetValue("Path")
                Catch ex3 As NullReferenceException
                    Try
                        RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sins", False)
                        SinsDir = RegPath.GetValue("Path")
                    Catch ex As NullReferenceException
                        Try
                            RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinstrinity", False)
                            SinsDir = RegPath.GetValue("Path")
                        Catch ex2 As NullReferenceException
                            If MsgBox("Sins is not properly installed on your computer.  Would you like to set a custom default path for the game?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
                                If SetCustomGamePath() = True Then
                                    SinsDir = My.Settings.SavedInstall
                                Else
                                    MsgBox("Conversion failed!", MsgBoxStyle.OkOnly, "Error")
                                    Return
                                End If
                            Else
                                MsgBox("Conversion failed!", MsgBoxStyle.OkOnly, "Error")
                                Return
                            End If
                        End Try
                    End Try
                End Try
            End If
        End If
        Dim writer As System.IO.StreamWriter
        Dim AppData As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
        My.Computer.FileSystem.CreateDirectory(AppData)
        writer = My.Computer.FileSystem.OpenTextFileWriter(AppData + "\sinsconversion.bat", True)
        writer.WriteLine("@echo off")
        If HasCustomEXE = True Then
            If PathType = "file" Then
                Dim FileType As String = " entity "
                If ConversionPath.EndsWith(".mesh") Then
                    FileType = " mesh "
                ElseIf ConversionPath.EndsWith(".particle") Then
                    FileType = " particle "
                ElseIf ConversionPath.EndsWith(".brushes") Then
                    FileType = " brushes "
                ElseIf ConversionPath.EndsWith(".entity") Then
                    FileType = " entity "
                End If
                Dim Format As String
                If ConvertToTXT Then
                    Format = " txt"
                Else
                    Format = ""
                End If
                writer.WriteLine("""" + CustomEXEPath + """" + FileType + """" + ConversionPath + """ """ + OutputLocation +
                                 ConversionPath.Remove(0, ConversionPath.LastIndexOf("\")) + """" + Format)
            ElseIf PathType = "folder" Then
                Dim fileindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
                If Not UsingSubfolders Then
                    fileindex = My.Computer.FileSystem.GetFiles(ConversionPath, FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
                Else
                    fileindex = My.Computer.FileSystem.GetFiles(ConversionPath, FileIO.SearchOption.SearchAllSubDirectories, "*.mesh", "*.particle", "*.brushes", "*.entity")
                    Dim subfolderindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetDirectories(ConversionPath)
                    For Each subfolder In subfolderindex
                        Dim SubfolderName As String = subfolder.Replace(ConversionPath + "\", "")
                        If Not ExcludedSubfolders.Contains(SubfolderName) Then
                            My.Computer.FileSystem.CreateDirectory(OutputLocation + "\" + SubfolderName)
                        End If
                    Next
                End If
                For Each file In fileindex
                    Dim FileType As String = " entity "
                    If file.EndsWith(".mesh") Then
                        FileType = " mesh "
                    ElseIf file.EndsWith(".particle") Then
                        FileType = " particle "
                    ElseIf file.EndsWith(".brushes") Then
                        FileType = " brushes "
                    ElseIf file.EndsWith(".entity") Then
                        FileType = " entity "
                    End If
                    Dim Format As String
                    If ConvertToTXT Then
                        Format = " txt"
                    Else
                        Format = ""
                    End If
                    If Not UsingSubfolders Then
                        writer.WriteLine("""" + CustomEXEPath + """" + FileType + """" + file + """ """ + OutputLocation + file.Remove(0, file.LastIndexOf("\")) + """" + Format)
                    Else
                        writer.WriteLine("""" + CustomEXEPath + """" + FileType + """" + file + """ """ + OutputLocation + file.Replace(ConversionPath, "") + """" + Format)
                    End If
                Next
            End If
        ElseIf HasCustomEXE = False Then
            writer.WriteLine("cd /D """ + SinsDir + """")
            If PathType = "file" Then
                Dim FileType As String = " entity "
                If ConversionPath.EndsWith(".mesh") Then
                    FileType = " mesh "
                ElseIf ConversionPath.EndsWith(".particle") Then
                    FileType = " particle "
                ElseIf ConversionPath.EndsWith(".brushes") Then
                    FileType = " brushes "
                ElseIf ConversionPath.EndsWith(".entity") Then
                    FileType = " entity "
                End If
                Dim Converter As String = "ConvertData_OriginalSins.exe"
                If UsingSins Then
                    Converter = "ConvertData_OriginalSins.exe"
                ElseIf UsingEntrench Then
                    Converter = "ConvertData_Entrenchment.exe"
                ElseIf UsingDiplo Then
                    Converter = "ConvertData_Diplomacy.exe"
                ElseIf UsingRebel Then
                    Converter = "ConvertData_Rebellion.exe"
                End If
                Dim Format As String
                If ConvertToTXT Then
                    Format = " txt"
                Else
                    Format = ""
                End If
                writer.WriteLine(Converter + FileType + """" + ConversionPath + """ """ + OutputLocation + ConversionPath.Remove(0, ConversionPath.LastIndexOf("\")) + """" + Format)
            ElseIf PathType = "folder" Then
                Dim fileindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
                If Not UsingSubfolders Then
                    fileindex = My.Computer.FileSystem.GetFiles(ConversionPath, FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
                Else
                    fileindex = My.Computer.FileSystem.GetFiles(ConversionPath, FileIO.SearchOption.SearchAllSubDirectories, "*.mesh", "*.particle", "*.brushes", "*.entity")
                    Dim subfolderindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetDirectories(ConversionPath)
                    For Each subfolder In subfolderindex
                        Dim SubfolderName As String = subfolder.Replace(ConversionPath + "\", "")
                        If Not ExcludedSubfolders.Contains(SubfolderName) Then
                            My.Computer.FileSystem.CreateDirectory(OutputLocation + "\" + SubfolderName)
                        End If
                    Next
                End If
                For Each file In fileindex
                    Dim FileType As String = " entity "
                    If file.EndsWith(".mesh") Then
                        FileType = " mesh "
                    ElseIf file.EndsWith(".particle") Then
                        FileType = " particle "
                    ElseIf file.EndsWith(".brushes") Then
                        FileType = " brushes "
                    ElseIf file.EndsWith(".entity") Then
                        FileType = " entity "
                    End If
                    Dim Converter As String = "ConvertData_OriginalSins.exe"
                    If UsingSins Then
                        Converter = "ConvertData_OriginalSins.exe"
                    ElseIf UsingEntrench Then
                        Converter = "ConvertData_Entrenchment.exe"
                    ElseIf UsingDiplo Then
                        Converter = "ConvertData_Diplomacy.exe"
                    ElseIf UsingRebel Then
                        Converter = "ConvertData_Rebellion.exe"
                    End If
                    Dim Format As String
                    If ConvertToTXT Then
                        Format = " txt"
                    Else
                        Format = ""
                    End If
                    If Not UsingSubfolders Then
                        writer.WriteLine(Converter + FileType + """" + file + """ """ + OutputLocation + file.Remove(0, file.LastIndexOf("\")) + """" + Format)
                    Else
                        writer.WriteLine(Converter + FileType + """" + file + """ """ + OutputLocation + file.Replace(ConversionPath, "") + """" + Format)
                    End If
                Next
            End If
        End If
        If My.Settings.Logging Then
            writer.WriteLine("copy """ + AppData + "\sinsconversion.bat"" " + My.Computer.FileSystem.SpecialDirectories.Desktop)
        End If
        writer.WriteLine("del """ + AppData + "\sinsconversion.bat""")
        writer.Close()
        Shell(AppData + "\sinsconversion.bat", AppWinStyle.Hide, True)
        MsgBox("Conversion complete!", , "Success")
        Dispatcher.Invoke(New cleanupthreaddataDelegate(AddressOf EndConversion))
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHelp.Click
        My.Windows.Instructions.ShowDialog()
    End Sub

    Private Sub btnCustom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCustom.Click
        Dim CustomLocation As New System.Windows.Forms.OpenFileDialog
        CustomLocation.Filter = "ConvertData|*.exe"
        CustomLocation.FilterIndex = 1
        CustomLocation.Title = "Select a custom ConvertData executable..."
        CustomLocation.InitialDirectory = "Desktop"
        If CustomLocation.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CustomOn(sender, e, CustomLocation.FileName)
        End If
    End Sub

    Private Sub btnTheme_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Height = 457
    End Sub

    Private Sub LoadTheme(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim NewBG As New ImageBrush
        Dim BGSource As System.Windows.Media.ImageSource
        If My.Settings.Theme = 2 Then
            BGSource = New BitmapImage(New Uri("/Images/skybox02environmentcube.png", UriKind.Relative))
            NewBG.ImageSource = BGSource
            Me.Background = NewBG
        End If
    End Sub

    Private Sub MainWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        GetRegistryPaths()
        If CheckPathForGames(False) = False Then
            If MsgBox("Sins is not properly installed on your computer.  Would you like to set a custom default path for the game?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
                SetCustomGamePath()
            End If
        End If
        If Not My.Settings.SavedEXE = "" Then
            CustomOn(sender, e, My.Settings.SavedEXE)
        End If
        If My.Settings.Logging = True Then
            btnLog.IsChecked = True
        End If
    End Sub

    Private Sub btnInPlace_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnInPlace.Checked
        If Not txtSource.Text Is Nothing _
        And Not txtSource.Text = "" Then
            If PathType = "file" Then
                txtOutput.Text = txtSource.Text.Remove(txtSource.Text.LastIndexOf("\"))
            ElseIf PathType = "folder" Then
                txtOutput.Text = txtSource.Text
            End If
            OutputSelectedAnimation(sender, e)
        Else
            btnInPlace.IsChecked = False
            Dim Sequence As New System.Windows.Media.Animation.Storyboard
            Dim SourceSelected1 As New System.Windows.Media.Animation.ColorAnimation
            SourceSelected1.To = Colors.Red
            SourceSelected1.Duration = TimeSpan.FromSeconds(0.5)
            SourceSelected1.AutoReverse = True
            Dim Brush As New SolidColorBrush
            Brush.Color = Colors.White
            lblConvert.Foreground = Brush
            Me.RegisterName("ForeColor", Brush)
            Animation.Storyboard.SetTargetName(SourceSelected1, "ForeColor")
            Animation.Storyboard.SetTargetProperty(SourceSelected1, New PropertyPath(SolidColorBrush.ColorProperty))
            Sequence.Children.Add(SourceSelected1)
            Sequence.Begin(Me)
            Me.UnregisterName("ForeColor")
        End If
    End Sub

    Private Sub btnFile_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnFile.Drop
        If e.Data.GetDataPresent("FileNameW", False) Then
            Dim names As System.Array = CType(e.Data.GetData("FileNameW", False), System.Array)
            Dim Path As String = CStr(names.GetValue(0))
            If Path.EndsWith(".entity") _
            Or Path.EndsWith(".mesh") _
            Or Path.EndsWith(".particle") _
            Or Path.EndsWith(".brushes") Then
                AcceptSourceFile(sender, e, CStr(names.GetValue(0)))
            Else
                MsgBox("Invalid file type", , "Error")
            End If
        End If
    End Sub

    Private Sub btnFile_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnFile.DragOver
        If e.Data.GetDataPresent("FileNameW", False) Then
            e.Effects = DragDropEffects.Copy
        Else
            e.Effects = DragDropEffects.None
        End If
    End Sub

    Private Sub btnFolder_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnFolder.Drop
        If e.Data.GetDataPresent("FileNameW", False) Then
            Dim names As System.Array = CType(e.Data.GetData("FileNameW", False), System.Array)
            AcceptSourceFolder(sender, e, CStr(names.GetValue(0)))
        End If
    End Sub

    Private Sub btnFolder_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnFolder.DragOver
        If e.Data.GetDataPresent("FileNameW", False) Then
            e.Effects = DragDropEffects.Copy
        Else
            e.Effects = DragDropEffects.None
        End If
    End Sub

    Private Sub btnOutput_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnOutput.Drop
        If e.Data.GetDataPresent("FileNameW", False) Then
            Dim names As System.Array = CType(e.Data.GetData("FileNameW", False), System.Array)
            txtOutput.Text = CStr(names.GetValue(0))
            OutputSelectedAnimation(sender, e)
        End If
    End Sub

    Private Sub btnOutput_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnOutput.DragOver
        If e.Data.GetDataPresent("FileNameW", False) Then
            e.Effects = DragDropEffects.Copy
        Else
            e.Effects = DragDropEffects.None
        End If
    End Sub

    Private Sub btnSourcePasteFolder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSourcePasteFolder.Click
        Dim iData As IDataObject = Clipboard.GetDataObject()
        If iData.GetDataPresent(DataFormats.Text) Then
            AcceptSourceFolder(sender, e, iData.GetData(DataFormats.Text))
        End If
    End Sub

    Private Sub btnOutputPasteFolder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOutputPasteFolder.Click
        Dim iData As IDataObject = Clipboard.GetDataObject()
        If iData.GetDataPresent(DataFormats.Text) Then
            txtOutput.Text = iData.GetData(DataFormats.Text)
            OutputSelectedAnimation(sender, e)
        End If
    End Sub

    Private Sub btnSub_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSub.Checked
        Me.Width = 729
        RetrieveSubfolders(sender, e)
    End Sub

    Private Sub btnSub_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSub.Unchecked
        Me.Width = 497
        ClearSubfolders(sender, e)
    End Sub

    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnConvert.Click
        UsingSins = btnSins.IsChecked
        UsingEntrench = btnEntrenchment.IsChecked
        UsingDiplo = btnDiplomacy.IsChecked
        UsingRebel = btnRebellion.IsChecked
        ConversionPath = txtSource.Text
        OutputLocation = txtOutput.Text
        UsingSubfolders = btnSub.IsChecked
        If UsingSubfolders Then
            ExcludedSubfolders = lstSubfolders.SelectedItems
        End If
        ConvertToTXT = btnToTXT.IsChecked
        ConvertToBIN = btnToBIN.IsChecked
        MainWindow.IsEnabled = False
        prgsConversion.IsIndeterminate = True
        Dim ConversionThread As New Thread(AddressOf Conversion)
        ConversionThread.IsBackground = True
        ConversionThread.Start()
    End Sub

    Private Sub EndConversion()
        prgsConversion.IsIndeterminate = False
        MainWindow.IsEnabled = True
    End Sub

    Private Sub btnAdvanced_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAdvanced.Click
        Dim AdvancedWindow As New AdvancedWindow
        AdvancedWindow.Show()
        Me.Close()
    End Sub

    Private Sub btnLog_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLog.Checked
        My.Settings.Logging = True
    End Sub

    Private Sub btnLog_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLog.Unchecked
        My.Settings.Logging = False
    End Sub
End Class