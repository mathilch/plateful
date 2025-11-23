import { EventOverviewDto } from "@/types/event-details.type";

export const createEventDefaultData: EventOverviewDto = {
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
        participantsCount: 0,
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