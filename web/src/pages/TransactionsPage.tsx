import { useState, useEffect, useMemo } from 'react';
import { useTransactions, usePersons, useCategories } from '../hooks';
import { Button, Input, Select, Table, Alert, Modal, ConfirmModal } from '../components/ui';
import type { Column } from '../components/ui';
import type { TransactionDto, CreateTransactionRequest, UpdateTransactionRequest } from '../types';
import { TransactionType } from '../types';

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
 * Página de gerenciamento de transações.
 * Permite criar e listar transações (despesas e receitas).
 */
export function TransactionsPage() {
  const {
    transactions,
    loading,
    error,
    createTransaction,
    updateTransaction,
    deleteTransaction,
    creating,
    updating,
    deleting,
    clearError,
  } = useTransactions();
  const { persons, loading: loadingPersons } = usePersons();
  const { categories, getCompatibleCategories, loading: loadingCategories } = useCategories();

  // Form de criação
  const [formData, setFormData] = useState<CreateTransactionRequest>({
    description: '',
    value: 0,
    type: TransactionType.Expense,
    categoryId: '',
    personId: '',
  });

  // Filtros
  const [filterPersonId, setFilterPersonId] = useState<string>('');
  const [filterCategoryId, setFilterCategoryId] = useState<string>('');

  // Modais
  const [viewTransaction, setViewTransaction] = useState<TransactionDto | null>(null);
  const [editTransaction, setEditTransaction] = useState<TransactionDto | null>(null);
  const [deleteTransactionData, setDeleteTransactionData] = useState<TransactionDto | null>(null);

  // Form de edição
  const [editFormData, setEditFormData] = useState<UpdateTransactionRequest>({
    description: '',
    value: 0,
    type: TransactionType.Expense,
    categoryId: '',
  });

  // Reset categoria quando o tipo muda no form de criação
  useEffect(() => {
    setFormData((prev) => ({ ...prev, categoryId: '' }));
  }, [formData.type]);

  // Reset categoria quando o tipo muda no form de edição
  useEffect(() => {
    if (editTransaction) {
      setEditFormData((prev) => ({ ...prev, categoryId: '' }));
    }
  }, [editFormData.type, editTransaction]);

  // Preenche form de edição quando seleciona uma transação
  useEffect(() => {
    if (editTransaction) {
      setEditFormData({
        description: editTransaction.description,
        value: editTransaction.value,
        type: editTransaction.type,
        categoryId: editTransaction.categoryId,
      });
    }
  }, [editTransaction]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const success = await createTransaction(formData);
    if (success) {
      setFormData({
        description: '',
        value: 0,
        type: TransactionType.Expense,
        categoryId: '',
        personId: '',
      });
    }
  };

  const handleEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editTransaction) return;
    const success = await updateTransaction(editTransaction.id, editFormData);
    if (success) {
      setEditTransaction(null);
    }
  };

  const handleDelete = async () => {
    if (!deleteTransactionData) return;
    const success = await deleteTransaction(deleteTransactionData.id);
    if (success) {
      setDeleteTransactionData(null);
    }
  };

  const isSelectedPersonMinor = (): boolean => {
    const person = persons.find((p) => p.id === formData.personId);
    return person ? person.age < 18 : false;
  };

  // Transações filtradas
  const filteredTransactions = useMemo(() => {
    return transactions.filter((t) => {
      if (filterPersonId && t.personId !== filterPersonId) return false;
      if (filterCategoryId && t.categoryId !== filterCategoryId) return false;
      return true;
    });
  }, [transactions, filterPersonId, filterCategoryId]);

  const compatibleCategories = getCompatibleCategories(formData.type);
  const editCompatibleCategories = getCompatibleCategories(editFormData.type);
  const isDataLoading = loadingPersons || loadingCategories;
  // Permite criar transações se houver pelo menos uma pessoa e
  // pelo menos uma categoria cadastrada (não exigimos que haja
  // categorias compatíveis no momento do tipo selecionado).
  const canCreateTransactions = persons.length > 0 && categories.length > 0;

  const columns: Column<TransactionDto>[] = [
    {
      key: 'createdAt',
      header: 'Data',
      render: (t) => new Date(t.createdAt).toLocaleDateString('pt-BR'),
    },
    {
      key: 'personName',
      header: 'Pessoa',
    },
    {
      key: 'description',
      header: 'Descrição',
    },
    {
      key: 'categoryDescription',
      header: 'Categoria',
    },
    {
      key: 'type',
      header: 'Tipo',
      render: (t) => (
        <span
          className={`badge ${t.type === TransactionType.Expense ? 'badge-expense' : 'badge-income'}`}
        >
          {t.typeDescription}
        </span>
      ),
    },
    {
      key: 'value',
      header: 'Valor',
      render: (t) => (
        <span
          className={t.type === TransactionType.Expense ? 'value-negative' : 'value-positive'}
        >
          {t.type === TransactionType.Expense ? '- ' : '+ '}
          {formatCurrency(t.value)}
        </span>
      ),
    },
    {
      key: 'actions',
      header: 'Ações',
      render: (t) => (
        <div className="action-buttons">
          <Button
            variant="secondary"
            size="small"
            onClick={() => setViewTransaction(t)}
          >
            Visualizar
          </Button>
          <Button
            variant="primary"
            size="small"
            onClick={() => setEditTransaction(t)}
          >
            Editar
          </Button>
          <Button
            variant="danger"
            size="small"
            onClick={() => setDeleteTransactionData(t)}
          >
            Excluir
          </Button>
        </div>
      ),
    },
  ];

  const personOptions = persons.map((p) => ({
    value: p.id,
    label: `${p.name} (${p.age} anos)${p.age < 18 ? ' - Menor' : ''}`,
  }));

  const categoryOptions = compatibleCategories.map((c) => ({
    value: c.id,
    label: c.description,
  }));

  const editCategoryOptions = editCompatibleCategories.map((c) => ({
    value: c.id,
    label: c.description,
  }));

  const typeOptions = [
    { value: TransactionType.Expense, label: 'Despesa' },
    {
      value: TransactionType.Income,
      label: isSelectedPersonMinor() ? 'Receita (indisponível para menores)' : 'Receita',
      disabled: isSelectedPersonMinor(),
    },
  ];

  const editTypeOptions = [
    { value: TransactionType.Expense, label: 'Despesa' },
    { value: TransactionType.Income, label: 'Receita' },
  ];

  // Opções para filtros (incluindo opção vazia)
  const filterPersonOptions = [
    { value: '', label: 'Todas as pessoas' },
    ...persons.map((p) => ({ value: p.id, label: p.name })),
  ];

  const filterCategoryOptions = [
    { value: '', label: 'Todas as categorias' },
    ...categories.map((c) => ({ value: c.id, label: c.description })),
  ];

  return (
    <div>
      {/* Formulário de cadastro */}
      <div className="card">
        <div className="card-header">
          <h2>Nova Transação</h2>
        </div>

        {error && (
          <Alert variant="error" onClose={clearError}>
            {error}
          </Alert>
        )}

        {isDataLoading ? (
          <div className="loading">Carregando dados...</div>
        ) : !canCreateTransactions ? (
          <Alert variant="warning">
            É necessário cadastrar pelo menos uma pessoa e uma categoria antes de criar transações.
          </Alert>
        ) : (
          <form onSubmit={handleSubmit}>
            <div className="form-row">
              <Select
                id="person"
                label="Pessoa"
                value={formData.personId}
                onChange={(e) => setFormData({ ...formData, personId: e.target.value })}
                options={personOptions}
                placeholder="Selecione uma pessoa"
                required
              />
              <Select
                id="type"
                label="Tipo"
                value={formData.type}
                onChange={(e) =>
                  setFormData({
                    ...formData,
                    type: e.target.value as TransactionType,
                  })
                }
                options={typeOptions}
                required
              />
            </div>

            <div className="form-row">
              <Select
                id="category"
                label="Categoria"
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                options={categoryOptions}
                placeholder="Selecione uma categoria"
                required
              />
              <Input
                id="value"
                label="Valor (R$)"
                type="number"
                value={formData.value || ''}
                onChange={(e) =>
                  setFormData({ ...formData, value: parseFloat(e.target.value) || 0 })
                }
                placeholder="0,00"
                min={0.01}
                step={0.01}
                required
              />
            </div>

            <Input
              id="description"
              label="Descrição"
              type="text"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Ex: Compra no supermercado"
              required
            />

            <Button type="submit" variant="primary" disabled={creating}>
              {creating ? 'Cadastrando...' : 'Cadastrar Transação'}
            </Button>
          </form>
        )}
      </div>

      {/* Lista de transações */}
      <div className="card">
        <div className="card-header">
          <h2>Transações Cadastradas</h2>
        </div>

        {/* Filtros */}
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

        <Table
          columns={columns}
          data={filteredTransactions}
          keyExtractor={(t) => t.id}
          loading={loading}
          emptyMessage="Nenhuma transação encontrada."
        />
      </div>

      {/* Modal de Visualização */}
      <Modal
        isOpen={!!viewTransaction}
        onClose={() => setViewTransaction(null)}
        title="Detalhes da Transação"
      >
        {viewTransaction && (
          <div className="transaction-details">
            <div className="detail-row">
              <strong>Data:</strong>
              <span>{new Date(viewTransaction.createdAt).toLocaleDateString('pt-BR')}</span>
            </div>
            <div className="detail-row">
              <strong>Pessoa:</strong>
              <span>{viewTransaction.personName}</span>
            </div>
            <div className="detail-row">
              <strong>Descrição:</strong>
              <span>{viewTransaction.description}</span>
            </div>
            <div className="detail-row">
              <strong>Categoria:</strong>
              <span>{viewTransaction.categoryDescription}</span>
            </div>
            <div className="detail-row">
              <strong>Tipo:</strong>
              <span className={`badge ${viewTransaction.type === TransactionType.Expense ? 'badge-expense' : 'badge-income'}`}>
                {viewTransaction.typeDescription}
              </span>
            </div>
            <div className="detail-row">
              <strong>Valor:</strong>
              <span className={viewTransaction.type === TransactionType.Expense ? 'value-negative' : 'value-positive'}>
                {viewTransaction.type === TransactionType.Expense ? '- ' : '+ '}
                {formatCurrency(viewTransaction.value)}
              </span>
            </div>
          </div>
        )}
      </Modal>

      {/* Modal de Edição */}
      <Modal
        isOpen={!!editTransaction}
        onClose={() => setEditTransaction(null)}
        title="Editar Transação"
      >
        {editTransaction && (
          <form onSubmit={handleEdit}>
            <div className="detail-row">
              <strong>Pessoa:</strong>
              <span>{editTransaction.personName}</span>
            </div>

            <Select
              id="editType"
              label="Tipo"
              value={editFormData.type}
              onChange={(e) =>
                setEditFormData({
                  ...editFormData,
                  type: e.target.value as TransactionType,
                })
              }
              options={editTypeOptions}
              required
            />

            <Select
              id="editCategory"
              label="Categoria"
              value={editFormData.categoryId}
              onChange={(e) => setEditFormData({ ...editFormData, categoryId: e.target.value })}
              options={editCategoryOptions}
              placeholder="Selecione uma categoria"
              required
            />

            <Input
              id="editValue"
              label="Valor (R$)"
              type="number"
              value={editFormData.value || ''}
              onChange={(e) =>
                setEditFormData({ ...editFormData, value: parseFloat(e.target.value) || 0 })
              }
              placeholder="0,00"
              min={0.01}
              step={0.01}
              required
            />

            <Input
              id="editDescription"
              label="Descrição"
              type="text"
              value={editFormData.description}
              onChange={(e) => setEditFormData({ ...editFormData, description: e.target.value })}
              placeholder="Ex: Compra no supermercado"
              required
            />

            <div className="modal-actions">
              <Button variant="secondary" onClick={() => setEditTransaction(null)}>
                Cancelar
              </Button>
              <Button type="submit" variant="primary" loading={updating}>
                Salvar
              </Button>
            </div>
          </form>
        )}
      </Modal>

      {/* Modal de Confirmação de Exclusão */}
      <ConfirmModal
        isOpen={!!deleteTransactionData}
        onClose={() => setDeleteTransactionData(null)}
        onConfirm={handleDelete}
        title="Excluir Transação"
        message={`Tem certeza que deseja excluir a transação "${deleteTransactionData?.description}"? Esta ação não pode ser desfeita.`}
        confirmText="Excluir"
        cancelText="Cancelar"
        variant="danger"
        loading={deleting}
      />
    </div>
  );
}
