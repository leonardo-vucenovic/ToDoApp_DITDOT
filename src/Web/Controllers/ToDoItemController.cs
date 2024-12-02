using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using ToDoApp.Application.TodoItems.Commands.DeleteToDoItem;
using ToDoApp.Application.TodoItems.Commands.CompleteToDoItem;

namespace ToDoApp.Web.Controllers;

public class ToDoItemController : Controller
{
    private readonly ISender _sender;

    public ToDoItemController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(command, cancellationToken);
            return RedirectToPage("/Index");
        }
        catch (ValidationException ex)
        {
            var errorMessages = ex.Errors.SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}")).ToList();
            TempData["Errors"] = string.Join("<br/>", errorMessages);
            return RedirectToPage("/Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Delete(List<int> itemsId)
    {
        if (itemsId == null || !itemsId.Any())
        {
            TempData["Errors"] = "No items selected for deletion.";
            return RedirectToPage("/Index");
        }
        var command = new DeleteTodoItemCommand { ItemIds = itemsId };
        await _sender.Send(command);
        return RedirectToPage("/Index");
    }
    [HttpPost]
    public async Task<IActionResult> Complete(List<int> itemsId)
    {
        if (itemsId == null || !itemsId.Any())
        {
            TempData["Errors"] = "There are no items selected to mark as complete.";
            return RedirectToPage("/Index");
        }
        var command = new CompleteTodoItemCommand { ItemIds = itemsId };
        await _sender.Send(command);
        return RedirectToPage("/Index");
    }
}
