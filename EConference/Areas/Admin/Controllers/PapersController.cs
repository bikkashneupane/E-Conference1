using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using EConference.Models.ViewModels;
using EConference.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EConference.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class PapersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        //private object allObj;

        public PapersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            if (User.IsInRole(SD.Role_Author))
            {
                return View("Index", _unitOfWork.Papers.GetAll(filter: p => p.UserId == currentUserId, includeProperties: "ConferenceName"));
            }
            else
            {
                return View("Index", _unitOfWork.Papers.GetAll(includeProperties: "ConferenceName"));
            }
        }

        public IActionResult Upsert(int? id)
        {
            PaperVM paperVM = new PaperVM()
            {
                Papers = new Papers(),
                ConferenceList = _unitOfWork.ConferenceName.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TopicList = PaperTopic.TopicList.Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.ID.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(paperVM);
            }
            //this is for edit
            paperVM.Papers = _unitOfWork.Papers.Get(id.GetValueOrDefault());
            if (paperVM.Papers == null)
            {
                return NotFound();
            }
            return View(paperVM);

        }

        public IActionResult Delete(int id)
        {
            _unitOfWork.Papers.Remove(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PaperVM paperVM)
        {
            if (ModelState.IsValid)
            {
                paperVM.Papers.UserId = _userManager.GetUserId(User);
                paperVM.Papers.Publisher = paperVM.Papers.Publisher.ToUpper();

                if (paperVM.Papers.Id == 0)
                {
                    _unitOfWork.Papers.Add(paperVM.Papers);
                }
                else
                {
                    _unitOfWork.Papers.Update(paperVM.Papers);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                paperVM.ConferenceList = _unitOfWork.ConferenceName.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
               
                if (paperVM.Papers.Id != 0)
                {
                    paperVM.Papers = _unitOfWork.Papers.Get(paperVM.Papers.Id);
                }
            }
            return View(paperVM);
        }

        //Not Using the API Call
        //#region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var currentUserId = _userManager.GetUserId(User);
        //    if (User.IsInRole(SD.Role_Author))
        //    {
        //        allObj = _unitOfWork.Papers.GetAll(filter: p => p.UserId == currentUserId, includeProperties: "ConferenceName");
        //    }
        //    else
        //    {
        //        allObj = _unitOfWork.Papers.GetAll(includeProperties: "ConferenceName");
        //    }
        //    return Json(new { data = allObj });
        //}

        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var objFromDb = _unitOfWork.Papers.Get(id);
        //    if (objFromDb == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }
        //    _unitOfWork.Papers.Remove(objFromDb);
        //    _unitOfWork.Save();
        //    return Json(new { success = true, message = "Delete Successful" });

        //}

        //#endregion
    }
}