import { useState, useCallback, useEffect } from 'react';
import { personService } from '../services/api';
import { extractErrorMessage } from './useAsync';
import type { PersonDto, CreatePersonRequest, UpdatePersonRequest } from '../types';

/**
 * Retorno do hook usePersons.
 */
interface UsePersonsReturn {
  persons: PersonDto[];
  loading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
  createPerson: (data: CreatePersonRequest) => Promise<boolean>;
  updatePerson: (id: string, data: UpdatePersonRequest) => Promise<boolean>;
  deletePerson: (id: string) => Promise<boolean>;
  creating: boolean;
  updating: boolean;
  deleting: boolean;
  clearError: () => void;
}

/**
 * Hook para gerenciamento de pessoas.
 * Abstrai a lógica de carregamento, criação e exclusão.
 */
export function usePersons(): UsePersonsReturn {
  const [persons, setPersons] = useState<PersonDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);
  const [updating, setUpdating] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await personService.getAll();
      setPersons(data);
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }, []);

  const createPerson = useCallback(
    async (data: CreatePersonRequest): Promise<boolean> => {
      setCreating(true);
      setError(null);
      try {
        const newPerson = await personService.create(data);
        setPersons((prev) => [...prev, newPerson]);
        return true;
      } catch (err) {
        setError(extractErrorMessage(err));
        return false;
      } finally {
        setCreating(false);
      }
    },
    []
  );

  const updatePerson = useCallback(
    async (id: string, data: UpdatePersonRequest): Promise<boolean> => {
      setUpdating(true);
      setError(null);
      try {
        const updatedPerson = await personService.update(id, data);
        setPersons((prev) =>
          prev.map((p) => (p.id === id ? updatedPerson : p))
        );
        return true;
      } catch (err) {
        setError(extractErrorMessage(err));
        return false;
      } finally {
        setUpdating(false);
      }
    },
    []
  );

  const deletePerson = useCallback(async (id: string): Promise<boolean> => {
    setDeleting(true);
    setError(null);
    try {
      await personService.delete(id);
      setPersons((prev) => prev.filter((p) => p.id !== id));
      return true;
    } catch (err) {
      setError(extractErrorMessage(err));
      return false;
    } finally {
      setDeleting(false);
    }
  }, []);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    persons,
    loading,
    error,
    refresh,
    createPerson,
    updatePerson,
    deletePerson,
    creating,
    updating,
    deleting,
    clearError,
  };
}
