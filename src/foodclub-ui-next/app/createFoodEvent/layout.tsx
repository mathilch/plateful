import RoundedLink from "@/components/core/roundedLink";


export default function CreateFormLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
    return (
        <>
            <h2>Create a new event</h2>

            <div className="">
                <RoundedLink href="/createFoodEvent/step/1" isActive={true} >Basics</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/2">When & Where</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/3">Pricing & Capacity</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/4">Diet & Allergens</RoundedLink>
                <RoundedLink href="/createFoodEvent/step/5">Preview</RoundedLink>
            </div>

            {children}
        </>
    );
}