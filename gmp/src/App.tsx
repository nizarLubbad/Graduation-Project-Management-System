// app.tsx
import React from "react";
import { Routes, Route } from "react-router-dom";
import SupervisorDashboard from "./dashboards/SupervisorDashboard";

import AuthPage from "./auth/AuthPage";
import { StudentDashboard } from "./dashboards/StudentDashboard";


function App() {
  return (
 
      <Routes>
             <Route path="/" element={<AuthPage/>} />
          <Route path="/dashboard/student" element={<StudentDashboard/>} />
          <Route path="/dashboard/supervisor" element={<SupervisorDashboard />} />
      </Routes>

  );
}

export default App;
