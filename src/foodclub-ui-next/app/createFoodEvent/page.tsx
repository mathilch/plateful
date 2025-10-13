import Link from "next/link";

export default function CreateFoodEvent() {
    return (
        <>
            <Link href="/" className="rounded-full px-3 py-1 text-sm sm:text-base hover:bg-muted hover:text-foreground transition-colors">Home</Link>
            <p>The page for food event creation.</p>
        </>
    );
}