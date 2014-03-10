using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Grean.Exude
{
    /// <summary>
    /// A test attribute used to adorn methods that creates first-class 
    /// executable test cases.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The xUnit.net unit testing framework provides various ways in which you
    /// can write Parameterized Tests, and while many of the options are
    /// convenient, either they only support constants (like the [InlineData]
    /// attribute), or they are releatively inconvenient to use, like the
    /// [ClassData] attribute. In all cases of data attributes supporting the
    /// [Theory] attribute, they are based on a contract of untyped (and boxed)
    /// arrays.
    /// </para>
    /// <para>
    /// The [FirstClassTests] attribute, on the other hand, enables you to
    /// write executable test cases as strongly-typed, first-class test
    /// objects.
    /// </para>
    /// <para>
    /// Write a method that returns IEnumerable&lt;ITestCase&gt;, or a type
    /// implementing that interface, like ITestCase[]; then adorn that method
    /// with the [FirstClassTests] attribute. The test runner will then execute
    /// all the returned test cases.
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
    /// <seealso cref="ITestCase" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is part of an inheritance hierarchy. Other developers may want to derive from it in order to extend its behaviour.")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FirstClassTestsAttribute : FactAttribute
    {
        /// <summary>
        /// Enumerates the test commands represented by this test method, by
        /// projecting the returned sequence of <see cref="ITestCase" /> into a
        /// sequence of xUnit.net ITestCommand instances.
        /// </summary>
        /// <param name="method">
        /// The test method adorned with the [FirstClassTests] attribute.
        /// </param>
        /// <returns>
        /// The xUnit.net ITestCommand instances produced by converting this
        /// method's returned sequence of <see cref="ITestCase" /> instances
        /// into a sequence of ITestCommand instances.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="method" /> represents a method that doesn't return
        /// IEnumerable&lt;ITestCase&gt; (or a compatible type, such as
        /// ITestCase[]).
        /// </exception>
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(
            IMethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (IsReturnTypeInvalid(method))
                throw new ArgumentException(
                    invalidReturnTypeErrorMessage,
                    "method");

            var testClassInstance = method.CreateInstance();
            var returnValue = method.MethodInfo.Invoke(testClassInstance, null);
            return from tc in (IEnumerable<ITestCase>)returnValue
                   select tc.ConvertToTestCommand(method);
        }

        private static bool IsReturnTypeInvalid(IMethodInfo method)
        {
            return !typeof(IEnumerable<ITestCase>).IsAssignableFrom(
                method.MethodInfo.ReturnType);
        }

        private const string invalidReturnTypeErrorMessage = @"The supplied method does not return IEnumerable<ITestCase>. When using the [FirstClassTests] attribute, the method it adorns must return IEnumerable<ITestCase>; for example:

[FirstClassTests]
public static IEnumerable<ITestCase> MyTestMethod()
{
    // Return FirstClassCommands here
}
";
    }
}
