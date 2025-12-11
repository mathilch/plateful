import { render, screen } from '@testing-library/react';
import MealCard from '@/components/core/meal-card/meal-card';
import { EventOverviewDto } from '@/types/event-details.type';

// Mock Next.js Link component
jest.mock('next/link', () => {
  return ({ children, href }: { children: React.ReactNode; href: string }) => {
    return <a href={href}>{children}</a>;
  };
});

describe('MealCard Component', () => {
  const mockEventData: EventOverviewDto = {
      eventId: '123',
      name: 'Italian Pasta Night',
      description: 'Join us for an authentic Italian pasta experience with homemade sauce and fresh ingredients. Perfect for pasta lovers!',
      imageThumbnail: 'https://example.com/pasta.jpg',
      price: 150,
      hostName: 'John Chef',
      hostRating: 4.8,
      tags: ['Italian', 'Pasta', 'Dinner', 'Vegetarian'],
      allergens: ['Gluten', 'Dairy', 'Eggs'],
      startDate: '2025-12-15',
      startTime: '19:00',
      participantsCount: 5,
      maxAllowedParticipants: 10,
      eventAddress: {
          streetAddress: 'Main St 123',
          postalCode: '1000',
          city: 'Copenhagen',
          region: 'Capital Region'
      },
      userId: 'user-456',
      minAllowedAge: 18,
      maxAllowedAge: 65,
      reservationEndDate: '2025-12-14',
      createdDate: '2025-12-01',
      pricePerSeat: 150,
      isActive: true,
      isPublic: true,
      eventFoodDetails: {
          id: 'food-789',
          eventId: '123',
          dietaryStyle: ['Vegetarian-Friendly', 'Organic'],
          allergens: ['Gluten', 'Dairy', 'Eggs'],
          name: 'Homemade Italian Pasta',
          ingredients: 'Fresh pasta, tomatoes, basil, olive oil, parmesan cheese',
          additionalFoodItems: 'Garlic bread, Italian wine, tiramisu dessert'
      }
  };

  it('should render event name', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('Italian Pasta Night')).toBeInTheDocument();
  });

  it('should render event price', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('DKK 150')).toBeInTheDocument();
  });

  it('should render host information', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText(/Hosted by John Chef/)).toBeInTheDocument();
    expect(screen.getByText(/4.8 ★/)).toBeInTheDocument();
  });

  it('should render location', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('Copenhagen')).toBeInTheDocument();
  });

  it('should render date and time', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText(/2025-12-15/)).toBeInTheDocument();
    expect(screen.getByText(/19:00/)).toBeInTheDocument();
  });

  it('should render participant count', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('5/10 seats')).toBeInTheDocument();
  });

  it('should display only first 3 tags', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('Italian')).toBeInTheDocument();
    expect(screen.getByText('Pasta')).toBeInTheDocument();
    expect(screen.getByText('Dinner')).toBeInTheDocument();
    expect(screen.getAllByText('+1')).toHaveLength(2); // One for tags, one for allergens
  });

  it('should display allergens', () => {
    render(<MealCard {...mockEventData} />);
    
    expect(screen.getByText('Gluten')).toBeInTheDocument();
    expect(screen.getByText('Dairy')).toBeInTheDocument();
    const plusOnes = screen.getAllByText('+1');
    expect(plusOnes.length).toBeGreaterThanOrEqual(1);
  });

  it('should truncate long descriptions', () => {
    const longDescription = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.';
    const eventWithLongDesc = { ...mockEventData, description: longDescription };
    render(<MealCard {...eventWithLongDesc} />);
    
    const description = screen.getByText(/Lorem ipsum dolor sit amet/);
    expect(description.textContent).toContain('...');
  });

  it('should render event image', () => {
    render(<MealCard {...mockEventData} />);
    
    const image = screen.getByAltText('Italian Pasta Night');
    expect(image).toHaveAttribute('src', 'https://example.com/pasta.jpg');
  });

  it('should have link to event details page', () => {
    render(<MealCard {...mockEventData} />);
    
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/viewEventPage?id=123');
  });

  it('should render without allergens', () => {
    const eventWithoutAllergens = { ...mockEventData, allergens: [] };
    render(<MealCard {...eventWithoutAllergens} />);
    
    expect(screen.queryByText('Gluten')).not.toBeInTheDocument();
  });

  it('should handle empty description', () => {
    const eventWithoutDescription = { ...mockEventData, description: '' };
    render(<MealCard {...eventWithoutDescription} />);
    
    expect(screen.queryByText(/Join us/)).not.toBeInTheDocument();
  });
});
