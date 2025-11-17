
import { CreateEventRequestDto } from "@prvacy/events-api-sdk/dist/generated/model";
import { eventDetailsMocks } from "../mocks/event-details-mocks";
import { postApiEvent } from "@prvacy/events-api-sdk"

export async function getRecentEventsForHomePage() {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/recent`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      }
    );

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Events fetch error:", err);
    return [];
  }
}

export async function getEventsByUserId(userId: string, accessToken: string) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/user/${userId}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${accessToken}`,
        },
      }
    );

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Events fetch error:", err);
    return [];
  }
}

function addAuthHeaders(): HeadersInit {
  return {
    // TODO: work on proper authentication flow
    Authorization: 'Bearer token123'
  };
  const token = localStorage.getItem("accessToken");
  if (token) {
    //return token;
  }
}

export async function postEvent(createEventRequest: CreateEventRequestDto) {


  const resp = await postApiEvent(createEventRequest, { headers: addAuthHeaders() });
  return resp.data;
}
