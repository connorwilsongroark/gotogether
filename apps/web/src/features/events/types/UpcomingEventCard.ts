export type UpcomingEventCard = {
  id: string;
  title: string;
  startsAt: string; // ISO
  endsAt?: string;  // ISO
  timeZoneId?: string;
  placeName?: string;
  goingCount: number;
};