
import { Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import  StudentDashboard  from "./dashboards/StudentDashboard";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";

import ProtectedRoute from "./auth/ProtectedRoute";

function App() {
  return (
    <Routes>

      <Route path="/" element={<AuthPage />} />


      <Route
        path="/dashboard/student"
        element={
         
            <StudentDashboard />
         
        }
      >

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
