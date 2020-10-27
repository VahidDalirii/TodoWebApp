using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using ToDoWebApp.Models;

namespace ToDoWebApp
{
    public class Database
    {
        private const string TODO_COLLECTION = "Todos";
        private const string ACCOUNT_COLLECTION = "Accounts";
        private readonly IMongoDatabase db;

        public Database(string dbName = "ToDoWebAppDB")
        {
            MongoClient client = new MongoClient("mongodb+srv://Vahid:Vahid26112@cluster0.yhcpv.mongodb.net/ToDoWebAppDB?retryWrites=true&w=majority");
            db = client.GetDatabase(dbName);
        }
        
        internal bool CheckIdAccountExists(Account account)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            List<Account> accounts = collection.Find(acc => true).ToList();
            foreach (var acc in accounts)
            {
                if (acc.UserName.ToLower() == account.UserName.ToLower() && acc.Password.ToLower() == account.Password.ToLower())
                    return true;
            }
            return false;
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
            return collection.Find(tds => tds.User.ToLower() == user.ToLower()).ToList();
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

            var filter = Builders<Account>.Filter.Eq("UserName", user);

            var updatePremium = Builders<Account>.Update
                .Set("IsPremium", true);

            collection.UpdateOne(filter, updatePremium);
        }

        internal Account GetAccount(string user)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            return collection.Find(acc => acc.UserName.ToLower() == user.ToLower()).FirstOrDefault();
        }

        internal void CreateAccount(Account account)
        {
            var collection = db.GetCollection<Account>(ACCOUNT_COLLECTION);
            collection.InsertOne(account);
        }

        internal List<Todo> GetTodosWithDate(string userName, DateTime date)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(td => td.User.ToLower() == userName.ToLower() && td.Date == date).ToList();
        }

        /// <summary>
        /// Filters todos after priority value 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns>A list of todos with same priority</returns>
        internal List<Todo> FilterTodos(string userName, string priority)
        {
            var collection = db.GetCollection<Todo>(TODO_COLLECTION);
            return collection.Find(td => td.User.ToLower() == userName.ToLower() && td.Priority == priority).ToList();
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
