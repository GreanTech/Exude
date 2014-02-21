using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Grean.Exude
{
    public class FirstClassCommand : TestCommand
    {
        public FirstClassCommand()
            : base(Reflector.Wrap(((Action)(() => { })).Method), "", 0)
        {
        }

        public override MethodResult Execute(object testClass)
        {
            throw new NotImplementedException();
        }
    }
}
