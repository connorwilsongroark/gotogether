import { Link } from "react-router-dom";
import Button from "../components/ui/Button";

export default function NotFound() {
  return (
    <div className='flex min-h-[60vh] flex-col items-center justify-center space-y-4 text-center'>
      <h1 className='text-3xl font-semibold'>404</h1>

      <p className='text-text-muted'>
        The page you’re looking for doesn’t exist.
      </p>

      <Button asChild variant='primary'>
        <Link to='/'>Go home</Link>
      </Button>
    </div>
  );
}
