using System;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace TestApplication.Module.Web {
    [ToolboxItemFilter("Xaf.Platform.Web")]
    public sealed partial class TestApplicationAspNetModule : ModuleBase {
        public TestApplicationAspNetModule() {
            InitializeComponent();
        }
    }
}
