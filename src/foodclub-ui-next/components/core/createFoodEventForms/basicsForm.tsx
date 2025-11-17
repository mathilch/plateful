"use client";

import { Label } from "@/components/ui/label";
import { ImageDropzone } from "../imageDropzone";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import CreateEventFormInput from "./createEventFormInput";
import CreateEventFormTextarea from "./createEventFormTextarea";
import MealCard from "../meal-card/meal-card";
import { EventOverviewDto } from "@/types/event-details.type";
import { FormBasics, FormWizardActionType, FormWizardStep, useFormWizardContext } from "./formWizardContext";
import { useRouter } from "next/navigation";


export default function BasicsForm() {
    let eventDetail: EventOverviewDto = {
        eventId: "5d92825a-133e-4ac8-8fa5-da8696a486a8",
        userId: "user-123",
        hostName: "Anna S.",
        hostRating: 4.8,
        name: "Noodles & More at Vesterport",
        maxAllowedParticipants: 7,
        minAllowedAge: 18,
        maxAllowedAge: 99,
        startDate: "2024-06-15",
        startTime: "19:00",
        reservationEndDate: "2024-06-14",
        tags: ["Vegetarian", "Gluten-Free"],
        participantsCount: 2,
        imageThumbnail:
            "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
        createdDate: "2024-06-01",
        price: 55,
        isActive: true,
        isPublic: true,
        eventFoodDetails: {
            id: "food-123",
            eventId: "5d92825a-133e-4ac8-8fa5-da8696a486a8",
            name: "Noodles & More",
            ingredients: "Noodles, vegetables, spices",
            additionalFoodItems: "Dessert included",
        },
    };

    const [formState, formDispatch] = useFormWizardContext();
    const router = useRouter();

    function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);


        const formDataObj: FormBasics = {
            title: formData.get("mealTitle") as string,
            description: formData.get("descriptionTextArea") as string,
        };

        formDispatch({ type: FormWizardActionType.Set, step: FormWizardStep.CreateFoodEvent, value: { basics: formDataObj } });

        router.push("/createFoodEvent/step/2");
    }


    return (
        // md:grid-cols-[2fr_1fr] 
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <form id="createFoodEventForm" onSubmit={onSubmit} >
                <ComponentsWrapper id="basicsForm">

                    <CreateEventFormInput
                        id="mealTitle"
                        labelText="Meal Title"
                        type="text"
                        placeholder="e.g., Cozy Curry Night"
                        required
                    />

                    <CreateEventFormTextarea
                        id="descriptionTextArea"
                        labelText="Description"
                        required
                        placeholder="Describe your menu, vibe, BYOB, etc."
                        className="h-48 bg-gray-50 text-sm font-normal text-[#9CA3AF] font-['Poppins']"
                    />



                    <div className="grid gap-2">
                        <Label htmlFor="coverPhotoDropzone" className="text-xs text-muted-gray font-bold">Cover Photo</Label>
                        <ImageDropzone />
                    </div>


                    {/* <button type="submit" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button> */}


                    {/* </div> */}
                </ComponentsWrapper>
            </form>


            <div className="flex flex-col">
                <ComponentsWrapper id="livePreview">
                    <h3>Live Preview</h3>
                    <MealCard key={123} {...eventDetail} />
                </ComponentsWrapper>

                 <button type="submit" form="createFoodEventForm" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button>
            </div>


        </div>
    );
}