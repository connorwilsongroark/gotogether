import { Routes, Route } from "react-router-dom";
import { AppLayout } from "./layouts/AppLayouts";

export default function App() {
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route path='/' element={<div>Home</div>} />
        <Route path='/dashboard' element={<div>Dashboard</div>} />
      </Route>
    </Routes>
  );
}
