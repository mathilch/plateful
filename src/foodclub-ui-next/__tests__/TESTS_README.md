# Frontend Tests

This directory contains basic frontend test suites for the FoodClub Next.js application.

## 6 test suites covering UI components and utilities.

## Setup

### Installing Dependencies

The testing dependencies are already installed. If you need to reinstall:

```bash
npm install --save-dev jest @testing-library/react @testing-library/jest-dom @testing-library/user-event jest-environment-jsdom @types/jest @testing-library/dom
```

## Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode (auto-reruns on file changes)
npm run test:watch

# Run tests with coverage report
npm run test:coverage
```

## Test Structure

### Component Tests (`__tests__/components/`)

Pure **frontend UI tests** focused on component rendering and user interactions:

- **roundedLink.test.tsx** (7 tests)
  - Rendering children and links
  - Active/inactive state styling
  - Disabled state handling
  - Pathname matching with Next.js router

- **fcInput.test.tsx** (6 tests)
  - Input rendering with labels
  - User typing interactions
  - Custom className application
  - Props forwarding (disabled, data attributes)
  - Required field validation

- **meal-card.test.tsx** (14 tests)
  - Event card display (name, price, location, date/time)
  - Image rendering
  - Host information display
  - Tag and allergen badge rendering
  - Truncation of long descriptions
  - Participant count display
  - Link to event details page
  - Conditional rendering (empty allergens, no description)

- **imageDropzone.test.tsx** (7 tests)
  - Dropzone placeholder rendering
  - Image preview display
  - File upload handling with drag-and-drop
  - Styling and CSS classes
  - Accept attribute for image files only

- **foodAppHeader.test.tsx** (8 tests)
  - Header navigation rendering
  - Login/signup button display (unauthenticated)
  - Sign out button and user profile (authenticated)
  - Dialog opening on button clicks
  - LocalStorage token management
  - Authentication state changes

### Utility Tests (`__tests__/utils/`)

- **jwt-decoder.helper.test.ts** (3 tests)
  - Valid JWT token decoding
  - Invalid token error handling
  - Custom payload type support

## Configuration

- **jest.config.js**: Jest configuration integrated with Next.js
  - jsdom test environment for browser APIs
  - Module path aliases (`@/` → project root)
  - Coverage collection settings
  
- **jest.setup.js**: Test environment setup with `@testing-library/jest-dom` matchers

## Test Focus

✅ **UI Rendering**: Components display correctly with proper content  
✅ **User Interactions**: Clicks, typing, drag-and-drop work as expected  
✅ **Visual States**: Active, disabled, loading states render correctly  
✅ **Props Handling**: Components respond correctly to prop changes  
✅ **Conditional Rendering**: UI adapts based on data/authentication  
✅ **Navigation**: Links and routing work correctly  
✅ **Forms**: Input fields accept and validate user input  

## Coverage Report

Run `npm run test:coverage` to generate a detailed coverage report showing which lines of code are covered by tests.

## When adding new tests, follow these patterns:

1. **Component Tests** → `__tests__/components/`
   - Test rendering, user interactions, and visual states
   - Mock Next.js modules (`next/link`, `next/navigation`, `next/image`)
   - Use `screen.getByRole`, `screen.getByText` for queries
   - Use `userEvent` for simulating interactions

2. **Utility Tests** → `__tests__/utils/`
   - Test pure functions and helper utilities
   - Cover edge cases and error conditions
   - No DOM or component testing

## Example Test Pattern

```typescript
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import MyComponent from '@/components/MyComponent';

describe('MyComponent', () => {
  it('should handle user interaction', async () => {
    const user = userEvent.setup();
    const mockHandler = jest.fn();
    
    render(<MyComponent onClick={mockHandler} />);
    
    const button = screen.getByRole('button');
    await user.click(button);
    
    expect(mockHandler).toHaveBeenCalled();
  });
});
```
