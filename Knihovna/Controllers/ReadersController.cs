using Knihovna.DTO;
using Knihovna.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
    public class ReadersController : Controller
    {
        private ReaderService _readerService;

        public ReadersController(ReaderService readerService)
        {
            _readerService = readerService;
        }


        //*******************************
        //********* INDEX  ************
        //*******************************
        public async Task<IActionResult> Index()
        {
            var allreaders = await _readerService.GetAllAsync();
            return View(allreaders);
        }
        //*******************************
        //********* CREATE START ************
        //*******************************
        public IActionResult Create()
        {
            return View();
        }
        //*******************************
        //********* CREATE END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ReaderDto readerDto)
        {
            await _readerService.CreateAsync(readerDto);
            return Redirect("Index");
        }
        //*******************************
        //********* EDIT START  ************
        //*******************************
        public async Task<IActionResult> EditAsync(int id)
        {
            var readerToEdit = await _readerService.GetByIdAsync(id);
            if (readerToEdit == null)
            {
                return View("NotFound");
            }
            return View(readerToEdit);
        }
        //*******************************
        //********* EDIT END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(ReaderDto readerDto)
        {
            await _readerService.EditAsync(readerDto);
            return Redirect("Index");
        }
        //*******************************
        //********* DELETE  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ReaderDto readerToDelete = await _readerService.GetByIdAsync(id);
            if (readerToDelete == null)
            {
                return View("NotFound");
            }
            await _readerService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}