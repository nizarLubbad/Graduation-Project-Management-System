// StudentProjectFiles.tsx
import { useEffect, useState } from "react";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { ProjectFile } from "../types/types";

interface Student {
  userId: number;
  name: string;
}

export default function StudentProjectFiles() {
  const { user } = useAuth();
  const [files, setFiles] = useState<ProjectFile[]>([]);
  const [fileName, setFileName] = useState("");
  const [fileUrl, setFileUrl] = useState("");
  const [loading, setLoading] = useState(false);
  const [teamMembers, setTeamMembers] = useState<Student[]>([]);

  const baseUrl = import.meta.env.VITE_API_URL;

  // =========================
  // 🔹 Fetch all team members
  // =========================
  const fetchTeamMembers = async () => {
    if (!user?.team?.teamId) return;
    try {
      const res = await fetch(`${baseUrl}/api/Students/all`);
      const data: { students: Student[] } = await res.json();
      const members = data.students.filter(s => user.team?.memberStudentIds?.includes(s.userId));
      setTeamMembers(members);
    } catch (err) {
      console.error(err);
    }
  };

  // =========================
  // 🔹 Fetch all team files
  // =========================
  const fetchFiles = async () => {
    if (!user?.team?.teamId) return;
    setLoading(true);
    try {
      const res = await fetch(`${baseUrl}/api/Link/team/${user.team.teamId}`);
      const data: ProjectFile[] = await res.json();
      setFiles(data);
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to load project files", "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTeamMembers();
    fetchFiles();
  }, [user?.team?.teamId]);

  const getStudentName = (id: number) => {
    const student = teamMembers.find(s => s.userId === id);
    return student ? student.name : "Unknown";
  };

  // =========================
  // 🔹 Upload new file
  // =========================
  const handleUpload = async () => {
    if (!user) return Swal.fire("Error", "User is not logged in!", "error");
    if (!fileName.trim() || !fileUrl.trim() || !user.team?.teamId) {
      return Swal.fire("Warning", "Please fill in both File Name and File URL!", "warning");
    }

    const newFile = {
      title: fileName,
      url: fileUrl,
      studentId: user.userId,
      teamId: user.team.teamId,
    };

    setLoading(true);
    try {
      const res = await fetch(`${baseUrl}/api/Link`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newFile),
      });
      if (!res.ok) throw new Error("Upload failed");
      const savedFile: ProjectFile = await res.json();
      setFiles(prev => [...prev, savedFile]);
      setFileName("");
      setFileUrl("");
      Swal.fire("Uploaded!", "File has been uploaded successfully.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error!", err instanceof Error ? err.message : "Failed to upload the file.", "error");
    } finally {
      setLoading(false);
    }
  };

  // =========================
  // 🔹 Delete file
  // =========================
  const handleDelete = async (id: number) => {
    const file = files.find(f => f.id === id);
    if (!file || file.studentId !== user?.userId) {
      Swal.fire("Error", "You can only delete your own files", "error");
      return;
    }

    const result = await Swal.fire({
      title: "Are you sure?",
      text: "This action cannot be undone!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    });

    if (!result.isConfirmed) return;

    setLoading(true);
    try {
      const res = await fetch(`${baseUrl}/api/Link/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error("Delete failed");
      setFiles(prev => prev.filter(f => f.id !== id));
      Swal.fire("Deleted!", "File has been deleted.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error!", "Failed to delete the file.", "error");
    } finally {
      setLoading(false);
    }
  };

  // =========================
  // 🔹 Edit file
  // =========================
  const handleEdit = async (file: ProjectFile) => {
    if (!user || file.studentId !== user.userId) {
      Swal.fire("Error", "You can only edit your own files", "error");
      return;
    }

    const { value: formValues } = await Swal.fire({
      title: "Edit File",
      html:
        `<input id="swal-file-title" class="swal2-input" placeholder="Title" value="${file.title}">` +
        `<input id="swal-file-url" class="swal2-input" placeholder="URL" value="${file.url}">`,
      focusConfirm: false,
      preConfirm: () => {
        const title = (document.getElementById("swal-file-title") as HTMLInputElement).value;
        const url = (document.getElementById("swal-file-url") as HTMLInputElement).value;
        if (!title || !url) Swal.showValidationMessage("Please fill both fields");
        return { title, url };
      },
    });

    if (!formValues) return;

    setLoading(true);
    try {
      const res = await fetch(`${baseUrl}/api/Link/${file.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ ...file, title: formValues.title, url: formValues.url }),
      });
      if (!res.ok) throw new Error("Update failed");
      const updatedFile: ProjectFile = await res.json();
      setFiles(prev => prev.map(f => f.id === file.id ? updatedFile : f));
      Swal.fire("Updated!", "File has been updated.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error!", "Failed to update the file.", "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold text-center text-teal-700">📁 Student Project Files</h1>

      {/* Upload Section */}
      <div className="flex flex-col sm:flex-row gap-3 items-center">
        <input
          type="text"
          placeholder="File Name"
          value={fileName}
          onChange={(e) => setFileName(e.target.value)}
          className="flex-1 p-3 border border-teal-300 rounded-md focus:outline-none focus:ring-2 focus:ring-teal-500"
        />
        <input
          type="text"
          placeholder="File URL"
          value={fileUrl}
          onChange={(e) => setFileUrl(e.target.value)}
          className="flex-1 p-3 border border-teal-300 rounded-md focus:outline-none focus:ring-2 focus:ring-teal-500"
        />
        <button
          onClick={handleUpload}
          disabled={loading}
          className="px-5 py-3 bg-teal-600 text-white font-semibold rounded-lg hover:bg-teal-700 transition disabled:opacity-50"
        >
          {loading ? "Uploading..." : "Upload"}
        </button>
      </div>

      {/* Files List */}
      {!loading && files.length === 0 ? (
        <p className="text-gray-500 text-center mt-4">No files uploaded yet.</p>
      ) : (
        <ul className="space-y-3 mt-4">
          {files.map((f) => (
            <li key={f.id} className="p-4 border border-teal-200 rounded-lg flex flex-col sm:flex-row justify-between items-start sm:items-center bg-white shadow hover:shadow-lg transition">
              <div className="flex-1">
                <p className="font-semibold text-teal-700 text-lg">{f.title}</p>
                <p className="text-sm text-gray-400">📅 {f.date}</p>
                <p className="text-sm text-gray-500">👤 Uploaded by: {getStudentName(f.studentId)}</p>
              </div>
              <div className="flex gap-2 mt-3 sm:mt-0">
                <a href={f.url} target="_blank" rel="noopener noreferrer" className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-sm">Download</a>
                {f.studentId === user?.userId && (
                  <>
                    <button onClick={() => handleEdit(f)} className="px-4 py-2 bg-yellow-500 text-white rounded-lg hover:bg-yellow-600 transition text-sm">Edit</button>
                    <button onClick={() => handleDelete(f.id)} disabled={loading} className="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition text-sm disabled:opacity-50">Delete</button>
                  </>
                )}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
