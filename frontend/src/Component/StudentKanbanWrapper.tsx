// src/booking/StudentKanbanWrapper.tsx
import { KanbanBoard } from "../Component/KanbanBoard";
import { useAuth } from "../context/AuthContext";

export default function StudentKanbanWrapper() {
  const { user } = useAuth();

  if (!user?.team?.teamId) {
    return (
      <p className="p-4 text-center text-red-500">
        ⚠️ You need to create a team first
      </p>
    );
  }

  // تحويل teamId إلى string لأنه prop معرف كـ string
  const teamId = String(user.team.teamId);

  return <KanbanBoard teamId={teamId} />;
}
