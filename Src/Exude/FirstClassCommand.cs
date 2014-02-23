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

        public FirstClassCommand(Action<object> testAction)
            : base(Reflector.Wrap(((Action)(() => { })).Method), "", 0)
        {
            this.testAction = testAction;
        }

        public override MethodResult Execute(object testClass)
        {
            this.testAction(testClass);
            return new PassedResult(
                Reflector.Wrap(this.testAction.Method),
                null);
        }

        public Action<object> TestAction
        {
            get { return this.testAction; }
        }
    }
}
