using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.DeleteToDoItem;
public record class DeleteTodoItemCommand : IRequest<Unit>
{
    public List<int> ItemIds { get; init; } = new List<int>();
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public DeleteTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        if (request.ItemIds != null && request.ItemIds.Any())
        {
            var itemsToDelete = _context.TodoItems
                .Where(item => request.ItemIds.Contains(item.Id))
                .ToList();

            _context.TodoItems.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}
