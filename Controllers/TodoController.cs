using Microsoft.AspNetCore.Mvc;
using TodoMongoMvc.Models;
using TodoMongoMvc.Services;

namespace TodoMongoMvc.Controllers;

public class TodoController : Controller
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

    public async Task<IActionResult> Index()
    {
        var todos = await _todoService.GetAllAsync();
        return View(todos);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem todo)
    {
        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        await _todoService.CreateAsync(todo);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        return View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, TodoItem todo)
    {
        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        await _todoService.UpdateAsync(id, todo);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        await _todoService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
