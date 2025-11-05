"use client";

import { useState } from "react";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import OrangeWrapper from "../wrappers/orangeWrapper";
import CreateEventFormInput from "./createEventFormInput";
import InputAddressAutocomplete from "./inputAddressAutocomplete";
import { useFormWizardContext } from "./formWizardContext";

export interface LocationDetails {
    streetNumber?: string;
    postalCode?: string;
    city?: string;
    region?: string;
}

export default function WhenWhereForm() {
    const [formLocationDetails, setFormLocationDetails] = useState<LocationDetails | null>(null);

    const [formState, formDispatch] = useFormWizardContext();

    return (
        // md:grid-cols-[2fr_1fr] 
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <ComponentsWrapper id="whenWhereForm">

                <div className="flex flex-col md:flex-row gap-5">
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

                <InputAddressAutocomplete
                    labelText={"Enter your address"}
                    placeholder="e.g. Nørrebrostræde 123, 2. tv"
                    id={"addressAutocomplete"}
                    setFormLocationDetailsAction={setFormLocationDetails} />

                <CreateEventFormInput
                    id="address"
                    labelText="Street address"
                    type="text"
                    placeholder="Street & number"
                    value={formLocationDetails?.streetNumber}
                    onChange={e => setFormLocationDetails(prev => ({
                        ...prev,
                        streetNumber: e.target.value
                    }))}
                    required
                />

                <div className="flex">

                    <div className="flex flex-col md:flex-row gap-5 flex-1">
                        <CreateEventFormInput
                            id="postalCode"
                            labelText="Postal code"
                            type="text"
                            placeholder="2200"
                            value={formLocationDetails?.postalCode}
                            wrapperClassName="flex-1"
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
                            wrapperClassName="flex-1"
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
                            wrapperClassName="flex-1"
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
                            wrapperClassName="flex-1"
                            disabled
                            required
                        />

                    </div>
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