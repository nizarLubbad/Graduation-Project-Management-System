import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Assignment, User } from "../types/types";

interface ProjectDisplay {
  id: string;
  title: string;
  teamName: string;
  members: string[];
}

export default function SupBoard() {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<ProjectDisplay[]>([]);

  useEffect(() => {
    // جلب المستخدم الحالي
    const user: User | null = JSON.parse(localStorage.getItem("currentUser") || "null");
    if (!user) return;

    // جلب البيانات المخزنة من localStorage
    const assignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");
    const users: User[] = JSON.parse(localStorage.getItem("users") || "[]");

    // فلترة المشاريع الخاصة بالمشرف الحالي وتحضير بيانات العرض
    const myProjects = assignments
      .filter(a => a.supervisorName === user.name)
      .map(a => ({
        id: a.teamId,
        title: a.projectTitle || a.teamName,
        teamName: a.teamName,
        members: a.members.map(id => users.find(u => u.id === id)?.name || id),
      }));

    setProjects(myProjects);
  }, []);

  // حالة عدم وجود مشاريع
  if (projects.length === 0) {
    return (
      <div className="flex justify-center items-center h-[70vh]">
        <p className="text-gray-600 text-center text-lg">
          No projects are currently supervised by you.
        </p>
      </div>
    );
  }

  return (
    <div className="p-4 sm:p-6 md:p-8 space-y-6">
      <h1 className="text-2xl sm:text-3xl font-bold text-gray-800">Supervised Projects</h1>

      {/* Grid responsive لكل الشاشات */}
      <ul className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
        {projects.map(project => (
          <li
            key={project.id}
            className="p-4 bg-white rounded-xl shadow hover:shadow-lg transition cursor-pointer flex flex-col justify-between"
            onClick={() => navigate(`/dashboard/supervisor/supervised-projects/${project.id}/Kanban`)}
          >
            <div>
              <p className="font-semibold text-lg mb-1 text-gray-800 truncate">{project.title}</p>
              <p className="text-sm text-gray-500 truncate">Team: {project.teamName}</p>
              <p className="text-sm text-gray-500 truncate">Members: {project.members.join(", ")}</p>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
