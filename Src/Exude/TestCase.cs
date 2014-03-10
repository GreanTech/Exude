using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// the test action is an instance of the test class hosting the test
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

    /// <summary>
    /// Represents a strongly-typed test case that can be turned into an
    /// xUnit.net ITestCommand when returned from a test method adorned with
    /// the <see cref="FirstClassTestsAttribute" />
    /// </summary>
    /// <typeparam name="T">
    /// The type of the test class hosting the test case.
    /// </typeparam>
    /// <seealso cref="FirstClassTestsAttribute" />
    /// <seealso cref="ITestCase" />
    /// <seealso cref="TestCase" />
    public class TestCase<T> : ITestCase
    {
        private Action<T> testAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCase{T}"/> class.
        /// </summary>
        /// <param name="testAction">
        /// The test action to be invoked when the test is executed.
        /// </param>
        /// <remarks>
        /// <para>
        /// When this test case is exececuted, the
        /// <paramref name="testAction" /> is invoked. The argument supplied to
        /// the test action is an instance of the test class hosting the test
        /// method adorned with an <see cref="FirstClassTestsAttribute" />, if
        /// any. However, xUnit.net may pass <see langword="null" /> as the
        /// test class instance. It's also possible that an
        /// <see cref="ArgumentException" /> is thrown if the type of the
        /// hosting test class is incompatible with <typeparamref name="T" />.
        /// </para>
        /// <para>
        /// The test action constructor argument is subsequently available as
        /// the <see cref="TestAction" /> property.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demonstrates how to write a strongly-typed
        /// Parameterized Test using <see cref="TestCase{T}" />. The containing
        /// test class that declares and implements both methods is called
        /// <strong>Sceanario</strong>.
        /// <code><![CDATA[public void AParameterizedTest(DateTimeOffset x, DateTimeOffset y)
        /// {
        ///     Assert.True(x < y);
        /// }
        /// 
        /// [FirstClassTests]
        /// public static TestCase<Scenario>[] RunAParameterizedTest()
        /// {
        ///     var testCases = new[] 
        ///     {
        ///         new 
        ///         {
        ///             x = new DateTimeOffset(2002, 10, 12, 18, 15, 0, TimeSpan.FromHours(1)),
        ///             y = new DateTimeOffset(2007,  4, 21, 18, 15, 0, TimeSpan.FromHours(1))
        ///         },
        ///         new
        ///         {
        ///             x = new DateTimeOffset(1970, 11, 25, 16, 10, 0, TimeSpan.FromHours(1)),
        ///             y = new DateTimeOffset(1972,  6,  6,  8,  5, 0, TimeSpan.FromHours(1))
        ///         },
        ///         new
        ///         {
        ///             x = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(1)),
        ///             y = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(0))
        ///         }
        ///     };
        ///     return testCases
        ///         .Select(tc =>
        ///             new TestCase<Scenario>(
        ///                 s => s.AParameterizedTest(tc.x, tc.y)))
        ///         .ToArray();
        /// }/// ]]></code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="testAction" /> is <see langword="null" />
        /// </exception>
        public TestCase(Action<T> testAction)
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

            return new FirstClassCommand(this.AdaptTest, method);
        }

        private void AdaptTest(object testClass)
        {
            if (!(testClass is T))
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "The supplied testClass instance isn't compatible with the generic parameter of this TestCase<{0}>. The instance type was {1}, but should have been convertible to {0}.",
                        typeof(T),
                        testClass.GetType()),
                    "testClass");

            this.testAction((T)testClass);
        }

        /// <summary>Gets the test action.</summary>
        /// <value>
        /// The test action originally supplied as a constructor argument.
        /// </value>
        /// <seealso cref="TestCase{T}(Action{T})" />
        public Action<T> TestAction
        {
            get { return this.testAction; }
        }
    }
}
