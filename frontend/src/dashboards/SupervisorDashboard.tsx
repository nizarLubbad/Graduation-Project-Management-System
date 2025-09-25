// src/dashboards/SupervisorDashboard.tsx
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useState } from "react";

export default function SupervisorDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [menuOpen, setMenuOpen] = useState(false);

  const handleLogout = () => navigate("/");

  const menuItems = [
    { path: "/dashboard/supervisor/SupBoard", label: "📊 Dashboard", exact: true },
    { path: "/dashboard/supervisor/supervised-projects", label: "📁 Supervised Projects" },
    { path: `/dashboard/supervisor/feedback`, label: "💬 Feedback" },
     { path: "/dashboard/supervisor/projectHistory", label: "📜 Project History" },
  ];

  return (
    <div className="h-screen flex flex-col bg-gray-100">
      {/* Header */}
      <header className="h-16 bg-gray-200 shadow flex items-center justify-between px-4 sm:px-6 relative z-30">
        <div>
          <h1 className="text-base sm:text-lg font-semibold text-gray-800">
            Graduation Project Management System
          </h1>
          <p className="text-xs sm:text-sm text-gray-600">
            Welcome back, {user?.name || "Supervisor"}
          </p>
        </div>

        <div className="flex items-center gap-3">
          {/* Hamburger for small screens */}
          <button
            onClick={() => setMenuOpen(!menuOpen)}
            className="lg:hidden p-2 bg-gray-300 rounded relative z-30"
          >
            <span className="block w-5 h-0.5 bg-black mb-1"></span>
            <span className="block w-5 h-0.5 bg-black mb-1"></span>
            <span className="block w-5 h-0.5 bg-black"></span>
          </button>

          {/* Role badge */}
          <span className="hidden sm:inline bg-gray-300 text-gray-800 px-2 py-0.5 rounded text-sm">
            Supervisor
          </span>

          {/* Edit Profile (Desktop) */}
          <button
            onClick={() => navigate("/edit-profile")}
            className="hidden sm:inline p-2 rounded-full bg-gray-300 hover:bg-gray-400 text-gray-800"
            title="Edit Profile"
          >
            👤
          </button>

          {/* Sign Out (Desktop) */}
          <button
            onClick={handleLogout}
            className="hidden lg:inline bg-gray-700 text-white px-3 py-1 rounded hover:bg-gray-300 hover:text-black text-sm"
          >
            Sign Out
          </button>
        </div>

        {/* Dropdown menu for mobile/tablet */}
        {menuOpen && (
          <div className="absolute top-full left-0 right-0 bg-white shadow-md border-b border-gray-300 flex flex-col lg:hidden z-20">
            {menuItems.map(item => (
              <NavLink
                key={item.path}
                to={item.path}
                end={item.exact}
                className={({ isActive }) =>
                  `block px-4 py-2 border-b last:border-b-0 ${
                    isActive ? "bg-black text-white" : "text-gray-700 hover:bg-gray-200"
                  }`
                }
                onClick={() => setMenuOpen(false)}
              >
                {item.label}
              </NavLink>
            ))}
            {/* Edit Profile (Mobile) */}
            <button
              onClick={() => {
                setMenuOpen(false);
                navigate("/edit-profile");
              }}
              className="px-4 py-2 text-left text-gray-800 hover:bg-gray-200 border-b last:border-b-0"
            >
              👤 Edit Profile
            </button>
            {/* Sign Out (Mobile) */}
            <button
              onClick={() => {
                setMenuOpen(false);
                handleLogout();
              }}
              className="px-4 py-2 text-left text-black hover:bg-gray-700 hover:text-white last:border-b-0"
            >
              Sign Out
            </button>
          </div>
        )}
      </header>

      <div className="flex flex-1">
        {/* Sidebar for large screens */}
        <aside className="hidden lg:block top-16 h-full w-64 bg-white shadow-md p-4">
          <nav className="space-y-2">
            {menuItems.map(item => (
              <NavLink
                key={item.path}
                to={item.path}
                end={item.exact}
                className={({ isActive }) =>
                  `block p-2 rounded font-medium transition ${
                    isActive ? "bg-black text-white" : "text-gray-700 hover:bg-gray-200 hover:text-black"
                  }`
                }
              >
                {item.label}
              </NavLink>
            ))}
          </nav>
        </aside>

        {/* Main content */}
        <main className="flex-1 p-4 sm:p-6 overflow-y-auto bg-white mt-16 lg:mt-0">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
