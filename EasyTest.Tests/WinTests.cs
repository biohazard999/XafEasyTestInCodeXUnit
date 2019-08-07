using System;
using Xunit;

namespace EasyTest.Tests
{
    public class WinTests : CommonTests<WinTestApplicationHelper>
    {
        [Fact]
        public void ChangeContactNameTest() => ChangeContactNameTest_();

        [Fact]
        public void WorkingWithTasks() => WorkingWithTasks_();

        [Fact]
        public void ChangeContactNameAgainTest()
            => ChangeContactNameAgainTest_();
    }
}
