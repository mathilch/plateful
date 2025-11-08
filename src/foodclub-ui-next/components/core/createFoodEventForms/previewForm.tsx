"use client";

import { postEvent } from "@/services/api/events-api.service";
import { useFormWizardContext } from "./formWizardContext";
import { CreateEventRequestDto } from "@prvacy/events-api-sdk/dist/generated/model";


export default function PreviewForm() {
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
        await postEvent(mockCreateEventRequest)
    }

    return (
        <form>
            <p>This is the step5.</p>
            <input type="submit" onClick={onSubmit} onSubmit={onSubmit} />
        </form>
    );
}