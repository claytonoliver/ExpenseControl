namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Entidade que representa uma pessoa no sistema de controle de gastos.
/// Uma pessoa pode ter múltiplas transações associadas.
/// </summary>
public class Person : Entity
{
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Idade da pessoa em anos.
    /// </summary>
    public int Age { get; private set; }

    /// <summary>
    /// Coleção de transações associadas a esta pessoa.
    /// </summary>
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    // Construtor para EF Core
    private Person() => Name = null!;

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    /// <param name="name">Nome da pessoa (obrigatório).</param>
    /// <param name="age">Idade da pessoa (deve ser positiva).</param>
    public Person(string name, int age)
    {
        ValidateName(name);
        ValidateAge(age);

        Name = name;
        Age = age;
    }

    /// <summary>
    /// Atualiza os dados da pessoa.
    /// </summary>
    public void Update(string name, int age)
    {
        ValidateName(name);
        ValidateAge(age);

        Name = name;
        Age = age;
        SetUpdatedAt();
    }

    /// <summary>
    /// Verifica se a pessoa é menor de idade (menos de 18 anos).
    /// Menores de idade só podem ter transações do tipo despesa.
    /// </summary>
    public bool IsMinor() => Age < 18;

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da pessoa é obrigatório.", nameof(name));
    }

    private static void ValidateAge(int age)
    {
        if (age <= 0)
            throw new ArgumentException("A idade deve ser um número inteiro positivo.", nameof(age));
    }
}
