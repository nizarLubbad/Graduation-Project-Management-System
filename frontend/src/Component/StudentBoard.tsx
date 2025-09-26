import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import Swal from "sweetalert2";

interface ProjectData {
  projectId: number;
  projectTitle: string;
  description: string;
  supervisorId?: number | null;
  supervisorName?: string | null;
  teamId: number;
  isCompleted: boolean;
}

interface TeamData {
  teamId: number;
  teamName: string;
  memberStudentIds: number[];
  project?: ProjectData | null;
  supervisorName?: string | null;
  supervisorId?: number | null;
}

interface Student {
  userId: number;
  name: string;
}

export default function StudentBoard() {
  const { user } = useAuth();
  const [team, setTeam] = useState<TeamData | null>(null);
  const [teamMembers, setTeamMembers] = useState<Student[]>([]);
  const baseUrl = import.meta.env.VITE_API_URL;

  const fetchProject = async (projectId: number) => {
    const res = await fetch(`${baseUrl}/api/Project/${projectId}`, {
      headers: { Authorization: `Bearer ${user?.token}` },
    });
    if (!res.ok) throw new Error("Failed to fetch project");
    const projectData: ProjectData = await res.json();
    return projectData;
  };

  const fetchTeamData = async () => {
    if (!user?.team?.teamId) return;
    try {
      const resTeam = await fetch(`${baseUrl}/api/Teams/${user.team.teamId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const teamData: TeamData = await resTeam.json();

      if (teamData.supervisorId) {
        try {
          const resSupervisor = await fetch(`${baseUrl}/api/Auth/${teamData.supervisorId}`, {
            headers: { Authorization: `Bearer ${user.token}` },
          });
          const supervisorData = await resSupervisor.json();
          teamData.supervisorName = supervisorData.name ?? "Unknown";
        } catch {
          teamData.supervisorName = "Unknown";
        }
      }

      if (teamData.project?.projectId) {
        const projectData = await fetchProject(teamData.project.projectId);
        teamData.project = { ...teamData.project, ...projectData };
      }

      setTeam(teamData);

      const resStudents = await fetch(`${baseUrl}/api/Students/all`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const studentsData: { students: Student[] } = await resStudents.json();
      const members = studentsData.students.filter(s =>
        teamData.memberStudentIds.includes(s.userId)
      );
      setTeamMembers(members);
    } catch (err) {
      console.error("Failed to fetch team/project data", err);
      Swal.fire("Error", "Failed to load project data", "error");
    }
  };

  useEffect(() => {
    fetchTeamData();
  }, [user]);

  if (!user) return <p>Loading user...</p>;
  if (!team) return <p>Loading project...</p>;
  if (!team.project) return <p>No project assigned yet!</p>;

  const { project } = team;

  const toggleProjectComplete = async () => {
    if (!project?.projectId) return;

    try {
      await fetch(`${baseUrl}/api/Project/${project.projectId}/complete`, {
        method: "PATCH",
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const updatedProject = await fetchProject(project.projectId);
      setTeam(prev => prev ? { ...prev, project: updatedProject } : null);

      Swal.fire({
        icon: "success",
        title: "Project status updated",
        text: `Project is now ${updatedProject.isCompleted ? "Completed ✅" : "Incomplete ❌"}`,
      });
    } catch (err) {
      console.error(err);
      Swal.fire({
        icon: "error",
        title: "Failed",
        text: err instanceof Error ? err.message : "Could not update project status",
      });
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-6">
      <div className="w-full max-w-3xl text-center">
        <h1 className="text-4xl md:text-5xl font-extrabold text-gray-800 mb-6">🎓 My Project</h1>

        <div className="bg-white rounded-3xl shadow-2xl p-6 md:p-10 text-left">
          {/* ✅ العنوان + زر التبديل */}
          <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-6">
            <p className="text-2xl font-semibold">
              Project Name:{" "}
              <span className="font-normal break-words">
                {project.projectTitle ?? "Untitled"}
              </span>
            </p>
            <button
              onClick={toggleProjectComplete}
              className="w-full md:w-auto bg-black text-white px-4 py-2 rounded-lg hover:bg-gray-800 transition"
            >
              {project.isCompleted ? "Mark Incomplete" : "Mark Complete"}
            </button>
          </div>

          {project.description && (
            <p className="text-lg text-gray-700 mb-4 break-words">
              Description: <span className="font-normal">{project.description}</span>
            </p>
          )}

          <p className="text-lg text-gray-700 mb-2">
            Supervisor: <span className="font-normal">{team.supervisorName ?? "Not assigned"}</span>
          </p>
          <p className="text-lg text-gray-700 mb-2">
            Team Members:{" "}
            <span className="font-normal">{teamMembers.map(m => m.name).join(", ")}</span>
          </p>
          <p className="text-lg mt-4 font-semibold">
            Status: {project.isCompleted ? "✅ Completed" : "❌ Incomplete"}
          </p>
        </div>
      </div>
    </div>
  );
}
