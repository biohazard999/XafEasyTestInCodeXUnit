using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace TestApplication.Module {
    [DefaultClassOptions]
    [Custom("Caption", "Task")]
    public class DemoTask : Task, IComparable {
        private Priority priority;
        private int estimatedWork;
        private int actualWork;
        public DemoTask(Session session)
            : base(session) {
        }

        public Priority Priority {
            get {
                return priority;
            }
            set {
                SetPropertyValue("Priority", ref priority, value);
            }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }
        [Association("Contact-DemoTask", typeof(Contact))]
        public XPCollection Contacts {
            get {
                return GetCollection("Contacts");
            }
        }
        public override string ToString() {
            return this.Subject;
        }
        [Action(ToolTip = "Postpone the task to the next day")]
        public void Postpone() {
            if(DueDate == DateTime.MinValue) {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }
        public int EstimatedWork {
            get { return estimatedWork; }
            set { SetPropertyValue<int>("EstimatedWork", ref estimatedWork, value); }
        }
        public int ActualWork {
            get { return actualWork; }
            set { SetPropertyValue<int>("ActualWork", ref actualWork, value); }
        }
    }

    public enum Priority {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    }
}
