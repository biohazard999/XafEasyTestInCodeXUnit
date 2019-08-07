using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class ContactListPageObject : ContactListPageObject<ContactListPageObject>
    {
        public ContactListPageObject(EasyTestFixtureBase fixture) : base(fixture) { }
    }
    
    public class ContactListPageObject<T> : ListPageObject<T>
        where T : ContactListPageObject<T>
    {
        public ContactListPageObject(EasyTestFixtureBase fixture) : base(fixture, "Contact") { }

        public ContactDetailPageObject OpenRecordByFullName(string title)
            => OpenRecord("Full Name", title, f => new ContactDetailPageObject(f));
    }
}
