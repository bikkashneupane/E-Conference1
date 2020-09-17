using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EConference.DataAccess.Repository.IRepository;
using EConference.Models;
using EConference.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EConference.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PapersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PapersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PaperVM paperVM)
        {
            if (ModelState.IsValid)
            {
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


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Papers.GetAll(includeProperties: "ConferenceName");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Papers.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Papers.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}