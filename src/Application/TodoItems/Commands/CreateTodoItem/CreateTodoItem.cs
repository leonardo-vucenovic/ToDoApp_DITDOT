using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }
    public string? Title { get; init; }
    public string? Note { get; init; }
    public PriorityLevel Priority { get; set; }
    public DateTime? Reminder { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists.FindAsync(request.ListId, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoList), request.ListId.ToString());
        var todoItem = _mapper.Map<TodoItem>(request);
        await _context.TodoItems.AddAsync(todoItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return todoItem.Id;
    }
}
