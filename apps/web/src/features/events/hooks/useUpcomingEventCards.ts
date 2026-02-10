import { useEffect, useState } from "react";
import type { UpcomingEventCard } from "../types/UpcomingEventCard";
import { getUpcomingEventCards } from "../api/eventsApi";

export function useUpcomingEventCards(limit = 10) {
  const [data, setData] = useState<UpcomingEventCard[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<unknown>(null);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      try {
        setIsLoading(true);
        const result = await getUpcomingEventCards({ limit });
        if (!cancelled) setData(result);
      } catch (e) {
        if (!cancelled) setError(e);
      } finally {
        if (!cancelled) setIsLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [limit]);

  return { data, isLoading, error };
}
