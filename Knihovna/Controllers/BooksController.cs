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

		public async Task<IActionResult> Index()
		{
			var allBooks = await _bookService.GetAllAsync();
			return View(allBooks);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task< IActionResult> CreateAsync(BookDto newBookDto)
		{
			_bookService.CreateAsync(newBookDto);
			return Redirect("Index");
		}
	 
		public async Task<IActionResult> EditAsync(int id)
		{
			var bookToEdit = await _bookService.GetByIdAsync(id);
			if (bookToEdit == null)
			{
				return View("NotFound");
			}
			return View(bookToEdit);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateAsync(BookDto bookDtoToEdit)
		{
			await _bookService.EditAsync(bookDtoToEdit);
			return Redirect("Index");
		}
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
	}
}
