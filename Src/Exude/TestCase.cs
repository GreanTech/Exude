using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grean.Exude
{
    public class TestCase : ITestCase
    {
        private Action<object> testAction;

        public TestCase(Action<object> testAction)
        {
            this.testAction = testAction;
        }

        public Action<object> TestAction
        {
            get { return this.testAction; }
        }
    }
}
