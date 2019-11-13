using System.Collections.Generic;
using EasyTest.Tests.PageObjects;
using Shouldly;
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
            var departmentDetail = new ApplicationPageObject(Fixture)
                .NavigateToDepartment()
                .OpenRecordByTitle("Development Department");

            departmentDetail
                .Positions()
                .Assert(p =>
                {
                    p.RowCount.ShouldBe(2);
                    p.GetValues(0, "Title").ShouldBe(new Dictionary<string, string>
                    {
                        ["Title"] = "Developer"
                    });
                    p.UnlinkAction.Enabled.ShouldBeFalse();
                })
                .SelectRow(0)
                .Assert(p => p.UnlinkAction.Enabled.ShouldBeTrue())
                .ExecuteAction(p => p.UnlinkAction)
                .Assert(p =>
                {
                    p.RowCount.ShouldBe(1);
                    p.GetValues(0, "Title").ShouldBe(new Dictionary<string, string>
                    {
                        ["Title"] = "Manager"
                    });
                });

            departmentDetail
                .Contacts()
                .Assert(c => c.UnlinkAction.Enabled.ShouldBeFalse())
                .SelectRow(0)
                .Assert(c => c.UnlinkAction.Enabled.ShouldBeTrue());
        }
    }
}
