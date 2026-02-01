import { NavLink } from "react-router-dom";
import { LayoutDashboard, Calendar, CalendarDays } from "lucide-react";

const linkClass = ({ isActive }: { isActive: boolean }) =>
  [
    "flex items-center gap-3 rounded-md px-3 py-2 text-sm transition-colors",
    isActive ? "bg-surface-hover font-medium" : "hover:bg-surface-hover",
  ].join(" ");

export default function SideNav() {
  return (
    <aside className='w-56 border-r border-border bg-bg p-3'>
      <div className='space-y-1'>
        <NavLink to='/dashboard' className={linkClass}>
          <LayoutDashboard className='h-4 w-4' />
          Dashboard
        </NavLink>

        <NavLink to='/calendar' className={linkClass}>
          <Calendar className='h-4 w-4' />
          Calendar
        </NavLink>

        <NavLink to='/events' className={linkClass}>
          <CalendarDays className='h-4 w-4' />
          Events
        </NavLink>
      </div>
    </aside>
  );
}
