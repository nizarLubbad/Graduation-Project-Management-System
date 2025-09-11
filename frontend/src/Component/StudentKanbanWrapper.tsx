// src/booking/StudentKanbanWrapper.tsx
import { KanbanBoard } from "../Component/KanbanBoard";
import { useAuth } from "../context/AuthContext";

export default function StudentKanbanWrapper() {
  const { user } = useAuth();

  if (!user?.team?.id) {
    return (
      <p className="p-4 text-center text-red-500">
        ⚠️ You need to create a team first
      </p>
    );
  }

  return <KanbanBoard teamId={user.team.id} />;
}
