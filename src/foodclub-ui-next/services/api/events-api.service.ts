import { SearchEventsRequestDto } from "@/types/search-events.type";
function dispatchFetchStart() {
  if (typeof window !== "undefined") window.dispatchEvent(new CustomEvent("app:fetch-start"));
}
function dispatchFetchEnd() {
  if (typeof window !== "undefined") window.dispatchEvent(new CustomEvent("app:fetch-end"));
}
async function fetchWithLoader(input: RequestInfo, init?: RequestInit) {
  dispatchFetchStart();
  try {
    const res = await fetch(input, init);
    return res;
  } finally {
    dispatchFetchEnd();
  }
}
import { toQueryParams } from "@/lib/utils";
import { CreateEventRequestDto } from "@Rameez349/events-api-sdk/dist/generated/model";
import { postApiEvent } from "@Rameez349/events-api-sdk";
import {EventReviewDto} from "@/types/event-review.type";
import {EventDetails, EventOverviewDto} from "@/types/event-details.type";


export async function getEventById(eventId: string): Promise<EventDetails | null> {
  console.log("Fetching event with id ", eventId);
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/${eventId}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return null;
    }
    const data: EventDetails = await res.json();
    return data;
  } catch (err) {
    console.error("Events fetch error:", err);
    return null;
  }
}

export async function getRecentEventsForHomePage() {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
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

export async function searchEventsBySelectedFilters(
  searchEventsRequestDto: Partial<SearchEventsRequestDto>,
  signal?: AbortSignal
): Promise<EventOverviewDto[]> {
  const queryParams = toQueryParams(searchEventsRequestDto as Partial<SearchEventsRequestDto>);
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const url = `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/search${queryParams}`;
    const res = await fetchWithLoader(url, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      signal,
    });

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return [];
    }

    const data = (await res.json()) as EventOverviewDto[];
    
    if (!Array.isArray(data)) {
        console.error("Expected array but got: ", data);
        return [];
    }
    return data;
    
  } catch (err: unknown) {
    if (err instanceof Error && err.name === "AbortError") {
      // request was aborted
      return [];
    }
    console.error("Events fetch error:", err);
    return [];
  }
}

export async function getEventsByUserId(userId: string, accessToken: string) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
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

export async function getEventsByUserAsParticipant(
  userId: string,
  accessToken: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/user-as-participant/${userId}`,
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

export async function getEventReviewsByHostUserId(
  userId: string,
  accessToken: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/user-reviews-as-host/${userId}`,
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

export async function getEventReviewsByEventId(
  eventId: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/${eventId}/reviews`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!res.ok) {
      console.error("Event reviews fetch failed:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Event reviews fetch error:", err);
    return [];
  }
}

export async function signUpForEvent(
  eventId: string,
  token: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/${eventId}/participate`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (!res.ok) {
      console.error("Cannot sign up for event:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Events fetch error:", err);
    return [];
  }
}

export async function withdrawFromEvent(
  eventId: string,
  token: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/${eventId}/withdraw`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (!res.ok) {
      console.error("Cannot sign up for event:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;

  } catch (err) {
    console.error("Events fetch error:", err);
    return [];
  }
}


function addAuthHeaders(accessToken: string): HeadersInit {
  return {
    // TODO: work on proper authentication flow
    Authorization: `Bearer ${accessToken}`,
  };
  const token = localStorage.getItem("accessToken");
  if (token) {
    //return token;
  }
}


export async function postEvent(createEventRequest: CreateEventRequestDto, accessToken: string) {

  const resp = await postApiEvent(createEventRequest, { headers: addAuthHeaders(accessToken) });
  return resp;
}

export async function submitEventReview(
  eventId: string,
  stars: number,
  comment: string,
  token: string
): Promise<EventReviewDto> {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetchWithLoader(
      `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/${eventId}/review`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ stars, comment }),
      }
    );

    if (!res.ok) {
      console.error("Cannot submit review:", res.status, res.statusText);
      throw new Error("Failed to submit review");
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Review submission error:", err);
    throw err;
  }
}
