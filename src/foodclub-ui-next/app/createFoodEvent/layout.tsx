"use client";

import FormWizardProvider from "@/components/core/createFoodEventForms/formWizardContext";
import RoundedLink from "@/components/core/roundedLink";
import { useState } from "react";


export default function CreateFormLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  const activeButtonState = useState<string>("/createFoodEvent/step/1");

  return (
    <div className="flex flex-col gap-5 p-5 ">
      <h2>Create a new event</h2>

      <div className="flex gap-3">
        <RoundedLink href="/createFoodEvent/step/1" activeButtonState={activeButtonState}>1. Basics</RoundedLink>
        <RoundedLink href="/createFoodEvent/step/2" activeButtonState={activeButtonState}>2. When & Where</RoundedLink>
        <RoundedLink href="/createFoodEvent/step/3" activeButtonState={activeButtonState}>3. Pricing & Capacity</RoundedLink>
        <RoundedLink href="/createFoodEvent/step/4" activeButtonState={activeButtonState}>4. Diet & Allergens</RoundedLink>
        <RoundedLink href="/createFoodEvent/step/5" activeButtonState={activeButtonState}>5. Preview</RoundedLink>
      </div>

      <FormWizardProvider>{children}</FormWizardProvider>

    </div>
  );
}