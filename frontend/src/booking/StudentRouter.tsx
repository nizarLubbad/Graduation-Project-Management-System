// ================================================
// StudentRouter.tsx
// Router component to control access for students
// - Checks if the user is logged in
// - Checks if the student is in a team
// - Checks if the team has a supervisor assigned
// - Displays messages accordingly
// ================================================

import { useAuth } from "../context/AuthContext";
import { Team, Assignment } from "../types/types";

export default function StudentRouter({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();

  // ================================
  // If user is not logged in
  // ================================
  if (!user) return <p>Please login first.</p>;

  // ================================
  // Load teams and supervisor assignments from localStorage
  // ================================
  const teams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
  const assignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");

  // ================================
  // Find the current user's team and assignment
  // ================================
  const myTeam = teams.find((t) => user.studentId && t.members.includes(user.studentId));
  const myAssignment = myTeam ? assignments.find((a) => a.teamId === myTeam.teamId) : null;

  // ================================
  // If user is not in a team
  // ================================
  if (!myTeam) return <p className="p-6">You are not in a team yet.</p>;

  // ================================
  // If user is in a team but no supervisor assigned
  // ================================
  if (myTeam && !myAssignment)
    return (
      <div className="p-6">
        <p>Your team is created but no supervisor assigned.</p>
        {user.studentId === myTeam.leaderId && <p>You can book a supervisor from the dashboard.</p>}
      </div>
    );

  // ================================
  // If user has a team and supervisor assigned, render children
  // ================================
  return <>{children}</>;
}
