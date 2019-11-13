using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class DepartmentListPageObject : DepartmentListPageObject<DepartmentListPageObject>
    {
        public DepartmentListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }
    
    public class DepartmentListPageObject<T> : ListPageObject<T>
        where T : DepartmentListPageObject<T>
    {
        public DepartmentListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Department") { }

        public DepartmentDetailPageObject OpenRecordByTitle(string title)
            => OpenRecord("Title", title, f => new DepartmentDetailPageObject(f));
    }
}
