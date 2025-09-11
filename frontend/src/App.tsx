import { Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import StudentDashboard from "./dashboards/StudentDashboard";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";
import ProtectedRoute from "./auth/ProtectedRoute";
import ProjectHistory from "./Component/ProjectHistory";
import Supboard from "./Component/SupBoard";
import StudentRouter from "./booking/StudentRouter";
import CreateTeam from "./booking/CreateTeam";
import BookingSupervisor from "./booking/BookingSupervisor";
import StudentKanbanWrapper from "./Component/StudentKanbanWrapper";
import SupervisorProjectKanban from "./Component/SupervisorProjectKanban";


function App() {


  return (
    <Routes>
      {/* Auth */}
      <Route path="/" element={<AuthPage />} />

      {/* Student Routing */}
      <Route path="/student-router" element={<StudentRouter>Default Content</StudentRouter>} />
      <Route
        path="/create-team"
        element={
          <ProtectedRoute allowedRoles={["student"]}>
            <CreateTeam />
          </ProtectedRoute>
        }
      />
      <Route
        path="/booking-supervisor"
        element={
          <ProtectedRoute allowedRoles={["student"]}>
            <BookingSupervisor />
          </ProtectedRoute>
        }
      />

      {/* Student Dashboard */}
      <Route
        path="/dashboard/student"
        element={
          <ProtectedRoute allowedRoles={["student"]}>
            <StudentDashboard />
          </ProtectedRoute>
        }
      >
        <Route path="KanbanBoard" element={<StudentKanbanWrapper />} />
        <Route path="projects" element={<h2 className="text-2xl font-semibold">📁 Projects Page</h2>} />
        <Route path="reports" element={<h2 className="text-2xl font-semibold">📑 Reports Page</h2>} />
        <Route path="feedback" element={<h2 className="text-2xl font-semibold">Feedback Page</h2>} />
        <Route path="projectHistory" element={<ProjectHistory />} />
      </Route>

      {/* Supervisor Dashboard */}
      <Route
        path="/dashboard/supervisor"
        element={
          <ProtectedRoute allowedRoles={["supervisor"]}>
            <SupervisorDashboard />
          </ProtectedRoute>
        }
      >
        <Route index element={<Supboard />} />
        <Route path="reviews" element={<h2 className="text-2xl font-semibold">📝 Reviews Page</h2>} />
        <Route path="supervised-projects" element={<h2 className="text-2xl font-semibold">📁 Supervised Projects Page</h2>} />
        <Route path="supervised-projects/:teamId/Kanban" element={<SupervisorProjectKanban />} />
        <Route path="reports" element={<h2 className="text-2xl font-semibold">📑 Reports Page</h2>} />
        <Route path="feedback" element={<h2 className="text-2xl font-semibold">💬 Feedback Page</h2>} />
      </Route>
    </Routes>
  );
}

export default App;
