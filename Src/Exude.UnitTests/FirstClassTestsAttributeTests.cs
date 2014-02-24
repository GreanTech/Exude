using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;

namespace Grean.Exude.UnitTests
{
    public class FirstClassTestsAttributeTests
    {
        [Fact]
        public void SutIsFactAttribute()
        {
            var sut = new FirstClassTestsAttribute();
            Assert.IsAssignableFrom<FactAttribute>(sut);
        }
    }
}
