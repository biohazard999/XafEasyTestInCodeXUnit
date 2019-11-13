using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
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
    }
}
