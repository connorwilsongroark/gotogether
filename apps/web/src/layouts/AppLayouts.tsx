import { Outlet } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Navbar from "../components/Navbar";

export function AppLayout() {
  const { isLoading, error } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Auth error: {error.message}</div>;
  }

  return (
    <>
      <Navbar />
      <main className='p-6'>
        <Outlet />
      </main>
    </>
  );
}
