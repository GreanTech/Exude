using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;

namespace Grean.Exude.UnitTests
{
    public class TestCaseTests
    {
        [Fact]
        public void SutIsTestCase()
        {
            var sut = new TestCase();
            Assert.IsAssignableFrom<ITestCase>(sut);
        }
    }
}
