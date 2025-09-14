import { useEffect, useState } from "react";
import Swal from "sweetalert2";
import { User, ProjectFile } from "../types/types";

export default function StudentProjectFiles() {
  // =========================
  // 🔹 State Management
  // =========================
  const [files, setFiles] = useState<ProjectFile[]>([]);
  const [fileUrl, setFileUrl] = useState<string>("");
  const [fileName, setFileName] = useState<string>("");

  const user: User = JSON.parse(localStorage.getItem("currentUser") || "{}");

  // =========================
  // 🔹 Fetch Files from LocalStorage (per team)
  // =========================
  useEffect(() => {
    if (!user?.team?.id) return;

    const savedFiles: ProjectFile[] = JSON.parse(localStorage.getItem("projectFiles") || "[]");
    const filtered: ProjectFile[] = savedFiles.filter(
      (f: ProjectFile) => f.projectId === user.team?.id
    );
    setFiles(filtered);
  }, [user.team?.id]);

  // =========================
  // 🔹 Save Files Helper
  // =========================
  const saveFiles = (updated: ProjectFile[]) => {
    const allFiles: ProjectFile[] = JSON.parse(localStorage.getItem("projectFiles") || "[]")
      .filter((f: ProjectFile) => f.projectId !== user.team?.id);
    localStorage.setItem("projectFiles", JSON.stringify([...allFiles, ...updated]));
    setFiles(updated);
  };

  // =========================
  // 🔹 Upload File
  // =========================
  const handleUpload = () => {
    if (!fileName.trim() || !fileUrl.trim()) return;

    const newFile: ProjectFile = {
      id: Date.now().toString(),
      projectId: user.team?.id || "",
      projectName: user.team?.name || "Unknown Project",
      uploaderId: user.id,
      uploaderName: user.name,
      uploaderRole: user.role,
      fileName,
      fileUrl,
      date: new Date().toLocaleString(),
    };

    saveFiles([...files, newFile]);
    setFileName("");
    setFileUrl("");
    Swal.fire("Uploaded!", "File has been uploaded.", "success");
  };

  // =========================
  // 🔹 Delete File
  // =========================
  const handleDelete = (id: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You cannot undo this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(result => {
      if (result.isConfirmed) {
        const updated: ProjectFile[] = files.filter((f: ProjectFile) => f.id !== id);
        saveFiles(updated);
        Swal.fire("Deleted!", "File has been deleted.", "success");
      }
    });
  };

  // =========================
  // 🔹 UI Rendering
  // =========================
  return (
    <div className="p-4 sm:p-6 md:p-8 max-w-4xl mx-auto space-y-6">
      {/* Title */}
      <h1 className="text-3xl sm:text-4xl font-bold text-center text-teal-700">
        📁 Project Files
      </h1>

      {/* Upload Section */}
      <div className="flex flex-col sm:flex-row gap-3 items-center">
        <input
          type="text"
          placeholder="File Name"
          value={fileName}
          onChange={e => setFileName(e.target.value)}
          className="flex-1 p-3 border border-teal-300 rounded-md focus:outline-none focus:ring-2 focus:ring-teal-500"
        />
        <input
          type="text"
          placeholder="File URL"
          value={fileUrl}
          onChange={e => setFileUrl(e.target.value)}
          className="flex-1 p-3 border border-teal-300 rounded-md focus:outline-none focus:ring-2 focus:ring-teal-500"
        />
        <button
          onClick={handleUpload}
          className="px-5 py-3 bg-teal-600 text-white font-semibold rounded-lg shadow hover:bg-teal-700 transition"
        >
          Upload
        </button>
      </div>

      {/* Files List */}
      {files.length === 0 ? (
        <p className="text-gray-500 text-center mt-4">No files uploaded yet.</p>
      ) : (
        <ul className="space-y-3">
          {files.map((f: ProjectFile) => (
            <li
              key={f.id}
              className="p-4 sm:p-5 border border-teal-200 rounded-lg flex flex-col sm:flex-row sm:justify-between items-start sm:items-center bg-white shadow hover:shadow-lg transition"
            >
              {/* File Details */}
              <div className="flex-1 w-full sm:mr-4">
                <p className="font-semibold text-teal-700 text-lg sm:text-xl">{f.fileName}</p>
                <p className="text-xs sm:text-sm text-gray-500">
                  Uploaded by: <span className="font-medium">{f.uploaderName}</span> ({f.uploaderRole})
                </p>
                <p className="text-xs sm:text-sm text-gray-400">📅 {f.date}</p>
              </div>

              {/* Actions */}
              <div className="flex gap-2 mt-3 sm:mt-0">
                <a
                  href={f.fileUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-sm sm:text-base"
                >
                  Download
                </a>
                {f.uploaderId === user.id && (
                  <button
                    onClick={() => handleDelete(f.id)}
                    className="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition text-sm sm:text-base"
                  >
                    Delete
                  </button>
                )}
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
