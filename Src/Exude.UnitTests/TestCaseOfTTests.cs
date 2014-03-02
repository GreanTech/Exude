using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;

namespace Grean.Exude.UnitTests
{
    public class TestCaseOfTTests
    {
        [Fact]
        public void SutIsTestCase()
        {
            var sut = new TestCase<object>();
            Assert.IsAssignableFrom<ITestCase>(sut);
        }
    }
}
