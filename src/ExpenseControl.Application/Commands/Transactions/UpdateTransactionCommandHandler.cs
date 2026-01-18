using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Handler para atualização de transação.
/// </summary>
public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<TransactionDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (transaction is null)
            return Result.Failure<TransactionDto>("Transação não encontrada.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure<TransactionDto>("Categoria não encontrada.");

        try
        {
            transaction.Update(
                request.Description,
                request.Value,
                request.Type,
                request.CategoryId,
                category);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<TransactionDto>(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<TransactionDto>(ex.Message);
        }

        await _transactionRepository.SaveChangesAsync(cancellationToken);

        var typeDescription = request.Type == TransactionType.Expense ? "Despesa" : "Receita";

        var dto = new TransactionDto(
            transaction.Id,
            transaction.Description,
            transaction.Value,
            transaction.Type,
            typeDescription,
            transaction.CategoryId,
            category.Description,
            transaction.PersonId,
            transaction.Person.Name,
            transaction.CreatedAt);

        return Result.Success(dto);
    }
}
