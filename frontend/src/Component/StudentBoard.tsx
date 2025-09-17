import { useEffect, useState } from "react";
import { Assignment, User } from "../types/types";

interface StudentProject {
  id: string;
  title: string;
  teamName: string;
  description: string;
  members: string[];
  supervisorName: string;
}

export default function StudentBoard() {
  const [project, setProject] = useState<StudentProject | null>(null);

  useEffect(() => {
    const currentUser: User | null = JSON.parse(
      localStorage.getItem("currentUser") || "null"
    );
    if (!currentUser?.team?.id) return;

    const assignments: Assignment[] = JSON.parse(
      localStorage.getItem("supervisorAssignments") || "[]"
    );

    const myAssignment = assignments.find(
      a => a.teamId === currentUser.team?.id
    );

    if (myAssignment) {
      setProject({
        id: myAssignment.teamId,
        title: myAssignment.projectTitle || myAssignment.teamName,
        teamName: myAssignment.teamName,
        description: myAssignment.projectDescription || "",
        members: myAssignment.members,
        supervisorName: myAssignment.supervisorName,
      });
    }
  }, []);

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-6">
      <div className="w-full max-w-3xl text-center">
        <h1 className="text-5xl font-extrabold text-gray-800 mb-10">🎓 My Project</h1>

        {project ? (
          <div className="bg-white rounded-3xl shadow-2xl p-10 text-left">
            <p className="text-2xl font-semibold mb-4">
              Project Name: <span className="font-normal">{project.title}</span>
            </p>
            {project.description && (
              <p className="text-lg text-gray-700 mb-4">
                Description: <span className="font-normal">{project.description}</span>
              </p>
            )}
            <p className="text-lg text-gray-700 mb-2">
              Supervisor: <span className="font-normal">{project.supervisorName}</span>
            </p>
            <p className="text-lg text-gray-700">
              Team Members: <span className="font-normal">{project.members.join(", ")}</span>
            </p>
          </div>
        ) : (
          <p className="text-gray-600 text-lg">You do not have an assigned project yet.</p>
        )}
      </div>
    </div>
  );
}
