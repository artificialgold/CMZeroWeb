using System.Configuration;

namespace CMZero.Web.Services
{
    public class SystemSettings : ISystemSettings
    {
        public string ApiKey
        {
            get
            {
                return GetStringValueFromAppSetting("ApiKey");
            }
        }

        private static string GetStringValueFromAppSetting(string appSettingName)
        {
            var appSetting = ConfigurationManager.AppSettings[appSettingName];

            if (appSetting == null)
            {
                throw new ConfigurationErrorsException(string.Format("App setting {0} was not found", appSettingName));
            }

            return appSetting;
        }
    }
}
