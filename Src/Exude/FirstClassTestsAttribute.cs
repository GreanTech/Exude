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
