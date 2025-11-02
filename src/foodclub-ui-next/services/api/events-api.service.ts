import { eventDetailsMocks } from "../mocks/event-details-mocks";

export async function getRecentEventsForHomePage() {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/recent`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        // If you want to force fresh data: next: { revalidate: 0 } (optional)
      }
    );

    if (!res.ok) {
      // either return an empty array or throw to be handled by caller
      console.error("Events fetch failed:", res.status, res.statusText);
      return []; // fallback
    }

    const data = await res.json();
    return data; // return parsed JSON array of events
  } catch (err) {
    console.error("Events fetch error:", err);
    return []; // fallback
  }
}
