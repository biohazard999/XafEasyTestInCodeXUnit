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
                .ExecuteActionIf(f => f.IsWeb, c => c.EditAction)
                .Do(c =>
                {
                    c.FirstName = "User_1";
                    c.LastName = "User_2";
                    c.Position = "Developer";
                })
                .ExecuteAction(c => c.SaveAction)
                .Assert(c =>
                {
                    c.FullName.ShouldBe("User_1 User_2");
                    c.Position.ShouldBe("Developer");
                })
                .ExecuteAction(c => c.SaveAndCloseAction);

            contactList.GetValues(1, "Full Name", "Position").ShouldBe(new Dictionary<string, string>
            {
                ["Full Name"] = "User_1 User_2",
                ["Position"] = "Developer"
            });
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
            var application = new ApplicationPageObject(Fixture);

            var contactList = application
                .NavigateToContact()
                .Assert(c =>
                {
                    c.GetValues(0, "Full Name").ShouldBe(new Dictionary<string, string>
                    {
                        ["Full Name"] = "John Nilsen"
                    });
                    c.GetValues(1, "Full Name").ShouldBe(new Dictionary<string, string>
                    {
                        ["Full Name"] = "Mary Tellitson"
                    });
                });

            var contactDetail = contactList
                .OpenRecordByFullName("Mary Tellitson")
                .ExecuteActionIf(f => f.IsWeb, c => c.EditAction)
                .Assert(c =>
                {
                    c.FullName.ShouldBe("Mary Tellitson");
                    c.Department.ShouldBe("Development Department");
                })
                .Do(c =>
                {
                    c.FirstName = "User_1";
                    c.LastName = "User_2";
                })
                .ExecuteAction(c => c.SaveAction);

            application
                .NavigateToContact()
                .Assert(c =>
                {
                    c.GetValues(0, "Full Name").ShouldBe(new Dictionary<string, string>
                    {
                        ["Full Name"] = "John Nilsen"
                    });
                    c.GetValues(1, "Full Name").ShouldBe(new Dictionary<string, string>
                    {
                        ["Full Name"] = "User_1 User_2"
                    });
                });
        }
    }
}
