using System;
using System.Linq;
using EasyTest.Tests.Utils;

namespace EasyTest.Tests.PageObjects
{
    public abstract class PageObject<T> where T : PageObject<T>
    {
        protected readonly EasyTestFixtureBase Fixture;
        public PageObject(EasyTestFixtureBase fixture)
            => Fixture = fixture;

        protected T This => (T)this;

        public T Assert(Action<T> assert)
        {
            assert(This);
            return This;
        }

        public virtual ActionPageObject Action(string actionName) => new ActionPageObject(Fixture, actionName);

        public T ExecuteAction(Func<T, ActionPageObject> action)
        {
            action(This).Execute();
            return This;
        }

        public T ExecuteAction<TPageObject>(Func<T, ActionPageObject> action, Func<EasyTestFixtureBase, TPageObject> pageObjectFactory, Action<TPageObject> executor)
        {
            action(This).Execute();
            executor(pageObjectFactory(Fixture));
            return This;
        }

        public T ExecuteActionIf(Predicate<EasyTestFixtureBase> predicate, Func<T, ActionPageObject> action)
        {
            if (predicate(Fixture))
            {
                return ExecuteAction(action);
            }
            return This;
        }

        public T Do(Action<T> action)
        {
            action(This);
            return This;
        }
    }
}
