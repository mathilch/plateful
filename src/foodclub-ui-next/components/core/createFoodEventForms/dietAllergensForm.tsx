"use client";

import { useState } from "react";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import CreateEventFormTextarea from "./createEventFormTextarea";
import { Label } from "@/components/ui/label";
import { Check } from "lucide-react";


export default function DietAllergensForm() {
    const [selectedDietaryStyles, setSelectedDietaryStyles] = useState<string[]>([
        "Vegetarian",
        "Vegan",
        "Pescatarian"
    ]);

    const [selectedAllergens, setSelectedAllergens] = useState<string[]>([
        "Gluten",
        "Nuts",
        "Dairy",
        "Eggs"
    ]);

    const dietaryStyles = [
        { id: "Vegetarian", label: "Vegetarian" },
        { id: "Vegan", label: "Vegan" },
        { id: "Pescatarian", label: "Pescatarian" },
        { id: "Halal-friendly", label: "Halal-friendly" },
        { id: "Kosher-friendly", label: "Kosher-friendly" },
    ];

    const allergens = [
        { id: "Gluten", label: "Gluten" },
        { id: "Nuts", label: "Nuts" },
        { id: "Dairy", label: "Dairy" },
        { id: "Eggs", label: "Eggs" },
        { id: "Soy", label: "Soy" },
        { id: "Shellfish", label: "Shellfish" },
        { id: "Sesame", label: "Sesame" },
        { id: "Mustard", label: "Mustard" },
    ];

    const toggleDietaryStyle = (style: string) => {
        setSelectedDietaryStyles(prev =>
            prev.includes(style)
                ? prev.filter(s => s !== style)
                : [...prev, style]
        );
    };

    const toggleAllergen = (allergen: string) => {
        setSelectedAllergens(prev =>
            prev.includes(allergen)
                ? prev.filter(a => a !== allergen)
                : [...prev, allergen]
        );
    };

    return (
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <ComponentsWrapper id="dietAllergensForm">

                <div className="grid gap-2">
                    <Label className="text-xs font-bold text-muted-foreground">
                        Dietary style (select all that apply)
                    </Label>
                    <div className="flex flex-wrap gap-2">
                        {dietaryStyles.map((style) => {
                            const isSelected = selectedDietaryStyles.includes(style.id);
                            return (
                                <button
                                    key={style.id}
                                    type="button"
                                    onClick={() => toggleDietaryStyle(style.id)}
                                    className={`inline-flex items-center gap-1.5 px-4 py-1.5 rounded-full text-sm font-medium transition-colors ${
                                        isSelected
                                            ? "bg-emerald-100 text-emerald-800 border border-emerald-600"
                                            : "bg-gray-100 text-gray-600 border border-gray-300"
                                    }`}
                                >
                                    {isSelected && <Check className="h-4 w-4" />}
                                    {style.label}
                                </button>
                            );
                        })}
                    </div>
                </div>

                <div className="grid gap-2">
                    <Label className="text-xs font-bold text-muted-foreground">
                        Allergens clearly labeled
                    </Label>
                    <div className="flex flex-wrap gap-2">
                        {allergens.map((allergen) => {
                            const isSelected = selectedAllergens.includes(allergen.id);
                            return (
                                <button
                                    key={allergen.id}
                                    type="button"
                                    onClick={() => toggleAllergen(allergen.id)}
                                    className={`px-4 py-1.5 rounded-full text-sm font-medium transition-colors ${
                                        isSelected
                                            ? "bg-orange-200 text-orange-900 border border-orange-400"
                                            : "bg-gray-100 text-gray-600 border border-gray-300"
                                    }`}
                                >
                                    {allergen.label}
                                </button>
                            );
                        })}
                    </div>
                </div>

                <CreateEventFormTextarea
                    id="notesForGuests"
                    labelText="Notes for guests (optional)"
                    placeholder="e.g., Traces of nuts possible; soy-free substitutes used."
                    className="h-32"
                />

            </ComponentsWrapper>


        </div>
    );
}