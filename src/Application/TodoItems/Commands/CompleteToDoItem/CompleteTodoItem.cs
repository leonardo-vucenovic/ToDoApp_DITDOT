
using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.CompleteToDoItem;
public record class CompleteTodoItemCommand : IRequest<Unit>
{
    public List<int> ItemIds { get; init; } = new List<int>();
}

public class CompleteToDoIdemCommand : IRequestHandler<CompleteTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CompleteToDoIdemCommand(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        if (request.ItemIds != null && request.ItemIds.Any())
        {
            var itemsToComplete = _context.TodoItems
                .Where(item => request.ItemIds.Contains(item.Id))
                .ToList();
            foreach (var item in itemsToComplete) { item.Done = true; }
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}
