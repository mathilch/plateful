import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import FoodAppHeader from '@/components/core/foodAppHeader';

// Mock Next.js modules
jest.mock('next/link', () => {
  return ({ children, href }: { children: React.ReactNode; href: string }) => {
    return <a href={href}>{children}</a>;
  };
});

jest.mock('next/image', () => ({
  __esModule: true,
  default: (props: any) => {
    // eslint-disable-next-line @next/next/no-img-element, jsx-a11y/alt-text
    return <img {...props} />;
  },
}));

jest.mock('next/navigation', () => ({
  useRouter: () => ({
    push: jest.fn(),
    replace: jest.fn(),
  }),
}));

// Mock child components
jest.mock('@/components/core/loginDialog', () => {
  return function MockLoginDialog({ open }: { open: boolean }) {
    return open ? <div data-testid="login-dialog">Login Dialog</div> : null;
  };
});

jest.mock('@/components/core/signUpDialog', () => {
  return function MockSignUpDialog({ open }: { open: boolean }) {
    return open ? <div data-testid="signup-dialog">SignUp Dialog</div> : null;
  };
});

jest.mock('@/components/ui/global-progress', () => {
  return function MockGlobalProgress() {
    return <div data-testid="global-progress">Progress</div>;
  };
});

// Mock localStorage
const localStorageMock = (() => {
  let store: Record<string, string> = {};
  return {
    getItem: jest.fn((key: string) => store[key] || null),
    setItem: jest.fn((key: string, value: string) => {
      store[key] = value.toString();
    }),
    removeItem: jest.fn((key: string) => {
      delete store[key];
    }),
    clear: jest.fn(() => {
      store = {};
    }),
  };
})();

Object.defineProperty(window, 'localStorage', { value: localStorageMock });

describe('FoodAppHeader Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    localStorageMock.clear();
  });

  it('should render header with main navigation', () => {
    render(<FoodAppHeader />);
    
    expect(screen.getByRole('banner')).toBeInTheDocument();
  });

  it('should show login button when not authenticated', () => {
    render(<FoodAppHeader />);
    
    expect(screen.getByText('Log in')).toBeInTheDocument();
  });

  it('should show signup button when not authenticated', () => {
    render(<FoodAppHeader />);
    
    expect(screen.getByText('Sign up')).toBeInTheDocument();
  });

  it('should open login dialog when login button is clicked', async () => {
    const user = userEvent.setup();
    render(<FoodAppHeader />);
    
    const loginButton = screen.getByText('Log in');
    await user.click(loginButton);
    
    expect(screen.getByTestId('login-dialog')).toBeInTheDocument();
  });

  it('should open signup dialog when signup button is clicked', async () => {
    const user = userEvent.setup();
    render(<FoodAppHeader />);
    
    const signupButton = screen.getByText('Sign up');
    await user.click(signupButton);
    
    expect(screen.getByTestId('signup-dialog')).toBeInTheDocument();
  });

  it('should show logout button when authenticated', () => {
    // Mock authenticated user
    const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkpvaG4ifQ.xyz';
    localStorageMock.getItem.mockReturnValue(mockToken);
    
    render(<FoodAppHeader />);
    
    expect(screen.getByText('Sign out')).toBeInTheDocument();
  });

  it('should display user profile link when authenticated', () => {
    const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkpvaG4ifQ.xyz';
    localStorageMock.getItem.mockReturnValue(mockToken);
    
    render(<FoodAppHeader />);
    
    expect(screen.getByText('John')).toBeInTheDocument(); // Username is displayed
  });

  it('should clear token on logout', async () => {
    const user = userEvent.setup();
    const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkpvaG4ifQ.xyz';
    localStorageMock.getItem.mockReturnValue(mockToken);
    
    render(<FoodAppHeader />);
    
    const logoutButton = screen.getByText('Sign out');
    await user.click(logoutButton);
    
    expect(localStorageMock.removeItem).toHaveBeenCalledWith('accessToken');
  });

  it('should render global progress component', () => {
    render(<FoodAppHeader />);
    
    expect(screen.getByTestId('global-progress')).toBeInTheDocument();
  });

  it('should have links to main pages', () => {
    render(<FoodAppHeader />);
    
    const links = screen.getAllByRole('link');
    expect(links.length).toBeGreaterThan(0);
  });
});
