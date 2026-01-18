import { useState, useCallback, useEffect } from 'react';
import { personService, categoryService } from '../services/api';
import { extractErrorMessage } from './useAsync';
import type { PersonTotalsReportDto, CategoryTotalsReportDto } from '../types';

/**
 * Retorno do hook useReports.
 */
interface UseReportsReturn {
  personTotals: PersonTotalsReportDto | null;
  categoryTotals: CategoryTotalsReportDto | null;
  loading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
  clearError: () => void;
}

/**
 * Hook para gerenciamento de relatórios.
 * Carrega totais por pessoa e por categoria.
 */
export function useReports(): UseReportsReturn {
  const [personTotals, setPersonTotals] = useState<PersonTotalsReportDto | null>(null);
  const [categoryTotals, setCategoryTotals] = useState<CategoryTotalsReportDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const [personData, categoryData] = await Promise.all([
        personService.getTotals(),
        categoryService.getTotals(),
      ]);
      setPersonTotals(personData);
      setCategoryTotals(categoryData);
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }, []);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    personTotals,
    categoryTotals,
    loading,
    error,
    refresh,
    clearError,
  };
}
