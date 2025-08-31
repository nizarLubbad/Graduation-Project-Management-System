// app.tsx
import React from "react";
import { BrowserRouter as  Routes, Route } from "react-router-dom";
import AuthPage from "./auth/AuthPage";
import Dashboard from "./pages/Dashboard";


function App() {
  return (
 
      <Routes>
        <Route path="/" element={<AuthPage />} />
        <Route path="/dashboard" element={<Dashboard />} />
   
      </Routes>

  );
}

export default App;
