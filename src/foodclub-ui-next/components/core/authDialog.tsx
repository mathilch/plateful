"use client";

import React, { useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";

type AuthDialogProps = {
  open: boolean;
  setOpenAction: (open: boolean) => void;
};

export default function AuthDialog({
  open,
  setOpenAction: setOpen,
}: AuthDialogProps) {
  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const res = await fetch("https://localhost:7083/api/User/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ Email, Password }),
      });

      if (!res.ok) {
        // Try to parse error message from the server, fall back to status text
        let msg = res.statusText;
        try {
          const json = await res.json();

          msg = json?.message || msg;
        } catch (err) {
          /* ignore JSON parse errors */
        }
        setError(msg || "Login failed");
        return;
      }
      // Server returns the token as plain text in the response body.
      const token = await res.text();

      if (!token) {
        setError("Login failed: no token returned");
        return;
      }

      try {
        localStorage.setItem("accessToken", token);
        // notify same-tab listeners that auth state changed
        window.dispatchEvent(new Event("authChanged"));
      } catch (err) {
        // ignore storage errors
        console.warn("Unable to store token in localStorage", err);
      }

      // Success: close dialog. Caller can handle routing / further auth handling.
      setOpen(false);
    } catch (err: any) {
      setError(err?.message || "Network error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Login to your account</DialogTitle>
          <DialogDescription>
            Enter your email below to login to your account
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit}>
          <div className="flex flex-col gap-6">
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="m@example.com"
                required
                value={Email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                type="password"
                required
                value={Password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <a
                href="#"
                className="text-sm justify-self-end underline-offset-4 hover:underline"
              >
                Forgot password?
              </a>
            </div>
          </div>

          {error && (
            <p className="text-sm text-red-600 mt-4" role="alert">
              {error}
            </p>
          )}

          <div className="mt-6">
            <Button
              type="submit"
              disabled={loading}
              className="py-2 px-12 text-white text-base font-bold font-['Poppins'] bg-emerald-800 rounded-xl hover:bg-emerald-700 hover:cursor-pointer"
            >
              {loading ? "Logging in..." : "Login"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
