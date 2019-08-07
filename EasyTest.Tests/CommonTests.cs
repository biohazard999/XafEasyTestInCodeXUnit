using System;
using System.Collections.Generic;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;
using Xunit;
using EasyTest.Tests.PageObjects;
using Shouldly;

namespace EasyTest.Tests
{
    public abstract class CommonTests<T> : IDisposable where T : EasyTestFixtureBase, new()
    {
        protected T Fixture { get; }

        public CommonTests()
            => Fixture = new T();

        public void Dispose()
            => Fixture.Dispose();

        protected void ChangeContactNameTest_()
        {
            var contactList = new ApplicationPageObject(Fixture)
                .NavigateToContact()
                .Assert(d =>
                {
                    d.RowCount.ShouldBe(2);
                    d.GetValues(0, "Full Name")
                        .ShouldBe(new Dictionary<string, string>()
                        {
                            ["Full Name"] = "John Nilsen"
                        });

                    d.GetValues(1, "Full Name")
                        .ShouldBe(new Dictionary<string, string>()
                        {
                            ["Full Name"] = "Mary Tellitson"
                        });
                });
            
            var contactDetail = contactList
                .OpenRecordByFullName("Mary Tellitson")
                .Assert(c =>
                {
                    c.FullName.ShouldBe("Mary Tellitson");
                    c.Department.ShouldBe("Development Department");
                    c.Position.ShouldBe("Manager");
                })
                .ExecuteActionIf(c => c.EditAction, f => f.IsWeb)
                        
            Fixture.CommandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");

            Assert.Equal("Mary Tellitson", Fixture.CommandAdapter.GetFieldValue("Full Name"));
            Assert.Equal("Development Department", Fixture.CommandAdapter.GetFieldValue("Department"));
            Assert.Equal("Manager", Fixture.CommandAdapter.GetFieldValue("Position"));

            if (Fixture.IsWeb)
            {
                Fixture.CommandAdapter.DoAction("Edit", null);
            }

            Fixture.CommandAdapter.SetFieldValue("First Name", "User_1");
            Fixture.CommandAdapter.SetFieldValue("Last Name", "User_2");

            Fixture.CommandAdapter.SetFieldValue("Position", "Developer");

            Fixture.CommandAdapter.DoAction("Save", null);

            Assert.Equal("User_1 User_2", Fixture.CommandAdapter.GetFieldValue("Full Name"));
            Assert.Equal("Developer", Fixture.CommandAdapter.GetFieldValue("Position"));
        }

        protected void WorkingWithTasks_()
        {
            var taskDetail = new ApplicationPageObject(Fixture)
               .NavigateTo("Demo Task")
               .OpenRecord("Subject", "Fix breakfast");

            taskDetail
                .List("Contacts")
                .Assert(c => c.RowCount.ShouldBe(0))
                .ExecuteAction(c => c.LinkAction, f => new ListPageObject(f, "Contact"), contactsPopup =>
                {
                    contactsPopup
                        .SelectRow(0)
                        .ExecuteAction(x => x.Action("OK"));
                })
                .Assert(c =>
                {
                    c.RowCount.ShouldBe(1);
                    c.GetValues(0, "Full Name").ShouldBe(new Dictionary<string, string>
                    {
                        ["Full Name"] = "John Nilsen"
                    });
                });
        }

        protected void ChangeContactNameAgainTest_()
        {
            Assert.Equal("John Nilsen", Fixture.CommandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.Equal("Mary Tellitson", Fixture.CommandAdapter.GetCellValue("Contact", 1, "Full Name"));

            Fixture.CommandAdapter.ProcessRecord("Contact", new string[] { "Full Name" }, new string[] { "Mary Tellitson" }, "");

            if (Fixture.IsWeb)
            {
                Fixture.CommandAdapter.DoAction("Edit", null);
            }

            Assert.Equal("Mary Tellitson", Fixture.CommandAdapter.GetFieldValue("Full Name"));
            Assert.Equal("Development Department", Fixture.CommandAdapter.GetFieldValue("Department"));

            Fixture.CommandAdapter.SetFieldValue("First Name", "User_1");
            Fixture.CommandAdapter.SetFieldValue("Last Name", "User_2");

            Fixture.CommandAdapter.DoAction("Save", null);
            Fixture.CommandAdapter.DoAction("Navigation", "Contact");

            Assert.Equal("John Nilsen", Fixture.CommandAdapter.GetCellValue("Contact", 0, "Full Name"));
            Assert.Equal("User_1 User_2", Fixture.CommandAdapter.GetCellValue("Contact", 1, "Full Name"));

        }
    }
}
