using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo1WebApp.Models
{
    public class Payment
    {
        public string CardNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string NameOnCard { get; set; }
    }
}
