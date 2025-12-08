import { parseJwt, JwtPayload } from '@/lib/jwt-decoder.helper';

describe('jwt-decoder.helper', () => {
  describe('parseJwt', () => {
    it('should decode a valid JWT token', () => {
      // Sample JWT token (header.payload.signature)
      // Payload: {"sub":"123","unique_name":"testuser","email":"test@example.com","exp":1735689600}
      const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjMiLCJ1bmlxdWVfbmFtZSI6InRlc3R1c2VyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwiZXhwIjoxNzM1Njg5NjAwfQ.4Kq0L5QZ0wX8Z9X0Z0Z0Z0Z0Z0Z0Z0Z0Z0Z0Z0Z0Z0';
      
      const decoded = parseJwt(token);
      
      expect(decoded).toBeDefined();
      expect(decoded.sub).toBe('123');
      expect(decoded.unique_name).toBe('testuser');
      expect(decoded.email).toBe('test@example.com');
      expect(decoded.exp).toBe(1735689600);
    });

    it('should throw an error for invalid JWT token', () => {
      const invalidToken = 'invalid.token.here';
      
      expect(() => parseJwt(invalidToken)).toThrow();
    });

    it('should decode token with custom payload type', () => {
      interface CustomPayload extends JwtPayload {
        role: string;
      }
      
      const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjMiLCJ1bmlxdWVfbmFtZSI6InRlc3R1c2VyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwiZXhwIjoxNzM1Njg5NjAwLCJyb2xlIjoiYWRtaW4ifQ.xyz';
      
      const decoded = parseJwt<CustomPayload>(token);
      
      expect(decoded.sub).toBe('123');
      expect(decoded.role).toBe('admin');
    });
  });
});
