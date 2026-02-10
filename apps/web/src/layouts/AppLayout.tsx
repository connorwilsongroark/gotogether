import { Outlet } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import TopBar from "../components/TopBar";
import SideNav from "../components/SideNav";

export function AppLayout() {
  const { isLoading, error, isAuthenticated } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Auth error: {error.message}</div>;
  }

  return (
    <>
      <TopBar />
      <div className='mx-auto flex max-w-5xl overflow-x-hidden'>
        {isAuthenticated && <SideNav />}
        <main className='flex-1 min-w-0 p-6'>
          <Outlet />
        </main>
      </div>
    </>
  );
}
