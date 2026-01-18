import { useState, useMemo } from 'react';
import { useReports, usePersons, useCategories } from '../hooks';
import { Alert, Select } from '../components/ui';

/**
 * Formata valor como moeda brasileira.
 */
function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
}

/**
 * Retorna a classe CSS conforme o valor (positivo, negativo ou neutro).
 */
function getValueClass(value: number): string {
  if (value > 0) return 'value-positive';
  if (value < 0) return 'value-negative';
  return 'value-neutral';
}

/**
 * Página de relatórios.
 * Exibe totais de receitas, despesas e saldo por pessoa e categoria.
 */
export function ReportsPage() {
  const { personTotals, categoryTotals, loading, error, clearError } = useReports();
  const { persons } = usePersons();
  const { categories } = useCategories();

  // Filtros
  const [filterPersonId, setFilterPersonId] = useState<string>('');
  const [filterCategoryId, setFilterCategoryId] = useState<string>('');

  // Dados filtrados de pessoas
  const filteredPersonTotals = useMemo(() => {
    if (!personTotals) return null;
    if (!filterPersonId) return personTotals;

    const filteredPersons = personTotals.personTotals.filter(
      (p) => p.personId === filterPersonId
    );

    const grandTotalIncome = filteredPersons.reduce((sum, p) => sum + p.totalIncome, 0);
    const grandTotalExpense = filteredPersons.reduce((sum, p) => sum + p.totalExpense, 0);
    const grandTotalBalance = grandTotalIncome - grandTotalExpense;

    return {
      personTotals: filteredPersons,
      grandTotalIncome,
      grandTotalExpense,
      grandTotalBalance,
    };
  }, [personTotals, filterPersonId]);

  // Dados filtrados de categorias
  const filteredCategoryTotals = useMemo(() => {
    if (!categoryTotals) return null;
    if (!filterCategoryId) return categoryTotals;

    const filteredCategories = categoryTotals.categoryTotals.filter(
      (c) => c.categoryId === filterCategoryId
    );

    const grandTotalIncome = filteredCategories.reduce((sum, c) => sum + c.totalIncome, 0);
    const grandTotalExpense = filteredCategories.reduce((sum, c) => sum + c.totalExpense, 0);
    const grandTotalBalance = grandTotalIncome - grandTotalExpense;

    return {
      categoryTotals: filteredCategories,
      grandTotalIncome,
      grandTotalExpense,
      grandTotalBalance,
    };
  }, [categoryTotals, filterCategoryId]);

  // Opções para filtros
  const filterPersonOptions = [
    { value: '', label: 'Todas as pessoas' },
    ...persons.map((p) => ({ value: p.id, label: p.name })),
  ];

  const filterCategoryOptions = [
    { value: '', label: 'Todas as categorias' },
    ...categories.map((c) => ({ value: c.id, label: c.description })),
  ];

  if (loading) {
    return <div className="loading">Carregando relatórios...</div>;
  }

  if (error) {
    return (
      <Alert variant="error" onClose={clearError}>
        {error}
      </Alert>
    );
  }

  return (
    <div>
      {/* Filtros */}
      <div className="card">
        <div className="card-header">
          <h2>Filtros</h2>
        </div>
        <div className="filters">
          <div className="form-row">
            <Select
              id="filterPerson"
              label="Filtrar por Pessoa"
              value={filterPersonId}
              onChange={(e) => setFilterPersonId(e.target.value)}
              options={filterPersonOptions}
            />
            <Select
              id="filterCategory"
              label="Filtrar por Categoria"
              value={filterCategoryId}
              onChange={(e) => setFilterCategoryId(e.target.value)}
              options={filterCategoryOptions}
            />
          </div>
        </div>
      </div>

      {/* Cards de resumo geral */}
      {filteredPersonTotals && (
        <div className="stats-grid">
          <div className="stat-card">
            <h3>Total de Receitas</h3>
            <div className="value value-positive">
              {formatCurrency(filteredPersonTotals.grandTotalIncome)}
            </div>
          </div>
          <div className="stat-card">
            <h3>Total de Despesas</h3>
            <div className="value value-negative">
              {formatCurrency(filteredPersonTotals.grandTotalExpense)}
            </div>
          </div>
          <div className="stat-card">
            <h3>Saldo Líquido</h3>
            <div className={`value ${getValueClass(filteredPersonTotals.grandTotalBalance)}`}>
              {formatCurrency(filteredPersonTotals.grandTotalBalance)}
            </div>
          </div>
        </div>
      )}

      {/* Relatório por pessoa */}
      <div className="card">
        <div className="card-header">
          <h2>Totais por Pessoa</h2>
        </div>

        {filteredPersonTotals && filteredPersonTotals.personTotals.length === 0 ? (
          <div className="empty-state">
            {filterPersonId ? 'Nenhum resultado para o filtro selecionado.' : 'Nenhuma pessoa cadastrada.'}
          </div>
        ) : (
          <div className="table-container">
            <table>
              <thead>
                <tr>
                  <th>Pessoa</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {filteredPersonTotals?.personTotals.map((person) => (
                  <tr key={person.personId}>
                    <td>{person.personName}</td>
                    <td className="value-positive">{formatCurrency(person.totalIncome)}</td>
                    <td className="value-negative">{formatCurrency(person.totalExpense)}</td>
                    <td className={getValueClass(person.balance)}>
                      {formatCurrency(person.balance)}
                    </td>
                  </tr>
                ))}
                {filteredPersonTotals && filteredPersonTotals.personTotals.length > 0 && (
                  <tr className="totals-row">
                    <td>
                      <strong>TOTAL GERAL</strong>
                    </td>
                    <td className="value-positive">
                      <strong>{formatCurrency(filteredPersonTotals.grandTotalIncome)}</strong>
                    </td>
                    <td className="value-negative">
                      <strong>{formatCurrency(filteredPersonTotals.grandTotalExpense)}</strong>
                    </td>
                    <td className={getValueClass(filteredPersonTotals.grandTotalBalance)}>
                      <strong>{formatCurrency(filteredPersonTotals.grandTotalBalance)}</strong>
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Relatório por categoria */}
      <div className="card">
        <div className="card-header">
          <h2>Totais por Categoria</h2>
        </div>

        {filteredCategoryTotals && filteredCategoryTotals.categoryTotals.length === 0 ? (
          <div className="empty-state">
            {filterCategoryId ? 'Nenhum resultado para o filtro selecionado.' : 'Nenhuma categoria cadastrada.'}
          </div>
        ) : (
          <div className="table-container">
            <table>
              <thead>
                <tr>
                  <th>Categoria</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {filteredCategoryTotals?.categoryTotals.map((category) => (
                  <tr key={category.categoryId}>
                    <td>{category.categoryDescription}</td>
                    <td className="value-positive">{formatCurrency(category.totalIncome)}</td>
                    <td className="value-negative">{formatCurrency(category.totalExpense)}</td>
                    <td className={getValueClass(category.balance)}>
                      {formatCurrency(category.balance)}
                    </td>
                  </tr>
                ))}
                {filteredCategoryTotals && filteredCategoryTotals.categoryTotals.length > 0 && (
                  <tr className="totals-row">
                    <td>
                      <strong>TOTAL GERAL</strong>
                    </td>
                    <td className="value-positive">
                      <strong>{formatCurrency(filteredCategoryTotals.grandTotalIncome)}</strong>
                    </td>
                    <td className="value-negative">
                      <strong>{formatCurrency(filteredCategoryTotals.grandTotalExpense)}</strong>
                    </td>
                    <td className={getValueClass(filteredCategoryTotals.grandTotalBalance)}>
                      <strong>{formatCurrency(filteredCategoryTotals.grandTotalBalance)}</strong>
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
