/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";

interface Project {
  id: number;
  projectName: string;
  supervisorId: number | null;
  teamId: number | null;
  teamName: string | null;
  teamMembers?: { userId: number; name: string }[];
}

interface Reply {
  id: number;
  content: string;
  date: string;
  studentId?: number;
  studentName?: string;
  supervisorId?: number;
  supervisorName?: string;
  authorName?: string;
}

interface Feedback {
  teamName: string;
  projectName: string;
  feedbackId: number;
  content: string;
  date: string;
  supervisorId: number;
  supervisorName?: string;
  teamId: number;
  replies?: Reply[];
}

export default function SupervisorFeedback() {
  const { user } = useAuth();
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProject, setSelectedProject] = useState<number | "">("");
  const [feedbackList, setFeedbackList] = useState<Feedback[]>([]);
  const [message, setMessage] = useState("");
  const [editFeedbackId, setEditFeedbackId] = useState<number | null>(null);
  const [replyMessage, setReplyMessage] = useState<{ [key: number]: string }>({});
  const [editReplyId, setEditReplyId] = useState<number | null>(null);
  const [editReplyMessage, setEditReplyMessage] = useState<{ [key: number]: string }>({});
  const token = user?.token || "";

  // جلب المشاريع + أعضاء الفريق
  useEffect(() => {
    if (!user) return;
    const fetchProjects = async () => {
      try {
        const res = await fetch(`${baseUrl}/api/Project`, {
          headers: { Authorization: `Bearer ${user.token}` },
        });
        const allProjects: Project[] = await res.json();

        const myProjects = allProjects.filter(p => p.supervisorId === user.userId);

        const projectsWithMembers = await Promise.all(
          myProjects.map(async p => {
            if (!p.teamId) return p;
            try {
              const resMembers = await fetch(`${baseUrl}/api/Students/all`, {
                headers: { Authorization: `Bearer ${user.token}` },
              });
              const studentsData: { students: { userId: number; name: string }[] } = await resMembers.json();
              // eslint-disable-next-line @typescript-eslint/no-unused-vars
              const members = studentsData.students.filter(s => true);
              return { ...p, teamMembers: members };
            } catch {
              return p;
            }
          })
        );

        setProjects(projectsWithMembers);
      } catch (err) {
        console.error(err);
      }
    };
    fetchProjects();
  }, [user]);

  // جلب feedbacks + replies
  useEffect(() => {
    if (!selectedProject) {
      setFeedbackList([]);
      return;
    }
    const fetchFeedbacks = async () => {
      try {
        const res = await fetch(`${baseUrl}/api/Feedback/Team/${selectedProject}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        const data: Feedback[] = await res.json();

        const feedbacksWithReplies = await Promise.all(
          data.map(async f => {
            const project = projects.find(p => p.teamId === f.teamId);

            const replyRes = await fetch(`${baseUrl}/api/Reply/feedback/${f.feedbackId}`, {
              headers: { Authorization: `Bearer ${token}` },
            });
            let replies: Reply[] = await replyRes.json();

            replies = replies.map(r => {
              if (r.supervisorId) return { ...r, authorName: r.supervisorName || user?.name || "Supervisor" };
              if (r.studentId) {
                const member = project?.teamMembers?.find(m => m.userId === r.studentId);
                return { ...r, authorName: r.studentName || member?.name || `Student #${r.studentId}` };
              }
              return { ...r, authorName: "Unknown" };
            });

            return {
              ...f,
              projectName: project?.projectName || "Unknown Project",
              teamName: project?.teamName || "Unknown Team",
              supervisorName: user?.role === "supervisor" ? user.name : f.supervisorName || "Supervisor",
              replies,
            };
          })
        );

        setFeedbackList(feedbacksWithReplies);
      } catch (err) {
        console.error(err);
      }
    };
    fetchFeedbacks();
  }, [selectedProject, token, projects, user]);

  // إرسال / تعديل feedback
  const handleSubmitFeedback = async () => {
    if (!selectedProject || !message.trim()) return;

    try {
      const res = await fetch(
        editFeedbackId ? `${baseUrl}/api/Feedback/${editFeedbackId}` : `${baseUrl}/api/Feedback`,
        {
          method: editFeedbackId ? "PUT" : "POST",
          headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
          body: JSON.stringify({
            content: message,
            supervisorId: user?.userId,
            teamId: selectedProject,
          }),
        }
      );
      const updatedFeedback: Feedback = await res.json();
      setFeedbackList(prev =>
        editFeedbackId ? prev.map(f => (f.feedbackId === editFeedbackId ? updatedFeedback : f)) : [updatedFeedback, ...prev]
      );
      setMessage("");
      setEditFeedbackId(null);
      Swal.fire(editFeedbackId ? "Updated!" : "Sent!", "Feedback saved successfully.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to save feedback.", "error");
    }
  };

  // حذف feedback
  const handleDeleteFeedback = async (fId: number) => {
    const result = await Swal.fire({
      title: "Are you sure?",
      text: "This feedback will be deleted!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    });
    if (!result.isConfirmed) return;

    try {
      await fetch(`${baseUrl}/api/Feedback/${fId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });
      setFeedbackList(prev => prev.filter(f => f.feedbackId !== fId));
      Swal.fire("Deleted!", "Feedback has been deleted.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to delete feedback.", "error");
    }
  };

  // إرسال reply
  const handleReply = async (feedbackId: number) => {
    const content = replyMessage[feedbackId]?.trim();
    if (!content) return;

    try {
      await fetch(`${baseUrl}/api/Reply`, {
        method: "POST",
        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
        body: JSON.stringify({
          content,
          feedbackId,
          supervisorId: user?.role === "supervisor" ? user.userId : undefined,
          studentId: user?.role === "student" ? user.userId : undefined,
          supervisorName: user?.role === "supervisor" ? user.name : undefined,
          studentName: user?.role === "student" ? user.name : undefined,
        }),
      });

      const replyRes = await fetch(`${baseUrl}/api/Reply/feedback/${feedbackId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      let replies: Reply[] = await replyRes.json();

      const project = projects.find(p => p.teamId === selectedProject);
      replies = replies.map(r => {
        if (r.supervisorId) return { ...r, authorName: r.supervisorName || user?.name || "Supervisor" };
        if (r.studentId) {
          const member = project?.teamMembers?.find(m => m.userId === r.studentId);
          return { ...r, authorName: r.studentName || member?.name || `Student #${r.studentId}` };
        }
        return { ...r, authorName: "Unknown" };
      });

      setFeedbackList(prev => prev.map(f => (f.feedbackId === feedbackId ? { ...f, replies } : f)));
      setReplyMessage(prev => ({ ...prev, [feedbackId]: "" }));
    } catch (err) {
      console.error(err);
    }
  };

  // تعديل reply
  const handleEditReply = async (replyId: number, feedbackId: number) => {
    const content = editReplyMessage[replyId]?.trim();
    if (!content) return;

    try {
      await fetch(`${baseUrl}/api/Reply/${replyId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
        body: JSON.stringify({ content }),
      });

      setFeedbackList(prev =>
        prev.map(f =>
          f.feedbackId === feedbackId
            ? {
                ...f,
                replies: f.replies?.map(r => (r.id === replyId ? { ...r, content } : r)),
              }
            : f
        )
      );
      setEditReplyId(null);
      setEditReplyMessage(prev => ({ ...prev, [replyId]: "" }));
      Swal.fire("Updated!", "Reply updated successfully.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to update reply.", "error");
    }
  };

  // حذف reply
  const handleDeleteReply = async (replyId: number, feedbackId: number) => {
    const result = await Swal.fire({
      title: "Are you sure?",
      text: "This reply will be deleted!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    });
    if (!result.isConfirmed) return;

    try {
      await fetch(`${baseUrl}/api/Reply/${replyId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });

      setFeedbackList(prev =>
        prev.map(f =>
          f.feedbackId === feedbackId ? { ...f, replies: f.replies?.filter(r => r.id !== replyId) } : f
        )
      );
      Swal.fire("Deleted!", "Reply has been deleted.", "success");
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to delete reply.", "error");
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold text-teal-700 text-center mb-6">💬 Supervisor Feedback</h1>

      <div className="flex flex-col sm:flex-row gap-4 mb-4">
        <select
          value={selectedProject}
          onChange={e => setSelectedProject(Number(e.target.value))}
          className="flex-1 p-3 border rounded-lg focus:ring-2 focus:ring-teal-700 w-full"
        >
          <option value="">-- Select a Project --</option>
          {projects.map(p => (
            <option key={p.id} value={p.teamId || 0}>
              {p.projectName} {p.teamName ? `(${p.teamName})` : ""}
            </option>
          ))}
        </select>

        <button
          onClick={handleSubmitFeedback}
          className="sm:w-1/4 p-3 bg-teal-700 text-white rounded-xl shadow hover:bg-teal-800 transition"
        >
          {editFeedbackId ? "Update Feedback" : "Send Feedback"}
        </button>
      </div>

      <textarea
        value={message}
        onChange={e => setMessage(e.target.value)}
        className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-700 mb-4 h-32 sm:h-40"
        placeholder="Write your feedback here..."
      />

      <div className="space-y-4">
        {feedbackList.length === 0 && <p className="text-gray-500">No feedback yet.</p>}
        {feedbackList.map(f => (
          <div key={f.feedbackId} className="p-4 border rounded-lg shadow-sm bg-white flex flex-col gap-3">
            {editFeedbackId === f.feedbackId ? (
              <>
                <textarea
                  value={message}
                  onChange={e => setMessage(e.target.value)}
                  className="w-full p-2 border rounded-lg mb-1"
                />
                <div className="flex gap-2 justify-end">
                  <button
                    onClick={handleSubmitFeedback}
                    className="px-2 py-1 bg-green-600 text-white rounded hover:bg-green-700"
                  >
                    Save
                  </button>
                  <button
                    onClick={() => {
                      setEditFeedbackId(null);
                      setMessage("");
                    }}
                    className="px-2 py-1 bg-gray-500 text-white rounded hover:bg-gray-600"
                  >
                    Cancel
                  </button>
                </div>
              </>
            ) : (
              <>
                
                <h1 className="text-sm text-teal-600 font-semibold">📌 Project: {f.projectName} ({f.teamName})</h1>
                <p className="text-gray-800">{f.content}</p>
                <h6 className="text-sm text-gray-500">Supervisor: {f.supervisorName}</h6>
                <h5 className="text-xs text-gray-400">{f.date}</h5>

                {f.supervisorId === user?.userId && (
                  <div className="flex gap-2 mt-1 justify-end">
                    <button
                      onClick={() => {
                        setEditFeedbackId(f.feedbackId);
                        setMessage(f.content);
                      }}
                      className="px-2 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600"
                    >
                      Edit
                    </button>
                    <button
                      onClick={() => handleDeleteFeedback(f.feedbackId)}
                      className="px-2 py-1 bg-red-600 text-white rounded hover:bg-red-700"
                    >
                      Delete
                    </button>
                  </div>
                )}
              </>
            )}

            <div className="ml-4 mt-3 space-y-2">
              {f.replies?.map(r => (
                <div
                  key={r.id}
                  className={`flex flex-col p-4 my-2 rounded-xl max-w-[90%] relative ${
                    r.studentId === user?.userId ? "bg-teal-200 self-end" : "bg-gray-300 self-start"
                  } shadow-md`}
                >
                  {editReplyId === r.id ? (
                    <>
                      <input
                        type="text"
                        value={editReplyMessage[r.id] || r.content}
                        onChange={e =>
                          setEditReplyMessage(prev => ({ ...prev, [r.id]: e.target.value }))
                        }
                        className="p-2 border rounded-lg mb-1 w-full"
                      />
                      <div className="flex gap-2 justify-end">
                        <button
                          onClick={() => handleEditReply(r.id, f.feedbackId)}
                          className="px-2 py-1 bg-green-600 text-white rounded hover:bg-green-700"
                        >
                          Save
                        </button>
                        <button
                          onClick={() => setEditReplyId(null)}
                          className="px-2 py-1 bg-gray-500 text-white rounded hover:bg-gray-600"
                        >
                          Cancel
                        </button>
                      </div>
                    </>
                  ) : (
                    <>
                      <p className="text-gray-800">
                        <span className="font-semibold">{r.authorName}:</span> {r.content}
                      </p>
                      <p className="text-xs text-gray-500 mt-1">{r.date}</p>
                      {(r.studentId === user?.userId || r.supervisorId === user?.userId) && (
                        <div className="flex gap-2 mt-1 justify-end">
                          <button
                            onClick={() => setEditReplyId(r.id)}
                            className="px-2 py-1 bg-green-900 text-white rounded hover:bg-green-600"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDeleteReply(r.id, f.feedbackId)}
                            className="px-2 py-1 bg-red-600 text-white rounded hover:bg-red-700"
                          >
                            Delete
                          </button>
                        </div>
                      )}
                    </>
                  )}
                </div>
              ))}

              <div className="flex gap-2 mt-1">
                <input
                  type="text"
                  placeholder="Write a reply..."
                  value={replyMessage[f.feedbackId] || ""}
                  onChange={e => setReplyMessage(prev => ({ ...prev, [f.feedbackId]: e.target.value }))}
                  className="flex-1 p-2 border rounded-lg focus:ring-1 focus:ring-teal-500"
                />
                <button
                  onClick={() => handleReply(f.feedbackId)}
                  className="px-3 py-1 bg-teal-600 text-white rounded-lg hover:bg-teal-700"
                >
                  Reply
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
