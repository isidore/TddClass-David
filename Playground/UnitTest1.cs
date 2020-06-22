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
        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestRegisterUser()
        {
            // test the register method on the users class to ensure unique user registrations
            var user = new User("Scott","Kirkland","srkirkland@ucdavis.edu","srkirkland","givingService");
            var users = new Users();
                users.Register(user);
            // Retrieve user from Users, using username as key
            var retrieved = users.FindUser("srkirkland");


                Approvals.Verify(retrieved);

        }

    }
}
    
