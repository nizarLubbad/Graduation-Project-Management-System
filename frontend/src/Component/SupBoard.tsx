import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Assignment, User, Team, ProjectDisplay } from "../types/types";



export default function SupBoard() {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<ProjectDisplay[]>([]);

  useEffect(() => {
    const user: User | null = JSON.parse(localStorage.getItem("currentUser") || "null");
    if (!user) return;

    const assignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");
    const users: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const teams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");

    const myProjects = assignments
      .filter(a => a.supervisorName === user.name)
      .map(a => {
        const team = teams.find(t => t.teamId === a.teamId);
        return {
          id: a.teamId,
          title: a.projectTitle || a.teamName,
          teamName: a.teamName,
          description: a.projectDescription || team?.projectDescription || "",
          members: a.members.map(id => users.find(u => u.id === id)?.name || id),
        };
      });

    setProjects(myProjects);
  }, []);

  if (projects.length === 0) 
    return <p className="p-6 text-gray-600">You currently have no projects under your supervision.</p>;
  
  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">Supervised Projects</h1>

      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {projects.map(project => (
          <li
            key={project.id}
            className="p-4 bg-white rounded-xl shadow cursor-pointer hover:shadow-lg transition"
            onClick={() => navigate(`/dashboard/supervisor/kanban/${project.id}/Kanban`)}
          >
            <p className="font-semibold text-lg mb-1">{project.title}</p>
            <p className="text-sm text-gray-500">Team: {project.teamName}</p>
            {project.description && (
              <p className="text-sm text-gray-400 mb-1">Description: {project.description}</p>
            )}
            <p className="text-sm text-gray-500">Members: {project.members.join(", ")}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
