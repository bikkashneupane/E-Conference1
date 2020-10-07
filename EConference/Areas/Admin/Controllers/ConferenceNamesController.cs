using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using EConference.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EConference.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Conference_Manager)]

    public class ConferenceNamesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConferenceNamesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ConferenceName ConferenceName = new ConferenceName();
            if (id == null)
            {
                //this is for create
                return View(ConferenceName);
            }
            //this is for edit
            ConferenceName = _unitOfWork.ConferenceName.Get(id.GetValueOrDefault());
            if (ConferenceName == null)
            {
                return NotFound();
            }
            return View(ConferenceName);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ConferenceName ConferenceName)
        {
            if (ModelState.IsValid)
            {
                if (ConferenceName.Id == 0)
                {
                    _unitOfWork.ConferenceName.Add(ConferenceName);

                }
                else
                {
                    _unitOfWork.ConferenceName.Update(ConferenceName);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(ConferenceName);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.ConferenceName.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.ConferenceName.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.ConferenceName.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}