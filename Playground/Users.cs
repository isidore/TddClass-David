using System;
using System.Collections;
using System.Collections.Generic;
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

        public bool Login(string v1, string v2)
        {
            return true;
        }
    }
}