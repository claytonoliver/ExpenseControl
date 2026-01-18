import { useState, useEffect } from 'react';
import { useCategories } from '../hooks';
import { Button, Input, Select, Table, Alert, Modal, ConfirmModal } from '../components/ui';
import type { Column } from '../components/ui';
import type { CategoryDto, CreateCategoryRequest, UpdateCategoryRequest } from '../types';
import { CategoryPurpose } from '../types';

/**
 * Página de gerenciamento de categorias.
 * Permite criar, editar, excluir e listar categorias.
 */
export function CategoriesPage() {
  const {
    categories,
    loading,
    error,
    createCategory,
    updateCategory,
    deleteCategory,
    creating,
    updating,
    deleting,
    clearError,
  } = useCategories();

  // Form de criação
  const [formData, setFormData] = useState<CreateCategoryRequest>({
    description: '',
    purpose: CategoryPurpose.Both,
  });

  // Modais
  const [viewCategory, setViewCategory] = useState<CategoryDto | null>(null);
  const [editCategory, setEditCategory] = useState<CategoryDto | null>(null);
  const [deleteCategoryData, setDeleteCategoryData] = useState<CategoryDto | null>(null);

  // Form de edição
  const [editFormData, setEditFormData] = useState<UpdateCategoryRequest>({
    description: '',
    purpose: CategoryPurpose.Both,
  });

  // Preenche form de edição quando seleciona uma categoria
  useEffect(() => {
    if (editCategory) {
      setEditFormData({
        description: editCategory.description,
        purpose: editCategory.purpose,
      });
    }
  }, [editCategory]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const success = await createCategory(formData);
    if (success) {
      setFormData({ description: '', purpose: CategoryPurpose.Both });
    }
  };

  const handleEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editCategory) return;
    const success = await updateCategory(editCategory.id, editFormData);
    if (success) {
      setEditCategory(null);
    }
  };

  const handleDelete = async () => {
    if (!deleteCategoryData) return;
    const success = await deleteCategory(deleteCategoryData.id);
    if (success) {
      setDeleteCategoryData(null);
    }
  };

  const getPurposeBadgeClass = (purpose: CategoryPurpose) => {
    switch (purpose) {
      case CategoryPurpose.Expense:
        return 'badge-expense';
      case CategoryPurpose.Income:
        return 'badge-income';
      case CategoryPurpose.Both:
        return 'badge-both';
      default:
        return '';
    }
  };

  const columns: Column<CategoryDto>[] = [
    {
      key: 'description',
      header: 'Descrição',
    },
    {
      key: 'purpose',
      header: 'Finalidade',
      render: (category) => (
        <span className={`badge ${getPurposeBadgeClass(category.purpose)}`}>
          {category.purposeDescription}
        </span>
      ),
    },
    {
      key: 'createdAt',
      header: 'Cadastrada em',
      render: (category) => new Date(category.createdAt).toLocaleDateString('pt-BR'),
    },
    {
      key: 'actions',
      header: 'Ações',
      render: (category) => (
        <div className="action-buttons">
          <Button
            variant="secondary"
            size="small"
            onClick={() => setViewCategory(category)}
          >
            Visualizar
          </Button>
          <Button
            variant="primary"
            size="small"
            onClick={() => setEditCategory(category)}
          >
            Editar
          </Button>
          <Button
            variant="danger"
            size="small"
            onClick={() => setDeleteCategoryData(category)}
          >
            Excluir
          </Button>
        </div>
      ),
    },
  ];

  const purposeOptions = [
    { value: CategoryPurpose.Expense, label: 'Despesa' },
    { value: CategoryPurpose.Income, label: 'Receita' },
    { value: CategoryPurpose.Both, label: 'Ambas' },
  ];

  return (
    <div>
      {/* Formulário de cadastro */}
      <div className="card">
        <div className="card-header">
          <h2>Nova Categoria</h2>
        </div>

        {error && (
          <Alert variant="error" onClose={clearError}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <Input
              id="description"
              label="Descrição"
              type="text"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Ex: Alimentação, Salário, Transporte"
              required
            />
            <Select
              id="purpose"
              label="Finalidade"
              value={formData.purpose}
              onChange={(e) =>
                setFormData({ ...formData, purpose: e.target.value as CategoryPurpose })
              }
              options={purposeOptions}
              required
            />
          </div>
          <Button type="submit" variant="primary" disabled={creating}>
            {creating ? 'Cadastrando...' : 'Cadastrar Categoria'}
          </Button>
        </form>
      </div>

      {/* Lista de categorias */}
      <div className="card">
        <div className="card-header">
          <h2>Categorias Cadastradas</h2>
        </div>

        <Table
          columns={columns}
          data={categories}
          keyExtractor={(category) => category.id}
          loading={loading}
          emptyMessage="Nenhuma categoria cadastrada."
        />
      </div>

      {/* Modal de Visualização */}
      <Modal
        isOpen={!!viewCategory}
        onClose={() => setViewCategory(null)}
        title="Detalhes da Categoria"
      >
        {viewCategory && (
          <div className="transaction-details">
            <div className="detail-row">
              <strong>Descrição:</strong>
              <span>{viewCategory.description}</span>
            </div>
            <div className="detail-row">
              <strong>Finalidade:</strong>
              <span className={`badge ${getPurposeBadgeClass(viewCategory.purpose)}`}>
                {viewCategory.purposeDescription}
              </span>
            </div>
            <div className="detail-row">
              <strong>Cadastrada em:</strong>
              <span>{new Date(viewCategory.createdAt).toLocaleDateString('pt-BR')}</span>
            </div>
          </div>
        )}
      </Modal>

      {/* Modal de Edição */}
      <Modal
        isOpen={!!editCategory}
        onClose={() => setEditCategory(null)}
        title="Editar Categoria"
      >
        {editCategory && (
          <form onSubmit={handleEdit}>
            <Input
              id="editDescription"
              label="Descrição"
              type="text"
              value={editFormData.description}
              onChange={(e) => setEditFormData({ ...editFormData, description: e.target.value })}
              placeholder="Ex: Alimentação, Salário, Transporte"
              required
            />

            <Select
              id="editPurpose"
              label="Finalidade"
              value={editFormData.purpose}
              onChange={(e) =>
                setEditFormData({ ...editFormData, purpose: e.target.value as CategoryPurpose })
              }
              options={purposeOptions}
              required
            />

            <div className="modal-actions">
              <Button variant="secondary" onClick={() => setEditCategory(null)}>
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
        isOpen={!!deleteCategoryData}
        onClose={() => setDeleteCategoryData(null)}
        onConfirm={handleDelete}
        title="Excluir Categoria"
        message={`Tem certeza que deseja excluir a categoria "${deleteCategoryData?.description}"? Esta ação não pode ser desfeita e pode afetar transações associadas.`}
        confirmText="Excluir"
        cancelText="Cancelar"
        variant="danger"
        loading={deleting}
      />
    </div>
  );
}
