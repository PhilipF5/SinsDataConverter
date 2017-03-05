Public Class Themes

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBack.Click
        Me.Hide()
    End Sub

    Private Sub imgTheme2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles imgTheme2.MouseDown
        My.Settings.Theme = 2
    End Sub
End Class
