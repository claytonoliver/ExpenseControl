using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Handler para criação de transação.
/// Valida regras de negócio:
/// - Categoria deve existir e ser compatível com o tipo de transação.
/// - Pessoa deve existir.
/// - Menores de idade só podem ter despesas.
/// </summary>
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPersonRepository _personRepository;

    public CreateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        IPersonRepository personRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _personRepository = personRepository;
    }

    public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Busca a categoria
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            return Result.Failure<TransactionDto>("Categoria não encontrada.");

        // Busca a pessoa
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
            return Result.Failure<TransactionDto>("Pessoa não encontrada.");

        try
        {
            // Cria a transação (validações no construtor incluem regras de negócio)
            var transaction = new Transaction(
                request.Description,
                request.Value,
                request.Type,
                request.CategoryId,
                request.PersonId,
                category,
                person);

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _transactionRepository.SaveChangesAsync(cancellationToken);

            // Retorna o DTO com os dados da transação criada
            var dto = new TransactionDto(
                transaction.Id,
                transaction.Description,
                transaction.Value,
                transaction.Type,
                GetTypeDescription(transaction.Type),
                transaction.CategoryId,
                category.Description,
                transaction.PersonId,
                person.Name,
                transaction.CreatedAt);

            return Result.Success(dto);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<TransactionDto>(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<TransactionDto>(ex.Message);
        }
    }

    /// <summary>
    /// Converte o enum para descrição legível.
    /// </summary>
    private static string GetTypeDescription(TransactionType type)
    {
        return type switch
        {
            TransactionType.Expense => "Despesa",
            TransactionType.Income => "Receita",
            _ => type.ToString()
        };
    }
}
