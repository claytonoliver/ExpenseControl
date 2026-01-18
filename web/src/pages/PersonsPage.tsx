import { useState, useEffect } from 'react';
import { usePersons } from '../hooks';
import { Button, Input, Table, Alert, Modal, ConfirmModal } from '../components/ui';
import type { Column } from '../components/ui';
import type { PersonDto, CreatePersonRequest, UpdatePersonRequest } from '../types';

/**
 * Página de gerenciamento de pessoas.
 * Permite criar, editar, excluir e listar pessoas.
 */
export function PersonsPage() {
  const {
    persons,
    loading,
    error,
    createPerson,
    updatePerson,
    deletePerson,
    creating,
    updating,
    deleting,
    clearError,
  } = usePersons();

  // Form de criação
  const [formData, setFormData] = useState<CreatePersonRequest>({ name: '', age: 0 });

  // Modais
  const [viewPerson, setViewPerson] = useState<PersonDto | null>(null);
  const [editPerson, setEditPerson] = useState<PersonDto | null>(null);
  const [deletePersonData, setDeletePersonData] = useState<PersonDto | null>(null);

  // Form de edição
  const [editFormData, setEditFormData] = useState<UpdatePersonRequest>({
    name: '',
    age: 0,
  });

  // Preenche form de edição quando seleciona uma pessoa
  useEffect(() => {
    if (editPerson) {
      setEditFormData({
        name: editPerson.name,
        age: editPerson.age,
      });
    }
  }, [editPerson]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const success = await createPerson(formData);
    if (success) {
      setFormData({ name: '', age: 0 });
    }
  };

  const handleEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editPerson) return;
    const success = await updatePerson(editPerson.id, editFormData);
    if (success) {
      setEditPerson(null);
    }
  };

  const handleDelete = async () => {
    if (!deletePersonData) return;
    const success = await deletePerson(deletePersonData.id);
    if (success) {
      setDeletePersonData(null);
    }
  };

  const columns: Column<PersonDto>[] = [
    {
      key: 'name',
      header: 'Nome',
    },
    {
      key: 'age',
      header: 'Idade',
      render: (person) => (
        <>
          {person.age} anos
          {person.age < 18 && (
            <span className="badge badge-expense" style={{ marginLeft: 8 }}>
              Menor
            </span>
          )}
        </>
      ),
    },
    {
      key: 'createdAt',
      header: 'Cadastrado em',
      render: (person) => new Date(person.createdAt).toLocaleDateString('pt-BR'),
    },
    {
      key: 'actions',
      header: 'Ações',
      render: (person) => (
        <div className="action-buttons">
          <Button
            variant="secondary"
            size="small"
            onClick={() => setViewPerson(person)}
          >
            Visualizar
          </Button>
          <Button
            variant="primary"
            size="small"
            onClick={() => setEditPerson(person)}
          >
            Editar
          </Button>
          <Button
            variant="danger"
            size="small"
            onClick={() => setDeletePersonData(person)}
          >
            Excluir
          </Button>
        </div>
      ),
    },
  ];

  return (
    <div>
      {/* Formulário de cadastro */}
      <div className="card">
        <div className="card-header">
          <h2>Nova Pessoa</h2>
        </div>

        {error && (
          <Alert variant="error" onClose={clearError}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <Input
              id="name"
              label="Nome"
              type="text"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="Digite o nome"
              required
            />
            <Input
              id="age"
              label="Idade"
              type="number"
              value={formData.age || ''}
              onChange={(e) => setFormData({ ...formData, age: parseInt(e.target.value) || 0 })}
              placeholder="Digite a idade"
              min={1}
              required
            />
          </div>
          <Button type="submit" variant="primary" disabled={creating}>
            {creating ? 'Cadastrando...' : 'Cadastrar Pessoa'}
          </Button>
        </form>
      </div>

      {/* Lista de pessoas */}
      <div className="card">
        <div className="card-header">
          <h2>Pessoas Cadastradas</h2>
        </div>

        <Table
          columns={columns}
          data={persons}
          keyExtractor={(person) => person.id}
          loading={loading}
          emptyMessage="Nenhuma pessoa cadastrada."
        />
      </div>

      {/* Modal de Visualização */}
      <Modal
        isOpen={!!viewPerson}
        onClose={() => setViewPerson(null)}
        title="Detalhes da Pessoa"
      >
        {viewPerson && (
          <div className="transaction-details">
            <div className="detail-row">
              <strong>Nome:</strong>
              <span>{viewPerson.name}</span>
            </div>
            <div className="detail-row">
              <strong>Idade:</strong>
              <span>
                {viewPerson.age} anos
                {viewPerson.age < 18 && (
                  <span className="badge badge-expense" style={{ marginLeft: 8 }}>
                    Menor
                  </span>
                )}
              </span>
            </div>
            <div className="detail-row">
              <strong>Cadastrado em:</strong>
              <span>{new Date(viewPerson.createdAt).toLocaleDateString('pt-BR')}</span>
            </div>
            {viewPerson.updatedAt && (
              <div className="detail-row">
                <strong>Atualizado em:</strong>
                <span>{new Date(viewPerson.updatedAt).toLocaleDateString('pt-BR')}</span>
              </div>
            )}
          </div>
        )}
      </Modal>

      {/* Modal de Edição */}
      <Modal
        isOpen={!!editPerson}
        onClose={() => setEditPerson(null)}
        title="Editar Pessoa"
      >
        {editPerson && (
          <form onSubmit={handleEdit}>
            <Input
              id="editName"
              label="Nome"
              type="text"
              value={editFormData.name}
              onChange={(e) => setEditFormData({ ...editFormData, name: e.target.value })}
              placeholder="Digite o nome"
              required
            />

            <Input
              id="editAge"
              label="Idade"
              type="number"
              value={editFormData.age || ''}
              onChange={(e) => setEditFormData({ ...editFormData, age: parseInt(e.target.value) || 0 })}
              placeholder="Digite a idade"
              min={1}
              required
            />

            <div className="modal-actions">
              <Button variant="secondary" onClick={() => setEditPerson(null)}>
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
        isOpen={!!deletePersonData}
        onClose={() => setDeletePersonData(null)}
        onConfirm={handleDelete}
        title="Excluir Pessoa"
        message={`Tem certeza que deseja excluir "${deletePersonData?.name}"? Todas as transações desta pessoa serão removidas. Esta ação não pode ser desfeita.`}
        confirmText="Excluir"
        cancelText="Cancelar"
        variant="danger"
        loading={deleting}
      />
    </div>
  );
}
