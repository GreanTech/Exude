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
            if (!typeof(IEnumerable<FirstClassCommand>).IsAssignableFrom(method.MethodInfo.ReturnType))
                throw new ArgumentException(
                    invalidReturnTypeErrorMessage,
                    "method");

            var testClassInstance = method.CreateInstance();
            var returnValue = method.MethodInfo.Invoke(testClassInstance, null);
            return from fcc in (IEnumerable<FirstClassCommand>)returnValue
                   select fcc as ITestCommand;
        }

        private const string invalidReturnTypeErrorMessage = @"The supplied method does not return IEnumerable<FirstClassCommand>. When using the [FirstClassTests] attribute, the method it adorns must return IEnumerable<FirstClassCommand>; for example:

[FirstClassTests]
public static IEnumerable<FirstClassCommand> MyTestMethod()
{
    // Return FirstClassCommands here
}
";
    }
}
