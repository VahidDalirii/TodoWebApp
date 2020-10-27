using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoWebApp.Models
{
    public class Payment
    {
        [StringLength(16, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }
        [DataType(DataType.Text)]
        public string NameOnCard { get; set; }
    }
}
