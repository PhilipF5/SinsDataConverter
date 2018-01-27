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
		private ConversionSettings _currentSettings = new ConversionSettings();

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
				_currentSettings.InputType = ConversionSettings.ConversionInputType.File;
			}
		}

		private void FolderButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceTextBox.Text = folderDialog.SelectedPath;
				_currentSettings.InputType = ConversionSettings.ConversionInputType.Directory;
			}
		}

		private void InPlaceCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			OutputTextBox.Text = (_currentSettings.InputType == ConversionSettings.ConversionInputType.Directory ? new DirectoryInfo(SourceTextBox.Text).FullName : new FileInfo(SourceTextBox.Text).DirectoryName);
		}

		private void OutputButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				OutputTextBox.Text = folderDialog.SelectedPath;
			}
		}

		private void OriginalSinsRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameVersion.OriginalSins;
		}

		private void EntrenchmentRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameVersion.Entrenchment;
		}

		private void DiplomacyRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameVersion.Diplomacy;
		}

		private void RebellionRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameVersion.Rebellion;
		}

		private void ToTxtRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.OutputType = ConversionSettings.ConversionOutputType.Txt;
		}

		private void ToBinRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.OutputType = ConversionSettings.ConversionOutputType.Bin;
		}
	}
}
