import { EventFoodDetails } from "./event-food-details.type";
import {EventParticipantDto} from "@/types/event-participant";
import {EventImage} from "@Rameez349/events-api-sdk/generated/model";
import {EventReviewDto} from "@/types/event-review.type";

export type EventOverviewDto = {
  eventId: string;
  userId: string;
  hostName: string;
  hostRating: number;
  name: string;
  maxAllowedParticipants: number;
  minAllowedAge: number;
  maxAllowedAge: number;
  startDate: string;
  startTime: string;
  reservationEndDate: string;
  tags: string[];
  participantsCount: number;
  imageThumbnail: string;
  createdDate: string;
  price: number;
  isActive: boolean;
  isPublic: boolean;
  eventFoodDetails: EventFoodDetails;
};

export type EventDetails = {
    eventId: string;
    userId: string;
    name: string;
    description: string;
    maxAllowedParticipants: number;
    minAllowedAge: number;
    maxAllowedAge: number;
    startDate: string;
    reservationEndDate: string;
    imageThumbnail: string;
    createdDate: string;
    isActive: boolean;
    isPublic: boolean;
    eventFoodDetails: EventFoodDetails;
    eventParticipants: EventParticipantDto[];
    eventImages: EventImage[];
    eventReviews: EventReviewDto[];
    
}
