namespace HamburgerMenu.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    public static class Settings
    {
        private const string user = "user";
        private const string clave = "clave";
        private const string isRemember = "isRemember";
        private static readonly string stringDefault = string.Empty;
        private static readonly bool boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Usuario
        {
            get => AppSettings.GetValueOrDefault(user, stringDefault);
            set => AppSettings.AddOrUpdateValue(user, value);
        }

        public static string Clave
        {
            get => AppSettings.GetValueOrDefault(clave, stringDefault);
            set => AppSettings.AddOrUpdateValue(clave, value);
        }

        public static bool IsRemember
        {
            get => AppSettings.GetValueOrDefault(isRemember, boolDefault);
            set => AppSettings.AddOrUpdateValue(isRemember, value);
        }

    }
}
