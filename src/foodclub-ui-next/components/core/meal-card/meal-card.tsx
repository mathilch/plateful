import { EventOverviewDto } from "@/types/event-details.type";
import { Badge } from "../../ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../ui/card";
import "./meal-card.css";

export default function MealCard(eventDetails: EventOverviewDto) {
  return (
    <Card className="max-w-sm border-gray-200 shadow-sm">
      <CardContent>
        <div className="overflow-hidden">
          <img
            src={eventDetails.imageThumbnail}
            className="rounded-t-lg w-full h-full object-cover"
          />
        </div>
      </CardContent>
      <CardHeader>
        <CardTitle className="text-lg font-bold">
          {eventDetails.name}
        </CardTitle>
        <CardDescription className="text-md">
          {eventDetails.description && (
            <div className="mb-2 text-gray-700">
              {eventDetails.description}
            </div>
          )}
          <div className="host-details">
            <div>Hosted by {eventDetails.hostName} · {eventDetails.hostRating} ★</div>
          </div>
          <div className="tags flex flex-wrap gap-1">
            {eventDetails.tags.map((tag) => (
              <Badge key={tag} variant="success">
                {tag}
              </Badge>
            ))}
          </div>
          {eventDetails.allergens && eventDetails.allergens.length > 0 && (
            <div className="allergens flex flex-wrap gap-1 mt-2 mb-2">
              {eventDetails.allergens.map((allergen) => (
                <Badge key={allergen} variant="warning" className="bg-orange-200 text-orange-900 border-none">
                  {allergen}
                </Badge>
              ))}
            </div>
          )}
          <div>
            {eventDetails.startDate} · {eventDetails.startTime}
          </div>
          <div className="reservation-status-and-pricing">
            <span className="font-bold">
              {eventDetails.participantsCount}/{eventDetails.maxAllowedParticipants} seats reserved
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
