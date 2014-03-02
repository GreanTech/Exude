using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    public class FirstClassCommand : TestCommand
    {
        private readonly Action<object> testAction;

        public FirstClassCommand(
            Action<object> testAction,
            IMethodInfo testMethod)
            : base(testMethod, null, -1)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            this.testAction = testAction;
        }

        public override MethodResult Execute(object testClass)
        {
            this.testAction(testClass);
            return new PassedResult(
                this.testMethod,
                null);
        }

        public override bool ShouldCreateInstance
        {
            get { return true; }
        }

        public Action<object> TestAction
        {
            get { return this.testAction; }
        }

        public IMethodInfo TestMethod
        {
            get { return this.testMethod; }
        }
    }
}