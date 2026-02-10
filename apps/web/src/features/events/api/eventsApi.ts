import type { UpcomingEventCard } from "../types/UpcomingEventCard";
import { mockUpcomingEventCards } from "../mock/mockUpcomingEventData";

export type GetUpcomingEventCardsParams = {
  limit?: number;
};

// Mock implementation of upcoming events response
export async function getUpcomingEventCards(
  params: GetUpcomingEventCardsParams = {},
): Promise<UpcomingEventCard[]> {
  const { limit = 10 } = params;

  const sorted = [...mockUpcomingEventCards].sort(
    (a, b) => new Date(a.startsAt).getTime() - new Date(b.startsAt).getTime(),
  );

  await new Promise((r) => setTimeout(r, 100));

  return sorted.slice(0, limit);
}
