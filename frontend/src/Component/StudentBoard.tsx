import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import Swal from "sweetalert2";

interface ProjectData {
  id: number;
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
  project?: ProjectData | null; // يمكن يكون undefined
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
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  const fetchTeamData = async () => {
    if (!user?.team?.teamId) return;

    try {
      const resTeam = await fetch(`${baseUrl}/api/Teams/${user.team.teamId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const teamData: TeamData = await resTeam.json();
      console.log("🔎 Team Data:", teamData);

      // جلب اسم المشرف إذا موجود
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

      setTeam(teamData);

      // جلب أعضاء الفريق
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
  if (!team.project) return <p>No project assigned yet!</p>; // تحقق صارم

  const { project } = team;

  const toggleProjectComplete = async () => {
    if (!project?.id) {
      Swal.fire("Error", "Project ID is missing!", "error");
      return;
    }

    console.log("🚀 Toggling project:", project);

    try {
      const res = await fetch(`${baseUrl}/api/Project/${project.id}/complete`, {
        method: "PATCH",
        headers: { Authorization: `Bearer ${user.token}` },
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error(`Failed to update project (status: ${res.status}, message: ${text})`);
      }

      const updatedRes = await fetch(`${baseUrl}/api/Project/${project.id}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });

      if (!updatedRes.ok) {
        throw new Error("Failed to fetch updated project");
      }

      const updatedProject: ProjectData = await updatedRes.json();
      console.log("✅ Updated Project:", updatedProject);

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
        <h1 className="text-5xl font-extrabold text-gray-800 mb-6">🎓 My Project</h1>

        <div className="bg-white rounded-3xl shadow-2xl p-10 text-left relative">
          <button
            onClick={toggleProjectComplete}
            className="absolute top-4 right-4 bg-black text-white px-4 py-2 rounded hover:bg-gray-800"
          >
            {project.isCompleted ? "Mark Incomplete" : "Mark Complete"}
          </button>

          <p className="text-2xl font-semibold mb-4">
            Project Name: <span className="font-normal">{project.projectTitle}</span>
          </p>
          {project.description && (
            <p className="text-lg text-gray-700 mb-4">
              Description: <span className="font-normal">{project.description}</span>
            </p>
          )}
          <p className="text-lg text-gray-700 mb-2">
            Supervisor: <span className="font-normal">{team.supervisorName ?? "Not assigned"}</span>
          </p>
          <p className="text-lg text-gray-700">
            Team Members: <span className="font-normal">{teamMembers.map(m => m.name).join(", ")}</span>
          </p>
          <p className="text-lg mt-2 font-semibold">
            Status: {project.isCompleted ? "✅ Completed" : "❌ Incomplete"}
          </p>
        </div>
      </div>
    </div>
  );
}
