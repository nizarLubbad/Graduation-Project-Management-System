import { Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import StudentDashboard from "./dashboards/StudentDashboard";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";
import ProtectedRoute from "./auth/ProtectedRoute";

import ProjectHistory from "./Component/ProjectHistory";
import CreateTeam from "./booking/CreateTeam";
import BookingSupervisor from "./booking/BookingSupervisor";
import SupervisorProjectKanban from "./Component/SupervisorProjectKanban"; 
import SupervisorFeedback from "./Component/SupervisorFeedback";
import StudentFeedback from "./Component/StudentFeedback";
import SupervisorProjectFiles from "./Component/SupervisorProjectFiles";
import StudentProjectFiles from "./Component/StudentProjectFiles";
import SupBoard from "./Component/SupBoard";
import StudentKanbanWrapper from "./Component/StudentKanbanWrapper";
import EditProfile from "./Component/EditProfile";
import StudentBoard from "./Component/StudentBoard";

function App() {
  return (
    <Routes>
      {/* الصفحة الرئيسية */}
      <Route path="/" element={<AuthPage />} />

      {/* مسار عام لتعديل الملف الشخصي */}
      <Route path="/edit-profile" element={<EditProfile />} />

      {/* Student Routes */}
      <Route path="/create-team" element={
        <ProtectedRoute allowedRoles={["student"]}>
          <CreateTeam />
        </ProtectedRoute>
      } />
      <Route path="/booking-supervisor" element={
        <ProtectedRoute allowedRoles={["student"]}>
          <BookingSupervisor />
        </ProtectedRoute>
      } />
      <Route path="/dashboard/student" element={
        <ProtectedRoute allowedRoles={["student"]}>
          <StudentDashboard />
        </ProtectedRoute>
      }>
        <Route path="KanbanBoard" element={<StudentKanbanWrapper />} />
        <Route path="projects" element={<StudentProjectFiles />} />
  
        <Route path="feedback" element={<StudentFeedback />} />
        <Route path="projectHistory" element={<ProjectHistory />} />
        <Route path="Myproject" element={<StudentBoard/>} />
    
      </Route>

      {/* Supervisor Routes */}
      <Route path="/dashboard/supervisor" element={
        <ProtectedRoute allowedRoles={["supervisor"]}>
          <SupervisorDashboard />
        </ProtectedRoute>
      }>
        <Route path="SupBoard" element={<SupBoard />} />
      
        <Route path="supervised-projects" element={<SupervisorProjectFiles />} />
        <Route path="kanban/:teamId/Kanban" element={<SupervisorProjectKanban />} />
        <Route path="projectHistory" element={<ProjectHistory />} />
  
        <Route path="feedback" element={<SupervisorFeedback />} />
      </Route>
    </Routes>
  );
}

export default App;
