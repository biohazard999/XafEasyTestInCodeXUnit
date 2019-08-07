using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTest.Tests.PageObjects
{
    public abstract class PageObject<T> where T : PageObject<T>
    {

        protected readonly EasyTestFixtureBase Fixture;
        public PageObject(EasyTestFixtureBase fixture)
            => Fixture = fixture;
    }

    public abstract class NestedListPageObject<T> : PageObject<T>
        where T : NestedListPageObject<T>
    {
        protected ITestControl GridControl { get; }
        protected string ListName { get; }

        public NestedListPageObject(EasyTestFixtureBase fixture, string listName) : base(fixture)
        {
            ListName = listName;
            GridControl = Fixture.Adapter.CreateTestControl(TestControlType.Table, listName);
        }


        public int RowCount => GridControl.GetInterface<IGridBase>().GetRowCount();

        public Dictionary<string, string> GetValues(int rowIndex, params string[] columnNames)
            => columnNames.Select(columnName => new
            {
                ColumnName = columnName,
                Value = Fixture.CommandAdapter.GetCellValue(ListName, rowIndex, columnName)

            }).ToDictionary(x => x.ColumnName, x => x.Value);

        public ActionPageObject UnlinkAction => new ActionPageObject(Fixture, $"{ListName}.Unlink");

    }

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
    }

    public class DepartmentListPageObject : PageObject
    {
        public DepartmentListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public DepartmentDetailPageObject OpenRecord(string title)
        {
            Fixture.CommandAdapter.ProcessRecord("Department", new string[] { "Title" }, new string[] { title }, "");

            return new DepartmentDetailPageObject(Fixture);
        }
    }

    public class DepartmentDetailPageObject : PageObject
    {
        public DepartmentDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public PositionListPageObject PositionList()
        {
            Fixture.CommandAdapter.DoAction("Positions", null);
            return new PositionListPageObject(Fixture);
        }
    }

    public class PositionListPageObject : NestedListPageObject
    {
        public PositionListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Positions") { }


    }

    public class ActionPageObject : ActionPageObject<ActionPageObject>
    {
        public ActionPageObject(EasyTestFixtureBase fixture, string actionName) : base(fixture, actionName)
        {

        }
    }
    
    public class ActionPageObject<T> : PageObject<T>
        where T : ActionPageObject<T>
    {
        protected string ActionName { get; }
        protected ITestControl TestControl { get; }

        public ActionPageObject(EasyTestFixtureBase fixture, string actionName) : base(fixture)
        {
            ActionName = actionName;
            TestControl = Fixture.Adapter.CreateTestControl(TestControlType.Action, "Positions.Unlink");
        }

        public bool Enabled => TestControl.GetInterface<IControlEnabled>().Enabled;
    }
}
