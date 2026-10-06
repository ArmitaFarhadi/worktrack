using Microsoft.AspNetCore.Mvc;
using WorkTrack.Api.Dtos;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;

namespace WorkTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkItemsController : ControllerBase
{
    private readonly IWorkItemStore _store;

    public WorkItemsController(IWorkItemStore store)
    {
        _store = store;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _store.GetAllAsync();

        var response = items
            .Select(ToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _store.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(item));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkItemRequest request)
    {
        var item = new WorkItem(
            request.Title,
            request.Description,
            request.Priority);

        await _store.AddAsync(item);

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id },
            ToResponse(item));
    }

    private static WorkItemResponse ToResponse(WorkItem item)
    {
        return new WorkItemResponse(
            item.Id,
            item.Title,
            item.Description,
            item.Status,
            item.Priority,
            item.AssignedTo?.Id,
            item.CreatedAt,
            item.ClosedAt);
    }
}