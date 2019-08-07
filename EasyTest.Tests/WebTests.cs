using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using Xunit;

namespace EasyTest.Tests
{
    public class WebTests : CommonTests<WebTestApplicationHelper>
    {
        [Fact]
        public void ChangeContactNameTest() => ChangeContactNameTest_();

        [Fact]
        public void WorkingWithTasks() => WorkingWithTasks_();

        [Fact]
        public void ChangeContactNameAgainTest()
            => ChangeContactNameAgainTest_();

        [Fact]
        public void UnlinkActionTest()
        {
            Fixture.CommandAdapter.DoAction("Navigation", "Department");
            Fixture.CommandAdapter.ProcessRecord("Department", new string[] { "Title" }, new string[] { "Development Department" }, "");

            Fixture.CommandAdapter.DoAction("Positions", null);

            var gridControl = Fixture.Adapter.CreateTestControl(TestControlType.Table, "Positions");
            Assert.Equal(2, gridControl.GetInterface<IGridBase>().GetRowCount());

            Assert.Equal("Developer", Fixture.CommandAdapter.GetCellValue("Positions", 0, "Title"));

            var unlink = Fixture.Adapter.CreateTestControl(TestControlType.Action, "Positions.Unlink");
            Assert.False(unlink.GetInterface<IControlEnabled>().Enabled);


            gridControl.GetInterface<IGridRowsSelection>().SelectRow(0);

            Assert.True(unlink.GetInterface<IControlEnabled>().Enabled);
            Fixture.CommandAdapter.DoAction("Positions.Unlink", null);

            Assert.Equal(1, gridControl.GetInterface<IGridBase>().GetRowCount());
            Assert.Equal("Manager", Fixture.CommandAdapter.GetCellValue("Positions", 0, "Title"));

            Fixture.CommandAdapter.DoAction("Contacts", null);
            unlink = Fixture.Adapter.CreateTestControl(TestControlType.Action, "Contacts.Unlink");
            Assert.False(unlink.GetInterface<IControlEnabled>().Enabled);
        }
    }
}
