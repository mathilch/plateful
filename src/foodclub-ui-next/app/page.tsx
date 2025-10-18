import "./home.css";
import Link from "next/link";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import MealCard from "@/components/core/meal-card/meal-card";
import { getRecentEventsForHomePage } from "@/services/api/events-api.service";

export default function Home() {
  const eventDetails = getRecentEventsForHomePage();

  return (
    <div>
      <div className="banner">
        <div className="left-section">
          <h1>Share meals. Reduce Waste. Meet neighbors.</h1>
          <p>Book or host home-cooked dinners across Denmark.</p>
          <div className="button-group">
            <Link
              href="#"
              className="py-2 px-6 text-white text-base font-bold font-['Poppins'] bg-emerald-800 rounded-xl hover:bg-emerald-700"
            >
              Discover events
            </Link>
            <Link
              href="#"
              className="py-2 px-6 border-2 rounded-xl border-emerald-800 text-emerald-800 text-base font-bold font-['Poppins'] hover:bg-muted"
            >
              Host a meal
            </Link>
          </div>
        </div>
        <div className="right-section">
          <img className="logo" src="/home_logo.png" alt="Fresh produce" />
        </div>
      </div>
      <div className="why-plateful-section">
        <h2>Why Plateful?</h2>
        <div className="platfeful-cards">
          <Card className="max-w-sm border-gray-200 shadow-sm">
            <CardHeader>
              <CardTitle className="text-xl font-bold">
                Affordable Meals
              </CardTitle>
              <CardDescription className="text-md">
                Enjoy home-cooked meals at fair prices while sharing costs.
              </CardDescription>
            </CardHeader>
          </Card>
          <Card className="max-w-sm border-gray-200 shadow-sm">
            <CardHeader>
              <CardTitle className="text-xl font-bold">
                Meet new people
              </CardTitle>
              <CardDescription className="text-md">
                Build local connections over shared meals.
              </CardDescription>
            </CardHeader>
          </Card>
          <Card className="max-w-sm border-gray-200 shadow-sm">
            <CardHeader>
              <CardTitle className="text-xl font-bold">
                Reduce food waste
              </CardTitle>
              <CardDescription className="text-md">
                Cook big, split portions, and save what matters.
              </CardDescription>
            </CardHeader>
          </Card>
        </div>
      </div>
      <div className="upcoming-dinners-section">
        <h2>Upcoming dinners</h2>
        <div className="upcoming-dinner-cards">
          {eventDetails.map((eventDetail) => (
            <MealCard key={eventDetail.id} {...eventDetail} />
          ))}
        </div>
      </div>
    </div>
  );
}
