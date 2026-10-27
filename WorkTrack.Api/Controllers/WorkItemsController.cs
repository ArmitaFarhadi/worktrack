using Microsoft.AspNetCore.Mvc;
using WorkTrack.Api.Dtos;
using WorkTrack.Domain.Contracts;
using WorkTrack.Domain.Dtos;
using WorkTrack.Domain.Entities;
using WorkTrack.Domain.Enums;

namespace WorkTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkItemsController : ControllerBase
{
    private readonly IWorkItemStore _store;
    private readonly ILogger<WorkItemsController> _logger;

    public WorkItemsController(IWorkItemStore store, ILogger<WorkItemsController> logger)
    {
        _store = store;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Priority? priority,
        [FromQuery] WorkItemStatus? status,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        if (page < 1)
        {

            ModelState.AddModelError(
                nameof(page),
                "Page must be greater than or equal to 1.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            ModelState.AddModelError(
                nameof(pageSize),
                "PageSize must be between 1 and 100.");
        }

        if (!string.IsNullOrWhiteSpace(sort) &&
            !string.Equals(
                sort,
                "dueDate",
                StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(
            nameof(sort),
            "Sort must be 'dueDate'.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var items = await _store.GetAllAsync(priority, status, sort, page, pageSize);

        var response = items.Items.Select(ToResponse).ToList();

        return Ok(new
        {
            page,
            pageSize,
            totalCount = items.TotalCount,
            items = response
        });
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
    public async Task<IActionResult> Create(
     CreateWorkItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            ModelState.AddModelError(
                nameof(request.Title),
                "Title is required.");
        }

        // API-level priority validation
        if (!Enum.IsDefined(request.Priority))
        {
            ModelState.AddModelError(
                nameof(request.Priority),
                "Invalid priority value.");
        }

        if (request.DueDate <= DateTime.UtcNow)
        {
            ModelState.AddModelError(
                nameof(request.DueDate),
                "Due date must be in the future.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var item = new WorkItem(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate);

        await _store.AddAsync(item);
        _logger.LogInformation(
        "Work item {WorkItemId} created with priority {Priority}",
        item.Id,
        item.Priority);

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
            item.ClosedAt,
            item.DueDate);
    }

    [HttpGet("high-priority")]
    public async Task<IActionResult> GetHighPriority()
    {
        var items = await _store.GetAllAsync(Priority.High);

        var response = items.Items
            .Select(ToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpPut("{id:guid}/priority")]
    public async Task<IActionResult> UpdatePriority(
        Guid id,
        UpdateWorkItemPriorityRequest request)
    {
        if (!request.Priority.HasValue)
        {
            ModelState.AddModelError(nameof(request.Priority),
                "Priority is required.");
            return ValidationProblem(ModelState);
        }
        // 1. Validate the requested priority

        // Check 2: Is priority a defined enum value?
        if (!Enum.IsDefined(request.Priority.Value))
        {
            ModelState.AddModelError(
                nameof(request.Priority),
                "Invalid priority value.");

            return ValidationProblem(ModelState);
        }

        // 2. Retrieve the WorkItem for updating
        var item = await _store.GetForUpdateAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        // 3. Return NotFound() if it doesn't exist

        // 4. Check whether the WorkItem is closed
        if (item.Status == WorkItemStatus.Closed)
        {
            return Conflict(
            "Cannot change priority of a closed work item.");
        }

        // 5. Call ChangePriority()
        item.ChangePriority(request.Priority.Value);

        // 6. Save changes
        await _store.SaveChangesAsync();
        return NoContent();

        // 7. Return NoContent()
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _store.GetForUpdateAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _store.Remove(item);

        await _store.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var item = await _store.GetForUpdateAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        if (item.Status == WorkItemStatus.Closed)
        {
            return Conflict("Work item is already closed.");
        }

        item.Close();

        await _store.SaveChangesAsync();

        return NoContent();
    }
}
