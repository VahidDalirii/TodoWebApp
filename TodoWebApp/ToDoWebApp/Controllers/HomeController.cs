using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoWebApp.Models;

namespace ToDoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Helper _helper = new Helper();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets all todos from db and sorts them after high priority 
        /// </summary>
        /// <returns>A sorted list of todos</returns>
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var todos = _helper.GetTodosForThisUser(User.Identity.Name);


                return View(todos);
            }

            return View();
        }


        /// <summary>
        /// Lets user to sort or filter todos after priority
        /// </summary>
        /// <param name="submit"></param>
        /// <param name="priority"></param>
        /// <returns>A list of sorted or filtered todos</returns>
        [HttpPost]
        public IActionResult Index(string submit, string priority, DateTime date)
        {

            if (submit.Equals("Sort"))
            {
                return View(_helper.GetTodosByPriority(User.Identity.Name, priority));
            }
            else if (submit.Equals("Filter") && !string.IsNullOrEmpty(priority))
            {
                return View(_helper.GetFilteredTodos(User.Identity.Name, priority));
            }
            else if (submit.Equals("Show"))
            {
                return View(_helper.GetTodosWithDate(User.Identity.Name, date));
            }
            return Redirect("/Home");
        }

        /// <summary>
        /// Gets a todo by todo's id from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns an object as todo</returns>
        //public IActionResult Show(string id)
        //{
        //    Helper helper = new Helper();
        //    return View(helper.GetTodoById(id));
        //}

        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Lets user to create a todo
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="priority"></param>
        /// <returns>Redirects to todos list</returns>
        [HttpPost]
        public IActionResult Create(Todo todo)
        {
            if (todo.Date.Date < DateTime.Today)
            {
                TempData["textmsg"] = "<script>alert('You can not choose a day befor today.');</script>";
                return View();
            }
            var todos = _helper.GetTodosForThisUser(User.Identity.Name);
            if (todos.Count >= 20)
            {
                TempData["textmsg"] = "<script>alert('You have reached to 20 saved tasks. You have to get premium version if you want to save more.);</script>";
                return Redirect("/Home");
            }
            _helper.SaveTodo(todo);

            return Redirect("/Home");
        }

        /// <summary>
        /// Gets a todo by todo's id from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns an object as todo</returns>
        public IActionResult Edit(string id)
        {
            return View(_helper.GetTodoById(id));
        }

        /// <summary>
        /// Lets user edits a todo with new values and saves the new values in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="priority"></param>
        /// <returns>Returns the edited todo with new values</returns>
        [HttpPost]
        public IActionResult Edit(string id, string title, string description, string date, string priority)
        {
            if (DateTime.Parse(date).Date < DateTime.Today)
            {
                TempData["textmsg"] = "<script>alert('You can not choose a day befor today.');</script>";
                return View(_helper.GetTodoById(id));
            }

            _helper.EditTodo(id, title, description, date, priority);

            return Redirect($"/Home");
        }

        /// <summary>
        /// Shows todo and asks if user is sure to delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(string id)
        {
            return View(_helper.GetTodoById(id));
        }

        /// <summary>
        /// Finds todo by id in db and deletes it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to list of todos</returns>
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            _helper.DeleteTodoById(id);

            return Redirect("/Home");
        }

        /// <summary>
        /// Opens a page to get premium account
        /// </summary>
        /// <returns></returns>
        public IActionResult Premium()
        {
            
            if (User.Identity.IsAuthenticated )
            {
                Account account = _helper.GetAccount(User.Identity.Name);
                if (account.IsPremium == false)
                {
                    return View();
                }
                else
                {
                    TempData["textmsg"] = "<script>alert('You already have a premium account.');</script>";
                    return Redirect("/Home");
                }
            }
            else if(!User.Identity.IsAuthenticated)
            {
                TempData["textmsg"] = "<script>alert('You have to log in first. Log in if you have an account, otherwise register yourself.');</script>";
                return Redirect("/Home");
            }

            return Redirect("/Home");
        }

        /// <summary>
        /// Updates account to premium
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Premium(Payment payment)
        {
            if (payment.ExpiryDate<DateTime.Today)
            {
                TempData["textmsg"] = "<script>alert('You can not choose a day befor today.');</script>";
                return View();
            }
            if (payment.NameOnCard.Any(c => char.IsDigit(c)))
            {
                TempData["textmsg"] = "<script>alert('The name must contain only alpha characters.');</script>";
                return View();
            }
            _helper.UpdatePremium(User.Identity.Name);
            TempData["textmsg"] = "<script>alert('Congratulations! You are now a Premium user. Now you can use all To Do apps functions and create how much To Dos you want.');</script>";
            return Redirect("/Home");
        }

        /// <summary>
        /// Shows about page
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Shows Contact page
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string submit)
        {
            TempData["textmsg"] = "<script>alert('Your message sended successfully');</script>";
            return Redirect("/Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
