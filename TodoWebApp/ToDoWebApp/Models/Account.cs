using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoWebApp.Models
{
    public class Account
    {
        public ObjectId Id { get; set; }
        public string User { get; set; }
        public bool IsPremium { get; set; }
    }
}
