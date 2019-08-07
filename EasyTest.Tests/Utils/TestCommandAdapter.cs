using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;

namespace EasyTest.Tests.Utils
{
    public class TestCommandAdapter
    {
        private readonly ICommandAdapter adapter;
        private readonly TestApplication testApplication;
        public TestCommandAdapter(ICommandAdapter webAdapter, TestApplication testApplication)
        {
            this.testApplication = testApplication;
            adapter = webAdapter;
        }
        
        internal void DoAction(string name, string paramValue) 
            => new ActionCommand().DoAction(adapter, name, paramValue);
        
        internal string GetActionValue(string name)
        {
            var control = adapter.CreateTestControl(TestControlType.Action, name).GetInterface<IControlText>();
            return control.Text;
        }

        internal string GetFieldValue(string fieldName) 
            => CheckFieldValuesCommand.GetFieldValue(adapter, fieldName);
        
        internal void ProcessRecord(string tableName, string[] columnNames, string[] values, string actionName)
        {
            ProcessRecordCommand command = new ProcessRecordCommand();
            command.SetApplicationOptions(testApplication);
            command.ProcessRecord(adapter, tableName, actionName, columnNames, values);
        }

        internal void SetFieldValue(string fieldName, string value) 
            => FillFieldCommand.SetFieldCommand(adapter, fieldName, value);
        
        public IGridColumn GetColumn(ITestControl testControl, string columnName)
        {
            foreach (IGridColumn column in testControl.GetInterface<IGridBase>().Columns)
            {
                if (string.Compare(column.Caption, columnName, testApplication.IgnoreCase) == 0)
                {
                    return column;
                }
            }
            return null;
        }
        
        internal string GetCellValue(string tableName, int row, string columnName)
        {
            var testControl = adapter.CreateTestControl(TestControlType.Table, tableName);
            var gridControl = testControl.GetInterface<IGridBase>();
            return gridControl.GetCellValue(row, GetColumn(testControl, columnName));
        }
        
        internal object GetTableRowCount(string tableName)
        {
            var gridControl = adapter.CreateTestControl(TestControlType.Table, tableName).GetInterface<IGridBase>();
            return gridControl.GetRowCount();
        }
    }
}