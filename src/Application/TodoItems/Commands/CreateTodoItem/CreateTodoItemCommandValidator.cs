namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Title cannot be empty or only whitespace.")
            .MaximumLength(200).WithMessage("Title must be at most 200 characters long.");
    }
}
