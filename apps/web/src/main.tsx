import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { Auth0Provider } from '@auth0/auth0-react'

const domain = import.meta.env.VITE_AUTH0_DOMAIN
const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID
const audience = import.meta.env.VITE_AUTH0_AUDIENCE

if (!domain || !clientId) {
  throw new Error("Missing VITE_AUTH0_DOMAIN or VITE_AUTH0_CLIENT_ID in apps/web/.env")
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Auth0Provider 
      domain={domain} 
      clientId={clientId}
      authorizationParams={{
        redirect_uri: window.location.origin,
        audience: audience,
      }}>
    <App />
    </Auth0Provider>
  </StrictMode>,
)
