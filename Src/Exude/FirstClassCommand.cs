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

        public Action<object> TestAction
        {
            get { return this.testAction; }
        }

        private static IMethodInfo ConvertToMethodInfo(
            Action<object> testAction)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            return Reflector.Wrap(testAction.Method);
        }
    }
}