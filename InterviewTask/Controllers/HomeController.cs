using System.Web.Mvc;
using InterviewTask.Models;
using InterviewTask.Services;
using System.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;
using NLog;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Text;

namespace InterviewTask.Controllers
{
    public class HomeController : Controller
    {
        /*
         * Prepare your opening times here using the provided HelperServiceRepository class.       
         */      


        private readonly IHelperServiceRepository _helperServiceRepository;
        private readonly IWeatherService _weatherService;
        private readonly ILogger _logger;

        public HomeController(IHelperServiceRepository helperServiceRepository, IWeatherService weatherService, ILogger logger)
        {
            this._helperServiceRepository = helperServiceRepository;
            this._weatherService = weatherService;
            this._logger = logger;
        }


        public ActionResult Index()
        {

            _logger.Info("Retrieving Helper services");

            var weekdayOpeningTimes = new List<int> { 9, 17 };
            var alternativeOpeningTime = new List<int> { 8, 12 };
            var weekendOpeningTimes = new List<int> { 10, 12 };
            var closedTimes = new List<int> { 0, 0 };


            var helperServiceList = _helperServiceRepository.Get();

            // Work out whether each of the helper service centers are open or closed on a given day.
            foreach (var helperService in helperServiceList)
            {
                helperService.IsOpen = false;

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:

                        if (Enumerable.SequenceEqual(helperService.MondayOpeningHours, weekdayOpeningTimes))
                        {                            
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {                               
                                helperService.IsOpen = true;
                            }

                        }
                        else if (Enumerable.SequenceEqual(helperService.MondayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >=  alternativeOpeningTime[0] && DateTime.Now.Hour <= alternativeOpeningTime[1])
                            {
                                helperService.IsOpen = true;
                            }
                        }
                        else if (Enumerable.SequenceEqual(helperService.MondayOpeningHours, closedTimes))
                        {
                            helperService.IsOpen = false;                          
                        }
                        
                        break;

                    case DayOfWeek.Tuesday:

                        if (Enumerable.SequenceEqual(helperService.TuesdayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }    
                        }
                        else if (Enumerable.SequenceEqual(helperService.TuesdayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }                           
                        }
                        else if (Enumerable.SequenceEqual(helperService.TuesdayOpeningHours, closedTimes))
                        {
                            helperService.IsOpen = false;
                        }

                        break;                        

                    case DayOfWeek.Wednesday:

                        if (Enumerable.SequenceEqual(helperService.WednesdayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }                            
                        }
                        else if (Enumerable.SequenceEqual(helperService.WednesdayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }                           
                        }
                        
                        break;

                    case DayOfWeek.Thursday:

                        if (Enumerable.SequenceEqual(helperService.ThursdayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }                            
                        }
                        else if (Enumerable.SequenceEqual(helperService.ThursdayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= alternativeOpeningTime[0] && DateTime.Now.Hour <= alternativeOpeningTime[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = alternativeOpeningTime[1];
                            }                           
                        }

                        break;

                    case DayOfWeek.Friday:

                        if (Enumerable.SequenceEqual(helperService.FridayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }                           
                        }
                        else if (Enumerable.SequenceEqual(helperService.FridayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= alternativeOpeningTime[0] && DateTime.Now.Hour <= alternativeOpeningTime[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = alternativeOpeningTime[1];
                            }                            
                        }

                        break;

                    case DayOfWeek.Saturday:

                        if (Enumerable.SequenceEqual(helperService.SaturdayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];

                            }                           
                        }
                        else if (Enumerable.SequenceEqual(helperService.SaturdayOpeningHours, weekendOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekendOpeningTimes[0] && DateTime.Now.Hour <= weekendOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekendOpeningTimes[1];
                            }                            
                        }
                        else if (Enumerable.SequenceEqual(helperService.SaturdayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= alternativeOpeningTime[0] && DateTime.Now.Hour <= alternativeOpeningTime[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = alternativeOpeningTime[1];
                            }
                        }

                        break;

                    case DayOfWeek.Sunday:

                        if (Enumerable.SequenceEqual(helperService.SundayOpeningHours, weekdayOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekdayOpeningTimes[0] && DateTime.Now.Hour <= weekdayOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekdayOpeningTimes[1];
                            }
                        }
                        else if (Enumerable.SequenceEqual(helperService.SundayOpeningHours, weekendOpeningTimes))
                        {
                            if (DateTime.Now.Hour >= weekendOpeningTimes[0] && DateTime.Now.Hour <= weekendOpeningTimes[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = weekendOpeningTimes[1];
                            }
                        }
                        else if (Enumerable.SequenceEqual(helperService.SundayOpeningHours, alternativeOpeningTime))
                        {
                            if (DateTime.Now.Hour >= alternativeOpeningTime[0] && DateTime.Now.Hour <= alternativeOpeningTime[1])
                            {
                                helperService.IsOpen = true;
                                helperService.ClosingTime = alternativeOpeningTime[1];
                            }
                        }

                        break;
                }
            }

            return View(helperServiceList);
        }

        [HttpPost]
        public ActionResult GetWeatherByCity(string AreaID)
        {
            _logger.Info("Getting weather information");

            var response = _weatherService.GetWeatherDetail(AreaID).Result;

            StringBuilder sb = new StringBuilder();
            sb.Append(response);

            sb.Replace("\\", "").Replace("{", "").Replace("}", "").Replace("[","").Replace("]","");

            var dataList = sb.ToString().Split(',');

            sb.Clear(); 

            var title = dataList[1].Split(':')[1].Replace("\\","").Replace("\"","");
            var description = dataList[2].Split(':')[1].Replace("\\", "").Replace("\"", "");                   

            return Json(new { Title = title, Description = description});
        }
    }
}