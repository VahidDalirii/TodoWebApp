﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using ToDo1WebApp.Models;

namespace ToDo1WebApp
{
    public class Database
    {
        private const string TODO_COLLECTION = "Todos";
        private const string ACCOUNT_COLLECTION = "Accounts";
        private readonly IMongoDatabase db;

        public Database(string dbName = "Todo-list")
        {
            MongoClient client = new MongoClient();
            db = client.GetDatabase(dbName);
            //          string connectionString =
            //@"mongodb://mongodb-todowebapp:6yyS6hY0vFESZMWykwlTKb983vyR36lVuc9T8EH5lhzusUQzUXqqHV99njNY0I2hYNFqBwRwC7EFBOYoxMaoMA==@mongodb-todowebapp.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@mongodb-todowebapp@";
            //          MongoClientSettings settings = MongoClientSettings.FromUrl(
            //            new MongoUrl(connectionString)
            //          );
            //          settings.SslSettings =
            //            new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            //          var mongoClient = new MongoClient(settings);
        }

        internal List<Account> GetAllAccounts()
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            return collection.Find(acc => true).ToList();
        }

        /// <summary>
        /// Saves a todo in db
        /// </summary>
        /// <param name="newTodo"></param>
        public void SaveTodo(Todo newTodo)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            collection.InsertOne(newTodo);
        }

        internal List<Todo> GetTodosForThisUser(string user)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(tds => tds.User == user).ToList();
        }

        /// <summary>
        /// Edits a todo with new values as parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="priority"></param>
        public void EditTodo(ObjectId id, string title, string description, string date, string priority)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);

            var filter = Builders<Todo>.Filter.Eq("Id", id);

            var updateName = Builders<Todo>.Update
                .Set("Title", title)
                .Set("Description", description)
                .Set("Date", date)
                .Set("Priority", priority);

            collection.UpdateOne(filter, updateName);
        }

        internal void UpdatePremium(string user)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);

            var filter = Builders<Account>.Filter.Eq("User", user);

            var updatePremium = Builders<Account>.Update
                .Set("IsPremium", true);

            collection.UpdateOne(filter, updatePremium);
        }

        internal Account GetAccount(string user)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            return collection.Find(acc => acc.User == user).FirstOrDefault();
        }

        internal void CreateAccount(Account account)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            collection.InsertOne(account);
        }

        internal List<Todo> GetTodosWithDate(string userName, DateTime date)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(td => td.User == userName && td.Date == date).ToList();
        }

        /// <summary>
        /// Filters todos after priority value 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns>A list of todos with same priority</returns>
        internal List<Todo> FilterTodos(string userName, string priority)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(td => td.User == userName && td.Priority == priority).ToList();
        }

        /// <summary>
        /// Gets a todo which matchs the id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An object as a todo</returns>
        internal Todo GetTodoById(ObjectId id)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(td => td.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Deletes a todo which matchs the id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTodo(ObjectId id)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            collection.DeleteOne(td => td.Id == id);
        }
    }
}
