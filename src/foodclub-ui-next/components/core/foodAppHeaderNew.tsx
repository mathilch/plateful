"use client"

import Link from "next/link";
import Image from 'next/image'
import { useState } from "react";
import AuthDialog from "./authDialog";



export default function FoodAppHeaderNew() {
    const [openAuthDialog, setOpenAuthDialog] = useState(false);

    return (
        <>
            {/* TODO: move this to a client component */}
            <AuthDialog open={openAuthDialog} setOpenAction={setOpenAuthDialog} />

            <header className="flex justify-evenly items-center gap-5 h-20 outline-1 outline-gray-200">

                <div className="flex h-20 gap-4 items-center">
                    <Link href="/" className="text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">
                        <Image
                            src="/source_logo_plateful.png"
                            alt="Logo"
                            width={73}
                            height={73}
                        />
                    </Link>
                    <Link href="/" className="py-4 px-1 text-primary-green text-xl font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">Plateful DK</Link>
                </div>

                <div className="flex h-14 gap-10">
                    <Link href="#" className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">Discover</Link>
                    <Link href="/createFoodEvent" className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">Host a Meal</Link>
                    <Link href="#" className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">How it Works</Link>
                </div>

                <div className="flex gap-4">
                    <Link href="#" onClick={() => setOpenAuthDialog(true)} className="py-2 px-8 border-1 border-primary-green rounded-[50%] text-primary-green text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors">Log in</Link>
                    <Link href="#" className="py-2 px-12 border-1 border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Sign up</Link>
                </div>


            </header>
        </>
    );
}