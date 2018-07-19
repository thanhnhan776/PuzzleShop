﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Library.UserCollection
{
    public class UserDAO
    {
        /// <summary>
        /// Check login by username or email
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public User checkLogin(string Username_Email, string Password)
        {
            //Get connection and PuzzleShopDB
            var db = utils.DBConnect.getDB();
            var Users = db.GetCollection<User>("User");
            var builder = Builders<User>.Filter;
            //Find account by username or password
            var filter = builder.Where(a => a.Username.Equals(Username_Email)) | builder.Where(a =>a.Email.Equals(Username_Email));
            
            List<User> list = Users.Find(filter).ToList();
            if(list.Count > 0)
            {
                string passwordHash = list[0].PasswordHash;
                if (MD5Hash.VerifyMD5Hash(Password, passwordHash))
                    return list[0];
            }
            return null;
        }


        /// <summary>
        /// Register, input must be validated first
        /// If Username or Email existed in system, throw Duplicate Exception
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        public void register(string username, string password, string firstName, string lastName, string email)
        {
            var db = utils.DBConnect.getDB();
            var accounts = db.GetCollection<User>("User");
            string passwordHash = MD5Hash.GetMD5Hash(password);
            try
            {
                accounts.InsertOne(new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Role = 1
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
