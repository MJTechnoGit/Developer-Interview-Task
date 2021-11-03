using InterviewTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTask.Services
{
    public interface IWeatherService
    {
        Task<string> GetWeatherDetail(string areaID);
    }
}
