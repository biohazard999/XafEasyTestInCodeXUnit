using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class DetailPageObject : DetailPageObject<DetailPageObject>
    {
        public DetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }

    public class DetailPageObject<T> : PageObject<T>
        where T : DetailPageObject<T>
    {
        public DetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public ListPageObject List(string tableName) => new ListPageObject(Fixture, tableName);

        public string GetValue(string fieldName) => Fixture.CommandAdapter.GetFieldValue(fieldName);
        public void SetValue(string fieldName, string value) => Fixture.CommandAdapter.SetFieldValue(fieldName, value);

        public ActionPageObject EditAction => Action("Edit");
        public ActionPageObject SaveAction => Action("Save");
        public ActionPageObject SaveAndCloseAction => Action("Save and Close");
        public ActionPageObject CloseAction => Action("Close");
    }
}
