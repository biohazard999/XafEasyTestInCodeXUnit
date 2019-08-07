using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class PositionListPageObject : PositionListPageObject<PositionListPageObject>
    {
        public PositionListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class PositionListPageObject<T> : NestedListPageObject<T>
        where T : PositionListPageObject<T>
    {
        public PositionListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Positions") { }
    }

}
