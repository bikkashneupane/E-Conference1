using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using EConference.Models.ViewModels;
using EConference.Utility;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Conference_Manager)]

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
            return View("Details", _unitOfWork.Conferences.Get(id));
        }

        public IActionResult Delete(int id)
        {
            List<Papers> updatedPapers = _unitOfWork.Papers.GetAll().Where(p => p.ConferenceID == _unitOfWork.Conferences.Get(id).Name.Id).ToList();

            foreach (Papers p in updatedPapers)
            {
                p.ConferenceName = null;
                _unitOfWork.Papers.Update(p);
            }

            _unitOfWork.Conferences.Remove(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Schedule()
        {
            List<List<Papers>> lists = PaperSorter.GroupPapers(_unitOfWork.Papers.GetAll().Where(p => !_unitOfWork.Conferences.GetAll().SelectMany(c => c.Papers).Contains(p)).ToList());

            TempData["sorted"] = JsonConvert.SerializeObject(lists);

            return View(new SchedulingViewModel { Groups = lists, Names = _unitOfWork.ConferenceName.GetAll().ToList() });
        }

        [HttpPost]
        public IActionResult Schedule(List<int> groupName, List<string> groupTime, List<string> groupDate)
        {
            List<List<Papers>> lists = JsonConvert.DeserializeObject<List<List<Papers>>>(TempData["sorted"].ToString());

            List<Conference> toSchedule = new List<Conference>();

            for (int i = 0; i < groupName.Count; i++)
            {
                if (!(groupName?[i] == 0 || groupTime?[i].Length == 0 || groupDate?[i].Length == 0))
                {
                    foreach (Papers p in lists[i])
                    {
                        p.ConferenceName = _unitOfWork.ConferenceName.Get(groupName[i]);
                        _unitOfWork.Papers.Update(p);
                    }
                    toSchedule.Add(new Conference()
                    {
                        Name = _unitOfWork.ConferenceName.Get(groupName[i]),
                        Papers = _unitOfWork.Papers.GetAll().Where(p => lists[i].Select(p => p.Id).Contains(p.Id)).ToList(),
                        ScheduledDate = DateTime.Parse(groupDate[i] + " " + groupTime[i]).ToString()
                    });
                }
            }

            foreach (Conference c in toSchedule)
            {
                _unitOfWork.Conferences.Add(c);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
