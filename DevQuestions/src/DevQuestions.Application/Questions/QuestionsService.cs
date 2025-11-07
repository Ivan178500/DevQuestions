using DevQuestion.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // валидация входных данных
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        // валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            throw new Exception("Too many open questions");
        }
        
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);

        await _questionsRepository.AddAsync(question, cancellationToken);

        _logger.LogInformation($"Question created with id {questionId}", questionId);
        
        return questionId;
    }


    /*public async Task<IActionResult> Update(
        Guid questionId,
        UpdateQuestionDto request,
        CancellationToken question)
    {
    }

    public async Task<IActionResult> Delete(Guid questionId, CancellationToken question)
    {
    }

    public async Task<IActionResult> SelectSolution(
        Guid questionId,
        Guid answerId,
        CancellationToken question)
    {
    }

    public async Task<IActionResult> AddAnswer(
        Guid questionId,
        AddAnswerDto request,
        CancellationToken question)
    {
    }*/
}