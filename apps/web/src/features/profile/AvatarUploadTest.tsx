import { useAuth0 } from "@auth0/auth0-react";
import { useState } from "react";
import { uploadAvatar } from "./api/avatarApi";

export function AvatarUploadTest() {
  const { isAuthenticated, getAccessTokenSilently } = useAuth0();
  const [file, setFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const [avatarUrl, setAvatarUrl] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  if (!isAuthenticated) return <div className='p-6'>Please log in.</div>;

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!file) return;

    setUploading(true);
    setError(null);

    // todo add refresh?

    try {
      const result = await uploadAvatar(getAccessTokenSilently, file);
      setAvatarUrl(result.avatarUrl);
    } catch (err: any) {
      setError(err?.message ?? "Upload failed.");
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className='p-6 max-w-xl space-y-4'>
      <h2 className='text-xl font-semibold'>Avatar Upload Test</h2>

      <form onSubmit={onSubmit} className='space-y-3'>
        <input
          type='file'
          accept='image/png,image/jpeg,image/webp'
          onChange={(e) => setFile(e.target.files?.[0] ?? null)}
        />

        <button
          type='submit'
          className='rounded-md border px-4 py-2 disabled:opacity-50'
          disabled={!file || uploading}
        >
          {uploading ? "Uploading…" : "Upload"}
        </button>
      </form>

      {error && <div className='text-sm text-red-600'>{error}</div>}

      {avatarUrl && (
        <div className='space-y-2'>
          <div className='text-sm'>
            Saved at: <code>{avatarUrl}</code>
          </div>
          <img
            src={avatarUrl}
            alt='Uploaded avatar preview'
            className='h-24 w-24 rounded-full border object-cover'
          />
        </div>
      )}
    </div>
  );
}
