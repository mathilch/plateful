import http from "k6/http";
import { check, sleep } from "k6";
import { Rate } from "k6/metrics";

const BASE_URL = __ENV.BASE_URL || "http://127.0.0.1:30001";

const no_events = new Rate("no_events_returned");
const missing_event_id = new Rate("missing_event_id");

export const options = {
  scenarios: {
    ramp_users: {
      executor: "ramping-vus",
      startVUs: 0,
      stages: [
        { duration: "1m", target: 50 },
        { duration: "1m", target: 100 },
        { duration: "1m", target: 200 },
        { duration: "1m", target: 350 },
        { duration: "1m", target: 500 },
        { duration: "2m", target: 500 }, // hold peak
        { duration: "1m", target: 0 },
      ],
      gracefulRampDown: "30s",
    },
  },
  thresholds: {
    http_req_failed: ["rate<0.02"],
    http_req_duration: ["p(95)<800"],

    // Quality checks (nice for report)
    no_events_returned: ["rate<0.001"],     // <0.1% empty recent list
    missing_event_id: ["rate<0.001"],       // <0.1% items missing EventId
  },
};

export default function () {
  // 1) Get recent events
  const recentRes = http.get(`${BASE_URL}/api/event/recent`);

  const okRecent = check(recentRes, {
    "recent status 200": (r) => r.status === 200,
    "recent returns JSON": (r) => (r.headers["Content-Type"] || "").includes("application/json"),
  });

  if (!okRecent) {
    // If status isn't 200 or it's not JSON, stop this iteration
    sleep(1);
    return;
  }

  // 2) Parse list
  let events;
  try {
    events = recentRes.json();
  } catch {
    sleep(1);
    return;
  }

  const hasEvents = Array.isArray(events) && events.length > 0;

  check(events, {
    "recent is non-empty array": (e) => Array.isArray(e) && e.length > 0,
  });

  if (!hasEvents) {
    no_events.add(1);
    sleep(1);
    return;
  }

  // 3) Pick random event and extract EventId
  const ev = events[Math.floor(Math.random() * events.length)];
  const eventId = ev?.eventId;

  if (!eventId) {
    missing_event_id.add(1);
    sleep(1);
    return;
  }

  // Small think time: user scans the list before clicking
  sleep(Math.random() * 1 + 0.2); // 0.2–1.2s

  // 4) Get details
  const detailsRes = http.get(`${BASE_URL}/api/event/${eventId}`);

  check(detailsRes, {
    "details status 200": (r) => r.status === 200,
    "details returns JSON": (r) => (r.headers["Content-Type"] || "").includes("application/json"),
  });

  // Longer think time: user reads the event details
  sleep(Math.random() * 2 + 1); // 1–3s
}
