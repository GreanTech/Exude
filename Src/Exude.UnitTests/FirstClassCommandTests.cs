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
            var sut = new FirstClassCommand();
            Assert.IsAssignableFrom<ITestCommand>(sut);
        }
    }
}
