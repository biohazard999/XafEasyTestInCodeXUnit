using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTest.Tests.PageObjects
{
    public class ApplicationPageObject : ApplicationPageObject<ApplicationPageObject>
    {
        public ApplicationPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class ApplicationPageObject<T> : PageObject<T>
        where T : ApplicationPageObject<T>
    {
        public ApplicationPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public DepartmentListPageObject NavigateToDepartment()
        {
            Fixture.CommandAdapter.DoAction("Navigation", "Department");
            return new DepartmentListPageObject(Fixture);
        }

        public ContactListPageObject NavigateToContact()
        {
            Fixture.CommandAdapter.DoAction("Navigation", "Contact");
            return new ContactListPageObject(Fixture);
        }

        public ListPageObject NavigateTo(string navigationName)
        {
            Fixture.CommandAdapter.DoAction("Navigation", navigationName);
            return new ListPageObject(Fixture, "Demo Task");
        }
    }
}
