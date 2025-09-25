// SupervisorProjectFiles.tsx
import { useEffect, useState } from "react";

import { useAuth } from "../context/AuthContext";

interface Project {
  id: number;
  projectName: string;
  description: string;
  isCompleted: boolean;
  supervisorId: number | null;
  supervisorName: string | null;
  teamId: number;
  teamName: string;
}

interface ProjectFile {
  id: number;
  title: string;
  url: string;
  date: string;
  studentId: number;
  teamId: number;
}

interface Student {
  userId: number;
  name: string;
}

export default function SupervisorProjectFiles() {
  const { user } = useAuth();
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<number | "">("");
  const [files, setFiles] = useState<ProjectFile[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState(false);

  // 🔹 Fetch supervisor projects
  useEffect(() => {
    if (!user) return;
    setLoading(true);
    fetch(`${baseUrl}/api/Project`)
      .then(res => res.json())
      .then((allProjects: Project[]) => {
        const myProjects = allProjects.filter(p => p.supervisorId === user.userId);
        setProjects(myProjects);
      })
      .catch(err => console.error("Error fetching projects:", err))
      .finally(() => setLoading(false));
  }, [user]);

  // 🔹 Fetch students
  useEffect(() => {
    fetch(`${baseUrl}/api/Students/all`)
      .then(res => res.json())
      .then((data: { students: Student[] }) => setStudents(data.students))
      .catch(err => console.error("Error fetching students:", err));
  }, []);

  // 🔹 Fetch files for selected project
  useEffect(() => {
    if (!selectedProject) {
      setFiles([]);
      return;
    }
    setLoading(true);
    fetch(`${baseUrl}/api/Link/team/${selectedProject}`)
      .then(res => res.json())
      .then((data: ProjectFile[]) => setFiles(data))
      .catch(err => console.error("Error fetching files:", err))
      .finally(() => setLoading(false));
  }, [selectedProject]);

  const getStudentName = (id: number) => {
    const student = students.find(s => s.userId === id);
    return student ? student.name : "Unknown";
  };

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold text-center text-teal-700">📁 Supervisor Project Files</h1>

      {/* Project Dropdown */}
      <select
        value={selectedProject}
        onChange={(e) => setSelectedProject(Number(e.target.value))}
        className="p-3 border border-teal-300 rounded w-full focus:outline-none focus:ring-2 focus:ring-teal-500"
      >
        <option value="">-- Select Project --</option>
        {projects.map(p => (
          <option key={p.id} value={p.teamId}>
            {p.projectName} ({p.teamName})
          </option>
        ))}
      </select>

      {/* Loading state */}
      {loading && <p className="text-center mt-4 text-gray-500">Loading...</p>}

      {/* Files List */}
      {!loading && files.length === 0 && selectedProject && (
        <p className="text-gray-500 text-center mt-4">No files uploaded for this project yet.</p>
      )}
      {!loading && files.length > 0 && (
        <ul className="space-y-3 mt-4">
          {files.map(f => (
            <li
              key={f.id}
              className="p-4 border border-teal-200 rounded-lg flex flex-col sm:flex-row justify-between items-start sm:items-center bg-white shadow hover:shadow-lg transition"
            >
              <div className="flex-1">
                <p className="font-semibold text-teal-700 text-lg">{f.title}</p>
                <p className="text-sm text-gray-400">📅 {f.date}</p>
                <p className="text-sm mt-1 text-gray-600">
                  Uploaded by: {getStudentName(f.studentId)}
                </p>
              </div>
              <div className="flex gap-2 mt-3 sm:mt-0">
                <a
                  href={f.url}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-sm"
                >
                  Download
                </a>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
