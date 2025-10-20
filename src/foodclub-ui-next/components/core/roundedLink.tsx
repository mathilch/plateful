import Link from "next/link";


export default function roundedLink({
    children,
    href,
    isActive = false
}: Readonly<{
    children: React.ReactNode;
    href: string,
    isActive?: boolean
}>) {

    return (
        // bg-green-light - active, bg-gray-light - inActive
        <Link href={href} className={`p-2 ${isActive ? "bg-green-light" : "bg-gray-light"} text-primary-green text-xs font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors`}>
            {children}
        </Link>
    );
}