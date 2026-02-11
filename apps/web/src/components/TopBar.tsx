import { Link, NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Button from "./ui/Button";

export default function TopBar() {
  const { isAuthenticated, user, loginWithRedirect, logout } = useAuth0();

  // Styling for link items
  const linkClass = ({ isActive }: { isActive: boolean }) =>
    [
      "text-sm transition-colors",
      "hover:underline",
      isActive ? "font-semibold underline" : "opacity-90 hover:opacity-100",
    ].join(" ");

  return (
    <header className='border-b border-border bg-bg text-text'>
      <nav className='mx-auto flex max-w-5xl items-center justify-between p-4'>
        {/* Left side  of navigation*/}
        <div className='flex items-center gap-6'>
          <Link to='/' className='text-lg font-semibold tracking-tight'>
            <img
              src='public\brand\logos\gotogether-logo.png'
              alt='GoTogether Logo'
              className='h-12 w-auto'
            />
          </Link>

          <div className='flex items-center gap-4'>
            <NavLink to='/' className={linkClass}>
              Home
            </NavLink>
            {isAuthenticated && (
              <NavLink to='/dashboard' className={linkClass}>
                Dashboard
              </NavLink>
            )}
          </div>
        </div>

        {/* Right side of navigation: auth actions */}
        <div className='flex items-center gap-3'>
          {/* If the user is not authenticated, show them log in button */}
          {!isAuthenticated ? (
            <Button variant='primary' onClick={() => loginWithRedirect()}>
              Log in
            </Button>
          ) : (
            // Otherwise, show them their user details & the log out button
            <>
              <span className='hidden text-sm sm:inline text-text-muted'>
                {user?.name ?? user?.email ?? "Account"}
              </span>

              <Button
                variant='secondary'
                onClick={() =>
                  logout({ logoutParams: { returnTo: window.location.origin } })
                }
              >
                Log out
              </Button>
            </>
          )}
        </div>
      </nav>
    </header>
  );
}
