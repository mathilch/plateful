

import Link from "next/link";

export default function FoodAppHeaderNew() {
    return (
        <header className="flex justify-evenly border-3 gap-5 border-black  m-2">

            <div>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">Plateful DK</Link>
            </div>

            <div>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">Discover</Link>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">Host a Meal</Link>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">How it Works</Link>
            </div>

            <div>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">Log in</Link>
                <Link href="#" className="border-x-3 border-black py-4 px-8 hover:bg-muted hover:text-foreground transition-colors">Sign up</Link>
            </div>

        </header>
    );
}