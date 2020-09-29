using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using EConference.Models.ViewModels;
using EConferenceSortingTest.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EConference.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ConferenceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /* Test data
        private List<Conference> db = new List<Conference>
        {
            new Conference { ID = 1, Name = new ConferenceName { Name = "TAS" }, ScheduledDate = DateTime.Now.ToString() },
            new Conference { ID = 2, Name = new ConferenceName { Name = "TAS" }, ScheduledDate = DateTime.Now.Subtract(TimeSpan.FromHours(5)).ToString() }
        };

        private List<Papers> paperList = new List<Papers>
        {
            new Papers { Id = 1, PaperId = "PAP1", PaperTopic = 1, PaperTitle = "Title of 1", Publisher = "Pub of 1", Participant = "John", TimeZone = TimeZoneInfo.Local.Id, Country = "Australia" },
            new Papers { Id = 2, PaperId = "PAP2", PaperTopic = 1, PaperTitle = "Title of 2", Publisher = "Pub of 2", Participant = "James", TimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC-02").Id, Country = "Australia" }
        };

        private List<ConferenceName> nameList = new List<ConferenceName>
        {
            new ConferenceName { Id = 1, Name = "TAB" },
            new ConferenceName { Id = 2, Name = "CQU" },
        };
        */

        public ConferenceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //return View("Index", db.OrderBy(c => c.ScheduledDate));
            return View("Index", _unitOfWork.Conferences.GetAll().OrderBy(c => c.ScheduledDate));
        }

        public IActionResult ViewDetails(int id)
        {
            return View("Details", _unitOfWork.Conferences.GetAll().Where(c => c.ID == id).First());
        }

        public IActionResult Delete(int id)
        {
            _unitOfWork.Conferences.Remove(id);
            return Index();
        }

        public IActionResult Schedule()
        {
            List<List<Papers>> lists = PaperSorter.GroupPapers(_unitOfWork.Papers.GetAll().Where(p => p.ConferenceName == null).ToList());

            TempData["sorted"] = JsonConvert.SerializeObject(lists);

            return View(new SchedulingViewModel { Groups = lists, Names = _unitOfWork.ConferenceName.GetAll().ToList() });
        }

        [HttpPost]
        public IActionResult Schedule(List<int> groupName, List<string> groupTime, List<string> groupDate)
        {
            List<List<Papers>> lists = JsonConvert.DeserializeObject<List<List<Papers>>>(TempData["sorted"].ToString());

            List<Conference> toSchedule = new List<Conference>();

            for(int i = 0; i < lists.Count; i++)
            {
                if (!(groupName[i] == 0 || groupTime[i].Length == 0 || groupDate[i].Length == 0))
                {
                    toSchedule.Add(new Conference()
                    {
                        Name = _unitOfWork.ConferenceName.GetAll().Where(cn => cn.Id == groupName[i]).FirstOrDefault(),
                        Papers = lists[i],
                        ScheduledDate = DateTime.Parse(groupDate[i] + " " + groupTime[i]).ToString()
                    });;
                }
            }

            foreach (Conference c in toSchedule)
                _unitOfWork.Conferences.Add(c);

            _unitOfWork.Save();

            //return View("Index", toSchedule.OrderBy(c => c.ScheduledDate));
            return Index();
        }
    }
}
