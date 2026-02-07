import type { CreatePlaceRequest, PlaceDto } from "../types/PlaceDtos";

const apiBase = import.meta.env.VITE_API_BASE_URL;

export async function createPlace(
  accessToken: string,
  req: CreatePlaceRequest,
): Promise<PlaceDto> {
  // Submit post request to the endpoint
  const res = await fetch(`${apiBase}/places`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
    body: JSON.stringify(req),
  });

  // If there's an error, throw it
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `Request failed: (${res.status})`);
  }

  // Return the json response
  return (await res.json()) as PlaceDto;
}
