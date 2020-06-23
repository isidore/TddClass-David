using System;
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
            var scott = GetUserScot(); var users = new Users();
                users.Register(scott);
            // Retrieve user from Users, using username as key
            var retrieved = users.FindUser("srkirkland");
            Approvals.Verify(retrieved);

        }
        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestForExistingUser()
        {
            // Scott register srkirkland, it works. Steve also tries to register srkirkland, but he gets an error
            var scott = GetUserScot();
            var users = new Users();
            Assert.IsTrue(users.Register(scott));

            var steve = new User("Steve", "Kirkland", "steve@ucdavis.edu", "srkirkland","baseball");
            var retrieved2 = users.Register(steve);
            Assert.IsFalse(retrieved2);
            Assert.AreEqual(users.FindUser("srkirkland").FirstName, "Scott");
        }

        private static User GetUserScot()
        {
            var scott = new User("Scott", "Kirkland", "srkirkland@ucdavis.edu", "srkirkland", "givingService");
            return scott;
        }

        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestAuthenticateUser()
        {
            var scott = GetUserScot();
            var users = new Users();
            Assert.IsTrue(users.Register(scott));
            var loggedIn = users.Login("srkirkland", "givingService");
            Assert.IsTrue(loggedIn.IsLoggedIn);
        }
        // Scott tried to log in yesterday and used the wrong password and could not log in
        [TestMethod, UseReporter(typeof(DiffReporter))]
        public void TestRejectBadPassword()
        {
            var scott = GetUserScot();
            var users = new Users();
            Assert.IsTrue(users.Register(scott));
            Assert.ThrowsException<Exception>(() => users.Login("srkirkland", "bogus"));
        }


        //   As a user I want to register so that I can log in
        //   As a registered user I want to log in so I can be authenticated
        //   As an authenticated user I want to log out so that I can be unauthenticated
        //   As an authenticated seller I want to create an auction so I can sell stuff
        //   As an auction I want to be started so that I can accept bids
        //   As an authenticated bidder I want to bid on a started auction so that I can become the highest bidder


        // to log in should not be log in
        // Scott can use the correct creditials and successfully log in
        // scott loggged in yesterday using his username and password
        // try to log on and not be able to log in
        // takes a username and password and sets to true as you are logged in 
        // enter a username and a passowrd


    }
}
    
