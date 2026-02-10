import Timeline from "../features/events/components/Timeline";
import { useUpcomingEventCards } from "../features/events/hooks/useUpcomingEventCards";

export default function Events() {
  const { data, isLoading, error } = useUpcomingEventCards(10);

  if (isLoading) return <div>Loading events...</div>;
  if (error) return <div>Failed to load events</div>;

  return (
    <div className='space-y-2'>
      <h1 className='text-xl font-semibold'>Events</h1>
      <p className='text-text-muted'>Your upcoming events</p>
      <Timeline events={data} />
    </div>
  );
}
