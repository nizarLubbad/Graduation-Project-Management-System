// src/Component/SupervisorProjectKanban.tsx
import { useParams } from "react-router-dom";
import { KanbanBoard } from "./KanbanBoard";

export default function SupervisorProjectKanban() {
  const { teamId } = useParams<{ teamId: string }>();

  if (!teamId) return <p className="p-4 text-center text-red-500">⚠️ No team selected</p>;

  return <KanbanBoard teamId={teamId} />;
}
