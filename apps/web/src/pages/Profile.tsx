import { useEffect, useState } from "react";
import { useMe } from "../features/profile/hooks/useMe";

export function Profile() {
  const { me, loading, error, update, canLoad } = useMe();

  const [displayName, setDisplayName] = useState("");
  const [saving, setSaving] = useState(false);
  const [saveError, setSaveError] = useState<string | null>(null);

  useEffect(() => {
    if (!me) return;
    setDisplayName(me.displayName ?? "");
  }, [me]);

  if (!canLoad) return <div className='p-6'>Please log in.</div>;
  if (loading) return <div className='p-6'>Loading profile...</div>;
  if (error) return <div className='p-6'>Error: {error}</div>;
  if (!me) return <div className='p-6'>No profile loaded.</div>;

  const onSave = async () => {
    setSaving(true);
    setSaveError(null);
    try {
      await update({
        displayName,
      });
    } catch (e: any) {
      setSaveError(e?.message ?? "Failed to save");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className='p-6 max-w-xl space-y-6'>
      <div className='space-y-1'>
        <h1 className='text-2xl font-semibold'>Profile</h1>
        <p className='text-sm opacity-80'>
          Signed in as {me.email ?? "unknown email"}
        </p>
      </div>

      <div className='space-y-2'>
        <label className='text-sm font-medium'>Display name</label>
        <input
          type='text'
          className='w-full rounded-md border px-3 py-2'
          value={displayName}
          onChange={(e) => setDisplayName(e.target.value)}
          placeholder='Your name'
        />
        <p className='text-xs opacity-70'>2-80 characters</p>
      </div>

      {saveError && <div className='text-sm text-red-600'>{saveError}</div>}

      <button
        className='rounded-md border px-4 py-2 disabled:opacity-50'
        onClick={onSave}
        disabled={saving}
      >
        {saving ? "Saving..." : "Save"}
      </button>

      <div className='text-xs opacity-70'>
        <div>Profile Id: {me.id}</div>
        <div>Auth0 sub: {me.auth0Sub}</div>
      </div>
    </div>
  );
}
