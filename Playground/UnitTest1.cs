using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//   As a user I want to register so that I can log in
//   As a registered user I want to log in so I can be authenticated
//   As an authenticated user I want to log out so that I can be unauthenticated
//   As an authenticated seller I want to create an auction so I can sell stuff
//   As an auction I want to be started so that I can accept bids
//   As an authenticated bidder I want to bid on a started auction so that I can become the highest bidder

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
        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestForExistingUser()
        {
            var scott = new User("Scott", "Kirkland", "srkirkland@ucdavis.edu", "srkirkland", "givingService");
            var users = new Users();
            Assert.IsTrue(users.Register(scott));

            var steve = new User("Steve", "Kirkland", "steve@ucdavis.edu", "srkirkland","baseball");
            var retrieved2 = users.Register(steve);
            Assert.IsFalse(retrieved2);
            Assert.AreEqual(users.FindUser("srkirkland").FirstName, "Scott");
        }

        // if  we have a user name we want to make sure that it's unique by not already checking it was in the user
        // if we are isn the regisiter function w
        // someone is trying to create a new user, but finds out that user already exists which means a userName equaled something already in userName
        // Scott register srkirkland, it works. Steve also tries to register srkirkland, but he gets an error





    }
}
    
