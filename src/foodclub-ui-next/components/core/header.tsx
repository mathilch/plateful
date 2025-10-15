"use client"

import Link from "next/link";
import LoginDialog from "./loginDialog";
import { useState } from "react";


export default function FoodAppHeader() {
    const [openAuthDialog, setOpenAuthDialog] = useState(false);

    return (
        <>
            <header className="w-full border-b">
                <div className="mx-auto max-w-5xl px-4">
                    <nav className="flex flex-row justify-center items-center gap-2 sm:gap-4 py-4">
                        <Link href="/" className="rounded-full px-3 py-1 text-sm sm:text-base hover:bg-muted hover:text-foreground transition-colors">Home</Link>
                        <Link href="#" className="rounded-full px-3 py-1 text-sm sm:text-base hover:bg-muted hover:text-foreground transition-colors">Explore</Link>
                        <Link href="createFoodEvent" className="rounded-full px-3 py-1 text-sm sm:text-base hover:bg-muted hover:text-foreground transition-colors">Host a Meal</Link>
                        <Link href="#" className="rounded-full px-3 py-1 text-sm sm:text-base hover:bg-muted hover:text-foreground transition-colors">Deals</Link>
                        <span className="mx-2 hidden sm:inline text-muted-foreground">•</span>
                        <Link href="#" onClick={() => setOpenAuthDialog(true)} className="rounded-full px-3 py-1 text-sm sm:text-base bg-primary text-primary-foreground hover:brightness-105 transition-colors">Log In</Link>
                    </nav>
                </div>
            </header>
            <LoginDialog open={openAuthDialog} setOpenAction={setOpenAuthDialog}/>
        </>
    );
}