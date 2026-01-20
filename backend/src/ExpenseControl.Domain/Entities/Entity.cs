namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Classe base abstrata para todas as entidades do domínio.
/// Contém propriedades comuns de identificação e auditoria.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Identificador único da entidade, gerado automaticamente.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Data e hora da última atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    /// <summary>
    /// Atualiza o timestamp de modificação.
    /// </summary>
    protected void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
