Public Class AdvancedWindow

    Dim RegPath As Microsoft.Win32.RegistryKey
    Dim SinsDir As String
    Dim HasCustomEXE As Boolean = False
    Dim CustomEXEPath As String
    Dim SubfolderIndex As New System.Collections.ObjectModel.Collection(Of String)
    Dim Direction As New System.Collections.ObjectModel.Collection(Of String)
    Dim Version As New System.Collections.ObjectModel.Collection(Of String)
    Dim Output As New System.Collections.ObjectModel.Collection(Of String)
    Dim Validate As New System.Collections.ObjectModel.Collection(Of Boolean)
    Dim FileType As New System.Collections.ObjectModel.Collection(Of String)
    Dim ConfirmedSettings As Integer = 0
    Dim RegSins As Microsoft.Win32.RegistryKey
    Dim RegEntrench As Microsoft.Win32.RegistryKey
    Dim RegDiplo As Microsoft.Win32.RegistryKey
    Dim RegTrinity As Microsoft.Win32.RegistryKey
    Dim RegRebel As Microsoft.Win32.RegistryKey
    Dim PathToMods As String
    Dim ModFolder As String
    Dim NewPathToMods As String
    Dim AppData As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
    Dim ClearQueueWhenDone As Boolean = False

    Delegate Sub cleanupthreaddataDelegate()

    Private Sub SetCustomGamePath()
        Dim ValidPath As Boolean = False
        Do Until ValidPath = True
            Dim folders As New System.Windows.Forms.FolderBrowserDialog
            folders.Description = "Select your custom game path"
            If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ValidPath = CheckForGames(True, folders.SelectedPath, My.Computer.FileSystem.GetDirectories(folders.SelectedPath, FileIO.SearchOption.SearchTopLevelOnly),
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
    End Sub

    Private Sub AdvancedWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
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
        If (My.Settings.SavedInstall Is Nothing Or My.Settings.SavedInstall = "") And CheckForGames() = False Then
            If MsgBox("Sins is not properly installed on your computer.  Would you like to set a custom default path for the game?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
                SetCustomGamePath()
            End If
        End If
    End Sub

    Private Function CheckForGames(Optional ByVal Custom As Boolean = False, Optional ByVal CustomPath As String = "",
                                   Optional ByVal FoldersInPath As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing,
                                   Optional ByVal FilesInPath As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing) As Boolean
        Dim HasGame As Boolean = True
        If Custom = False Then
            Try
                SinsDir = RegRebel.GetValue("Path") + "\"
            Catch NoRebellion As NullReferenceException
                DisableRebellionOptions()
                Try
                    SinsDir = RegTrinity.GetValue("Path") + "\"
                Catch NoTrinity As NullReferenceException
                    Try
                        SinsDir = RegDiplo.GetValue("Path") + "\"
                    Catch NoDiplomacy As NullReferenceException
                        DisableDiplomacyOptions()
                        Try
                            SinsDir = RegEntrench.GetValue("Path") + "\"
                        Catch NoEntrenchment As NullReferenceException
                            DisableEntrenchmentOptions()
                            Try
                                SinsDir = RegSins.GetValue("Path") + "\"
                            Catch NoSins As NullReferenceException
                                DisableSinsOptions()
                                DisableAllOptions()
                                HasGame = False
                            End Try
                        End Try
                    End Try
                End Try
            End Try
            If btnRebellion.IsEnabled Then
                DisableDiplomacyOptions()
                DisableEntrenchmentOptions()
                DisableSinsOptions()
            End If
        Else
            If Not CustomPath.Contains("Rebellion") Then
                DisableRebellionOptions()
                If Not FoldersInPath.Contains("Diplomacy") Then
                    DisableDiplomacyOptions()
                    If Not FoldersInPath.Contains("Entrenchment") Then
                        DisableEntrenchmentOptions()
                        If Not FilesInPath.Contains(CustomPath + "\Sins of a Solar Empire.exe") Then
                            DisableSinsOptions()
                            DisableAllOptions()
                            HasGame = False
                        End If
                    End If
                End If
            Else
                DisableDiplomacyOptions()
                DisableEntrenchmentOptions()
                DisableSinsOptions()
            End If
        End If
        Return HasGame
    End Function

    Private Sub DisableRebellionOptions(Optional ByVal Enabled As Boolean = False)
        btnRebellion.IsEnabled = Enabled
        btnRDRebel.IsEnabled = Enabled
        btnEMRebel.IsEnabled = Enabled
    End Sub

    Private Sub DisableDiplomacyOptions(Optional ByVal Enabled As Boolean = False)
        btnDiplomacy.IsEnabled = Enabled
        btnRDDiplo.IsEnabled = Enabled
        btnEMDiplo.IsEnabled = Enabled
    End Sub

    Private Sub DisableEntrenchmentOptions(Optional ByVal Enabled As Boolean = False)
        btnEntrenchment.IsEnabled = Enabled
        btnRDEntrench.IsEnabled = Enabled
        btnEMEntrench.IsEnabled = Enabled
    End Sub

    Private Sub DisableSinsOptions(Optional ByVal Enabled As Boolean = False)
        btnSins.IsEnabled = Enabled
        btnRDSins.IsEnabled = Enabled
    End Sub

    Private Sub DisableAllOptions(Optional ByVal Enabled As Boolean = False)
        btnReferenceData.IsEnabled = Enabled
        btnManifest.IsEnabled = Enabled
        btnRelocateMods.IsEnabled = Enabled
    End Sub

    Private Sub CustomOn(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs, ByVal Path As String)
        CustomEXEPath = Path
        HasCustomEXE = True
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

    Private Sub btnBasic_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBasic.Click
        Dim MainWindow As New MainWindow
        MainWindow.Show()
        Me.Close()
    End Sub

    Private Sub btnAddToQueue_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddToQueue.Click
        AddToQueueMenu.IsOpen = True
    End Sub

    Private Sub AddFile(ByVal FileNames As String())
        For Each file As String In FileNames
            lstQueue.Items.Add(file)
            Direction.Add("")
            Version.Add("")
            Output.Add("")
            Validate.Add(False)
            If file.EndsWith(".mesh") Then
                FileType.Add(" mesh ")
            ElseIf file.EndsWith(".particle") Then
                FileType.Add(" particle ")
            ElseIf file.EndsWith(".brushes") Then
                FileType.Add(" brushes ")
            ElseIf file.EndsWith(".entity") Then
                FileType.Add(" entity ")
            End If
        Next
    End Sub

    Private Sub btnAddFile_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddFile.Click
        Dim files As New System.Windows.Forms.OpenFileDialog
        files.Filter = "Mesh|*.mesh|Particle|*.particle|Brushes|*.brushes|Entity|*.entity"
        files.FilterIndex = 4
        files.Title = "Select a file to convert..."
        files.InitialDirectory = "Desktop"
        files.Multiselect = True
        If files.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AddFile(files.FileNames)
        End If
    End Sub

    Private Sub AddFolder(ByVal Folder As String)
        Dim fileindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        If Not btnSub.IsChecked Then
            fileindex = My.Computer.FileSystem.GetFiles(Folder, FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
        Else
            fileindex = My.Computer.FileSystem.GetFiles(Folder, FileIO.SearchOption.SearchAllSubDirectories, "*.mesh", "*.particle", "*.brushes", "*.entity")
        End If
        For Each Job In fileindex
            lstQueue.Items.Add(Job)
            Direction.Add(" ")
            Version.Add(" ")
            Output.Add(" ")
            Validate.Add(False)
            If Job.EndsWith(".mesh") Then
                FileType.Add(" mesh ")
            ElseIf Job.EndsWith(".particle") Then
                FileType.Add(" particle ")
            ElseIf Job.EndsWith(".brushes") Then
                FileType.Add(" brushes ")
            ElseIf Job.EndsWith(".entity") Then
                FileType.Add(" entity ")
            End If
        Next
    End Sub

    Private Sub btnAddFolder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddFolder.Click
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            AddFolder(folders.SelectedPath)
        End If
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRemove.Click
        RemoveItems(lstQueue.SelectedItems)
    End Sub

    Private Sub RemoveItems(ByVal SourceList)
        Dim ItemsList As New System.Collections.ObjectModel.Collection(Of String)
        For Each Job In SourceList
            ItemsList.Add(Job)
        Next
        For Each Job In ItemsList
            Dim Index As Integer = lstQueue.Items.IndexOf(Job)
            Direction.RemoveAt(Index)
            Version.RemoveAt(Index)
            Output.RemoveAt(Index)
            Validate.RemoveAt(Index)
            FileType.RemoveAt(Index)
            ConfirmedSettings -= 1
            lstQueue.Items.Remove(Job)
        Next
        Return
    End Sub

    Private Sub btnOutput_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOutput.Click
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtOutput.Text = folders.SelectedPath
            btnInPlace.IsChecked = False
        End If
    End Sub

    Private Sub btnCustom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCustom.Click
        If HasCustomEXE Then
            CustomOff(sender, e)
            btnCustom.Content = "Custom EXE..."
        Else
            Dim CustomLocation As New System.Windows.Forms.OpenFileDialog
            CustomLocation.Filter = "ConvertData|*.exe"
            CustomLocation.FilterIndex = 1
            CustomLocation.Title = "Select a custom ConvertData executable..."
            CustomLocation.InitialDirectory = "Desktop"
            If CustomLocation.ShowDialog() = Windows.Forms.DialogResult.OK Then
                CustomOn(sender, e, CustomLocation.FileName)
                btnCustom.Content = "Clear EXE"
            End If
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnUpdate.Click
        If CheckForGames() = False And HasCustomEXE = False Then
            MsgBox("Sins is not installed on this computer, so you must choose a custom ConvertData.exe and try again!", , "Error")
            Return
        End If
        If Not lstQueue.SelectedItems.Count > 0 Then
            If Not MsgBox("No items selected; settings will be applied to the entire queue.  Continue?", MsgBoxStyle.YesNo, "No selection!") = Windows.Forms.DialogResult.Yes Then
                Return
            Else
                For Each Job As Object In lstQueue.Items
                    Dim Index As Integer = lstQueue.Items.IndexOf(Job)
                    If btnToTXT.IsChecked Then
                        Direction.Item(Index) = "txt"
                    ElseIf btnToBIN.IsChecked Then
                        Direction.Item(Index) = ""
                    End If
                    If btnSins.IsChecked Then
                        Version.Item(Index) = SinsDir + "ConvertData_OriginalSins.exe"
                    ElseIf btnEntrenchment.IsChecked Then
                        Version.Item(Index) = SinsDir + "ConvertData_Entrenchment.exe"
                    ElseIf btnDiplomacy.IsChecked Then
                        Version.Item(Index) = SinsDir + "ConvertData_Diplomacy.exe"
                    ElseIf btnRebellion.IsChecked Then
                        Version.Item(Index) = SinsDir + "ConvertData_Rebellion.exe"
                    ElseIf HasCustomEXE Then
                        Version.Item(Index) = CustomEXEPath
                    End If
                    If Not txtOutput.Text = Nothing _
                    And Not txtOutput.Text = "" _
                    And Not btnInPlace.IsChecked Then
                        Output.Item(Index) = txtOutput.Text
                    ElseIf btnInPlace.IsChecked Then
                        Output.Item(Index) = Job.Remove(Job.LastIndexOf("\"))
                    End If
                    If btnValidate.IsChecked Then
                        Validate.Item(Index) = True
                    Else
                        Validate.Item(Index) = False
                    End If
                    ConfirmedSettings = ConfirmedSettings + 1
                Next
                MsgBox("Settings applied to all items!")
            End If
        Else
            For Each Job As Object In lstQueue.SelectedItems
                Dim Index As Integer = lstQueue.Items.IndexOf(Job)
                If btnToTXT.IsChecked Then
                    Direction.Item(Index) = "txt"
                ElseIf btnToBIN.IsChecked Then
                    Direction.Item(Index) = ""
                End If
                If btnSins.IsChecked Then
                    Version.Item(Index) = SinsDir + "ConvertData_OriginalSins.exe"
                ElseIf btnEntrenchment.IsChecked Then
                    Version.Item(Index) = SinsDir + "ConvertData_Entrenchment.exe"
                ElseIf btnDiplomacy.IsChecked Then
                    Version.Item(Index) = SinsDir + "ConvertData_Diplomacy.exe"
                ElseIf btnRebellion.IsChecked Then
                    Version.Item(Index) = SinsDir + "ConvertData_Rebellion.exe"
                ElseIf HasCustomEXE Then
                    Version.Item(Index) = CustomEXEPath
                End If
                If Not txtOutput.Text = Nothing _
                And Not txtOutput.Text = "" _
                And Not btnInPlace.IsChecked Then
                    Output.Item(Index) = txtOutput.Text
                ElseIf btnInPlace.IsChecked Then
                    Output.Item(Index) = Job.Remove(Job.LastIndexOf("\"))
                End If
                If btnValidate.IsChecked Then
                    Validate.Item(Index) = True
                Else
                    Validate.Item(Index) = False
                End If
                ConfirmedSettings = ConfirmedSettings + 1
            Next
            MsgBox("Settings applied to " + lstQueue.SelectedItems.Count.ToString() + " item(s)!")
        End If
        ResetSettings(sender, e)
    End Sub

    Private Sub ResetSettings(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CustomOff(sender, e)
        btnToTXT.IsChecked = False
        btnToBIN.IsChecked = False
        btnSins.IsChecked = False
        btnEntrenchment.IsChecked = False
        btnDiplomacy.IsChecked = False
        btnRebellion.IsChecked = False
        txtOutput.Text = Nothing
        btnValidate.IsChecked = False
        btnInPlace.IsChecked = False
    End Sub

    Private Sub btnRelocateMods_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRelocateMods.Click
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        folders.Description = "Select a new Sins Mods folder"
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewPathToMods = folders.SelectedPath
            Dim NewestGame As String = "Rebellion "
            Dim NewestVersion As String
            Try
                NewestVersion = RegRebel.GetValue("Version")
            Catch NoRebellion As NullReferenceException
                NewestGame = "Diplomacy "
                Try
                    NewestVersion = RegTrinity.GetValue("Version")
                Catch NoTrinity As NullReferenceException
                    Try
                        NewestVersion = RegDiplo.GetValue("Version")
                    Catch NoDiplo As NullReferenceException
                        NewestGame = "Entrenchment "
                        Try
                            NewestVersion = RegEntrench.GetValue("Version")
                        Catch NoEntrench As NullReferenceException
                            NewestGame = ""
                            Try
                                NewestVersion = RegSins.GetValue("Version")
                            Catch NoSins As NullReferenceException
                                MsgBox("Sins is not properly installed on this computer!", , "Error")
                                Return
                            End Try
                        End Try
                    End Try
                End Try
            End Try
            If NewestGame = "Rebellion " Then
                PathToMods = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\My Games\Ironclad Games\Sins of a Solar Empire Rebellion"
            Else
                If Environment.OSVersion.Version.Major > 5 Then
                    PathToMods = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData.Replace("Roaming\Xtreme Studios\Sins Data Converter\2.0", "Local\Ironclad Games\Sins of a Solar Empire")
                Else
                    PathToMods = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData.Replace("Application Data\Xtreme Studios\Sins Data Converter\2.0", "Local Settings\Application Data\Ironclad Games\Sins of a Solar Empire")
                End If
            End If
            NewestVersion = NewestVersion.Remove(NewestVersion.LastIndexOf("."), NewestVersion.Length)
            ModFolder = "\Mods-" + NewestGame + NewestVersion
            Dim writer As System.IO.StreamWriter
            My.Computer.FileSystem.CreateDirectory(AppData)
            writer = My.Computer.FileSystem.OpenTextFileWriter(AppData + "\movemods.bat", True)
            writer.WriteLine("@echo off")
            writer.WriteLine("mkdir /D """ + PathToMods + ModFolder + """ """ + NewPathToMods + ModFolder + """")
            writer.WriteLine("del """ + AppData + "\movemods.bat""")
            writer.Close()
            prgsConversion.IsIndeterminate = True
            AdvancedWindow.IsEnabled = False
            Dim RelocationThread As New Thread(AddressOf RelocateMods)
            RelocationThread.IsBackground = True
            RelocationThread.Start()
        End If
    End Sub

    Private Sub RelocateMods()
        My.Computer.FileSystem.CreateDirectory(NewPathToMods + ModFolder)
        My.Computer.FileSystem.CopyDirectory(PathToMods + ModFolder, NewPathToMods + ModFolder)
        My.Computer.FileSystem.DeleteDirectory(PathToMods + ModFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Shell(AppData + "\movemods.bat", AppWinStyle.Hide, True)
        MsgBox("Mods relocated!", , "Success")
        Dispatcher.Invoke(New cleanupthreaddataDelegate(AddressOf EndRelocation))
    End Sub

    Private Sub EndRelocation()
        prgsConversion.IsIndeterminate = False
        AdvancedWindow.IsEnabled = True
    End Sub

    Private Sub Conversion()
        Dim writer As System.IO.StreamWriter
        Dim AppData As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
        My.Computer.FileSystem.CreateDirectory(AppData)
        writer = My.Computer.FileSystem.OpenTextFileWriter(AppData + "\sinsconversion.bat", True)
        writer.WriteLine("@echo off")
        writer.WriteLine("cd /D ""C:\""")
        Dim Index As Integer
        For Each Job As String In lstQueue.Items
            Index = lstQueue.Items.IndexOf(Job)
            writer.WriteLine("""" + Version.Item(Index) + """ " + FileType.Item(Index) + " """ + Job + """ """ + Output.Item(Index) + Job.Remove(0, Job.LastIndexOf("\")) + """ " + Direction.Item(Index))
        Next
        'writer.WriteLine("del """ + AppData + "\sinsconversion.bat""")
        writer.Close()
        Shell(AppData + "\sinsconversion.bat", AppWinStyle.NormalFocus, True)
        MsgBox("Conversion complete!", , "Success")
        Dispatcher.Invoke(New cleanupthreaddataDelegate(AddressOf EndConversion))
    End Sub

    Private Sub EndConversion()
        prgsConversion.IsIndeterminate = False
        AdvancedWindow.IsEnabled = True
        If ClearQueueWhenDone = True Then
            RemoveItems(lstQueue.Items)
            ClearQueueWhenDone = False
        End If
    End Sub

    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnConvert.Click
        If Not lstQueue.Items.Count = ConfirmedSettings Then
            MsgBox(CStr(lstQueue.Items.Count - ConfirmedSettings) + " item(s) have no settings applied to them.  Conversion aborted.", , "Error")
            Return
        End If
        AdvancedWindow.IsEnabled = False
        prgsConversion.IsIndeterminate = True
        Dim ConversionThread As New Thread(AddressOf Conversion)
        ConversionThread.IsBackground = True
        ConversionThread.Start()
    End Sub

    Private Sub btnReferenceData_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnReferenceData.Click
        RDMenu.IsOpen = True
    End Sub

    Private Sub QueueReferenceData(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs, ByVal GameFolder As String, ByVal GameTitle As String, ByVal ConvertData As String)
        Dim RDPath As String
        If MsgBox("This will clear any items currently in the queue.  Continue?", MsgBoxStyle.YesNo, "Warning") = Windows.Forms.DialogResult.Yes Then
            RemoveItems(lstQueue.Items)
        Else
            Return
        End If
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        folders.Description = "Save ReferenceData in..."
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RDPath = CreateRDFolders(folders.SelectedPath, GameTitle)
            If RDPath = "failed" Then
                Return
            End If
            Dim fileindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + GameFolder + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\GameInfo")
                Validate.Add(False)
                FileType.Add(" entity ")
                ConfirmedSettings += 1
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + GameFolder + "\Mesh", FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Mesh")
                Validate.Add(False)
                FileType.Add(" mesh ")
                ConfirmedSettings += 1
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + GameFolder + "\Particle", FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Particle")
                Validate.Add(False)
                FileType.Add(" particle ")
                ConfirmedSettings += 1
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + GameFolder + "\Window", FileIO.SearchOption.SearchTopLevelOnly, "*.mesh", "*.particle", "*.brushes", "*.entity")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Window")
                Validate.Add(False)
                FileType.Add(" brushes ")
                ConfirmedSettings += 1
            Next
            ClearQueueWhenDone = True
            btnConvert_Click(sender, e)
        End If
    End Sub

    Private Sub QueueReferenceData(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim GameTitle As String = "Rebellion"
        Dim ConvertData As String = "ConvertData_Rebellion.exe"
        Dim RDPath As String
        If MsgBox("This will clear any items currently in the queue.  Continue?", MsgBoxStyle.YesNo, "Warning") = Windows.Forms.DialogResult.Yes Then
            lstQueue.Items.Clear()
        Else
            Return
        End If
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        folders.Description = "Save ReferenceData in..."
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RDPath = CreateRDFolders(folders.SelectedPath, GameTitle)
            If RDPath = "failed" Then
                Return
            End If
            Dim fileindex As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\GameInfo")
                Validate.Add(False)
                FileType.Add(" entity ")
                ConfirmedSettings += 1
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + "\Mesh", FileIO.SearchOption.SearchTopLevelOnly, "*.mesh")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Mesh")
                Validate.Add(False)
                FileType.Add(" mesh ")
                ConfirmedSettings += 1
                Dim Temp As Integer = lstQueue.Items.IndexOf(Job)
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + "\Particle", FileIO.SearchOption.SearchTopLevelOnly, "*.particle")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Particle")
                Validate.Add(False)
                FileType.Add(" particle ")
                ConfirmedSettings += 1
            Next
            fileindex = My.Computer.FileSystem.GetFiles(SinsDir + "\Window", FileIO.SearchOption.SearchTopLevelOnly, "*.brushes")
            For Each Job In fileindex
                lstQueue.Items.Add(Job)
                Direction.Add("txt")
                Version.Add(SinsDir + ConvertData)
                Output.Add(RDPath + "\Window")
                Validate.Add(False)
                FileType.Add(" brushes ")
                ConfirmedSettings += 1
            Next
            ClearQueueWhenDone = True
            btnConvert_Click(sender, e)
        End If
    End Sub

    Private Function CreateRDFolders(ByVal Root As String, ByVal GameTitle As String) As String
        Dim RDPath As String = Root + "\" + GameTitle + " Reference Data"
        Try
            My.Computer.FileSystem.CreateDirectory(RDPath)
        Catch ex As Exception
            MsgBox("Failed to create ReferenceData directory!", MsgBoxStyle.OkOnly, "Error")
            Return "failed"
        End Try
        Try
            My.Computer.FileSystem.CreateDirectory(RDPath + "\GameInfo")
            My.Computer.FileSystem.CreateDirectory(RDPath + "\Mesh")
            My.Computer.FileSystem.CreateDirectory(RDPath + "\Particle")
            My.Computer.FileSystem.CreateDirectory(RDPath + "\Window")
        Catch ex As Exception
            My.Computer.FileSystem.DeleteDirectory(RDPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            MsgBox("Failed to create ReferenceData directory!", MsgBoxStyle.OkOnly, "Error")
            Return "failed"
        End Try
        Return RDPath
    End Function

    Private Sub btnRDSins_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRDSins.Click
        QueueReferenceData(sender, e, "", "Original Sins", "ConvertData_OriginalSins.exe")
    End Sub

    Private Sub btnRDEntrench_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRDEntrench.Click
        QueueReferenceData(sender, e, "\Entrenchment", "Entrenchment", "ConvertData_Entrenchment.exe")
    End Sub

    Private Sub btnRDDiplo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRDDiplo.Click
        QueueReferenceData(sender, e, "\Diplomacy", "Diplomacy", "ConvertData_Diplomacy.exe")
    End Sub

    Private Sub btnRDRebel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRDRebel.Click
        QueueReferenceData(sender, e)
    End Sub

    Private Sub btnManifest_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnManifest.Click
        EMMenu.IsOpen = True
    End Sub

    Private Sub BuildEntityManifest(ByVal Game As String)
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        folders.Description = "Select your root mod folder (directory above GameInfo)"
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim writer As System.IO.StreamWriter
            writer = My.Computer.FileSystem.OpenTextFileWriter(folders.SelectedPath + "\entity.manifest", True)
            writer.WriteLine("TXT")
            Dim sinsentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In My.Computer.FileSystem.GetFiles(SinsDir + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                sinsentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
            Next
            Dim entrenchentityindex As New System.Collections.ObjectModel.Collection(Of String)
            If Game = "Entrenchment" Then
                For Each Entity In My.Computer.FileSystem.GetFiles(SinsDir + "\Entrenchment\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                    If Not sinsentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) Then
                        entrenchentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
                    End If
                Next
            End If
            Dim diploentityindex As New System.Collections.ObjectModel.Collection(Of String)
            If Game = "Diplomacy" Then
                For Each Entity In My.Computer.FileSystem.GetFiles(SinsDir + "\Entrenchment\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                    If Not sinsentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) Then
                        entrenchentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
                    End If
                Next
                For Each Entity In My.Computer.FileSystem.GetFiles(SinsDir + "\Diplomacy\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                    If Not sinsentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) _
                    And Not entrenchentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) Then
                        diploentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
                    End If
                Next
            End If
            Dim newentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In My.Computer.FileSystem.GetFiles(folders.SelectedPath + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                If Not sinsentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) _
                And Not entrenchentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) _
                And Not diploentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) Then
                    newentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
                End If
            Next
            Dim finalentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In sinsentityindex
                finalentityindex.Add(Entity)
            Next
            For Each Entity In entrenchentityindex
                finalentityindex.Add(Entity)
            Next
            For Each Entity In diploentityindex
                finalentityindex.Add(Entity)
            Next
            For Each Entity In newentityindex
                finalentityindex.Add(Entity)
            Next
            writer.WriteLine("entityNameCount " + CStr(finalentityindex.Count))
            For Each Entity In finalentityindex
                writer.WriteLine("entityName """ + Entity + """")
            Next
            writer.Close()
            MsgBox("Manifest created successfully!", , "Success")
        End If
    End Sub

    Private Sub BuildEntityManifest(ByVal Game As String, ByVal Rebellion As String)
        Dim folders As New System.Windows.Forms.FolderBrowserDialog
        folders.Description = "Select your root mod folder (directory above GameInfo)"
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim writer As System.IO.StreamWriter
            writer = My.Computer.FileSystem.OpenTextFileWriter(folders.SelectedPath + "\entity.manifest", True)
            writer.WriteLine("TXT")
            Dim rebelentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In My.Computer.FileSystem.GetFiles(SinsDir + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                rebelentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
            Next
            Dim newentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In My.Computer.FileSystem.GetFiles(folders.SelectedPath + "\GameInfo", FileIO.SearchOption.SearchTopLevelOnly, "*.entity")
                If Not rebelentityindex.Contains(Entity.Remove(0, Entity.LastIndexOf("\") + 1)) Then
                    newentityindex.Add(Entity.Remove(0, Entity.LastIndexOf("\") + 1))
                End If
            Next
            Dim finalentityindex As New System.Collections.ObjectModel.Collection(Of String)
            For Each Entity In newentityindex
                finalentityindex.Add(Entity)
            Next
            writer.WriteLine("entityNameCount " + CStr(finalentityindex.Count))
            For Each Entity In finalentityindex
                writer.WriteLine("entityName """ + Entity + """")
            Next
            writer.Close()
            MsgBox("Manifest created successfully!", , "Success")
        End If
    End Sub

    Private Sub btnEMEntrench_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnEMEntrench.Click
        BuildEntityManifest("Entrenchment")
    End Sub

    Private Sub btnEMDiplo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnEMDiplo.Click
        BuildEntityManifest("Diplomacy")
    End Sub

    Private Sub btnEMRebel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnEMRebel.Click
        BuildEntityManifest("Rebellion", "Rebellion")
    End Sub
End Class
