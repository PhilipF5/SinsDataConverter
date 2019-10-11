using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace SinsDataConverter.App
{
	public class AppConfig : ApplicationSettingsBase
	{
		public static AppConfig Default { get; } = (AppConfig)Synchronized(new AppConfig());

		protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Save();
			base.OnPropertyChanged(sender, e);
		}

		[UserScopedSetting]
		[DefaultSettingValue("false")]
		public bool EnableLogging
		{
			get => (bool)this[nameof(EnableLogging)];
			set
			{
				this[nameof(EnableLogging)] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue(null)]
		public string ScriptsLocation
		{
			get => (string)this[nameof(ScriptsLocation)];
			set
			{
				this[nameof(ScriptsLocation)] = value;
			}
		}
	}
}
