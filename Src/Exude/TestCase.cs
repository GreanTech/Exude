using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    /// <summary>
    /// Represents a weakly-typed test case that can be turned into an
    /// xUnit.net ITestCommand when returned from a test method adorned with
    /// the <see cref="FirstClassTestsAttribute" />
    /// </summary>
    /// <seealso cref="FirstClassTestsAttribute" />
    /// <seealso cref="ITestCase" />
    /// <seealso cref="TestCase{T}" />
    public class TestCase : ITestCase
    {
        private Action<object> testAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCase"/> class.
        /// </summary>
        /// <param name="testAction">
        /// The test action to be invoked when the test is executed.
        /// </param>
        /// <remarks>
        /// <para>
        /// When this test case is exececuted, the
        /// <paramref name="testAction" /> is invoked. The argument supplied to
        /// the test action is on instance of the test class hosting the test
        /// method adorned with an <see cref="FirstClassTestsAttribute" />, if
        /// any. However, xUnit.net may pass <see langword="null" /> as the
        /// test class instance.
        /// </para>
        /// <para>
        /// The test action constructor argument is subsequently available as
        /// the <see cref="TestAction" /> property.
        /// </para>
        /// </remarks>
        /// <example>
        /// This simple example returns three test cases that all pass:
        /// <code><![CDATA[[FirstClassTests]
        /// public static IEnumerable<ITestCase> YieldFirstClassTests()
        /// {
        ///     yield return new TestCase(_ => Assert.Equal(1, 1));
        ///     yield return new TestCase(_ => Assert.Equal(2, 2));
        ///     yield return new TestCase(_ => Assert.Equal(3, 3));
        /// }]]>
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="testAction" /> is <see langword="null" />
        /// </exception>
        /// <seealso cref="TestAction" />
        public TestCase(Action<object> testAction)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            this.testAction = testAction;
        }

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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method" /> is <see langword="null" />
        /// </exception>
        public ITestCommand ConvertToTestCommand(IMethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return new FirstClassCommand(this.testAction, method);
        }

        /// <summary>Gets the test action.</summary>
        /// <value>
        /// The test action originally supplied as a constructor argument.
        /// </value>
        /// <seealso cref="TestCase(Action{object})" />
        public Action<object> TestAction
        {
            get { return this.testAction; }
        }
    }

    public class TestCase<T> : ITestCase
    {
        private Action<T> testAction;

        public TestCase(Action<T> testAction)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            this.testAction = testAction;
        }

        public ITestCommand ConvertToTestCommand(IMethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return new FirstClassCommand(this.AdaptTest, method);
        }

        private void AdaptTest(object testClass)
        {
            if (!(testClass is T))
                throw new ArgumentException(
                    string.Format(
                        "The supplied testClass instance isn't compatible with the generic parameter of this TestCase<{0}>. The instance type was {1}, but should have been convertible to {0}.",
                        typeof(T),
                        testClass.GetType()),
                    "testClass");

            this.testAction((T)testClass);
        }

        public Action<T> TestAction
        {
            get { return this.testAction; }
        }
    }
}
