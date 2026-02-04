import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";

const TestApi = () => {
  const [result, setResult] = useState<string>("");
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();

  const callApi = async () => {
    try {
      if (!isAuthenticated) {
        setResult("User is not authenticated");
        return;
      }

      // Get the access token
      const token = await getAccessTokenSilently({
        authorizationParams: { audience: "https://api.gotogether.dev" },
      });
      console.log(token);
      // Call protected endpoint with Bearer token
      const res = await fetch("http://localhost:5274/api/v1/secret", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      //Handle response
      if (!res.ok) {
        const text = await res.text();
        setResult(`Error ${res.status}: ${text}`);
        return;
      }

      const text = await res.text();
      setResult(text);
    } catch (err) {
      if (err instanceof Error) {
        setResult(`Error calling API: ${err.message}`);
      } else {
        setResult("Unknown error calling API");
      }
      console.error(err);
    }
  };

  return (
    <div>
      <button onClick={callApi}>Call API</button>
      <p>{result}</p>
    </div>
  );
};

export default TestApi;
