"use client";

import { useState } from "react";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import OrangeWrapper from "../wrappers/orangeWrapper";
import CreateEventFormCounter from "./createEventFormCounter";
import CreateEventFormInput from "./createEventFormInput";
import CreateEventFormTextarea from "./createEventFormTextarea";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";


export default function PriceCapacityForm() {
    const [seatsAvailable, setSeatsAvailable] = useState(6);
    const [pricePerSeat, setPricePerSeat] = useState(65);
    const [ingredientCost] = useState(228);

    // Calculations
    const totalRevenue = seatsAvailable * pricePerSeat;
    const platformFee = 30;
    const potentialEarnings = totalRevenue - platformFee - ingredientCost;

    return (
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <ComponentsWrapper id="priceCapacityForm">

                <div className="flex gap-5">
                    <CreateEventFormCounter
                        id="seatsAvailable"
                        labelText="Seats available"
                        value={seatsAvailable}
                        onChange={setSeatsAvailable}
                        min={1}
                        max={20}
                        step={1}
                    />

                    <CreateEventFormInput
                        id="pricePerSeat"
                        labelText="Price per seat (DKK)"
                        type="number"
                        value={pricePerSeat}
                        onChange={(e) => setPricePerSeat(Number(e.target.value))}
                        min={0}
                        required
                    />

                    <div className="grid gap-2 flex-1">
                        <Label className="text-xs font-bold text-muted-foreground">
                            Payment method
                        </Label>
                        <div className="h-10 bg-gray-50 rounded-md border border-input px-3 py-2 text-sm text-muted-foreground flex items-center">
                            Pay offline (cash/MobilePay)
                        </div>
                    </div>
                </div>

                <div className="grid gap-2">
                    <Label className="text-xs font-bold text-muted-foreground">
                        Ingredient cost estimator
                    </Label>
                    
                    <OrangeWrapper>
                        <p className="text-sm font-semibold text-gray-800">
                            Estimated cost per guest: 38 DKK • Suggested price: 55 – 70 DKK
                        </p>
                        <p className="text-xs text-gray-600">
                            Adjust price to balance affordability & coverage.
                        </p>
                    </OrangeWrapper>
                </div>

                <div className="grid gap-2">
                    <Label className="text-xs font-bold text-muted-foreground">
                        Cancellation policy
                    </Label>
                    <div className="h-10 bg-gray-50 rounded-md border border-input px-3 py-2 text-sm text-muted-foreground flex items-center">
                        Flexible (cancel up to 24h before)
                    </div>
                </div>

                <CreateEventFormTextarea
                    id="notesForGuests"
                    labelText="Notes for guests (optional)"
                    placeholder="e.g., BYOB, slippers welcome, allergy heads-up..."
                    className="h-32"
                />

            </ComponentsWrapper>


            <ComponentsWrapper id="earningsSummary">
                <h3 className="text-lg font-semibold">Earnings summary</h3>

                <div className="space-y-3">
                    <div className="flex justify-between text-sm">
                        <span className="text-muted-foreground">Seats × Price</span>
                        <span className="font-medium">
                            {seatsAvailable} × {pricePerSeat} = DKK {totalRevenue}
                        </span>
                    </div>

                    <div className="flex justify-between text-sm">
                        <span className="text-muted-foreground">Platform fee</span>
                        <span className="font-medium">DKK {platformFee}</span>
                    </div>

                    <div className="flex justify-between text-sm">
                        <span className="text-muted-foreground">Estimated ingredients</span>
                        <span className="font-medium">≈ DKK {ingredientCost}</span>
                    </div>

                    <div className="h-px bg-gray-200" />

                    <div className="flex justify-between items-center pt-2">
                        <span className="font-semibold text-base">Potential earnings</span>
                        <span className="text-xl font-bold text-emerald-800">
                            ≈ DKK {potentialEarnings}
                        </span>
                    </div>
                </div>

                <Button 
                    className="w-full bg-emerald-800 hover:bg-emerald-700 text-white font-semibold h-11"
                >
                    Save & Continue
                </Button>
            </ComponentsWrapper>


        </div>
    );
}