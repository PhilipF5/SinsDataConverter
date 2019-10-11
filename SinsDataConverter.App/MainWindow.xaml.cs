using Microsoft.Win32;
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
using SinsDataConverter.Core;

namespace SinsDataConverter.App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ConversionSettings _currentSettings = new ConversionSettings();
		private bool _enableLogging;
		private string _scriptsLocation;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void _reset()
		{
			ConversionEngine.StartNew(_scriptsLocation, _enableLogging);
			SourceTextBox.Text = "";
			InPlaceCheckBox.IsChecked = false;
			OutputTextBox.Text = "";
			ToBinRadioButton.IsChecked = false;
			ToTxtRadioButton.IsChecked = false;
			OriginalSinsRadioButton.IsChecked = false;
			EntrenchmentRadioButton.IsChecked = false;
			DiplomacyRadioButton.IsChecked = false;
			RebellionRadioButton.IsChecked = false;
			_currentSettings = new ConversionSettings();
		}

		private void _setInPlace()
		{
			switch (_currentSettings.InputType)
			{
				case ConversionSettings.ConversionInputType.File:
					OutputTextBox.Text = new FileInfo(SourceTextBox.Text).DirectoryName;
					break;
				case ConversionSettings.ConversionInputType.Directory:
					OutputTextBox.Text = new DirectoryInfo(SourceTextBox.Text).FullName;
					break;
				default:
					throw new InvalidOperationException("Input type of conversion is not recognized");
			}
		}

		private void _showError(string message)
		{
			System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void FileButton_Click(object sender, RoutedEventArgs e)
		{
			var filesDialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = "Desktop",
				Filter = "Sins Data Files|*.brushes;*.entity;*.mesh;*.particle|Brushes|*.brushes|Entity|*.entity|Mesh|*.mesh|Particle|*.particle",
				FilterIndex = 1,
				Title = "Select a file to convert..."
			};
			if (filesDialog.ShowDialog() == true)
			{
				SourceTextBox.Text = filesDialog.FileName;
				_currentSettings.InputType = ConversionSettings.ConversionInputType.File;
				if (InPlaceCheckBox.IsChecked == true)
				{
					_setInPlace();
				}
			}
		}

		private void FolderButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceTextBox.Text = folderDialog.SelectedPath;
				_currentSettings.InputType = ConversionSettings.ConversionInputType.Directory;
				if (InPlaceCheckBox.IsChecked == true)
				{
					_setInPlace();
				}
			}
		}

		private void InPlaceCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(SourceTextBox.Text))
			{
				_setInPlace();
			}
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
			_currentSettings.Version = GameEdition.OriginalSins;
		}

		private void EntrenchmentRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameEdition.Entrenchment;
		}

		private void DiplomacyRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameEdition.Diplomacy;
		}

		private void RebellionRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.Version = GameEdition.Rebellion;
		}

		private void ToTxtRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.OutputType = ConversionSettings.ConversionOutputType.Txt;
		}

		private void ToBinRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			_currentSettings.OutputType = ConversionSettings.ConversionOutputType.Bin;
		}

		private async void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
			if (!_currentSettings.IsValid())
			{
				_showError("All settings are required");
				return;
			}

			try
			{
				ConversionEngine.AddJobs(ConversionJob.Create(SourceTextBox.Text, OutputTextBox.Text, _currentSettings));
				ConversionEngine.CreateScriptFile();
				ProgressBar.IsIndeterminate = true;
				var startTime = DateTime.Now;
				await ConversionEngine.Run();
				var endTime = DateTime.Now;
				ProgressBar.IsIndeterminate = false;
				System.Windows.MessageBox.Show($"Conversion job started at {startTime.ToString()}{Environment.NewLine}Finished at {endTime.ToString()}", "Conversion finished", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				_showError(ex.Message + Environment.NewLine + ex.ParamName);
			}
			catch (DirectoryNotFoundException ex)
			{
				_showError(ex.Message);
			}
			catch (FileNotFoundException ex)
			{
				_showError(ex.Message + Environment.NewLine + ex.FileName);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_enableLogging = AppConfig.Default.EnableLogging;
			_scriptsLocation = String.IsNullOrEmpty(AppConfig.Default.ScriptsLocation) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : AppConfig.Default.ScriptsLocation;

			ExeManager.ScanForInstalls();
			ConversionEngine.StartNew(_scriptsLocation, _enableLogging);
			OriginalSinsRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.OriginalSins);
			EntrenchmentRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Entrenchment);
			DiplomacyRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Diplomacy);
			RebellionRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Rebellion);
		}

		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			_reset();
		}
	}
}
