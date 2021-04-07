using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDevKit.Utilities.Tests
{
    [TestFixture]
    public class MiscellanyTests
    {
        [Test]
        public void InvokeCmd()
        {
            Miscellany.InvokeCmd(new string[] { "cd c:\\Windows", "echo %CD%" },out var output,out var  error);
            Assert.IsTrue(output.Contains("c:\\Windows"));
            Assert.IsTrue(error==string.Empty);
        }

        [Test]
        public void InvokeCmd_Error()
        {
            Miscellany.InvokeCmd(new string[] { "NOT_RECOGNIZED_COMMAND" }, out var output, out var error);
            Assert.IsTrue(error.Contains("'NOT_RECOGNIZED_COMMAND' is not recognized"));
        }
    }
}
