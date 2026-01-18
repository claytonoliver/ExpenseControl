import type { ButtonHTMLAttributes, ReactNode } from 'react';

/**
 * Variantes de estilo do botão.
 */
type ButtonVariant = 'primary' | 'success' | 'danger' | 'secondary';

/**
 * Props do componente Button.
 */
interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: 'small' | 'medium';
  loading?: boolean;
  children: ReactNode;
}

/**
 * Componente de botão reutilizável com variantes de estilo.
 */
export function Button({
  variant = 'primary',
  size = 'medium',
  loading = false,
  children,
  disabled,
  className = '',
  ...props
}: ButtonProps) {
  const variantClasses: Record<ButtonVariant, string> = {
    primary: 'btn-primary',
    success: 'btn-success',
    danger: 'btn-danger',
    secondary: 'btn-secondary',
  };

  const sizeClasses = {
    small: 'btn-small',
    medium: '',
  };

  const classes = [
    'btn',
    variantClasses[variant],
    sizeClasses[size],
    loading ? 'btn-loading' : '',
    className,
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <button
      className={classes}
      disabled={disabled || loading}
      {...props}
    >
      {loading ? 'Carregando...' : children}
    </button>
  );
}
