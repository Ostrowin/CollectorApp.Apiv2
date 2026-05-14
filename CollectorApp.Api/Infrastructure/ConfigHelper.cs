using System;
using System.Configuration;

namespace CollectorApp.Api.Infrastructure
{
    public static class ConfigHelper
    {
        public static string GetSetting(string key, string envName)
        {
            var envValue = Environment.GetEnvironmentVariable(envName, EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(envValue))
                return envValue;

            return ConfigurationManager.AppSettings[key];
        }
    }
}