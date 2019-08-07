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

        protected T This => (T)this;

        public T Assert(Action<T> assert)
        {
            assert(This);
            return This;
        }

        public virtual ActionPageObject Action(string actionName) => new ActionPageObject(Fixture, actionName);
    }

    public class NestedListPageObject : NestedListPageObject<NestedListPageObject>
    {
        public NestedListPageObject(EasyTestFixtureBase fixture, string listName) : base(fixture, listName) { }

    }

    public abstract class NestedListPageObject<T> : ListPageObject<T>
        where T : NestedListPageObject<T>
    {

        public NestedListPageObject(EasyTestFixtureBase fixture, string listName) : base(fixture, listName) { }
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

    public class ListPageObject : ListPageObject<ListPageObject>
    {
        public ListPageObject(EasyTestFixtureBase fixture, string tableName) : base(fixture, tableName) { }
    }

    public class ListPageObject<T> : PageObject<T>
        where T : ListPageObject<T>
    {
        protected string TableName { get; }
        protected ITestControl TestControl { get; }

        public ListPageObject(EasyTestFixtureBase fixture, string tableName) : base(fixture)
        {
            TableName = tableName;
            TestControl = Fixture.Adapter.CreateTestControl(TestControlType.Table, tableName);
        }

        public TDetailPageObject OpenRecord<TDetailPageObject>(string columnName, string value, Func<EasyTestFixtureBase, TDetailPageObject> pageObjectFactory)
        {
            Fixture.CommandAdapter.ProcessRecord(TableName, new string[] { columnName }, new string[] { value }, "");

            return pageObjectFactory(Fixture);
        }

        public DetailPageObject OpenRecord(string columnName, string value)
            => OpenRecord(columnName, value, f => new DetailPageObject(f));

        public int RowCount => TestControl.GetInterface<IGridBase>().GetRowCount();

        public Dictionary<string, string> GetValues(int rowIndex, params string[] columnNames)
            => columnNames.Select(columnName => new
            {
                ColumnName = columnName,
                Value = Fixture.CommandAdapter.GetCellValue(TableName, rowIndex, columnName)

            }).ToDictionary(x => x.ColumnName, x => x.Value);

        public ActionPageObject NestedAction(string actionName) => base.Action($"{TableName}.{actionName}");

        public ActionPageObject UnlinkAction => NestedAction("Unlink");
        public ActionPageObject LinkAction => NestedAction("Link");

        public T SelectRow(int rowIndex)
        {
            TestControl.GetInterface<IGridRowsSelection>().SelectRow(rowIndex);
            return This;
        }

        public T ExecuteAction(Func<T, ActionPageObject> action)
        {
            action(This).Execute();
            return This;
        }

        public T ExecuteAction<TPageObject>(Func<T, ActionPageObject> action, Func<EasyTestFixtureBase, TPageObject> pageObjectFactory, Action<TPageObject> executor)
        {
            action(This).Execute();
            executor(pageObjectFactory(Fixture));
            return This;
        }

    }

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

    public class ContactListPageObject : ContactListPageObject<ContactListPageObject>
    {
        public ContactListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class ContactListPageObject<T> : ListPageObject<T>
        where T : ContactListPageObject<T>
    {
        public ContactListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Contact") { }

        public ContactDetailPageObject OpenRecordByFullName(string title)
            => OpenRecord("Full Name", title, f => new ContactDetailPageObject(f));
    }

    public class DetailPageObject : DetailPageObject<DetailPageObject>
    {
        public DetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class DetailPageObject<T> : PageObject<T>
        where T : DetailPageObject<T>
    {
        public DetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public ListPageObject List(string tableName) => new ListPageObject(Fixture, tableName);
    }


    public class ContactDetailPageObject : DepartmentDetailPageObject<ContactDetailPageObject>
    {
        public ContactDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class ContactDetailPageObject<T> : DetailPageObject<T>
        where T : ContactDetailPageObject<T>
    {
        public ContactDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        
    }
    
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

    public class PositionListPageObject : PositionListPageObject<PositionListPageObject>
    {
        public PositionListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class PositionListPageObject<T> : NestedListPageObject<T>
        where T : PositionListPageObject<T>
    {
        public PositionListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Positions") { }
    }

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
