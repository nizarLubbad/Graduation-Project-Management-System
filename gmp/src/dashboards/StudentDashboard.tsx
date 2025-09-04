import {  useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function StudentDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const handleLogout = () => {
    navigate("/"); 
  };

  return (
    <div className="h-screen flex flex-col bg-gray-100">
    {/* Header */}
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
          className="bg-red-500 text-white px-3 py-1 rounded hover:bg-pink-600"
        >
          Sign Out
        </button>
      </div>
    </header>
    </div>
  )
}
