using System;
using System.Collections.Generic;
using NUnit.Framework;
using DevExpress.EasyTest.Framework;
using EasyTest.Tests.Utils;
using Xunit;

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
            var control = Fixture.Adapter.CreateTestControl(TestControlType.Table, "");
            var table = control.GetInterface<IGridBase>();
            Assert.Equal(2, table.GetRowCount());

            var column = Fixture.CommandAdapter.GetColumn(control, "Full Name");

            Assert.Equal("John Nilsen", table.GetCellValue(0, column));
            Assert.Equal("Mary Tellitson", table.GetCellValue(1, column));

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
            Fixture.CommandAdapter.DoAction("Navigation", "Default.Demo Task");
            Fixture.CommandAdapter.ProcessRecord("Demo Task", new string[] { "Subject" }, new string[] { "Fix breakfast" }, "");

            var control = Fixture.Adapter.CreateTestControl(TestControlType.Table, "Contacts");
            var table = control.GetInterface<IGridBase>();
            Assert.Equal(0, table.GetRowCount());

            Fixture.CommandAdapter.DoAction("Contacts.Link", null);
            control = Fixture.Adapter.CreateTestControl(TestControlType.Table, "Contact");
            control.GetInterface<IGridRowsSelection>().SelectRow(0);
            Fixture.CommandAdapter.DoAction("OK", null);

            control = Fixture.Adapter.CreateTestControl(TestControlType.Table, "Contacts");
            table = control.GetInterface<IGridBase>();
            Assert.Equal(1, table.GetRowCount());
            Assert.Equal("John Nilsen", Fixture.CommandAdapter.GetCellValue("Contacts", 0, "Full Name"));
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
