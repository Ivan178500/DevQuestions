using DevQuestion.Contracts.Questions;
using FluentValidation;

namespace DevQuestions.Application.Questions;

public class CreateAuestionValidator:AbstractValidator<CreateQuestionDto>
{
    public CreateAuestionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500).WithMessage("Заголовок невалидный.");

        RuleFor(x => x.Text).MaximumLength(5000).WithMessage("Текст невалидный");

        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.TagIds).NotEmpty();
    }
}