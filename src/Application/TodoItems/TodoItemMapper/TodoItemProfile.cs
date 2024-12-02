using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TodoItems.TodoItemMapper;
public class TodoItemProfile : Profile
{
    public TodoItemProfile()
    {
        CreateMap<CreateTodoItemCommand, TodoItem>()
            .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.ListId));
    }
}
