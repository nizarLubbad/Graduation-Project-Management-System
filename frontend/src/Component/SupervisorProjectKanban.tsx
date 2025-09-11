import { useParams } from "react-router-dom";
import { KanbanBoard } from "./KanbanBoard";

export default function SupervisorProjectKanban() {
  const { teamId } = useParams<{ teamId: string }>();
  if (!teamId) return <p>⚠️ No team selected</p>;
  return <KanbanBoard teamId={teamId} />;
}
