using EasyTest.Tests.Utils;

namespace EasyTest.Tests
{
    public class WinTestApplicationHelper : WinEasyTestFixtureHelperBase
    {
        public WinTestApplicationHelper() : base("TestApplication.Win", "TestApplication.Win.exe") { }
    }

    public class WebTestApplicationHelper : WebEasyTestFixtureHelperBase
    {
        public WebTestApplicationHelper() : base(@"..\..\..\..\TestApplication.Web") { }
    }
}
