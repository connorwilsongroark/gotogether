import { useEffect, useMemo, useRef, useState } from "react";
import type { UpcomingEventCard } from "../types/UpcomingEventCard";
import TimelineItem from "./TimelineItem";
import { TruckElectric } from "lucide-react";

type TimelineProps = {
  events: UpcomingEventCard[];
};

export default function Timeline({ events }: TimelineProps) {
  // Memoize the sorted items to avoid redundant sorting
  const sortedEvents = useMemo(
    () =>
      [...events].sort(
        (a, b) =>
          new Date(a.startsAt).getTime() - new Date(b.startsAt).getTime(),
      ),
    [events],
  );

  // Declare ref and state variables for the scrollable timeline
  const scrollerRef = useRef<HTMLDivElement | null>(null);
  const [canScrollLeft, setCanScrollLeft] = useState(false);
  const [canScrollRight, setCanScrollRight] = useState(false);

  // Determine which (or both) side(s) display the 'fade' for overflowing items
  const updateFadeState = () => {
    const el = scrollerRef.current;
    if (!el) return;

    // Keep this tiny
    const epsilon = 2;

    const left = el.scrollLeft;
    const cw = el.clientWidth;
    const sw = el.scrollWidth;

    const nextCanLeft = left > epsilon;
    const nextCanRight = left + cw < sw - epsilon;

    setCanScrollLeft(nextCanLeft);
    setCanScrollRight(nextCanRight);
  };

  // Refs and state for desktop dragging
  const isDragging = useRef(false);
  const hasDragged = useRef(false);
  const startX = useRef(0);
  const scrollStart = useRef(0);

  // Handlers for desktop dragging mouse events
  const onMouseDown = (e: React.MouseEvent) => {
    const el = scrollerRef.current;
    if (!el) return;

    isDragging.current = true;
    hasDragged.current = false;
    startX.current = e.pageX;
    scrollStart.current = el.scrollLeft;
  };

  const onMouseMove = (e: React.MouseEvent) => {
    if (!isDragging.current) return;

    const el = scrollerRef.current;
    if (!el) return;

    const dx = e.pageX - startX.current;

    // We only want to consider a 'drag' if the cursor has moved more than a small margin, in order to prevent imperfect clicks from dragging
    if (Math.abs(dx) > 5) {
      hasDragged.current = true;
    }

    el.scrollLeft = scrollStart.current - dx;
  };

  const stopDragging = () => {
    isDragging.current = false;
  };

  useEffect(() => {
    updateFadeState();

    const el = scrollerRef.current;
    if (!el) return;

    const onScroll = () => updateFadeState();
    el.addEventListener("scroll", onScroll, { passive: true });

    const ro = new ResizeObserver(() => updateFadeState());
    ro.observe(el);

    return () => {
      el.removeEventListener("scroll", onScroll);
      ro.disconnect();
    };
  }, [sortedEvents.length]);

  return (
    <div className='relative'>
      <div
        ref={scrollerRef}
        onMouseDown={onMouseDown}
        onMouseMove={onMouseMove}
        onMouseUp={stopDragging}
        onMouseLeave={stopDragging}
        onClickCapture={(e) => {
          if (hasDragged.current) {
            e.stopPropagation();
            e.preventDefault();
          }
        }}
        className='w-full flex gap-3 overflow-x-auto scroll-smooth snap-x snap-mandatory py-2 [-ms-overflow-style:none] [scrollbar-width:none] [&::-webkit-scrollbar]:hidden cursor-grab active:cursor-grabbing select-none'
        role='list'
        aria-label='Upcoming events'
      >
        {sortedEvents.map((e) => (
          <div
            key={e.id}
            className='snap-start shrink-0 w-[280px] sm:w-[320px] md:w-[360px]'
            role='listitem'
          >
            <TimelineItem event={e} />
          </div>
        ))}
      </div>

      {/* Left-side fade. Conditionally rendered by scroll position states */}
      {canScrollLeft && (
        <div
          aria-hidden='true'
          className='pointer-events-none absolute inset-y-0 left-0 w-12 z-20
               bg-linear-to-r from-bg to-transparent'
        />
      )}
      {/* Right-side fade. Conditionally rendered by scroll position states */}
      {canScrollRight && (
        <div
          aria-hidden='true'
          className='pointer-events-none absolute inset-y-0 right-0 w-12 z-20
               bg-linear-to-l from-bg to-transparent'
        />
      )}
    </div>
  );
}
