using System.Text;
using ICD.Common.Utils;
using ICD.Common.Utils.IO;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public static class ButtonActionMapUtils
	{

		public static string GetXmlContentForSettings(IButtonActionSettings settings)
		{
			using (IcdMemoryStream stream = new IcdMemoryStream())
			{
				using (IcdXmlTextWriter writer = new IcdXmlTextWriter(stream, new UTF8Encoding(false)))
				{
					settings.WriteElements(writer);
				}

				//todo: Is this bad?
				return Encoding.UTF8.GetString(stream.WrappedMemoryStream.ToArray(), 0, (int)stream.WrappedMemoryStream.Length);
			}
		}

		public static IButtonActionSettings GetSettingsForButtonActionMap(IButtonAction action)
		{
			return ReflectionUtils.CreateInstance<IButtonActionSettings>(action.SettingsClass);
		}

		public static IButtonActionSettings GetSettingsCopyForButtonActionMap(IButtonAction action)
		{
			IButtonActionSettings settings = GetSettingsForButtonActionMap(action);
			action.CopySettings(settings);
			return settings;
		}
	}
}