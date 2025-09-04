import {  NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function StudentDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const handleLogout = () => {
    navigate("/"); 
  };

  return (
    <div className="h-screen flex flex-col bg-gray-100">

    <header className="h-20 bg-gray-200 shadow flex items-center justify-between px-6">
      {/* Left */}
      <div>
        <h1 className="text-lg font-semibold text-gray-800">
          Graduation Project Management System
        </h1>
        <p className="text-sm text-gray-600">
          Welcome back, {user?.name || "Guest"}
        </p>
      </div>

      {/* Right */}
      <div className="flex items-center gap-4">
        <span className="text-gray-700 font-medium">
          {user?.role === "student" ? "Student" : "Supervisor"}
        </span>
        <button
          onClick={handleLogout}
          className="bg-pink-500 text-white px-3 py-1 rounded hover:bg-pink-300"
        >
          Sign Out
        </button>
      </div>
    </header>
    <div className="flex flex-1">
        
        <aside className="w-64 bg-white shadow-md p-4">
          <nav className="space-y-2">
            <NavLink
              to="/dashboard/student"
              end
              className={({ isActive }) =>
                `block p-2 rounded font-medium transition ${
                  isActive
                    ? "bg-blue-600 text-white"
                    : "hover:bg-pink-600 hover:text-white text-gray-700"
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
                    ? "bg-blue-600 text-white"
                    : "hover:bg-pink-600 hover:text-white text-gray-700"
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
                    ? "bg-blue-600 text-white"
                    : "hover:bg-pink-600 hover:text-white text-gray-700"
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
                    ? "bg-blue-600 text-white"
                    : "hover:bg-pink-600 hover:text-white text-gray-700"
                }`
              }
            >
              💬 Feedback
            </NavLink>
          </nav>
        </aside>
            {/* Content */}
      <main className="flex-1 p-6 overflow-y-auto bg-white">
          <Outlet />
        </main>
    </div>
    </div>
  )
}