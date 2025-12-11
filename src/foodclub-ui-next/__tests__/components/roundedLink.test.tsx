import { render, screen } from '@testing-library/react';
import RoundedLink from '@/components/core/roundedLink';

// Mock next/navigation
jest.mock('next/navigation', () => ({
  usePathname: jest.fn(),
}));

const { usePathname } = require('next/navigation');

describe('RoundedLink Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render children correctly', () => {
    usePathname.mockReturnValue('/home');
    
    render(<RoundedLink href="/home">Home</RoundedLink>);
    
    expect(screen.getByRole('link', { name: 'Home' })).toBeInTheDocument();
  });

  it('should apply active styles when pathname matches href', () => {
    usePathname.mockReturnValue('/about');
    
    render(<RoundedLink href="/about">About</RoundedLink>);
    
    const link = screen.getByRole('link', { name: 'About' });
    expect(link).toHaveClass('bg-green-light');
  });

  it('should apply inactive styles when pathname does not match href', () => {
    usePathname.mockReturnValue('/home');
    
    render(<RoundedLink href="/about">About</RoundedLink>);
    
    const link = screen.getByRole('link', { name: 'About' });
    expect(link).toHaveClass('bg-gray-light');
  });

  it('should respect explicit isActive prop', () => {
    usePathname.mockReturnValue('/home');
    
    render(<RoundedLink href="/about" isActive={true}>About</RoundedLink>);
    
    const link = screen.getByRole('link', { name: 'About' });
    expect(link).toHaveClass('bg-green-light');
  });

  it('should apply disabled styles when isDisabled is true', () => {
    usePathname.mockReturnValue('/home');
    
    render(<RoundedLink href="/about" isDisabled={true}>About</RoundedLink>);
    
    const link = screen.getByRole('link', { name: 'About' });
    expect(link).toHaveClass('pointer-events-none');
    expect(link).toHaveClass('opacity-50');
    expect(link).toHaveClass('cursor-not-allowed');
  });

  it('should have correct href attribute', () => {
    usePathname.mockReturnValue('/home');
    
    render(<RoundedLink href="/contact">Contact</RoundedLink>);
    
    const link = screen.getByRole('link', { name: 'Contact' });
    expect(link).toHaveAttribute('href', '/contact');
  });
});
