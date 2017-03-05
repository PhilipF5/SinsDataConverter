Public Class ReferenceData

    Dim folders As New System.Windows.Forms.FolderBrowserDialog
    Dim RegPath As Microsoft.Win32.RegistryKey
    Dim SinsDir As String

    Private Sub btnSins_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSins.Click
        folders.Description = "Select a location to save ReferenceData"
        If folders.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim RDOutput As String = folders.SelectedPath
            If IntPtr.Size = 8 Then
                RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sins", False)
                Try
                    SinsDir = RegPath.GetValue("Path")
                Catch ex As NullReferenceException
                    Try
                        RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Stardock\Drengin.net\sinstrinity", False)
                        SinsDir = RegPath.GetValue("Path")
                    Catch ex2 As NullReferenceException
                        MsgBox("Sins is not properly installed on this computer!", , "Error")
                        Return
                    End Try
                End Try
            Else
                RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sins", False)
                Try
                    SinsDir = RegPath.GetValue("Path")
                Catch ex As NullReferenceException
                    Try
                        RegPath = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Stardock\Drengin.net\sinstrinity", False)
                        SinsDir = RegPath.GetValue("Path")
                    Catch ex2 As NullReferenceException
                        MsgBox("Sins is not properly installed on this computer!", , "Error")
                        Return
                    End Try
                End Try
            End If
        End If
    End Sub
End Class
