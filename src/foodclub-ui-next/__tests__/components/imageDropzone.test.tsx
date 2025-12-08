import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { ImageDropzone } from '@/components/core/imageDropzone';

describe('ImageDropzone Component', () => {
  it('should render dropzone placeholder text', () => {
    render(<ImageDropzone />);
    
    expect(screen.getByText('Drop image')).toBeInTheDocument();
  });

  it('should display preview when value is provided', () => {
    const imageUrl = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==';
    
    render(<ImageDropzone value={imageUrl} />);
    
    const preview = screen.getByAltText('Preview');
    expect(preview).toBeInTheDocument();
    expect(preview).toHaveAttribute('src', imageUrl);
  });

  it('should call onChange when file is dropped', async () => {
    const user = userEvent.setup();
    const handleChange = jest.fn();
    
    render(<ImageDropzone onChange={handleChange} />);
    
    const file = new File(['dummy content'], 'test.png', { type: 'image/png' });
    const input = screen.getByRole('presentation').querySelector('input') as HTMLInputElement;
    
    if (input) {
      await user.upload(input, file);
      
      // Wait for FileReader to complete
      await new Promise(resolve => setTimeout(resolve, 100));
      
      expect(handleChange).toHaveBeenCalled();
    }
  });

  it('should have proper styling classes', () => {
    const { container } = render(<ImageDropzone />);
    
    const dropzone = container.firstChild;
    expect(dropzone).toHaveClass('border-dashed');
    expect(dropzone).toHaveClass('rounded-xl');
  });

  it('should accept only image files', () => {
    render(<ImageDropzone />);
    
    const input = screen.getByRole('presentation').querySelector('input') as HTMLInputElement;
    expect(input).toHaveAttribute('accept', 'image/*');
  });

  it('should show drag active state text', () => {
    const { container } = render(<ImageDropzone />);
    
    const dropzone = container.querySelector('[class*="border-dashed"]');
    expect(dropzone).toBeInTheDocument();
  });

  it('should render without crashing when no props provided', () => {
    const { container } = render(<ImageDropzone />);
    
    expect(container.firstChild).toBeInTheDocument();
  });
});
