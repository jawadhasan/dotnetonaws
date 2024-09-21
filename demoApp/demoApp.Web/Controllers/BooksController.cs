using System.Threading.Tasks;
using demoApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository _bookRepo;

        public BooksController(BookRepository bookRepository)
        {
            _bookRepo = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _bookRepo.GetBooks();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = _bookRepo.GetById(id);
            return Ok(book);

        }

        [HttpGet("getwithauthors")]
        public async Task<IActionResult> GetWithAuthors()
        {
            var data = await _bookRepo.GetBooksWithAuthorAsync();
            return Ok(data);
        }

        //[HttpGet("getwithauthorIds")]
        //public async Task<IActionResult> GetWithAuthorIds()
        //{
        //    var data = await _bookRepo.GetBookWithAuthorIdsAsync();
        //    return Ok(data);
        //}

        [HttpGet("author/{id}")]
        public async Task<IActionResult> Author(int id)
        {
            var data = await _bookRepo.GetAuthor(id);
            return Ok(data);
        }


        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _bookRepo.GetCategories();
            return Ok(data);
        }

        [HttpGet("getByCategoryId")]
        public async Task<IActionResult> GetByCategories(int catId)
        {
            var data = await _bookRepo.GetBooksByCategory(catId);
            return Ok(data);
        }




    }
}
