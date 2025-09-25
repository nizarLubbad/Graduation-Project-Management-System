/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import Swal from "sweetalert2";

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
  feedbackId: number;
  content: string;
  date: string;
  supervisorId?: number;
  supervisorName?: string;
  teamId: number;
  projectName?: string;
  teamName?: string;
  replies?: Reply[];
}

interface TeamData {
  teamId: number;
  teamName: string;
  memberStudentIds: number[];
  project: {
    id: number;
    projectTitle: string;
    description: string;
    supervisorId?: number | null;
  };
  supervisorId?: number | null;
  supervisorName?: string | null;
}

interface Student {
  userId: number;
  name: string;
}

export default function StudentFeedback() {
  const { user } = useAuth();
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  const [team, setTeam] = useState<TeamData | null>(null);
  const [teamMembers, setTeamMembers] = useState<Student[]>([]);
  const [feedbackList, setFeedbackList] = useState<Feedback[]>([]);
  const [replyMessage, setReplyMessage] = useState<{ [key: number]: string }>({});
  const [editReplyId, setEditReplyId] = useState<number | null>(null);

  // جلب بيانات الفريق وأعضاء الفريق
  const fetchTeam = async () => {
    if (!user?.team?.teamId) return;

    try {
      const resTeam = await fetch(`${baseUrl}/api/Teams/${user.team.teamId}`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const teamData: TeamData = await resTeam.json();

      // جلب اسم المشرف
      if (teamData.supervisorId) {
        try {
          const resSup = await fetch(`${baseUrl}/api/Auth/${teamData.supervisorId}`, {
            headers: { Authorization: `Bearer ${user.token}` },
          });
          if (resSup.ok) {
            const supData = await resSup.json();
            teamData.supervisorName = supData.name || "Unknown";
          }
        } catch {
          teamData.supervisorName = "Unknown";
        }
      }

      setTeam(teamData);

      // جلب كل الطلاب وتحديد أعضاء الفريق
      const resStudents = await fetch(`${baseUrl}/api/Students/all`, {
        headers: { Authorization: `Bearer ${user.token}` },
      });
      const studentsData: { students: Student[] } = await resStudents.json();
      const members = studentsData.students.filter(s =>
        teamData.memberStudentIds.includes(s.userId)
      );
      setTeamMembers(members);

    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to load team data", "error");
    }
  };

  // جلب feedbacks مع الردود
  const fetchFeedbacks = async () => {
    if (!team) return;

    try {
      const res = await fetch(`${baseUrl}/api/Feedback/Team/${team.teamId}`, {
        headers: { Authorization: `Bearer ${user?.token}` },
      });
      let data: Feedback[] = [];
      if (res.ok) data = await res.json();

      // إذا لا يوجد feedback، عنصر افتراضي
      if (data.length === 0) {
        data = [
          {
            feedbackId: 0,
            content: "No feedback yet.",
            date: "",
            supervisorId: team.project.supervisorId || undefined,
            supervisorName: team.supervisorName || "Supervisor",
            teamId: team.teamId,
            projectName: team.project.projectTitle,
            teamName: team.teamName,
            replies: [],
          },
        ];
      } else {
        // إثراء كل feedback بالردود وبيانات الفريق
        data = await Promise.all(
          data.map(async f => {
            const replyRes = await fetch(`${baseUrl}/api/Reply/feedback/${f.feedbackId}`, {
              headers: { Authorization: `Bearer ${user?.token}` },
            });
            let replies: Reply[] = [];
            if (replyRes.ok) replies = await replyRes.json();

            const repliesWithAuthor = replies.map(r => {
              let authorName = "Unknown";

              if (r.studentId) {
                if (r.studentId === user?.userId) authorName = user?.name || "Unknown";
                else {
                  const member = teamMembers.find(m => m.userId === r.studentId);
                  authorName = r.studentName || member?.name || `Student #${r.studentId}`;
                }
              } else if (r.supervisorId) {
                authorName = team?.supervisorName || "Supervisor";
              }

              return { ...r, authorName };
            });

            return {
              ...f,
              projectName: team.project.projectTitle,
              teamName: team.teamName,
              supervisorName: team.supervisorName || "Supervisor",
              replies: repliesWithAuthor,
            };
          })
        );
      }

      setFeedbackList(data);
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to fetch feedbacks", "error");
    }
  };

  useEffect(() => {
    fetchTeam();
  }, [user]);

  useEffect(() => {
    fetchFeedbacks();
  }, [team, teamMembers]);

  // إضافة أو تعديل رد
  const handleReply = async (feedbackId: number, replyId?: number) => {
    const content = replyId ? replyMessage[replyId]?.trim() : replyMessage[feedbackId]?.trim();
    if (!content || feedbackId === 0) return;

    try {
      if (replyId) {
        await fetch(`${baseUrl}/api/Reply/${replyId}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json", Authorization: `Bearer ${user?.token}` },
          body: JSON.stringify({ content }),
        });
        setEditReplyId(null);
      } else {
        await fetch(`${baseUrl}/api/Reply`, {
          method: "POST",
          headers: { "Content-Type": "application/json", Authorization: `Bearer ${user?.token}` },
          body: JSON.stringify({
            content,
            feedbackId,
            studentId: user?.userId,
            studentName: user?.name,
          }),
        });
      }
      fetchFeedbacks();
      setReplyMessage(prev => ({ ...prev, [replyId || feedbackId]: "" }));
    } catch (err) {
      console.error(err);
      Swal.fire("Error", "Failed to save reply.", "error");
    }
  };

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const handleDeleteReply = async (replyId: number) => {
    Swal.fire({
      title: "Are you sure?",
      text: "This action cannot be undone!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(async result => {
      if (!result.isConfirmed) return;
      await fetch(`${baseUrl}/api/Reply/${replyId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${user?.token}` },
      });
      fetchFeedbacks();
      Swal.fire("Deleted!", "Reply has been deleted.", "success");
    });
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-6">
      <div className="w-full max-w-3xl text-center space-y-6">
        <h1 className="text-5xl font-extrabold text-gray-800 mb-6">💬Feedback</h1>

        {feedbackList.map(f => (
          <div key={f.feedbackId} className="p-4 border rounded-lg shadow-sm bg-white flex flex-col gap-3">
         
            <h1 className="text-sm text-teal-600 font-semibold">   
              📌 Project: {f.projectName} ({f.teamName})
            </h1>
            <p className="text-gray-800">{f.content}</p>
            <h3 className="text-sm text-gray-500">Supervisor: {f.supervisorName}</h3>
            <h5 className="text-xs text-gray-400">{f.date}</h5>

            {f.replies?.map(r => (
              <div
                key={r.id}
                className={`p-3 rounded-lg max-w-[80%] ${
                  r.studentId === user?.userId ? "bg-teal-100 self-end" : "bg-gray-200 self-start"
                } relative`}
              >
                {editReplyId === r.id ? (
                  <div className="flex gap-2">
                    <input
                      type="text"
                      value={replyMessage[r.id] || ""}
                      onChange={e => setReplyMessage(prev => ({ ...prev, [r.id]: e.target.value }))}
                      className="flex-1 p-2 border rounded-lg focus:ring-1 focus:ring-blue-500"
                    />
                    <button onClick={() => handleReply(f.feedbackId, r.id)} className="px-2 py-1 bg-blue-600 text-white rounded-lg">
                      Save
                    </button>
                  </div>
                ) : (
                  <p className="text-gray-800">
                    <span className="font-semibold">{r.authorName}:</span> {r.content}
                  </p>
                )}
                <p className="text-xs text-gray-400">{r.date}</p>

                {(user?.userId === r.studentId || user?.userId === team?.supervisorId) && editReplyId !== r.id && (
                  <div className="flex gap-1 absolute top-1 right-1">
                    <button
                      onClick={() => {
                        setEditReplyId(r.id);
                        setReplyMessage(prev => ({ ...prev, [r.id]: r.content }));
                      }}
                      className="text-xs text-blue-600"
                    >
                      Edit
                    </button>
                    <button onClick={() => handleDeleteReply(r.id)} className="text-xs text-red-600">
                      Delete
                    </button>
                  </div>
                )}
              </div>
            ))}

            {f.feedbackId !== 0 && (
              <div className="flex gap-2 mt-2">
                <input
                  type="text"
                  placeholder="Write a reply..."
                  value={replyMessage[f.feedbackId] || ""}
                  onChange={e => setReplyMessage(prev => ({ ...prev, [f.feedbackId]: e.target.value }))}
                  className="flex-1 p-2 border rounded-lg focus:ring-1 focus:ring-teal-500"
                />
                <button onClick={() => handleReply(f.feedbackId)} className="px-3 py-1 bg-teal-600 text-white rounded-lg hover:bg-teal-700">
                  Reply
                </button>
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
