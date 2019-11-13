using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class ContactDetailPageObject : ContactDetailPageObject<ContactDetailPageObject>
    {
        public ContactDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }
    
    public class ContactDetailPageObject<T> : DetailPageObject<T>
        where T : ContactDetailPageObject<T>
    {
        public ContactDetailPageObject(EasyTestFixtureBase fixture) : base(fixture) { }

        public string FirstName
        {
            get => GetValue("First Name");
            set => SetValue("First Name", value);
        }

        public string LastName
        {
            get => GetValue("Last Name");
            set => SetValue("Last Name", value);
        }

        public string FullName
        {
            get => GetValue("Full Name");
            set => SetValue("Full Name", value);
        }

        public string Department
        {
            get => GetValue("Department");
            set => SetValue("Department", value);
        }

        public string Position
        {
            get => GetValue("Position");
            set => SetValue("Position", value);
        }
    }

}
