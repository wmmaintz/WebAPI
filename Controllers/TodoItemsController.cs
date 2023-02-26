using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

		/// <summary>
		/// GET: api/TodoItems
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

		/// <summary>
		/// GET: api/TodoItems/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

		/// <summary>
		/// PUT: api/TodoItems/5
		/// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/// </summary>
		/// <param name="id"></param>
		/// <param name="todoItem"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

			var todoItemDTO = await _context.TodoItems.FindAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			_context.Entry(todoItem).State = EntityState.Modified;
			todoItem.Name = todoItemDTO.Name;
			todoItem.IsComplete = todoItemDTO.IsComplete;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
			{
				return NotFound();
			}

			return NoContent();
        }

		/// <summary>
		/// POST: api/TodoItems
		/// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/// </summary>
		/// <param name="todoItemDTO"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
			var todoItem = new TodoItem
			{
				IsComplete = todoItemDTO.IsComplete,
				Name = todoItemDTO.Name
			};

			_context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
				ItemToDTO(todoItem));

		}

		/// <summary>
		/// DELETE: api/TodoItems/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
		}

		private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
			new TodoItemDTO
			{
				Id = todoItem.Id,
				Name = todoItem.Name,
				IsComplete = todoItem.IsComplete
			};
	}
}
