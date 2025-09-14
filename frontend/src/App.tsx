import { Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import StudentDashboard from "./dashboards/StudentDashboard";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";
import ProtectedRoute from "./auth/ProtectedRoute";
import ProjectHistory from "./Component/ProjectHistory";
import StudentRouter from "./booking/StudentRouter";
import CreateTeam from "./booking/CreateTeam";
import BookingSupervisor from "./booking/BookingSupervisor";
import SupervisorProjectKanban from "./Component/SupervisorProjectKanban"; 
import SupervisorFeedback from "./Component/SupervisorFeedback";
import StudentFeedback from "./Component/StudentFeedback"
import SupervisorProjectFiles from "./Component/SupervisorProjectFiles";
import StudentProjectFiles from "./Component/StudentProjectFiles";
import StudentKanbanWrapper from "./Component/StudentKanbanWrapper";
import SupBoard from "./Component/SupBoard";

function App() {

  return (
    <Routes>
      <Route path="/" element={<AuthPage />} />

      {/* توجيه الطالب حسب الحالة */}
      <Route
        path="/student-router"
        element={<StudentRouter>Default Content</StudentRouter>}
      />
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

      {/* Dashboard الطلاب */}
      <Route
        path="/dashboard/student"
        element={
          <ProtectedRoute allowedRoles={["student"]}>
            <StudentDashboard />
          </ProtectedRoute>
        }
      >
  <Route path="KanbanBoard" element={<StudentKanbanWrapper />} />

        <Route path="projects" element={<StudentProjectFiles/>} />
        <Route path="reports" element={<h2 className="text-2xl font-semibold">📑 Reports Page</h2>} />
        <Route path="feedback" element={<StudentFeedback />} />
        <Route path="projectHistory" element={<ProjectHistory />} />
      </Route>

      {/* Dashboard المشرفين */}
      <Route
        path="/dashboard/supervisor"
        element={
          <ProtectedRoute allowedRoles={["supervisor"]}>
            <SupervisorDashboard />
          </ProtectedRoute>
        }
      >
        <Route index element={<SupBoard/>} />
        <Route path="reviews" element={<h2 className="text-2xl font-semibold">📝 Reviews Page</h2>} />
        <Route path="supervised-projects" element={<SupervisorProjectFiles/>} />
        <Route path="kanban/:teamId/Kanban" element={<SupervisorProjectKanban />} />
        <Route path="reports" element={<h2 className="text-2xl font-semibold">📑 Reports Page</h2>} />
        <Route path="feedback" element={<SupervisorFeedback />} />

      </Route>
    </Routes>
  );
}

export default App;
