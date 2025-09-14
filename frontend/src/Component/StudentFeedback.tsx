import { useEffect, useState } from "react";
import { Feedback, User, Reply } from "../types/types";
import Swal from "sweetalert2";

export default function StudentFeedback() {
  // ==========================================
  // الحالة (State)
  // ==========================================
  const [feedbacks, setFeedbacks] = useState<Feedback[]>([]); // قائمة الفيدباك للفريق
  const [replyText, setReplyText] = useState<{ [key: string]: string }>({}); // نص الرد لكل فيدباك
  const [editReplyId, setEditReplyId] = useState<string | null>(null); // لتحديد إذا كنا نعدل رد موجود

  // ==========================================
  //  جلب الفيدباك للفريق الحالي
  // ==========================================
  useEffect(() => {
    const user: User | null = JSON.parse(localStorage.getItem("currentUser") || "null");
    if (!user || !user.team) return;

    const allFeedbacks: Feedback[] = JSON.parse(localStorage.getItem("feedback") || "[]");
    const filtered = allFeedbacks
      .filter(f => f.projectId === user.team?.id)
      .map(f => ({
        ...f,
        projectName: f.projectName || user.team?.name || "Unknown Project",
      }));
    setFeedbacks(filtered);
  }, []);

  // ==========================================
  //  حفظ الفيدباك بعد أي تعديل
  // ==========================================
  const saveFeedbacks = (updated: Feedback[]) => {
    setFeedbacks(updated);
    localStorage.setItem("feedback", JSON.stringify(updated));
  };

  // ==========================================
  // إضافة رد جديد أو تعديل رد موجود
  // ==========================================
  const handleReply = (feedbackId: string) => {
    const user: User = JSON.parse(localStorage.getItem("currentUser") || "{}");
    if (!replyText[feedbackId]?.trim()) return;

    const updated = feedbacks.map(f => {
      if (f.id === feedbackId) {
        const replies = f.replies || [];
        if (editReplyId) {
          // تعديل رد موجود
          return {
            ...f,
            replies: replies.map(r =>
              r.id === editReplyId
                ? { ...r, message: replyText[feedbackId], date: new Date().toLocaleString() }
                : r
            ),
          };
        } else {
          // إضافة رد جديد
          const newReply: Reply = {
            id: Date.now().toString(),
            authorId: user.id,
            authorName: user.name,
            authorRole: "student",
            message: replyText[feedbackId],
            date: new Date().toLocaleString(),
          };
          return { ...f, replies: [...replies, newReply] };
        }
      }
      return f;
    });

    saveFeedbacks(updated);
    setReplyText(prev => ({ ...prev, [feedbackId]: "" }));
    setEditReplyId(null);
  };

  // ==========================================
  //  تعديل نص الرد عند الضغط على Edit
  // ==========================================
  const handleEditReply = (feedbackId: string, replyId: string, msg: string) => {
    setEditReplyId(replyId);
    setReplyText(prev => ({ ...prev, [feedbackId]: msg }));
  };

  // ==========================================
  //  حذف رد مع تأكيد عبر SweetAlert2
  // ==========================================
  const handleDeleteReply = (feedbackId: string, replyId: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You can't undo this action!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(result => {
      if (result.isConfirmed) {
        const updated = feedbacks.map(f =>
          f.id === feedbackId ? { ...f, replies: f.replies?.filter(r => r.id !== replyId) } : f
        );
        saveFeedbacks(updated);
        Swal.fire("Deleted!", "Reply has been deleted.", "success");
      }
    });
  };

  // ==========================================
  //  حالة عدم وجود فيدباك
  // ==========================================
  if (feedbacks.length === 0) {
    return <p className="p-6 text-gray-600 text-center">🚫 No feedback yet.</p>;
  }

  // ==========================================
  //  واجهة المستخدم (UI)
  // ==========================================
  return (
    <div className="p-4 sm:p-6 lg:p-8 max-w-4xl mx-auto space-y-6">
      <h1 className="text-2xl sm:text-3xl font-bold text-center">📩 Received Feedback</h1>

      {feedbacks.map(f => (
        <div key={f.id} className="p-4 bg-white rounded-lg shadow flex flex-col gap-3">
          {/* الرسالة الرئيسية */}
          <div>
            <p className="font-semibold text-gray-800">{f.message}</p>
            <p className="text-sm text-teal-600 font-semibold">📌 Project: {f.projectName}</p>
            <p className="text-sm text-gray-500">👨‍🏫 Supervisor: {f.supervisorName}</p>
            <p className="text-xs text-gray-400">📅 {f.date}</p>
          </div>

          {/* الردود */}
          <div className="space-y-2">
            {f.replies?.map(r => (
              <div key={r.id} className="p-2 bg-gray-100 border-l-4 border-teal-600 rounded flex flex-col gap-1">
                <p className="text-gray-800">
                  <span className="font-semibold">
                    {r.authorRole === "student" ? "👨‍🎓 Student" : "👨‍🏫 Supervisor"} {r.authorName}:
                  </span>{" "}
                  {r.message}
                </p>
                <p className="text-xs text-gray-400">📅 {r.date}</p>

                {/* أزرار التعديل والحذف للرد الخاص بالطالب الحالي */}
                {r.authorId === JSON.parse(localStorage.getItem("currentUser") || "{}").id && (
                  <div className="flex gap-2 flex-wrap text-xs">
                    <button
                      onClick={() => handleEditReply(f.id, r.id, r.message)}
                      className="text-blue-600"
                    >
                      Edit
                    </button>
                    <button
                      onClick={() => handleDeleteReply(f.id, r.id)}
                      className="text-red-600"
                    >
                      Delete
                    </button>
                  </div>
                )}
              </div>
            ))}
          </div>

          {/* صندوق كتابة الرد */}
          <div className="flex flex-col sm:flex-row gap-2">
            <textarea
              value={replyText[f.id] || ""}
              onChange={e => setReplyText(prev => ({ ...prev, [f.id]: e.target.value }))}
              className="flex-1 p-2 border rounded-lg w-full sm:w-auto"
              placeholder="Write a reply..."
            />
            <button
              onClick={() => handleReply(f.id)}
              className="px-4 py-2 bg-teal-700 text-white rounded-lg sm:self-end"
            >
              {editReplyId ? "Update Reply" : "Send Reply"}
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}
