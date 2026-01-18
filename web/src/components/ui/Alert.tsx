import type { ReactNode } from 'react';

/**
 * Variantes de alerta.
 */
type AlertVariant = 'error' | 'success' | 'warning' | 'info';

/**
 * Props do componente Alert.
 */
interface AlertProps {
  variant: AlertVariant;
  children: ReactNode;
  onClose?: () => void;
}

/**
 * Componente de alerta para mensagens de feedback.
 */
export function Alert({ variant, children, onClose }: AlertProps) {
  const variantClasses: Record<AlertVariant, string> = {
    error: 'alert-error',
    success: 'alert-success',
    warning: 'alert-warning',
    info: 'alert-info',
  };

  return (
    <div className={`alert ${variantClasses[variant]}`}>
      <span>{children}</span>
      {onClose && (
        <button className="alert-close" onClick={onClose}>
          &times;
        </button>
      )}
    </div>
  );
}
