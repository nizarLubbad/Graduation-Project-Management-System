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

  if (!user) return <p>Please login first.</p>;

  const teams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
  const assignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");

  const myTeam = teams.find((t) => user.studentId && t.members.includes(user.studentId));
  const myAssignment = myTeam ? assignments.find((a) => a.teamId === myTeam.teamId) : null;

  if (!myTeam) return <p className="p-6">You are not in a team yet.</p>;
  if (myTeam && !myAssignment)
    return (
      <div className="p-6">
        <p>Your team is created but no supervisor assigned.</p>
        {user.studentId === myTeam.leaderId && <p>You can book a supervisor from the dashboard.</p>}
      </div>
    );

  return <>{children}</>;
}
