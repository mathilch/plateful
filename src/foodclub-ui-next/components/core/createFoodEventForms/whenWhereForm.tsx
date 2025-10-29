"use client";

import { useState } from "react";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import OrangeWrapper from "../wrappers/orangeWrapper";
import CreateEventFormInput from "./createEventFormInput";
import InputAddressAutocomplete from "./inputAddressAutocomplete";

export interface LocationDetails {
    streetNumber?: string;
    postalCode?: string;
    city?: string;
    region?: string;
}

export default function WhenWhereForm() {
    const [formLocationDetails, setFormLocationDetails] = useState<LocationDetails | null>(null);

    return (
        // md:grid-cols-[2fr_1fr]
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <ComponentsWrapper id="whenWhereForm">

                <div className="flex gap-5">
                    <CreateEventFormInput
                        id="date"
                        labelText="Date"
                        type="date"
                        // placeholder="e.g., Cozy Curry Night"
                        required
                    />

                    <CreateEventFormInput
                        id="startTime"
                        labelText="Start time"
                        type="time"
                        // placeholder="e.g., Cozy Curry Night"
                        required
                    />

                    <CreateEventFormInput
                        id="endTime"
                        labelText="End time (optional)"
                        type="time"
                        // placeholder="e.g., Cozy Curry Night"
                        required
                    />

                </div>

                {/* TODO: work on address selection options, eg: show list of regions */}

                <InputAddressAutocomplete
                    labelText={"Address autocomplete test"}
                    id={"addressAutocomplete"}
                    setFormLocationDetailsAction={setFormLocationDetails} />

                <CreateEventFormInput
                    id="address"
                    labelText="Address"
                    type="text"
                    placeholder="Street & number"
                    value={formLocationDetails?.streetNumber}
                    onChange={e => setFormLocationDetails(prev => ({
                        ...prev,
                        streetNumber: e.target.value
                    }))}
                    required
                />


                <div className="flex gap-5">
                    <CreateEventFormInput
                        id="postalCode"
                        labelText="Postal code"
                        type="text"
                        placeholder="2200"
                        value={formLocationDetails?.postalCode}
                        onChange={e => setFormLocationDetails(prev => ({
                            ...prev,
                            postalCode: e.target.value
                        }))}
                        required
                    />

                    <CreateEventFormInput
                        id="city"
                        labelText="City"
                        type="text"
                        placeholder="Copenhagen"
                        value={formLocationDetails?.city}
                        onChange={e => setFormLocationDetails(prev => ({
                            ...prev,
                            city: e.target.value
                        }))}
                        required
                    />

                    <CreateEventFormInput
                        id="region"
                        labelText="Region"
                        type="text"
                        placeholder="Capital Region"
                        value={formLocationDetails?.region}
                        onChange={e => setFormLocationDetails(prev => ({
                            ...prev,
                            region: e.target.value
                        }))}
                        required
                    />

                    <CreateEventFormInput
                        id="country"
                        labelText="Country"
                        type="text"
                        placeholder="Denmark"
                        value="Denmark"
                        disabled
                        required
                    />

                </div>


                <OrangeWrapper>
                    <p className="text-muted-gray text-xs font-bold">Your exact address is only shared with confirmed guests.</p>
                </OrangeWrapper>


                {/* </div> */}
            </ComponentsWrapper>


            {/* TODO: work on the map component, remove hardcoded width */}
            <ComponentsWrapper id="locationPreview" className="w-100">
                <h3>Location Preview</h3>
                {/* <MealCard key={123} {...eventDetail} /> */}
            </ComponentsWrapper>


        </div>
    );
}