using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SinsDataConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private bool _folderSelected = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void FileButton_Click(object sender, RoutedEventArgs e)
		{
			var filesDialog = new OpenFileDialog()
			{
				InitialDirectory = "Desktop",
				Filter = "Brushes|*.brushes|Entity|*.entity|Mesh|*.mesh|Particle|*.particle",
				FilterIndex = 2,
				Title = "Select a file to convert..."
			};
			if (filesDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceTextBox.Text = filesDialog.FileName;
				_folderSelected = false;
			}
		}

		private void FolderButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceTextBox.Text = folderDialog.SelectedPath;
				_folderSelected = true;
			}
		}

		private void InPlaceCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			OutputTextBox.Text = (_folderSelected ? new DirectoryInfo(SourceTextBox.Text).FullName : new FileInfo(SourceTextBox.Text).DirectoryName);
		}

		private void OutputButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				OutputTextBox.Text = folderDialog.SelectedPath;
			}
		}
	}
}
