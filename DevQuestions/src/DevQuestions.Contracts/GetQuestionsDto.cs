namespace DevQuestion.Contracts;

public record GetQuestionsDto(string Search, Guid[] TagIds);