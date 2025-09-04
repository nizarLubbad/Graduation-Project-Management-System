
import { Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import  StudentDashboard  from "./dashboards/StudentDashboard";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";

import ProtectedRoute from "./auth/ProtectedRoute";
import { KanbanBoard } from "./Component/KanbanBoard";

function App() {
  return (
    <Routes>

      <Route path="/" element={<AuthPage />} />

      /*
  📝 Added child routes to StudentDashboard

  - The default index route renders the KanbanBoard component.
  - "/projects" route displays a simple heading for the Projects Page.
  - "/reports" route displays a simple heading for the Reports Page.
  - "/feedback" route displays a simple heading for the Feedback Page.

  These routes are nested inside the ProtectedRoute wrapper
  allowing only users with the "student" role to access them.
*/
      <Route
        path="/dashboard/student"
        element={
          <ProtectedRoute allowedRoles={["student"]}>
            <StudentDashboard />
          </ProtectedRoute>
        }
      >
          <Route
          index
          element={<KanbanBoard />}
        />
      
        <Route path="projects" element={<h2 className="text-2xl font-semibold">📁 Projects Page</h2>} />
        <Route path="reports" element={<h2 className="text-2xl font-semibold">📑 Reports Page</h2>} />
        <Route path="feedback" element={<h2 className="text-2xl font-semibold">Feedback Page</h2>} />
      </Route>
      
    

      {/* مسار المشرف */}
      <Route
        path="/dashboard/supervisor"
        element={
          <ProtectedRoute allowedRoles={["supervisor"]}>
            <SupervisorDashboard />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
}

export default App;
