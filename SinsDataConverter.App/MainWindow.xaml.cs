using Microsoft.Win32;
using SinsDataConverter.Core;
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

namespace SinsDataConverter.App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ConversionEngine Engine;
		private ConversionSettings Settings;
		private bool EnableLogging;
		private string ScriptsPath;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Reset()
		{
			Engine = new ConversionEngine(ScriptsPath, EnableLogging);
			Settings = new ConversionSettings();
			var textFields = new List<TextBox> { OutputTextBox, SourceTextBox };
			var toggles = new List<Primitives.ToggleButton>
			{
				InPlaceCheckBox,
				ToBinRadioButton,
				ToTxtRadioButton,
				OriginalSinsRadioButton,
				EntrenchmentRadioButton,
				DiplomacyRadioButton,
				RebellionRadioButton,
			};

			textFields.ForEach(field => (field.Text = string.Empty));
			toggles.ForEach(toggle => (toggle.IsChecked = false));
		}

		private void SetInPlace()
		{
			switch (Settings.InputType)
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

		private void ShowError(string message)
		{
			System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void FileButton_Click(object sender, RoutedEventArgs e)
		{
			var filters = new Dictionary<string, string>
			{
				{ "Sins Data Files", string.Join(';', FileTypes.All.Select(type => $"*{type.Extension}")) }
			}
				.Concat(FileTypes.All.ToDictionary(type => type.Name, type => type.Pattern))
				.Select(filter => $"{filter.Key}|{filter.Value}");

			var filesDialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = "Desktop",
				Filter = string.Join('|', filters),
				FilterIndex = 1,
				Title = "Select a file to convert...",
			};

			if (filesDialog.ShowDialog())
			{
				SourceTextBox.Text = filesDialog.FileName;
				Settings.InputType = ConversionSettings.ConversionInputType.File;
				if (InPlaceCheckBox.IsChecked == true)
				{
					SetInPlace();
				}
			}
		}

		private void FolderButton_Click(object sender, RoutedEventArgs e)
		{
			var folderDialog = new FolderBrowserDialog();
			if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceTextBox.Text = folderDialog.SelectedPath;
				Settings.InputType = ConversionSettings.ConversionInputType.Directory;
				if (InPlaceCheckBox.IsChecked == true)
				{
					SetInPlace();
				}
			}
		}

		private void InPlaceCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(SourceTextBox.Text))
			{
				SetInPlace();
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
			Settings.Version = GameEdition.OriginalSins;
		}

		private void EntrenchmentRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			Settings.Version = GameEdition.Entrenchment;
		}

		private void DiplomacyRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			Settings.Version = GameEdition.Diplomacy;
		}

		private void RebellionRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			Settings.Version = GameEdition.Rebellion;
		}

		private void ToTxtRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			Settings.OutputType = ConversionSettings.ConversionOutputType.Txt;
		}

		private void ToBinRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			Settings.OutputType = ConversionSettings.ConversionOutputType.Bin;
		}

		private async void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
			if (!Settings.IsValid)
			{
				ShowError("All settings are required");
				return;
			}

			try
			{
				Engine.AddJobs(ConversionJob.Create(SourceTextBox.Text, OutputTextBox.Text, Settings));
				Engine.CreateScriptFile();
				ProgressBar.IsIndeterminate = true;
				var startTime = DateTime.Now.ToString();
				await Engine.Run();
				var endTime = DateTime.Now.ToString();
				ProgressBar.IsIndeterminate = false;
				System.Windows.MessageBox.Show(
					$"Conversion job started at {startTime}{Environment.NewLine}Finished at {endTime}",
					"Conversion finished",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				ShowError(ex.Message + Environment.NewLine + ex.ParamName);
			}
			catch (DirectoryNotFoundException ex)
			{
				ShowError(ex.Message);
			}
			catch (FileNotFoundException ex)
			{
				ShowError(ex.Message + Environment.NewLine + ex.FileName);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			EnableLogging = AppConfig.Default.EnableLogging;
			ScriptsPath = String.IsNullOrEmpty(AppConfig.Default.ScriptsLocation) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : AppConfig.Default.ScriptsLocation;
			Reset();

			ExeManager.ScanForInstalls();
			OriginalSinsRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.OriginalSins);
			EntrenchmentRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Entrenchment);
			DiplomacyRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Diplomacy);
			RebellionRadioButton.IsEnabled = ExeManager.HasExeForEdition(GameEdition.Rebellion);
		}

		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			Reset();
		}
	}
}
