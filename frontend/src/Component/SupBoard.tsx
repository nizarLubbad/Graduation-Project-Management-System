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
    <div>
      
    </div>
  )
}

