using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Playground
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestMethod1()
        {
            // create a user class first
            var user = new User("firstname", "lastName", "email@email.com", "userName", "Password");
            Approvals.Verify(user);
            //
        }
    }

    }
