using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Queries.Persons;

/// <summary>
/// Handler para consulta de totais por pessoa.
/// Calcula total de receitas, despesas e saldo para cada pessoa,
/// além do total geral.
/// </summary>
public class GetPersonTotalsQueryHandler : IRequestHandler<GetPersonTotalsQuery, PersonTotalsReportDto>
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetPersonTotalsQueryHandler(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<PersonTotalsReportDto> Handle(GetPersonTotalsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _personRepository.GetAllAsync(cancellationToken);
        var allTransactions = await _transactionRepository.GetAllAsync(cancellationToken);

        var personTotals = new List<PersonTotalDto>();

        // Calcula totais para cada pessoa
        foreach (var person in persons)
        {
            var personTransactions = allTransactions.Where(t => t.PersonId == person.Id);

            var totalIncome = personTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value);

            var totalExpense = personTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value);

            var balance = totalIncome - totalExpense;

            personTotals.Add(new PersonTotalDto(
                person.Id,
                person.Name,
                totalIncome,
                totalExpense,
                balance));
        }

        // Calcula totais gerais
        var grandTotalIncome = personTotals.Sum(p => p.TotalIncome);
        var grandTotalExpense = personTotals.Sum(p => p.TotalExpense);
        var grandTotalBalance = grandTotalIncome - grandTotalExpense;

        return new PersonTotalsReportDto(
            personTotals,
            grandTotalIncome,
            grandTotalExpense,
            grandTotalBalance);
    }
}
