import { useState, useCallback } from 'react';
import type { ApiError } from '../types';
import axios from 'axios';

/**
 * Estado de uma operação assíncrona.
 */
interface AsyncState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
}

/**
 * Retorno do hook useAsync.
 */
interface UseAsyncReturn<T> extends AsyncState<T> {
  execute: (...args: unknown[]) => Promise<T | null>;
  setData: (data: T | null) => void;
  reset: () => void;
}

/**
 * Extrai mensagem de erro de uma resposta de API.
 */
function extractErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as ApiError | undefined;
    if (data?.error) return data.error;
    if (data?.title) return data.title;
    if (data?.errors) {
      const messages = Object.values(data.errors).flat();
      return messages.join(', ');
    }
    return error.message;
  }
  if (error instanceof Error) {
    return error.message;
  }
  return 'Erro desconhecido';
}

/**
 * Hook genérico para operações assíncronas com gerenciamento de estado.
 */
export function useAsync<T>(
  asyncFunction: (...args: unknown[]) => Promise<T>,
  immediate = false
): UseAsyncReturn<T> {
  const [state, setState] = useState<AsyncState<T>>({
    data: null,
    loading: immediate,
    error: null,
  });

  const execute = useCallback(
    async (...args: unknown[]): Promise<T | null> => {
      setState((prev) => ({ ...prev, loading: true, error: null }));
      try {
        const result = await asyncFunction(...args);
        setState({ data: result, loading: false, error: null });
        return result;
      } catch (error) {
        const message = extractErrorMessage(error);
        setState((prev) => ({ ...prev, loading: false, error: message }));
        return null;
      }
    },
    [asyncFunction]
  );

  const setData = useCallback((data: T | null) => {
    setState((prev) => ({ ...prev, data }));
  }, []);

  const reset = useCallback(() => {
    setState({ data: null, loading: false, error: null });
  }, []);

  return {
    ...state,
    execute,
    setData,
    reset,
  };
}

export { extractErrorMessage };
