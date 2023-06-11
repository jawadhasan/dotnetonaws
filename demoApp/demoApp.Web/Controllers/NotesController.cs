using demoApp.Core;
using demoApp.Data;
using demoApp.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesRepository _notesRepository;

        public NotesController(NotesRepository notesRepository)
        {
                _notesRepository = notesRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _notesRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var note = _notesRepository.GetById(id);
            return Ok(note);

        }


        [HttpPost("add")]
        public IActionResult Add([FromBody] NoteDto noteDto)
        {

            var note = new Note();
            note.Title = noteDto.Title;
            note.Content = noteDto.Content;
            note.Details = noteDto.Details;
            note.CategoryId = noteDto.CategoryId;
            note.UserId = noteDto.UserId; //todo: get from context

            var insertedNote = _notesRepository.Insert(note);

            return Ok(insertedNote);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] NoteDto noteDto)
        {

            var editableNote = _notesRepository.GetById(id);

            //TODO: Validation

            editableNote.Title = noteDto.Title;
            editableNote.Content = noteDto.Content;
            editableNote.Details = noteDto.Details;
            editableNote.CategoryId = noteDto.CategoryId;

            var updatedNote = _notesRepository.Update(editableNote); //can return editableNoted instead?

            return Ok(updatedNote);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _notesRepository.RemoveById(id);
            return Ok();
        }
    }
}
