import type { UpcomingEventCard } from "../types/UpcomingEventCard";

type TimelineItemProps = {
  event: UpcomingEventCard;
};

export default function TimelineItem({ event }: TimelineItemProps) {
  return (
    <div className='rounded-2xl border p-4'>
      <h3 className='font-semibold truncate'>{event.title}</h3>
      <div className='text-sm text-muted-foreground'>
        {new Date(event.startsAt).toLocaleString()}
      </div>
      <div className='text-sm'>{event.goingCount} going</div>
    </div>
  );
}
