using Knihovna.Services;
using Microsoft.AspNetCore.Mvc;
using Knihovna.DTO;
namespace Knihovna.Controllers
{
	public class BooksController : Controller
	{
		private BookService _bookService;

		public BooksController(BookService bookService)
		{
			_bookService = bookService;
		}
		//*******************************
		//********* INDEX   ************
		//*******************************
		public async Task<IActionResult> Index()
		{
			var allBooks = await _bookService.GetAllAsync();
			return View(allBooks);
		}
		//*******************************
		//********* CREATE START  ************
		//*******************************
		public IActionResult Create()
		{
			return View();
		}
		//*******************************
		//********* CREATE END  ************
		//*******************************
		[HttpPost]
		public async Task< IActionResult> CreateAsync(BookDto newBookDto)
		{
			_bookService.CreateAsync(newBookDto);
			return Redirect("Index");
		}
		//*******************************
		//********* UPDATE START   ************
		//*******************************
		public async Task<IActionResult> EditAsync(int id)
		{
			var bookToEdit = await _bookService.GetByIdAsync(id);
			if (bookToEdit == null)
			{
				return View("NotFound");
			}
			return View(bookToEdit);
		}
		//*******************************
		//********* UPDATE END   ************
		//*******************************
		[HttpPost]
		public async Task<IActionResult> UpdateAsync(BookDto bookDtoToEdit)
		{
			await _bookService.EditAsync(bookDtoToEdit);
			return Redirect("Index");
		}
		//*******************************
		//********* DELETE  ************
		//*******************************
		[HttpPost]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			BookDto bookToDelete = await _bookService.GetByIdAsync(id);
			if (bookToDelete == null)
			{
				return View("NotFound");
			}
			await  _bookService.DeleteAsync(id);
			return RedirectToAction("Index");
		}
		//*******************************
		//********* REZERVATION   ************
		//*******************************
		[HttpPost]
		public async Task<IActionResult> RezervationAsync(int id)
		{
			BookDto bookToRezervation = await _bookService.GetByIdAsync(id);
			if(bookToRezervation == null)
			{
				return View("NotFound");
			}
			await _bookService.ReservationAsync(id);
			return RedirectToAction("Index");
		}		
		//*******************************
		//********* BORROW   ************
		//*******************************
		[HttpPost]
		public async Task<IActionResult> BorrowAsync(int id)
		{
			BookDto bookToBorrow= await _bookService.GetByIdAsync(id);
			if(bookToBorrow == null)
			{
				return View("NotFound");
			}
			await _bookService.BorrowAsync(id);
			return RedirectToAction("Index");
		}
	}
}
