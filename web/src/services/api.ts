import axios from 'axios';
import type {
  PersonDto,
  CategoryDto,
  TransactionDto,
  PersonTotalsReportDto,
  CategoryTotalsReportDto,
  CreatePersonRequest,
  UpdatePersonRequest,
  CreateCategoryRequest,
  UpdateCategoryRequest,
  CreateTransactionRequest,
  UpdateTransactionRequest,
} from '../types';

/**
 * Configuração base do axios para comunicação com a API.
 */
const api = axios.create({
  baseURL: 'https://localhost:7002/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Serviço para gerenciamento de pessoas.
 */
export const personService = {
  /**
   * Lista todas as pessoas cadastradas.
   */
  getAll: async (): Promise<PersonDto[]> => {
    const response = await api.get<PersonDto[]>('/persons');
    return response.data;
  },

  /**
   * Obtém totais de receitas, despesas e saldo por pessoa.
   */
  getTotals: async (): Promise<PersonTotalsReportDto> => {
    const response = await api.get<PersonTotalsReportDto>('/persons/totals');
    return response.data;
  },

  /**
   * Cria uma nova pessoa.
   */
  create: async (data: CreatePersonRequest): Promise<PersonDto> => {
    const response = await api.post<PersonDto>('/persons', data);
    return response.data;
  },

  /**
   * Atualiza uma pessoa existente.
   */
  update: async (id: string, data: UpdatePersonRequest): Promise<PersonDto> => {
    const response = await api.put<PersonDto>(`/persons/${id}`, data);
    return response.data;
  },

  /**
   * Deleta uma pessoa e todas as suas transações.
   */
  delete: async (id: string): Promise<void> => {
    await api.delete(`/persons/${id}`);
  },
};

/**
 * Serviço para gerenciamento de categorias.
 */
export const categoryService = {
  /**
   * Lista todas as categorias cadastradas.
   */
  getAll: async (): Promise<CategoryDto[]> => {
    const response = await api.get<CategoryDto[]>('/categories');
    return response.data;
  },

  /**
   * Obtém totais de receitas, despesas e saldo por categoria.
   */
  getTotals: async (): Promise<CategoryTotalsReportDto> => {
    const response = await api.get<CategoryTotalsReportDto>('/categories/totals');
    return response.data;
  },

  /**
   * Cria uma nova categoria.
   */
  create: async (data: CreateCategoryRequest): Promise<CategoryDto> => {
    const response = await api.post<CategoryDto>('/categories', data);
    return response.data;
  },

  /**
   * Atualiza uma categoria existente.
   */
  update: async (id: string, data: UpdateCategoryRequest): Promise<CategoryDto> => {
    const response = await api.put<CategoryDto>(`/categories/${id}`, data);
    return response.data;
  },

  /**
   * Deleta uma categoria.
   */
  delete: async (id: string): Promise<void> => {
    await api.delete(`/categories/${id}`);
  },
};

/**
 * Serviço para gerenciamento de transações.
 */
export const transactionService = {
  /**
   * Lista todas as transações cadastradas.
   */
  getAll: async (): Promise<TransactionDto[]> => {
    const response = await api.get<TransactionDto[]>('/transactions');
    return response.data;
  },

  /**
   * Cria uma nova transação.
   */
  create: async (data: CreateTransactionRequest): Promise<TransactionDto> => {
    const response = await api.post<TransactionDto>('/transactions', data);
    return response.data;
  },

  /**
   * Atualiza uma transação existente.
   */
  update: async (id: string, data: UpdateTransactionRequest): Promise<TransactionDto> => {
    const response = await api.put<TransactionDto>(`/transactions/${id}`, data);
    return response.data;
  },

  /**
   * Deleta uma transação.
   */
  delete: async (id: string): Promise<void> => {
    await api.delete(`/transactions/${id}`);
  },
};

export default api;
