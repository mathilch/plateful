import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import FcInput from '@/components/core/fcInput';
import '@testing-library/jest-dom';

describe('FcInput Component', () => {
  it('should render input with label', () => {
    render(<FcInput type="email" />);
    
    expect(screen.getByLabelText('Email')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('m@example.com')).toBeInTheDocument();
  });

  it('should render with correct type attribute', () => {
    render(<FcInput type="password" />);
    
    const input = screen.getByLabelText('Email');
    expect(input).toHaveAttribute('type', 'password');
  });

  it('should accept user input', async () => {
    const user = userEvent.setup();
    render(<FcInput type="email" />);
    
    const input = screen.getByLabelText('Email');
    await user.type(input, 'test@example.com');
    
    expect(input).toHaveValue('test@example.com');
  });

  it('should apply custom className', () => {
    render(<FcInput type="email" className="custom-class" />);
    
    const input = screen.getByLabelText('Email');
    expect(input).toHaveClass('custom-class');
    expect(input).toHaveClass('h-12'); // default class
  });

  it('should be marked as required', () => {
    render(<FcInput type="email" />);
    
    const input = screen.getByLabelText('Email');
    expect(input).toBeRequired();
  });

  it('should forward additional props', () => {
    render(<FcInput type="email" data-testid="email-input" disabled />);
    
    const input = screen.getByTestId('email-input');
    expect(input).toBeDisabled();
  });
});
