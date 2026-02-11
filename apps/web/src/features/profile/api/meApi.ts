import type { GetTokenSilentlyOptions } from "@auth0/auth0-react";

export type Me = {
  id: string;
  auth0Sub: string;
  email?: string | null;
  displayName: string;
  avatarUrl?: string | null;
  timeZoneId?: string | null;
  createdAt: string;
  updatedAt?: string | null;
};

// Keep PATCH fields optional so we can update just one thing at a time, if requested
export type UpdateMeRequest = {
  displayName?: string | null;
  avatarUrl?: string | null;
  timeZoneId?: string | null;
};

async function authedFetch(
  getAccessTokenSilently: (o?: GetTokenSilentlyOptions) => Promise<string>,
  input: RequestInfo | URL,
  init?: RequestInit,
) {
  const token = await getAccessTokenSilently();
  return fetch(input, {
    ...init,
    headers: {
      ...(init?.headers ?? {}),
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  });
}

export async function getMe(getAccessTokenSilently: any): Promise<Me> {
  const res = await authedFetch(getAccessTokenSilently, "api/v1/me");
  if (!res.ok) throw new Error(`GET /api/v1/me failed: ${res.status}`);
  return res.json();
}

export async function patchMe(
  getAccessTokenSilently: any,
  body: UpdateMeRequest,
): Promise<Me> {
  const res = await authedFetch(getAccessTokenSilently, "api/v1/me", {
    method: "PATCH",
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`PATCH /api/v1/me failed: ${res.status} ${text}`);
  }
  return res.json();
}
