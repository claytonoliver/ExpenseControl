import type { InputHTMLAttributes } from 'react';

/**
 * Props do componente Input.
 */
interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
}

/**
 * Componente de input com label.
 */
export function Input({ label, error, id, ...props }: InputProps) {
  return (
    <div className="form-group">
      <label htmlFor={id}>{label}</label>
      <input id={id} className={error ? 'input-error' : ''} {...props} />
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}

/**
 * Props do componente Select.
 */
interface SelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label: string;
  error?: string;
  options: Array<{ value: string | number; label: string; disabled?: boolean }>;
  placeholder?: string;
}

/**
 * Componente de select com label.
 */
export function Select({ label, error, id, options, placeholder, ...props }: SelectProps) {
  return (
    <div className="form-group">
      <label htmlFor={id}>{label}</label>
      <select id={id} className={error ? 'input-error' : ''} {...props}>
        {placeholder && <option value="">{placeholder}</option>}
        {options.map((option) => (
          <option key={option.value} value={option.value} disabled={option.disabled}>
            {option.label}
          </option>
        ))}
      </select>
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}
