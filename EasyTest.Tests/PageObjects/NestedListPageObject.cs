using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public class NestedListPageObject : NestedListPageObject<NestedListPageObject>
    {
        public NestedListPageObject(EasyTestFixtureBase fixture, string listName) : base(fixture, listName) { }
    }
    
    public abstract class NestedListPageObject<T> : ListPageObject<T>
        where T : NestedListPageObject<T>
    {
        public NestedListPageObject(EasyTestFixtureBase fixture, string listName) : base(fixture, listName) { }
    }
}
