import { useParams } from "react-router-dom";
import { KanbanBoard } from "./KanbanBoard";

export default function SupervisorProjectKanban() {
  const { teamId } = useParams<{ teamId: string }>();
  if (!teamId) return <p>⚠️ No team selected</p>;

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Project Kanban</h1>
      <KanbanBoard teamId={teamId} />
    </div>
  );
}
