using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Playground
{
    [TestClass]
    [UseReporter(typeof(DiffReporter))]
    public class UserTest
    {

        [TestMethod]
        public void TestCreateUser()
        {
            // create a user class first
            var user = new User("firstname", "lastName", "email@email.com", "userName", "Password");

            Approvals.Verify(user);
            //

        }
        [TestMethod]
        public void TestRegisterUser()
        { 
            // test the register method on the users class to ensure unique user registrations
            var scott = GetUserScot(); var users = new Users();
                users.Register(scott);
            // Retrieve user from Users, using username as key
            var retrieved = users.FindUser("srkirkland");
            Approvals.Verify(retrieved);

        }
        [TestMethod]
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

        [TestMethod]
        public void TestAuthenticateUser()
        {
            var scott = GetUserScot();
            var users = new Users();
            users.Register(scott);
            var loggedIn = users.Login("srkirkland", "givingService");
            Assert.IsTrue(loggedIn.IsLoggedIn);
        }
        // Scott tried to log in yesterday and used the wrong password and could not log in
        [TestMethod]
        public void TestRejectBadPassword()
        {
            var (users,scott) = SetupScott(true);
            Assert.ThrowsException<Exception>(() => users.Login("srkirkland", "bogus"));
        }

        public  (Users, User) SetupScott(bool isRegistered)
        {
            var scott = GetUserScot();
            var users = new Users();
            if (isRegistered)
            {
                users.Register(scott);
            }
            return (users, scott) ;
        }

        // 

        // wes tries to log in but wasn't registered
        [TestMethod]
        public void TestCantLoginIfNotRegistered()
        {
            var (users, scott) = SetupScott(false);
            Assert.ThrowsException<Exception>(() => users.Login("wes", "bogus"));
        }

        // scott wants to log out.

        [TestMethod]
        public void TestLogOut()
        {
            var (users, scott) = SetupScott(true);
            users.Login(scott.UserName, scott.Password);
            users.Logout(scott.UserName);
            Assert.IsFalse(scott.IsLoggedIn);
        }

        [TestMethod]
        public void TestUnknownUserCanLogOut()
        {
            new Users().Logout("unknown");
            
        }
        // add seller component to user
        // scott was a seller (method of IsSeller, call it and return a true if seller, and false if not seller)

        [TestMethod]
        public void TestMakeSeller()
        {
            // checking if user has seller attribute of true (indicating seller)
            //Write a test that fails by us making scott a seller and testing Is he a seller - false. Fails as well if Scott is not a seller and is asked if he is
            var (users, scott) = SetupScott(true);
            users.MakeSeller(scott);
            Assert.IsTrue(scott.IsSeller);
        }




        //   As an authenticated seller I want to create an auction so I can sell stuff
        //   As an auction I want to be started so that I can accept bids
        //   As an authenticated bidder I want to bid on a started auction so that I can become the highest bidder



    }
}
    
