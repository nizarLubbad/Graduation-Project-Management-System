import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useState, useEffect } from "react";
import { Assignment } from "../types/types";
import { Menu } from "lucide-react";

export default function StudentDashboard() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);
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

  return (
    <div className="h-screen flex flex-col bg-gray-100">
      {/* Header */}
      <header className="h-16 bg-teal-700/90 backdrop-blur-md shadow-md flex items-center justify-between px-4 sm:px-6">
        <div className="flex flex-col sm:flex-row sm:items-center sm:gap-4">
          <h1 className="text-lg sm:text-xl font-bold text-white">
            Graduation Project Management
          </h1>
          <p className="text-xs sm:text-sm text-white/80">
            Welcome back, {user?.name || "Guest"}
          </p>
          {user?.team?.id && (
            <p className="text-xs sm:text-sm text-white/80">
              Team: <span className="font-semibold">{user.team.name}</span>
            </p>
          )}
        </div>

        <div className="flex items-center gap-2">
          {/* Hamburger menu for mobile */}
          <button
            className="lg:hidden p-2 bg-teal-600 text-white rounded-md hover:bg-teal-500 transition"
            onClick={() => setIsOpen(!isOpen)}
          >
            <Menu className="h-5 w-5" />
          </button>

          <span className="hidden sm:inline bg-white/30 text-white px-2 py-0.5 rounded text-sm">
            Student
          </span>

          <button
            onClick={handleLogout}
            className="hidden sm:inline bg-white text-teal-700 px-3 py-1 rounded hover:bg-white/80 hover:text-teal-900 transition"
          >
            Sign Out
          </button>
        </div>
      </header>

      <div className="flex flex-1">
        {canAccessDashboard ? (
          <>
            {/* Sidebar */}
            <aside
              className={`fixed lg:static top-16 left-0 h-full lg:h-auto w-64 bg-white shadow-md p-4 transform transition-transform duration-300 ease-in-out z-20
              ${isOpen ? "translate-x-0" : "-translate-x-full lg:translate-x-0"} rounded-r-xl`}
            >
              <nav className="space-y-2">
                {[
                  { path: "/dashboard/student/KanbanBoard", label: "📊 Dashboard" },
                  { path: "/dashboard/student/projects", label: "📁 Projects" },
                  { path: "/dashboard/student/reports", label: "📑 Reports" },
                  { path: "/dashboard/student/feedback", label: "💬 Feedback" },
                  { path: "/dashboard/student/projectHistory", label: "📜 Project History" },
                ].map(link => (
                  <NavLink
                    key={link.path}
                    to={link.path}
                    end
                    className={({ isActive }) =>
                      `block p-2 rounded font-medium transition ${
                        isActive
                          ? "bg-teal-700 text-white"
                          : "text-gray-700 hover:bg-gray-200 hover:text-black"
                      }`
                    }
                  >
                    {link.label}
                  </NavLink>
                ))}

                <button
                  onClick={handleLogout}
                  className="block w-full text-left px-2 py-1 mt-4 bg-teal-700 text-white rounded hover:bg-teal-600 lg:hidden transition"
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

            {/* Main content */}
            <main className="flex-1 p-4 sm:p-6 lg:p-8 overflow-y-auto bg-gray-50 mt-16 lg:mt-0 rounded-lg">
              <Outlet />
            </main>
          </>
        ) : (
          // Student without team or assignment
          <main className="flex-1 flex items-center justify-center bg-gray-50 mt-16 lg:mt-0">
            <div className="text-center p-6 bg-white rounded-xl shadow-lg max-w-md mx-4">
              <h2 className="text-xl sm:text-2xl font-semibold text-gray-800 mb-2">
                ⚠️ You are not in a team or no supervisor assigned yet
              </h2>
              <p className="text-gray-600 mb-4">
                Please create a team and book a supervisor to access the dashboard features.
              </p>
              <button
                onClick={() => navigate("/create-team")}
                className="bg-teal-700 text-white px-4 py-2 rounded hover:bg-teal-600 transition"
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
