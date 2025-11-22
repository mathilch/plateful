"use client";

import { CreateEventRequestDto } from "@Rameez349/events-api-sdk/dist/generated/model/createEventRequestDto";
import { postEvent } from "@/services/api/events-api.service";
import { useFormWizardContext } from "./formWizardContext";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import MealCard from "../meal-card/meal-card";
import { EventOverviewDto } from "@/types/event-details.type";
import { parse } from 'date-fns';


export default function PreviewForm() {
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

    // Mock data for CreateEventRequestDto
    const mockCreateEventRequest = {
        name: "(test)Noodles & More at Vesterport",
        description: "Join us for a delightful evening of noodles and more!",
        maxAllowedParticipants: 7,
        minAllowedAge: 0,
        maxAllowedAge: 99,
        startDate: "2024-06-15",
        reservationEndDate: "2024-06-14",
        price: 55,
        imageThumbnail: "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
        isPublic: true,
        streetAddress: "Vesterport 123",
        postalCode: "2100",
        city: "Copenhagen",
        region: "Capital Region",
        country: "Denmark",
        eventFoodDetails: {
            name: "Noodles & More",
            ingredients: "Noodles, vegetables, spices",
            additionalFoodItems: "Dessert included",
        },
    } as CreateEventRequestDto;

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();

        const startDatetimeStr = formState.whenWhere?.date + " " + formState.whenWhere?.startTime;
        const endDatetimeStr = formState.whenWhere?.date + " " + formState.whenWhere?.endTime;

        // parse as local time
        const eventStartDateTime = parse(startDatetimeStr, "yyyy-MM-dd HH:mm", new Date());
        const eventEndDateTime = formState.whenWhere?.endTime ? parse(endDatetimeStr, "yyyy-MM-dd HH:mm", new Date()) : undefined;
        const reservationEndDateTime = new Date(eventStartDateTime)
        reservationEndDateTime.setHours(reservationEndDateTime.getHours() - 1);

        // TODO: check if basics are null
        const createEventReq: CreateEventRequestDto = {
            name: formState.basics?.title ?? "",
            description: formState.basics?.description ?? "",
            pricePerSeat: formState.priceCapacity?.pricePerSeat ?? 0,
            maxAllowedParticipants: formState.priceCapacity?.seatsAvailable ?? 0,
            minAllowedAge: 0,
            maxAllowedAge: 99,
            startDate: eventStartDateTime.toISOString(),
            reservationEndDate: reservationEndDateTime.toISOString(),
            endDate: eventEndDateTime ? eventEndDateTime.toISOString() : undefined,
            imageThumbnail: "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
            isPublic: true,
            eventFoodDetails: {
                dietaryStyles: formState.dietAllergens?.dietaryPreferences ?? [],
                allergens: formState.dietAllergens?.allergens ?? [],
            },
            images: [],
            streetAddress: formState.whenWhere?.streetAddress ?? "",
            postalCode: formState.whenWhere?.postalCode ?? "",
            city: formState.whenWhere?.city ?? "",
            region: formState.whenWhere?.region ?? "",
        };

        const res = await postEvent(createEventReq);
        console.log("Event created result:", res);
    }

    return (
        <div className="flex justify-center">

            <ComponentsWrapper id="livePreview">
                <form id="livePreviewForm" className="contents" onSubmit={onSubmit}>
                    <h3>Live Preview</h3>
                    <MealCard key={123} {...eventDetail} />
                    <button type="submit" form="livePreviewForm" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button>
                </form>
            </ComponentsWrapper>

        </div>
    );
}