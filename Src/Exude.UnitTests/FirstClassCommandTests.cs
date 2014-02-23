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
        public void ActionIsCorrect()
        {
            Action<object> expected = _ => { };
            var sut = new FirstClassCommand(expected);

            Action<object> actual = sut.Action;

            Assert.Equal(expected, actual);
        }
    }
}
