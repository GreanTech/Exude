using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    public class TestCase : ITestCase
    {
        private Action<object> testAction;

        public TestCase(Action<object> testAction)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            this.testAction = testAction;
        }

        public ITestCommand ConvertToTestCommand(IMethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            return new FirstClassCommand(this.testAction, method);
        }

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
            return new FirstClassCommand(
                this.AdaptedTestAction,
                Reflector.Wrap(this.testAction.Method));
        }

        private Action<object> AdaptedTestAction
        {
            get
            {
                Action<object> a = testClass =>
                {
                    if (!(testClass is T))
                        throw new ArgumentException(
                            string.Format(
                                "The supplied testClass instance isn't compatible with the generic parameter of this TestCase<{0}>. The instance type was {1}, but should have been convertible to {0}.",
                                typeof(T),
                                testClass.GetType()),
                            "testClass");

                    this.testAction((T)testClass);
                };
                return a;
            }
        }

        public Action<T> TestAction
        {
            get { return this.testAction; }
        }
    }
}
