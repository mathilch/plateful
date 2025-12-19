import http from "k6/http";
import { check, sleep } from "k6";


export const options = {
  scenarios: {
    ramping_vus: {
      executor: "ramping-vus",
      startVUs: 0,
      stages: [
        { duration: "10s", target: 50 },
        { duration: "10s", target: 100 },
        { duration: "10s", target: 200 },
        { duration: "30s", target: 350 },
        { duration: "40s", target: 500 },
        { duration: "30s", target: 500 }, 
        { duration: "10s", target: 0 },
      ],
      gracefulRampDown: "30s",
    },
  },
  thresholds: {
    http_req_failed: ["rate<0.02"],     // <2% failures acceptable
    http_req_duration: ["p(95)<2000"],   // p95 under 800ms (tune)
  },
};

export default function () {
  const res = http.get('http://127.0.0.1:30011/');

  check(res, {
    "status is 200": (r) => r.status === 200,
  });

  sleep(1); // controls per-VU request rate
}
