using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoAPI.Models;


namespace TodoAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem {Name = "Item 1"});
                _context.SaveChanges();
            }
        }


        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.OrderBy(a => a.Id).ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }


        [HttpPut]
        [Route("{id}")]
        public ActionResult<TodoItem> UpdateById(long id, TodoItem todoItem)
        {
            var item = _context.TodoItems.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = todoItem.Name;
            item.IsComplete = todoItem.IsComplete;
            
            _context.Update(item);
            _context.SaveChanges();

            return item;
        }


        [HttpPut]
        [Route("{id}/complete")]
        public ActionResult<TodoItem> CompleteTodo(long id)
        {
            var item = _context.TodoItems.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.IsComplete = true;
            _context.TodoItems.Update(item);
            _context.SaveChanges();
            return item;
        }
        
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(long id)
        {
            var item = _context.TodoItems.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }

        
        [HttpPost]
        public ActionResult<TodoItem> Post(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            return todoItem;
        }
        
    }
}