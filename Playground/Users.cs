using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ApprovalTests.Core;

namespace Playground
{
    public class Users
    {
        public bool Register(User user)
        {
            if (users.ContainsKey(user.UserName))
            {
                return false;
            }

            users[user.UserName] = user;
            return true;
        }

        private Dictionary<string, User> users = new Dictionary<string, User>();

        public User FindUser(string userName)
        {
           // return user for username
           return users[userName];
        }

        public User Login(string username, string password)
        {
            // try to find the user
            var user = FindUser(username);
            if (user != null)
            {
                if (user.Password == password)
                {
                    user.IsLoggedIn = true;
                    return user;
                }
                else
                {
                    user.IsLoggedIn = false;
                }

            }
            // if the user exists
            // return the user else return nothing
            return null;
        }
    }
}