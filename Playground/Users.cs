using System.Collections;
using System.Collections.Generic;
using ApprovalTests.Core;

namespace Playground
{
    public class Users
    {
        public void Register(User user)
        {
            users[user.UserName] = user;
        }

        private Dictionary<string, User> users = new Dictionary<string, User>();

        public User FindUser(string userName)
        {
           // return user for username
           return users[userName];
        }
    }
}