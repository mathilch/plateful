import RoundedLink from "@/components/core/roundedLink";


export default function CreateFormLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
    return (
        <div className="flex flex-col gap-5 p-5 ">
            <h2>Create a new event</h2>

            <div className="flex gap-3">
                <RoundedLink href="/createFoodEvent/step/1" isActive={true} >1. Basics</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/2">2. When & Where</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/3">3. Pricing & Capacity</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/4">4. Diet & Allergens</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/5">5. Preview</RoundedLink>
            </div>

            {children}
        </div>
    );
}