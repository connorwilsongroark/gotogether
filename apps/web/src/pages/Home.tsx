import TestApi from "../components/TestApi";

export default function Home() {
  return (
    <div className='space-y-2'>
      <h1 className='text-xl font-semibold'>Home</h1>
      <p className='text-text-muted'>
        Landing page / marketing / intro goes here.
      </p>
      <TestApi />
    </div>
  );
}
