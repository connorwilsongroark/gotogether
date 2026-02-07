import { useAuth0 } from "@auth0/auth0-react";
import { useState } from "react";
import type { CreatePlaceRequest, PlaceDto } from "../types/PlaceDtos";
import { createPlace } from "../api/placesApi";

export function useCreatePlace() {
  const { getAccessTokenSilently } = useAuth0();

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function submit(req: CreatePlaceRequest): Promise<PlaceDto> {
    // Set state indicating submission is starting and there's no error
    setIsSubmitting(true);
    setError(null);

    try {
      const token = await getAccessTokenSilently();
      return await createPlace(token, req);
    } catch (e) {
      const message = e instanceof Error ? e.message : "Unknown error";
      setError(message);
      throw e;
    } finally {
      setIsSubmitting(false);
    }
  }

  return { submit, isSubmitting, error };
}
