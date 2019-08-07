using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class DepartmentDetailPageObject : DepartmentDetailPageObject<DepartmentDetailPageObject>
    {
        public DepartmentDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }
    
    public class DepartmentDetailPageObject<T> : DetailPageObject<T>
        where T : DepartmentDetailPageObject<T>
    {
        public DepartmentDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public PositionListPageObject Positions()
        {
            Fixture.CommandAdapter.DoAction("Positions", null);
            return new PositionListPageObject(Fixture);
        }

        public NestedListPageObject Contacts()
        {
            Fixture.CommandAdapter.DoAction("Contacts", null);
            return new NestedListPageObject(Fixture, "Contacts");
        }
    }
}
