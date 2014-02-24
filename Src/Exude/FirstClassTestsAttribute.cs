using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Grean.Exude
{
    public class FirstClassTestsAttribute : FactAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(
            IMethodInfo method)
        {
            var testClassInstance = method.CreateInstance();
            var returnValue = method.MethodInfo.Invoke(testClassInstance, null);
            return from fcc in (IEnumerable<FirstClassCommand>)returnValue
                   select fcc as ITestCommand;
        }
    }
}
