import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { Assignment, User, Feedback, Reply } from "../types/types";

// ============================
// SupervisorFeedback Component
// إدارة الـ Feedback للمشرفين: إنشاء، تعديل، حذف، إضافة ردود
// ============================
export default function SupervisorFeedback() {
  // ---------- State ----------
  const [assignments, setAssignments] = useState<Assignment[]>([]); // المشاريع تحت إشراف المشرف
  const [selectedProject, setSelectedProject] = useState<string>(""); // المشروع المحدد لإرسال feedback
  const [message, setMessage] = useState(""); // نص feedback الحالي
  const [feedbackList, setFeedbackList] = useState<Feedback[]>([]); // قائمة الـ feedbacks
  const [editId, setEditId] = useState<string | null>(null); // لتعديل feedback موجود
  const [replyText, setReplyText] = useState<{ [key: string]: string }>({}); // نص الرد لكل feedback
  const [editReplyId, setEditReplyId] = useState<string | null>(null); // لتعديل reply موجود

  // ---------- Load Data on Mount ----------
  useEffect(() => {
    const user: User | null = JSON.parse(localStorage.getItem("currentUser") || "null");
    if (!user) return;

    // جلب المشاريع الخاصة بالمشرف الحالي
    const allAssignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");
    setAssignments(allAssignments.filter(a => a.supervisorName === user.name));

    // جلب feedbacks الخاصة بالمشرف الحالي
    const savedFeedback: Feedback[] = JSON.parse(localStorage.getItem("feedback") || "[]");
    const filtered = savedFeedback.map(f => ({
      ...f,
      projectName: f.projectName || assignments.find(a => a.teamId === f.projectId)?.projectTitle || "Unknown Project",
    }));
    setFeedbackList(filtered.filter(f => f.supervisorId === user.id));
  }, []);

  // ---------- Save Feedbacks to LocalStorage ----------
  const saveFeedback = (updated: Feedback[]) => {
    setFeedbackList(updated);
    localStorage.setItem("feedback", JSON.stringify(updated));
  };

  // ---------- Submit Feedback ----------
  const handleSubmit = () => {
    if (!selectedProject || !message.trim()) {
      Swal.fire("Error", "Please select a project and write feedback.", "error");
      return;
    }

    const user: User = JSON.parse(localStorage.getItem("currentUser") || "{}");
    const project = assignments.find(a => a.teamId === selectedProject);

    if (editId) {
      // تعديل feedback موجود
      const updated = feedbackList.map(f =>
        f.id === editId ? { ...f, message, date: new Date().toLocaleString() } : f
      );
      saveFeedback(updated);
      setEditId(null);
      Swal.fire("Updated!", "Feedback has been updated.", "success");
    } else {
      // إضافة feedback جديد
      const feedback: Feedback = {
        id: Date.now().toString(),
        projectId: selectedProject,
        projectName: project?.projectTitle || project?.teamName || "Unknown Project",
        supervisorId: user.id,
        supervisorName: user.name,
        studentId: project?.members[0] || "",
        message,
        date: new Date().toLocaleString(),
        replies: [],
      };
      saveFeedback([...feedbackList, feedback]);
      Swal.fire("Sent!", "Feedback has been sent successfully.", "success");
    }

    setMessage("");
    setSelectedProject("");
  };

  // ---------- Reply Functions ----------
  const handleReply = (feedbackId: string) => {
    const user: User = JSON.parse(localStorage.getItem("currentUser") || "{}");
    if (!replyText[feedbackId]?.trim()) return;

    const updated = feedbackList.map(f => {
      if (f.id === feedbackId) {
        const replies = f.replies || [];
        if (editReplyId) {
          // تعديل reply موجود
          return {
            ...f,
            replies: replies.map(r =>
              r.id === editReplyId ? { ...r, message: replyText[feedbackId], date: new Date().toLocaleString() } : r
            ),
          };
        } else {
          // إضافة reply جديد
          const newReply: Reply = {
            id: Date.now().toString(),
            authorId: user.id,
            authorName: user.name,
            authorRole: "supervisor",
            message: replyText[feedbackId],
            date: new Date().toLocaleString(),
          };
          return { ...f, replies: [...replies, newReply] };
        }
      }
      return f;
    });

    saveFeedback(updated);
    setReplyText(prev => ({ ...prev, [feedbackId]: "" }));
    setEditReplyId(null);
  };

  const handleEditReply = (feedbackId: string, replyId: string, msg: string) => {
    setEditReplyId(replyId);
    setReplyText(prev => ({ ...prev, [feedbackId]: msg }));
  };

  const handleDeleteReply = (feedbackId: string, replyId: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You can't undo this action!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(result => {
      if (result.isConfirmed) {
        const updated = feedbackList.map(f =>
          f.id === feedbackId ? { ...f, replies: f.replies?.filter(r => r.id !== replyId) } : f
        );
        saveFeedback(updated);
        Swal.fire("Deleted!", "Reply has been deleted.", "success");
      }
    });
  };

  // ---------- Edit / Delete Feedback ----------
  const handleEdit = (id: string, msg: string, projectId: string) => {
    setEditId(id);
    setMessage(msg);
    setSelectedProject(projectId);
  };

  const handleDelete = (id: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(result => {
      if (result.isConfirmed) {
        saveFeedback(feedbackList.filter(f => f.id !== id));
        Swal.fire("Deleted!", "Your feedback has been deleted.", "success");
      }
    });
  };

  // ---------- Render ----------
  return (
    <div className="p-4 sm:p-6 lg:p-8 max-w-4xl mx-auto">
      <h1 className="text-3xl font-bold text-teal-700 text-center mb-6">💬 Supervisor Feedback</h1>

      {/* Select Project + Send Button */}
      <div className="flex flex-col sm:flex-row gap-4 mb-4">
        <select
          value={selectedProject}
          onChange={e => setSelectedProject(e.target.value)}
          className="flex-1 p-3 border rounded-lg focus:ring-2 focus:ring-teal-700 w-full"
        >
          <option value="">-- Select a Project --</option>
          {assignments.map(a => (
            <option key={a.teamId} value={a.teamId}>
              {a.projectTitle} ({a.teamName})
            </option>
          ))}
        </select>

        <button
          onClick={handleSubmit}
          className="sm:w-1/4 p-3 bg-teal-700 text-white rounded-xl shadow hover:bg-teal-800 transition"
        >
          {editId ? "Update Feedback" : "Send Feedback"}
        </button>
      </div>

      {/* Feedback Textarea */}
      <textarea
        value={message}
        onChange={e => setMessage(e.target.value)}
        className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-700 mb-4 h-32 sm:h-40"
        placeholder="Write your feedback here..."
      />

      {/* Feedback List */}
      <div className="space-y-4">
        {feedbackList.length === 0 && <p className="text-gray-500">No feedback yet.</p>}
        {feedbackList.map(f => (
          <div key={f.id} className="p-4 border rounded-lg shadow-sm bg-white flex flex-col gap-3">
            {/* Feedback Message */}
            <div>
              <p className="text-gray-800 font-medium">{f.message}</p>
              <p className="text-sm text-teal-600 font-semibold">📌 Project: {f.projectName}</p>
              <p className="text-sm text-gray-500">Supervisor: {f.supervisorName}</p>
              <p className="text-xs text-gray-400">{f.date}</p>
            </div>

            {/* Edit / Delete Feedback */}
            <div className="flex flex-wrap gap-2">
              <button
                onClick={() => handleEdit(f.id, f.message, f.projectId)}
                className="bg-blue-600 text-white px-3 py-1 rounded-lg hover:bg-blue-700"
              >
                Edit
              </button>
              <button
                onClick={() => handleDelete(f.id)}
                className="bg-red-500 text-white px-3 py-1 rounded-lg hover:bg-red-600"
              >
                Delete
              </button>
            </div>

            {/* Replies */}
            <div className="space-y-2">
              {f.replies?.map(r => (
                <div key={r.id} className="p-2 bg-gray-100 border-l-4 border-teal-600 rounded">
                  <p className="text-gray-800">
                    <span className="font-semibold">
                      {r.authorRole === "student" ? "👨‍🎓 Student" : "👨‍🏫 Supervisor"} {r.authorName}:
                    </span>{" "}
                    {r.message}
                  </p>
                  <p className="text-xs text-gray-400">📅 {r.date}</p>
                  {r.authorId === JSON.parse(localStorage.getItem("currentUser") || "{}").id && (
                    <div className="flex gap-2 mt-1 flex-wrap">
                      <button
                        onClick={() => handleEditReply(f.id, r.id, r.message)}
                        className="text-blue-600 text-xs"
                      >
                        Edit
                      </button>
                      <button
                        onClick={() => handleDeleteReply(f.id, r.id)}
                        className="text-red-600 text-xs"
                      >
                        Delete
                      </button>
                    </div>
                  )}
                </div>
              ))}
            </div>

            {/* Reply Box */}
            <div>
              <textarea
                value={replyText[f.id] || ""}
                onChange={e => setReplyText(prev => ({ ...prev, [f.id]: e.target.value }))}
                className="w-full p-2 border rounded-lg"
                placeholder="Write a reply..."
              />
              <button
                onClick={() => handleReply(f.id)}
                className="mt-2 px-4 py-2 bg-teal-700 text-white rounded-lg"
              >
                {editReplyId ? "Update Reply" : "Send Reply"}
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
