import type { UpcomingEventCard } from "../types/UpcomingEventCard";

const inHours = (h: number) => new Date(Date.now() + h * 60 * 60 * 1000).toISOString();
const inDaysAt = (days: number, hour24: number, minute = 0) => {
  const d = new Date();
  d.setDate(d.getDate() + days);
  d.setHours(hour24, minute, 0, 0);
  return d.toISOString();
};


export const mockUpcomingEventCards: UpcomingEventCard[] = [
  {
    id: "evt_001",
    title: "Coffee + plan spring hikes",
    startsAt: inHours(6),
    endsAt: inHours(7.5),
    timeZoneId: "America/New_York",
    placeName: "Blackbird Café",
    goingCount: 3,
  },
  {id: "evt_002", title: "Hiking at Shenandoah National Park", startsAt: inDaysAt(2, 9), goingCount: 5},
  {id: "evt_003", title: "Kayaking on the Potomac", startsAt: inDaysAt(5, 14), goingCount: 2},
]