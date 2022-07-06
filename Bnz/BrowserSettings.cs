namespace Bnz.UI.Tests
{
    public class BrowserSettings
    {
        public string Browser { get; set; }
        public string LogFolderPath { get; set; }

        public override string ToString()
        {
            return $"Browser Settings: Browser: {Browser}";
        }
    }
}