"use client";

import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import { EventDetails } from "@/types/event-details.type";
import { UserDetails } from "@/types/user-details.type";
import { EventReviewDto } from "@/types/event-review.type";

import { JwtPayload, parseJwt } from "@/lib/jwt-decoder.helper";
import {
  getEventById,
  withdrawFromEvent,
  submitEventReview,
  getEventReviewsByEventId,
} from "@/services/api/events-api.service";
import { signUpForEvent } from "@/services/api/events-api.service";
import { getUserById } from "@/services/api/user-api.service";
import LocationMiniMap from "@/components/core/createFoodEventForms/locationMiniMap";

export default function EventDetailsClient() {
  const searchParams = useSearchParams();
  const id = searchParams.get("id");
  const router = useRouter();

  const [event, setEvent] = useState<EventDetails | null>(null);
  const [host, setHost] = useState<UserDetails | null>(null);
  const [reviews, setReviews] = useState<EventReviewDto[]>([]);
  const [reviewStars, setReviewStars] = useState(5);
  const [reviewComment, setReviewComment] = useState("");
  const [isSubmittingReview, setIsSubmittingReview] = useState(false);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const storedToken = localStorage.getItem("accessToken");
    setToken(storedToken);
  }, []);

  const getInitials = (name?: string) => {
    if (!name) return "";
    return name
      .split(" ")
      .filter(Boolean)
      .map((word) => word[0])
      .join("")
      .toLowerCase();
  };
  const ingredients = event?.eventFoodDetails?.ingredients;
  const ingredientsArray = Array.isArray(ingredients)
    ? ingredients
    : typeof ingredients === "string"
      ? ingredients.split(",").map((i) => i.trim())
      : [];

  useEffect(() => {
    if (!id) return;
    try {
      (async () => {
        try {
          const eventData = await getEventById(id as string);
          setEvent(eventData);
          const hostData = await getUserById(eventData!.userId);
          setHost(hostData);
        } catch (err) {
          console.error("Failed to fetch event details:", err);
        }
      })();
    } catch (err) {
      console.error("Invalid token:", err);
    }
  }, [id, router]);

  useEffect(() => {
    if (!event?.eventId) return;

    (async () => {
      try {
        const reviewsData = await getEventReviewsByEventId(event.eventId);
        setReviews(reviewsData);
      } catch (err) {
        console.error("Failed to fetch reviews:", err);
      }
    })();
  }, [event?.eventId]);

  const getReserveButtonProps = () => {
    if (!token || !event)
      return {
        text: "Reserve a seat",
        disabled: true,
        reason: "Log in required",
      };

    const decoded = parseJwt<JwtPayload>(token);
    const userId = decoded.sub;
    const birthdate = decoded.birthdate;

    if (!birthdate)
      return {
        text: "Unable to sign up",
        disabled: true,
        reason: "User needs to set their birthday",
      };

    const today = new Date();
    const birthday = new Date(birthdate);
    let age = today.getFullYear() - birthday.getFullYear();
    const monthDiff = today.getMonth() - birthday.getMonth();
    const dayDiff = today.getDate() - birthday.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) age--;

    const isParticipating = event.eventParticipants.some(
      (p) => p.userId == userId
    );
    const spotsLeft =
      event.maxAllowedParticipants - event.eventParticipants.length;
    const correctAge = event.minAllowedAge <= age && age <= event.maxAllowedAge;

    if (spotsLeft <= 0)
      return {
        text: "Unable to sign up",
        disabled: true,
        reason: "No more space",
      };
    if (age > 120)
      return {
        text: "Unable to sign up",
        disabled: true,
        reason: "User need to set his birthday before signing up",
      };
    if (!correctAge)
      return {
        text: "Unable to sign up",
        disabled: true,
        reason: "Age constraints",
      };
    if (isParticipating) return { text: "Cancel reservation", disabled: false };

    return { text: "Reserve a seat", disabled: false };
  };

  const reserveButtonProps = getReserveButtonProps();

  const handleReserveSeat = async () => {
    if (!event) return;

    try {
      if (!token) return;

      const decoded = parseJwt(token);
      const userId = decoded.sub;

      if (event.eventParticipants.every((p) => p.userId != userId)) {
        await signUpForEvent(event.eventId, token);
      } else {
        await withdrawFromEvent(event.eventId, token);
      }
      const updatedEvent = await getEventById(event.eventId);
      setEvent(updatedEvent);
    } catch (err) {
      console.error("Failed to update event participant", err);
    }
  };

  const handleSubmitReview = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!event || !reviewComment.trim()) return;

    setIsSubmittingReview(true);
    try {
      if (!token) return;

      await submitEventReview(event.eventId, reviewStars, reviewComment, token);

      // Refresh reviews to show new review
      const updatedReviews = await getEventReviewsByEventId(event.eventId);
      setReviews(updatedReviews);

      // Reset form
      setReviewStars(5);
      setReviewComment("");
    } catch (err) {
      console.error("Failed to submit review:", err);
      alert("Failed to submit review. Please try again.");
    } finally {
      setIsSubmittingReview(false);
    }
  };

  return (
    <main className="max-w-6xl mx-auto px-4 py-10">
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-10">
        <div className="lg:col-span-2">
          <div className="w-full h-80 bg-amber-100 rounded-xl">
            {event?.imageThumbnail && (
              <img
                src={event?.imageThumbnail}
                className="w-full h-full object-cover rounded-xl"
              />
            )}
          </div>
        </div>

        <div className="bg-white border rounded-xl p-6 h-fit shadow-sm">
          <div className="text-base font-semibold text-gray-600">
            {event?.startDate} · {event?.startTime}
          </div>
          <div className="text-sm text-gray-600 mb-4">
            {event?.eventAddress?.streetAddress},{" "}
            {event?.eventAddress?.postalCode} {event?.eventAddress?.city}
          </div>

          <div className="mb-4">
            <div className="flex items-center justify-between text-sm mb-1">
              <span>
                {event?.eventParticipants.length} /{" "}
                {event?.maxAllowedParticipants} seats left
              </span>
            </div>

            <div className="w-full bg-gray-200 h-2 rounded-full">
              <div
                className="bg-green-700 h-2 rounded-full"
                style={{
                  width: `${event && event.maxAllowedParticipants > 0
                      ? (event.eventParticipants.length /
                        event.maxAllowedParticipants) *
                      100
                      : 0
                    }`,
                }}
              />
            </div>
          </div>

          <div className="text-gray-800 text-lg font-semibold mb-6">
            Price per seat <br />
            <span className="text-green-800 text-2xl font-bold">
              DKK {event?.pricePerSeat}
            </span>
          </div>

          <button
            onClick={handleReserveSeat}
            disabled={reserveButtonProps.disabled}
            className={`w-full py-3 text-white rounded-full font-medium cursor-pointer ${reserveButtonProps.text === "Cancel reservation"
                ? "bg-red-600 hover:bg-red-700"
                : "bg-green-800 hover:bg-green-900"
              }`}
          >
            {reserveButtonProps.text}
          </button>
          {reserveButtonProps.reason && (
            <p className="text-sm text-red-600 mt-1">
              {reserveButtonProps.reason}
            </p>
          )}
        </div>
      </div>

      <div className="mt-10">
        <h1 className="text-3xl font-extrabold">{event?.name}</h1>

        <div className="flex items-center gap-3 mt-4">
          <div className="w-10 h-10 bg-green-200 rounded-full flex items-center justify-center font-bold">
            {getInitials(host?.name)}
          </div>
          <div className="flex flex-col">
            <span className="font-medium">
              {host?.name} •{" "}
              <span className="text-green-700">
                {host?.verified ? "Verified ✅" : "Not yet verified ⛔"}
              </span>
            </span>
            <span className="text-sm text-orange-600 font-semibold">
              ★{" "}
              {!host?.score || host?.score === 0
                ? "Host has not been rated yet"
                : host?.score}{" "}
            </span>
          </div>
          {event?.eventFoodDetails?.allergens.map((ingredient) => (
            <span
              key={ingredient}
              className="bg-green-100 text-green-700 text-xs px-3 py-1 rounded-full"
            >
              {ingredient}
            </span>
          ))}
        </div>

        <div className="mt-6">
          <h2 className="font-semibold text-lg">About the meal</h2>
          <p className="mt-2 text-gray-700 leading-relaxed max-w-xl">
            {event?.description}
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-10 mt-10">
          <div className="bg-blue-100 h-64 rounded-xl flex items-center justify-center text-gray-500">
            <LocationMiniMap
              address={event?.eventAddress.streetAddress}
              city={event?.eventAddress.city}
              postalCode={event?.eventAddress.postalCode}
            />
          </div>

          <div>
            <h2 className="font-semibold text-lg mb-4">Reviews</h2>

            <div className="space-y-4">
              {reviews?.length ? (
                reviews?.map((review) => (
                  <div
                    key={review.reviewId}
                    className="flex items-start gap-3 p-4 bg-white rounded-xl border shadows-sm"
                  >
                    <div className="w-10 h-10 bg-green-200 rounded-full flex items-center justify-center font-bold">
                      {getInitials(review.username)}
                    </div>

                    <div>
                      <div className="font-medium flex items-center gap-2">
                        <span>{review.username}</span>
                        <span className="text-yellow-500">
                          {Array.from({ length: review.stars }, (_, i) => (
                            <span key={i}>★</span>
                          ))}
                        </span>
                      </div>
                      <p className="text-sm text-gray-600">{review.comment}</p>
                    </div>
                  </div>
                ))
              ) : (
                <p className="text-gray-500 text-sm">No reviews yet!</p>
              )}
            </div>

            {(() => {
              const token = localStorage.getItem("accessToken");
              if (!token || !event) return null;
              const decoded = parseJwt(token);
              const userId = decoded.sub;
              return userId !== event.userId &&
                event.eventParticipants.some((p) => p.userId === userId);
            })() && (
                <div className="mt-6 p-4 bg-gray-50 rounded-xl border">
                  <h3 className="font-semibold text-md mb-3">Leave a Review</h3>
                  <form onSubmit={handleSubmitReview} className="space-y-3">
                    <div>
                      <label className="block text-sm font-medium mb-2">
                        Rating
                      </label>
                      <div className="flex gap-2">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <button
                            key={star}
                            type="button"
                            onClick={() => setReviewStars(star)}
                            className={`text-2xl ${star <= reviewStars
                                ? "text-yellow-500"
                                : "text-gray-300"
                              } hover:text-yellow-400 transition-colors`}
                          >
                            ★
                          </button>
                        ))}
                        <span className="ml-2 text-sm text-gray-600 self-center">
                          {reviewStars} star{reviewStars !== 1 ? "s" : ""}
                        </span>
                      </div>
                    </div>

                    <div>
                      <label className="block text-sm font-medium mb-2">
                        Comment
                      </label>
                      <textarea
                        value={reviewComment}
                        onChange={(e) => setReviewComment(e.target.value)}
                        placeholder="Share your experience..."
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent resize-none"
                        rows={3}
                        required
                      />
                    </div>

                    <button
                      type="submit"
                      disabled={isSubmittingReview || !reviewComment.trim()}
                      className="w-full py-2 bg-green-800 text-white rounded-lg font-medium hover:bg-green-900 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
                    >
                      {isSubmittingReview ? "Submitting..." : "Submit Review"}
                    </button>
                  </form>
                </div>
              )}
          </div>
        </div>
      </div>
    </main>
  );
}
