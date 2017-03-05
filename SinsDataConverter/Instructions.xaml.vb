Public Class Instructions

    Private Sub Instructions_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        txtHelp.Text =
            "The Sins Data Conversion Utility is an easy way to convert Sins of a Solar Empire data files back and forth between TXT and BIN format.  It does this by " +
            "using the command line converters included in Sins (which means Sins has to be installed).  This is not an ""in-place"" conversion utility, and you can " +
            "select a custom output destination for the converted files.  You can select either a single file, or an entire folder, or even a folder and all of its " +
            "subfolders, as the conversion source.  You can then select any folder you like as the output folder for the converted file(s).  Then you can pick whether " +
            "you are converting from TXT to BIN, or BIN to TXT.  Finally, you specify whether the files are from/for original Sins, Entrenchment, or Diplomacy.  Then you " +
            "simply click the big ""CONVERT"" button.  Depending on the size of the operation you have asked the program to perform, it may take some time to complete, " +
            "during which the program will appear to stop responding.  Despite this, it will be working in the background, and it will notify you when it has finished.  " +
            "If you used this program before the release of v1.2, there are some things you should know.  First, you'll have noticed that the interface is slightly different " +
            "to make room for some new features.  There is now a button (checkbox actually) that automates in-place conversions, which just means you don't have to set the " +
            "output folder separately in those cases.  You can now drag-and-drop files and folders to convert, but you have to drag them onto the relevant button.  For example, " +
            "if you have a file you want to convert, drag it onto the ""File"" button next to the Source field, not onto the Source field itself.  Finally, there is now an " +
            "exclusion list for conversions that include subfolders.  You can select multiple folders to exclude by using the CTRL key while clicking items in the list."
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBack.Click
        Me.Hide()
    End Sub
End Class
