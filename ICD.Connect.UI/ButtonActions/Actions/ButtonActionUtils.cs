using System.Text;
using ICD.Common.Utils;
using ICD.Common.Utils.IO;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public static class ButtonActionUtils
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

		public static IButtonActionSettings GetSettingsForButtonAction(IButtonAction action)
		{
			return ReflectionUtils.CreateInstance<IButtonActionSettings>(action.SettingsClass);
		}

		public static IButtonActionSettings GetSettingsCopyForButtonAction(IButtonAction action)
		{
			IButtonActionSettings settings = GetSettingsForButtonAction(action);
			action.CopySettings(settings);
			return settings;
		}
	}
}