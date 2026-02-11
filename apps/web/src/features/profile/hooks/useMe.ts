import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useMemo, useState } from "react";
import { getMe, patchMe, type Me, type UpdateMeRequest } from "../api/meApi";

export function useMe() {
  const {
    isAuthenticated,
    isLoading: authLoading,
    getAccessTokenSilently,
  } = useAuth0();

  const [me, setMe] = useState<Me | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const canLoad = isAuthenticated && !authLoading;

  // load and provision on first authenticated render
  useEffect(() => {
    if (!canLoad) return;

    let cancelled = false;
    (async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await getMe(getAccessTokenSilently);
        if (!cancelled) setMe(data);
      } catch (e: any) {
        if (!cancelled) setError(e?.message ?? "Failed to load profile");
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [canLoad, getAccessTokenSilently]);

  const update = useMemo(
    () => async (body: UpdateMeRequest) => {
      if (!canLoad) throw new Error("Not Authenticated.");
      const updated = await patchMe(getAccessTokenSilently, body);
      setMe(updated);
      return updated;
    },
    [canLoad, getAccessTokenSilently],
  );

  return { me, loading, error, update, canLoad };
}
