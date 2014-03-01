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

            return new FirstClassCommand(this.testAction);
        }

        public Action<object> TestAction
        {
            get { return this.testAction; }
        }
    }
}
