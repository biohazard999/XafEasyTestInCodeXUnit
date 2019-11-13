using System;
using System.Linq;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class ActionPageObject : ActionPageObject<ActionPageObject>
    {
        public ActionPageObject(EasyTestFixtureBase fixture, string actionName) : base(fixture, actionName) { }
    }
    
    public class ActionPageObject<T> : PageObject<T>
        where T : ActionPageObject<T>
    {
        protected string ActionName { get; }
        protected ITestControl TestControl { get; }

        public ActionPageObject(EasyTestFixtureBase fixture, string actionName) : base(fixture)
        {
            ActionName = actionName;
            TestControl = Fixture.Adapter.CreateTestControl(TestControlType.Action, actionName);
        }

        public bool Enabled => TestControl.GetInterface<IControlEnabled>().Enabled;

        public T Execute()
        {
            Fixture.CommandAdapter.DoAction(ActionName, null);
            return This;
        }
    }
}
