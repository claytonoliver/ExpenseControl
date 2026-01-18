import { useState, useCallback, useEffect } from 'react';
import { categoryService } from '../services/api';
import { extractErrorMessage } from './useAsync';
import type { CategoryDto, CreateCategoryRequest, UpdateCategoryRequest } from '../types';
import { CategoryPurpose, TransactionType } from '../types';

/**
 * Retorno do hook useCategories.
 */
interface UseCategoriesReturn {
  categories: CategoryDto[];
  loading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
  createCategory: (data: CreateCategoryRequest) => Promise<boolean>;
  updateCategory: (id: string, data: UpdateCategoryRequest) => Promise<boolean>;
  deleteCategory: (id: string) => Promise<boolean>;
  creating: boolean;
  updating: boolean;
  deleting: boolean;
  clearError: () => void;
  getCompatibleCategories: (transactionType: TransactionType) => CategoryDto[];
}

/**
 * Hook para gerenciamento de categorias.
 * Abstrai a lógica de carregamento e criação.
 */
export function useCategories(): UseCategoriesReturn {
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);
  const [updating, setUpdating] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await categoryService.getAll();
      setCategories(data);
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }, []);

  const createCategory = useCallback(
    async (data: CreateCategoryRequest): Promise<boolean> => {
      setCreating(true);
      setError(null);
      try {
        const newCategory = await categoryService.create(data);
        setCategories((prev) => [...prev, newCategory]);
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

  const updateCategory = useCallback(
    async (id: string, data: UpdateCategoryRequest): Promise<boolean> => {
      setUpdating(true);
      setError(null);
      try {
        const updatedCategory = await categoryService.update(id, data);
        setCategories((prev) =>
          prev.map((c) => (c.id === id ? updatedCategory : c))
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

  const deleteCategory = useCallback(
    async (id: string): Promise<boolean> => {
      setDeleting(true);
      setError(null);
      try {
        await categoryService.delete(id);
        setCategories((prev) => prev.filter((c) => c.id !== id));
        return true;
      } catch (err) {
        setError(extractErrorMessage(err));
        return false;
      } finally {
        setDeleting(false);
      }
    },
    []
  );

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  /**
   * Filtra categorias compatíveis com o tipo de transação.
   */
  const getCompatibleCategories = useCallback(
    (transactionType: TransactionType): CategoryDto[] => {
      return categories.filter((category) => {
        if (category.purpose === CategoryPurpose.Both) return true;
        if (transactionType === TransactionType.Expense) {
          return category.purpose === CategoryPurpose.Expense;
        }
        if (transactionType === TransactionType.Income) {
          return category.purpose === CategoryPurpose.Income;
        }
        return false;
      });
    },
    [categories]
  );

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    categories,
    loading,
    error,
    refresh,
    createCategory,
    updateCategory,
    deleteCategory,
    creating,
    updating,
    deleting,
    clearError,
    getCompatibleCategories,
  };
}
