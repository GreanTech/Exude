using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    /// <summary>
    /// Represents a test-case that can be turned into an xUnit.net
    /// ITestCommand when returned from a test method adorned with the
    /// <see cref="FirstClassTestsAttribute" />.
    /// </summary>
    /// <seealso cref="FirstClassTestsAttribute" />
    /// <seealso cref="TestCase" />
    /// <seealso cref="TestCase{T}" />
    public interface ITestCase
    {
        /// <summary>
        /// Converts the instance to an xUnit.net ITestCommand instance.
        /// </summary>
        /// <param name="method">
        /// The method adorned by a <see cref="FirstClassTestsAttribute" />.
        /// </param>
        /// <returns>
        /// An xUnit.net ITestCommand that represents the executable test case.
        /// </returns>
        /// <seealso cref="FirstClassTestsAttribute" />
        ITestCommand ConvertToTestCommand(IMethodInfo method);
    }
}
