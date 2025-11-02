import { EventFoodDetails } from "./event-food-details.type";

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
