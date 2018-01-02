using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ProjectName_DAL.Model;
using Newtonsoft;

namespace Web_RabbitMQ.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GameAssignmentModel currGameAssignmentModel = new GameAssignmentModel
            {
                GameId = 1,
                Message = "Message has been sended."
            };
            AMQP_Web.Helper.GameSuggestionHelper.SendGameSuggestionListAsync(currGameAssignmentModel.GameId.ToString(), currGameAssignmentModel);
            return View();
        }

        
    }
}