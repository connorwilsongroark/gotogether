import { useAuth0 } from "@auth0/auth0-react";
import TestApi from "./components/TestApi";

export default function App() {
  const { isAuthenticated, isLoading, loginWithRedirect, error } = useAuth0();

  if (isLoading) return <div>Loading…</div>;
  if (error) return <div>Auth error: {error.message}</div>;

  return (
    <div className="p-6">
      <h1>GoTogether</h1>
      {!isAuthenticated ? (
        <button onClick={() => loginWithRedirect({
  authorizationParams: {
    prompt: "login"
  }
})}>Log in</button>
      ) : (
        <div>Logged in ✅
          <TestApi />
        </div>
      )}
    </div>
  );
}
