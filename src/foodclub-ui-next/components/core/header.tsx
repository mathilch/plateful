"use client";

import Link from "next/link";
import Image from "next/image";
import { useState, useEffect } from "react";
import AuthDialog from "./authDialog";
import { jwtDecode } from "jwt-decode";

export default function AppHeader() {
  const [openAuthDialog, setOpenAuthDialog] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);

  useEffect(() => {
    checkToken();

    const onStorage = (e: StorageEvent) => {
      if (e.key === "accessToken") checkToken();
    };

    const onAuthChanged = () => checkToken();

    window.addEventListener("storage", onStorage);
    window.addEventListener("authChanged", onAuthChanged);

    return () => {
      window.removeEventListener("storage", onStorage);
      window.removeEventListener("authChanged", onAuthChanged);
    };
  }, []);

  function handleLogout() {
    try {
      localStorage.removeItem("accessToken");
    } catch (err) {
      /* ignore */
    }
    setIsAuthenticated(false);
    // notify other listeners (same-tab)
    window.dispatchEvent(new Event("authChanged"));
  }

  function checkToken() {
    const token = localStorage.getItem("accessToken");
    if (token) {
      const decoded: any = jwtDecode(token);
      setUser(decoded.unique_name);

      setIsAuthenticated(true);
    }
  }

  return (
    <>
      <AuthDialog open={openAuthDialog} setOpenAction={setOpenAuthDialog} />
      <header className="flex justify-evenly items-center gap-5 h-20 outline-1 outline-gray-200">
        <div className="flex h-20 gap-4 items-center">
          <Link
            href="/"
            className="text-gray-800 text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            <Image
              src="/source_logo_plateful.png"
              alt="Logo"
              width={73}
              height={73}
            />
          </Link>
          <Link
            href="/"
            className="py-4 px-1 text-emerald-800 text-xl font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            Plateful DK
          </Link>
        </div>

        <div className="flex h-14 gap-10">
          <Link
            href="#"
            className="py-4 px-1 text-gray-800 text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            Discover
          </Link>
          <Link
            href="#"
            className="py-4 px-1 text-gray-800 text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            How it Works
          </Link>
          {isAuthenticated ? (
            <Link
              href="/createFoodEvent"
              className="py-4 px-1 text-gray-800 text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
            >
              Host a Meal
            </Link>
          ) : (
            <></>
          )}
        </div>
        {!isAuthenticated ? (
          <div className="flex gap-4">
            <Link
              href="#"
              onClick={() => setOpenAuthDialog(true)}
              className="py-2 px-8 border-2 rounded-xl border-emerald-800 text-emerald-800 text-base font-bold font-['Poppins'] hover:bg-muted"
            >
              Log in
            </Link>
            <Link
              href="#"
              className="py-2 px-12 text-white text-base font-bold font-['Poppins'] bg-emerald-800 rounded-xl hover:bg-emerald-700"
            >
              Sign up
            </Link>
          </div>
        ) : (
          <div className="flex gap-4">
            <span className="py-2 px-8 text-gray-800 text-base font-bold font-['Poppins']">
              Welcome, {user}
            </span>
            <button
              onClick={handleLogout}
              className="py-2 px-8 border-2 rounded-xl border-emerald-800 text-emerald-800 text-base font-bold font-['Poppins'] hover:bg-muted"
            >
              Sign out
            </button>
          </div>
        )}
      </header>
    </>
  );
}
