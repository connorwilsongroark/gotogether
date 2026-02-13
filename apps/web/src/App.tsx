import { Routes, Route } from "react-router-dom";
import { AppLayout } from "./layouts/AppLayout";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import NotFound from "./pages/NotFound";
import Events from "./pages/Events";
import { Profile } from "./pages/Profile";
import { AvatarUploadTest } from "./features/profile/AvatarUploadTest";

export default function App() {
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route path='/' element={<Home />} />
        <Route path='/dashboard' element={<Dashboard />} />
        <Route path='/events' element={<Events />} />
        <Route path='/profile' element={<Profile />} />
        <Route path='/avatar-test' element={<AvatarUploadTest />} />
        {/* Catch-all 404 */}
        <Route path='*' element={<NotFound />} />
      </Route>
    </Routes>
  );
}
