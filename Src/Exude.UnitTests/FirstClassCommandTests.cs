using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;
using Xunit.Sdk;

namespace Grean.Exude.UnitTests
{
    public class FirstClassCommandTests
    {
        [Fact]
        public void SutIsTestCommand()
        {
            Action<object> dummyAction = _ => { };
            var sut = new FirstClassCommand(dummyAction);
            Assert.IsAssignableFrom<ITestCommand>(sut);
        }

        [Fact]
        public void TestActionIsCorrect()
        {
            Action<object> expected = _ => { };
            var sut = new FirstClassCommand(expected);

            Action<object> actual = sut.TestAction;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteInvokesAction()
        {
            var verified = false;
            var obj = new object();
            Action<object> spy = x => verified = x == obj;
            var sut = new FirstClassCommand(spy);

            sut.Execute(obj);

            Assert.True(verified, "Spy should have been invoked.");
        }

        [Fact]
        public void ExecuteSuccessfullyReturnsCorrectResult()
        {
            var sut = new FirstClassCommand(_ => { });
            var actual = sut.Execute(new object());
            Assert.IsAssignableFrom<PassedResult>(actual);
        }
    }
}
