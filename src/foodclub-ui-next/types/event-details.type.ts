import { EventFoodDetails } from "./event-food-details.type";
import {EventParticipantDto} from "@/types/event-participant";
import {EventImage} from "@Rameez349/events-api-sdk/generated/model";
import {EventReviewDto} from "@/types/event-review.type";
import {EventAddress} from "@/types/event-address";

export type EventOverviewDto = {
  eventId: string;
  userId: string;
  hostName: string;
  hostRating: number;
  name: string;
  description?: string;
  maxAllowedParticipants: number;
  minAllowedAge: number;
  maxAllowedAge: number;
  startDate: string;
  startTime: string;
  reservationEndDate: string;
  tags: string[];
  allergens?: string[];
  participantsCount: number;
  imageThumbnail: string;
  createdDate: string;
  pricePerSeat: number;
  isActive: boolean;
  isPublic: boolean;
  eventAddress: EventAddress
  eventFoodDetails: EventFoodDetails;
  price: number;
};

export type EventDetails = {
    eventId: string;
    userId: string;
    name: string;
    description: string;
    pricePerSeat: number;
    maxAllowedParticipants: number;
    minAllowedAge: number;
    maxAllowedAge: number;
    startDate: string;
    reservationEndDate: string;
    imageThumbnail: string;
    createdDate: string;
    isActive: boolean;
    isPublic: boolean;
    eventAddress: EventAddress;
    eventFoodDetails: EventFoodDetails;
    eventParticipants: EventParticipantDto[];
    eventImages: EventImage[];
    eventReviews: EventReviewDto[];
    
}
