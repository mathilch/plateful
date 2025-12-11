import { jwtDecode } from "jwt-decode";

export interface JwtPayload {
  sub: string; // subject (user id)
  unique_name: string; // user name
  email: string;
  exp: number; // expiration timestamp
  birthdate?: string;
}

export function parseJwt<T = JwtPayload>(token: string): T {
  return jwtDecode<T>(token);
}
