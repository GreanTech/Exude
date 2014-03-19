using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    /// <summary>
    /// An executable test case, represented as an xUnit.net ITestCommand.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is mostly the result of converting an
    /// <see cref="ITestCase" /> instance to an xUnit.net ITestCommand by
    /// invoking <see cref="ITestCase.ConvertToTestCommand(IMethodInfo)" />.
    /// </para>
    /// </remarks>
    /// <seealso cref="ITestCase" />
    /// <seealso cref="FirstClassTestsAttribute" />
    public class FirstClassCommand : TestCommand
    {
        private readonly Action<object> testAction;
        private readonly bool shouldCreateInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstClassCommand"/>
        /// class.
        /// </summary>
        /// <param name="testAction">
        /// The test action to be invoked when the test is executed.
        /// </param>
        /// <param name="testMethod">
        /// The test method with which this instance is associated. This will
        /// likely be the method adorned with an
        /// <see cref="FirstClassTestsAttribute" />.
        /// </param>
        /// <param name="shouldCreateInstance">
        /// Indicates whether an instance of the type that hosts the
        /// <paramref name="testMethod" /> should be created. For static types,
        /// the value for this parameter is <see langword="false" />. For non-
        /// static types, the value for this parameter is <see langword="true" />.
        /// </param>
        /// <remarks>
        /// <para>
        /// The <paramref name="testAction" /> constructor argument is
        /// subsequently available as the <see cref="TestAction" /> property.
        /// Likewise, the <paramref name="testMethod" /> and the
        /// <paramref name="shouldCreateInstance" /> constructor arguments are
        /// subsequently available as the <see cref="HostTestMethod" /> and
        /// <see cref="ShouldCreateInstance" /> properties, respectively.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="testAction" /> is <see langword="null" />
        /// </exception>
        /// <seealso cref="TestAction" />
        /// <seealso cref="HostTestMethod" />
        /// <seealso cref="ShouldCreateInstance" />
        public FirstClassCommand(
            Action<object> testAction,
            IMethodInfo testMethod,
            bool shouldCreateInstance)
            : base(testMethod, null, -1)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            this.testAction = testAction;
            this.shouldCreateInstance = shouldCreateInstance;

        }

        /// <summary>
        /// </summary>
        /// <param name="testClass"></param>
        /// <returns></returns>
        /// <inheritdoc />
        public override MethodResult Execute(object testClass)
        {
            this.testAction(testClass);
            return new PassedResult(
                this.testMethod,
                null);
        }

        /// <summary>
        /// </summary>
        /// <inheritdoc />
        public override bool ShouldCreateInstance
        {
            get { return this.shouldCreateInstance; }
        }

        /// <summary>Gets the test action.</summary>
        /// <value>
        /// The test action originally supplied as a constructor argument.
        /// </value>
        /// <seealso cref="FirstClassCommand(Action{object}, IMethodInfo, bool)" />
        public Action<object> TestAction
        {
            get { return this.testAction; }
        }

        /// <summary>Gets the test method.</summary>
        /// <value>
        /// The test method originally supplied as a constructor argument.
        /// </value>
        public IMethodInfo HostTestMethod
        {
            get { return this.testMethod; }
        }
    }
}