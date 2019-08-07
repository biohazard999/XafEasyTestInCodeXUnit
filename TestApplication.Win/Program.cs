using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;

namespace TestApplication.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
            TestApplication.EasyTest.InMemoryDataStoreProvider.Register();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            var winApplication = new TestApplicationWindowsFormsApplication();
#if EASYTEST
            winApplication.ConnectionString = $"XpoProvider={TestApplication.EasyTest.InMemoryDataStoreProvider.XpoProviderTypeString}";
#endif
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try
            {
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}