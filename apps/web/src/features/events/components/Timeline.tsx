import type { UpcomingEventCard } from "../types/UpcomingEventCard";
import TimelineItem from "./TimelineItem";

type TimelineProps = {
  events: UpcomingEventCard[];
};

export default function Timeline({ events }: TimelineProps) {
  const sortedEvents = [...events].sort(
    (a, b) => new Date(a.startsAt).getTime() - new Date(b.startsAt).getTime(),
  );

  return (
    <>
      <div className='space-y-3'>
        {sortedEvents.map((e) => (
          <TimelineItem key={e.id} event={e} />
        ))}
      </div>
    </>
  );
}
