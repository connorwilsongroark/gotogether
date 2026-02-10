import { Routes, Route } from "react-router-dom";
import { AppLayout } from "./layouts/AppLayout";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import NotFound from "./pages/NotFound";
import Events from "./pages/Events";

export default function App() {
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route path='/' element={<Home />} />
        <Route path='/dashboard' element={<Dashboard />} />
        <Route path='/events' element={<Events />} />
        {/* Catch-all 404 */}
        <Route path='*' element={<NotFound />} />
      </Route>
    </Routes>
  );
}
