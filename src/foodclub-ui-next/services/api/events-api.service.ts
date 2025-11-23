//import { CreateEventRequestDto } from "@prvacy/events-api-sdk/dist/generated/model";
import { SearchEventsRequestDto } from "@/types/search-events.type";
import { eventDetailsMocks } from "../mocks/event-details-mocks";
import { toQueryParams } from "@/lib/utils";
import {EventOverviewDto} from "@/types/event-details.type";
//import { postApiEvent } from "@prvacy/events-api-sdk"


export async function getEventById(eventId: string, accessToken: string) {
    try {
        process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
        const res = await fetch(
            `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/events/${eventId}`, 
            {
                method: "GET",
                headers: { 
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}` 
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

export async function searchEventsBySelectedFilters(
  searchEventsRequestDto: Partial<SearchEventsRequestDto>,
  signal?: AbortSignal
) {
  const queryParams = toQueryParams(searchEventsRequestDto as Partial<SearchEventsRequestDto>);
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const url = `${process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL}/api/event/search${queryParams}`;
    const res = await fetch(url, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      signal,
    });

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return [];
    }

    const data = await res.json();
    return data;
  } catch (err) {
    if ((err as any)?.name === "AbortError") {
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

export async function getEventsByUserAsParticipant(
  userId: string,
  accessToken: string
) {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
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
    const res = await fetch(
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

export async function signUpForEvent(
    eventId: string,
    token: string
) {
    try {
        process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
        const res = await fetch(
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
        const res = await fetch(
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

function addAuthHeaders(): HeadersInit {
  return {
    // TODO: work on proper authentication flow
    Authorization: "Bearer token123",
  };
  const token = localStorage.getItem("accessToken");
  if (token) {
    //return token;
  }
}

// export async function postEvent(createEventRequest: CreateEventRequestDto) {

//   const resp = await postApiEvent(createEventRequest, { headers: addAuthHeaders() });
//   return resp.data;
// }
