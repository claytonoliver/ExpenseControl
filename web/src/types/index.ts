/**
 * Tipos compartilhados da aplicação.
 * Espelham os DTOs do backend.
 */

// Constantes para tipos de transação e finalidade de categoria
export const TransactionType = {
  Expense: 'Expense',
  Income: 'Income',
} as const;

export type TransactionType = (typeof TransactionType)[keyof typeof TransactionType];

export const CategoryPurpose = {
  Expense: 'Expense',
  Income: 'Income',
  Both: 'Both',
} as const;

export type CategoryPurpose = (typeof CategoryPurpose)[keyof typeof CategoryPurpose];

// DTOs
export interface PersonDto {
  id: string;
  name: string;
  age: number;
  createdAt: string;
  updatedAt: string | null;
}

export interface CategoryDto {
  id: string;
  description: string;
  purpose: CategoryPurpose;
  purposeDescription: string;
  createdAt: string;
}

export interface TransactionDto {
  id: string;
  description: string;
  value: number;
  type: TransactionType;
  typeDescription: string;
  categoryId: string;
  categoryDescription: string;
  personId: string;
  personName: string;
  createdAt: string;
}

export interface PersonTotalDto {
  personId: string;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface PersonTotalsReportDto {
  personTotals: PersonTotalDto[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandTotalBalance: number;
}

export interface CategoryTotalDto {
  categoryId: string;
  categoryDescription: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface CategoryTotalsReportDto {
  categoryTotals: CategoryTotalDto[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandTotalBalance: number;
}

// Requests
export interface CreatePersonRequest {
  name: string;
  age: number;
}

export interface UpdatePersonRequest {
  name: string;
  age: number;
}

export interface CreateCategoryRequest {
  description: string;
  purpose: CategoryPurpose;
}

export interface UpdateCategoryRequest {
  description: string;
  purpose: CategoryPurpose;
}

export interface CreateTransactionRequest {
  description: string;
  value: number;
  type: TransactionType;
  categoryId: string;
  personId: string;
}

export interface UpdateTransactionRequest {
  description: string;
  value: number;
  type: TransactionType;
  categoryId: string;
}

// API Error
export interface ApiError {
  error?: string;
  title?: string;
  errors?: Record<string, string[]>;
}
