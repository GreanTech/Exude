using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;
using Xunit.Sdk;
using System.Reflection;

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

        [Fact]
        public void CreateTestMethodWithFirstClassTestsReturnsCorrectResult()
        {
            var method = Reflector.Wrap(this.GetType().GetMethod(
                "CreateFirstClassTests",
                BindingFlags.Static | BindingFlags.NonPublic));
            var sut = new FirstClassTestsAttribute();

            var actual = sut.CreateTestCommands(method);

            Assert.Equal(
                3,
                actual.OfType<FirstClassCommand>().Count());
            Assert.True(
                actual
                    .OfType<FirstClassCommand>()
                    .All(fcc => fcc.TestAction.Method.DeclaringType == this.GetType()),
                "All test actions should be declared here.");
        }

        private static IEnumerable<FirstClassCommand> CreateFirstClassTests()
        {
            yield return new FirstClassCommand(_ => { });
            yield return new FirstClassCommand(_ => { });
            yield return new FirstClassCommand(_ => { });
        }

        [Fact]
        public void CreateTestsForMethodWithoutFirstClassTestsThrows()
        {
            var method = Reflector.Wrap(this.GetType().GetMethod(
                "VoidTests",
                BindingFlags.Static | BindingFlags.NonPublic));
            var sut = new FirstClassTestsAttribute();

            Assert.Throws<ArgumentException>(
                () => sut.CreateTestCommands(method));
        }

        private static void VoidTests() { }
    }
}
