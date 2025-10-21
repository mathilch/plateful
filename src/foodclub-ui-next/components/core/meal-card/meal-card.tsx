import { EventDetails } from "@/types/event-details.type";
import { Badge } from "../../ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../ui/card";
import "./meal-card.css";

export default function MealCard(eventDetails: EventDetails) {
  return (
    <Card className="max-w-sm border-gray-200 shadow-sm">
      <CardContent>
        <div className="h-48 w-84 overflow-hidden">
          <img
            src={eventDetails.imageUrl}
            className="rounded-t-lg w-full h-full object-cover"
          />
        </div>
      </CardContent>
      <CardHeader>
        <CardTitle className="text-lg font-bold">
          {eventDetails.title}
        </CardTitle>
        <CardDescription className="text-md">
          <div className="host-details">
            <div>Hosted by {eventDetails.hostName}</div>
            <div>{eventDetails.hostRating}</div>
          </div>
          <div className="tags">
            {eventDetails.tags.map((tag) => (
              <Badge key={tag} variant="success">
                {tag}
              </Badge>
            ))}
          </div>
          <div>
            {eventDetails.date} · {eventDetails.time}
          </div>
          <div className="reservation-status-and-pricing">
            <span className="font-bold">
              {eventDetails.seatsAvailable}/{eventDetails.totalSeats} seats left
            </span>
            <span className="font-bold text-green-800">
              DKK: {eventDetails.price}
            </span>
          </div>
        </CardDescription>
      </CardHeader>
    </Card>
  );
}
