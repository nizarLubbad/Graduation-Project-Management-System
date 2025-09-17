// src/dashboards/StudentDashboard.tsx
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useState, useEffect } from "react";
import { Assignment } from "../types/types";

export default function StudentDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [menuOpen, setMenuOpen] = useState(false);
  const [canAccessDashboard, setCanAccessDashboard] = useState(false);

  useEffect(() => {
    if (!user?.team?.id) {
      setCanAccessDashboard(false);
      return;
    }
    const assignments: Assignment[] = JSON.parse(
      localStorage.getItem("supervisorAssignments") || "[]"
    );
    const myAssignment = assignments.find(a => a.teamId === user.team?.id);
    setCanAccessDashboard(!!myAssignment);
  }, [user]);

  const handleLogout = () => navigate("/");

  const menuItems = [
    { path: "KanbanBoard", label: "📊 Dashboard" },
    { path: "projects", label: "📁 Projects" },
    { path: "feedback", label: "💬 Feedback" },
    { path: "projectHistory", label: "📜 Project History" },
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
            Welcome back, {user?.name || "Guest"}
          </p>
          {user?.team?.id && (
            <p className="text-xs text-gray-500">
              Team: <span className="font-semibold">{user.team.name}</span>
            </p>
          )}
        </div>

        <div className="flex items-center gap-3">
          {/* Hamburger for small screens */}
          <button
            onClick={() => setMenuOpen(!menuOpen)}
            className="sm:hidden p-2 bg-gray-300 rounded relative z-30"
          >
            <span className="block w-5 h-0.5 bg-black mb-1"></span>
            <span className="block w-5 h-0.5 bg-black mb-1"></span>
            <span className="block w-5 h-0.5 bg-black"></span>
          </button>

          {/* Label */}
          <span className="hidden sm:inline bg-gray-300 text-gray-800 px-2 py-0.5 rounded text-sm">
            Student
          </span>

          {/* Edit Profile (Desktop) */}
          <button
            onClick={() => navigate("/edit-profile")}
            className="hidden sm:inline p-2 rounded bg-gray-300 hover:bg-gray-400 text-gray-800"
            title="Edit Profile"
          >
            👤
          </button>

          {/* Sign Out (Desktop) */}
          <button
            onClick={handleLogout}
            className="hidden sm:inline bg-black text-white px-3 py-1 rounded hover:bg-gray-800 text-sm"
          >
            Sign Out
          </button>
        </div>

        {/* Dropdown menu for mobile */}
        {menuOpen && (
          <div className="absolute top-full left-0 right-0 bg-white shadow-md border-b border-gray-300 flex flex-col sm:hidden z-20">
            {menuItems.map(item => (
              <NavLink
                key={item.path}
                to={`/dashboard/student/${item.path}`}
                className={({ isActive }) =>
                  `block px-4 py-2 border-b last:border-b-0 ${
                    isActive
                      ? "bg-black text-white"
                      : "text-gray-700 hover:bg-gray-200"
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
        <aside className="hidden sm:block top-16 h-full w-64 bg-white shadow-md p-4">
          <nav className="space-y-2">
            {menuItems.map(item => (
              <NavLink
                key={item.path}
                to={`/dashboard/student/${item.path}`}
                className={({ isActive }) =>
                  `block p-2 rounded font-medium transition ${
                    isActive
                      ? "bg-black text-white"
                      : "text-gray-700 hover:bg-gray-200 hover:text-black"
                  }`
                }
              >
                {item.label}
              </NavLink>
            ))}
          </nav>
        </aside>

        {/* Main content */}
        {canAccessDashboard ? (
          <main className="flex-1 p-4 sm:p-6 overflow-y-auto bg-white mt-16 sm:mt-0">
            <Outlet />
          </main>
        ) : (
          <main className="flex-1 flex items-center justify-center bg-white mt-16 sm:mt-0">
            <div className="text-center">
              <h2 className="text-xl font-semibold text-gray-800 mb-2">
                ⚠️ You are not in a team or no supervisor assigned yet
              </h2>
              <p className="text-gray-600 mb-4">
                Please create a team and book a supervisor to access the dashboard features.
              </p>
              <button
                onClick={() => navigate("/create-team")}
                className="bg-black text-white px-4 py-2 rounded hover:bg-gray-800"
              >
                Create Team
              </button>
            </div>
          </main>
        )}
      </div>
    </div>
  );
}
