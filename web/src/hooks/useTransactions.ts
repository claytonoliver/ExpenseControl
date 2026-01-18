import { useState, useCallback, useEffect } from 'react';
import { transactionService } from '../services/api';
import { extractErrorMessage } from './useAsync';
import type { TransactionDto, CreateTransactionRequest, UpdateTransactionRequest } from '../types';

/**
 * Retorno do hook useTransactions.
 */
interface UseTransactionsReturn {
  transactions: TransactionDto[];
  loading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
  createTransaction: (data: CreateTransactionRequest) => Promise<boolean>;
  updateTransaction: (id: string, data: UpdateTransactionRequest) => Promise<boolean>;
  deleteTransaction: (id: string) => Promise<boolean>;
  creating: boolean;
  updating: boolean;
  deleting: boolean;
  clearError: () => void;
}

/**
 * Hook para gerenciamento de transações.
 * Abstrai a lógica de carregamento e criação.
 */
export function useTransactions(): UseTransactionsReturn {
  const [transactions, setTransactions] = useState<TransactionDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);
  const [updating, setUpdating] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await transactionService.getAll();
      setTransactions(data);
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }, []);

  const createTransaction = useCallback(
    async (data: CreateTransactionRequest): Promise<boolean> => {
      setCreating(true);
      setError(null);
      try {
        const newTransaction = await transactionService.create(data);
        setTransactions((prev) => [...prev, newTransaction]);
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

  const updateTransaction = useCallback(
    async (id: string, data: UpdateTransactionRequest): Promise<boolean> => {
      setUpdating(true);
      setError(null);
      try {
        const updatedTransaction = await transactionService.update(id, data);
        setTransactions((prev) =>
          prev.map((t) => (t.id === id ? updatedTransaction : t))
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

  const deleteTransaction = useCallback(
    async (id: string): Promise<boolean> => {
      setDeleting(true);
      setError(null);
      try {
        await transactionService.delete(id);
        setTransactions((prev) => prev.filter((t) => t.id !== id));
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

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    transactions,
    loading,
    error,
    refresh,
    createTransaction,
    updateTransaction,
    deleteTransaction,
    creating,
    updating,
    deleting,
    clearError,
  };
}
