import TestApi from "../components/TestApi";
import { PlaceCreateForm } from "../features/places/components/PlaceCreateForm";

export default function Home() {
  return (
    <div className='space-y-2'>
      <h1 className='text-xl font-semibold'>Home</h1>
      <p className='text-text-muted'>
        Landing page / marketing / intro goes here.
      </p>
      <PlaceCreateForm />
    </div>
  );
}
