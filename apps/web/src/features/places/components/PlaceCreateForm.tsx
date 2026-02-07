import { useState } from "react";
import { useCreatePlace } from "../hooks/useCreatePlace";

type Props = {
  onCreated?: (placeId: string) => void;
};

export function PlaceCreateForm({ onCreated }: Props) {
  const { submit, isSubmitting, error } = useCreatePlace();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();

    const result = await submit({
      name: name.trim(),
      description: description.trim() ? description.trim() : null,
    });

    setName("");
    setDescription("");
    onCreated?.(result.id);
  }

  const nameTooLong = name.length > 120;
  const descTooLong = description.length > 2000;
  const disabled = isSubmitting || !name.trim() || nameTooLong || descTooLong;

  return (
    <form onSubmit={onSubmit} className='space-y-1'>
      {/* Place Name input */}
      <div>
        <label className='block text-sm font-medium'>Name</label>
        <input
          type='text'
          className=''
          value={name}
          onChange={(e) => setName(e.target.value)}
          maxLength={150}
          placeholder="e.g., Monk's Coffee Shop"
        />
        <div className='text-xs opacity-70'>
          {name.length}/120 {nameTooLong && " - too long"}
        </div>
      </div>

      {/* Place Description input */}
      <div className='space-y-1'>
        <label htmlFor='block text-sm font-medium'>Description</label>
        <textarea
          className='w-full rounded-md border px-3 py-2'
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          rows={4}
          maxLength={2200}
          placeholder='Optional details...'
        />
        <div className='text-xs opacity-70'>
          {description.length}/2000 {descTooLong && " - too long"}
        </div>
      </div>

      {/* Display errors if any */}
      {error && (
        <div className='rounded-md border p-3 text-sm'>
          <div className='font-medium'>Error</div>
          <div className='opacity-80'>{error}</div>
        </div>
      )}

      <button
        type='submit'
        disabled={disabled}
        className='rounded-md border px-4 py-2 disabled:opacity-50'
      >
        {isSubmitting ? "Creating..." : "Create Place"}
      </button>
    </form>
  );
}
