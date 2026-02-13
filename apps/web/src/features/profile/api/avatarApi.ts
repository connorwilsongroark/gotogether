import type { GetTokenSilentlyOptions } from "@auth0/auth0-react";

const apiBase = import.meta.env.VITE_API_BASE_URL;

export async function uploadAvatar(
  getAccessTokenSilently: (o?: GetTokenSilentlyOptions) => Promise<string>,
  file: File,
): Promise<{ avatarUrl: string }> {
  const token = await getAccessTokenSilently();

  const form = new FormData();
  form.append("file", file); // MUST BE A "file" TO MATCH THE API IMPLEMENTATION

  const res = await fetch(`${apiBase}/me/avatar`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${token}`,
      // NOTE: Do not set content-type manually for the form data
    },
    body: form,
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Upload failed: ${res.status} ${text}`);
  }

  return res.json();
}
