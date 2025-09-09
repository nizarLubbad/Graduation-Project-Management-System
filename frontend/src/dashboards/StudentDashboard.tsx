import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useState } from "react";

export default function StudentDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);

  const handleLogout = () => {
    navigate("/");
  };

  return (
    <div className="h-screen flex flex-col bg-gray-100">
      {/* Header */}
      <header className="h-16 bg-gray-200 shadow flex items-center justify-between px-4 sm:px-6">
        {/* Left */}
        <div>
          <h1 className="text-base sm:text-lg font-semibold text-gray-800">
            Graduation Project Management System
          </h1>
          <p className="text-xs sm:text-sm text-gray-600">
            Welcome back, {user?.name || "Guest"}
          </p>
        </div>

        {/* Right */}
        <div className="flex items-center gap-3">
          <span className="hidden sm:inline bg-gray-300 text-gray-800 px-2 py-0.5 rounded text-sm">
            {user?.role === "student" ? "Student" : "Supervisor"}
          </span>

          {/* Mobile Menu Button */}
          <button
            className="lg:hidden p-2 text-gray-700 hover:bg-gray-300 rounded"
            onClick={() => setIsOpen(!isOpen)}
          >
            ☰
          </button>

          {/* Logout for larger screens */}
          <button
            onClick={handleLogout}
            className="hidden sm:inline bg-gray-700 text-white px-3 py-1 rounded hover:bg-gray-300 hover:text-black text-sm"
          >
            Sign Out
          </button>
        </div>
      </header>

      <div className="flex flex-1">
        {/* Sidebar */}
        <aside
          className={`fixed lg:static top-16 left-0 h-full lg:h-auto w-64 bg-white shadow-md p-4 transform transition-transform duration-300 ease-in-out z-20
          ${isOpen ? "translate-x-0" : "-translate-x-full lg:translate-x-0"}`}
        >
          <nav className="space-y-2">
            <NavLink
              to="/dashboard/student"
              end
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-black text-white"
                    : "text-gray-700 hover:bg-gray-200 hover:text-black"
                }`
              }
            >
              📊 Dashboard
            </NavLink>

            <NavLink
              to="/dashboard/student/projects"
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-black text-white"
                    : "text-gray-700 hover:bg-gray-200 hover:text-black"
                }`
              }
            >
              📁 Projects
            </NavLink>

            <NavLink
              to="/dashboard/student/reports"
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-black text-white"
                    : "text-gray-700 hover:bg-gray-200 hover:text-black"
                }`
              }
            >
              📑 Reports
            </NavLink>

            <NavLink
              to="/dashboard/student/feedback"
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-black text-white"
                    : "text-gray-700 hover:bg-gray-200 hover:text-black"
                }`
              }
            >
              💬 Feedback
            </NavLink>

            <NavLink
              to="/dashboard/student/projectHistory"
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-black text-white"
                    : "text-gray-700 hover:bg-gray-200 hover:text-black"
                }`
              }
            >
              📜 Project History
            </NavLink>

            {/* Logout for mobile */}
            <button
              onClick={handleLogout}
              className="block w-full text-left px-2 py-1 mt-4 bg-gray-700 text-white rounded hover:bg-gray-300 hover:text-black lg:hidden"
            >
              Sign Out
            </button>
          </nav>
        </aside>

        {/* Overlay for mobile */}
        {isOpen && (
          <div
            className="fixed inset-0 bg-black bg-opacity-30 z-10 lg:hidden"
            onClick={() => setIsOpen(false)}
          />
        )}

        {/* Content */}
        <main className="flex-1 p-4 sm:p-6 overflow-y-auto bg-white mt-16 lg:mt-0">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
