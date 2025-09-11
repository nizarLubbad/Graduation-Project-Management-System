import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User, Team } from "../types/types";

export default function CreateTeam() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const [students, setStudents] = useState<User[]>([]);
  const [selectedMembers, setSelectedMembers] = useState<string[]>([]);
  const [teamName, setTeamName] = useState("");

  useEffect(() => {
    if (!user || !user.studentId) return;
    if (user.status) navigate("/dashboard/student");

    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const availableStudents = storedUsers.filter(
      (u) => u.role === "student" && !u.status && u.studentId !== user.studentId
    );
    setStudents(availableStudents);
  }, [user, navigate]);

  const toggleMember = (studentId: string) => {
    setSelectedMembers((prev) =>
      prev.includes(studentId) ? prev.filter((id) => id !== studentId) : [...prev, studentId]
    );
  };

  const handleConfirm = () => {
    if (!teamName || !user || !user.studentId) return;

    const newTeam: Team = {
      teamId: user.studentId,
      leaderId: user.studentId,
      teamName,
      members: [user.studentId, ...selectedMembers],
    };

    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const updatedUsers = storedUsers.map(u => {
      if (u.studentId && newTeam.members.includes(u.studentId)) {
        return { ...u, status: true, team: { id: newTeam.teamId, name: newTeam.teamName } };
      }
      return u;
    });
    localStorage.setItem("users", JSON.stringify(updatedUsers));

    const storedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
    localStorage.setItem("teams", JSON.stringify([...storedTeams, newTeam]));

    login({ ...user, status: true, team: { id: newTeam.teamId, name: newTeam.teamName } });

    Swal.fire({
      icon: "success",
      title: "Team Created!",
      text: `You have created your team: ${newTeam.teamName}`,
      confirmButtonText: "Choose a Supervisor"
    }).then(() => {
      navigate("/booking-supervisor");
    });
  };

  return (
    <div className="p-4 sm:p-6 lg:p-8 max-w-3xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-xl sm:text-2xl font-bold text-teal-700 mb-4 text-center">Create Team</h2>

      {/* إدخال اسم الفريق */}
      <input
        type="text"
        placeholder="Team Name"
        value={teamName}
        onChange={(e) => setTeamName(e.target.value)}
        className="w-full p-2 border rounded mb-4 text-sm sm:text-base"
      />

      {/* الأعضاء المحددين */}
      <h3 className="text-base sm:text-lg font-semibold mb-2">Selected Members ({selectedMembers.length})</h3>
      <div className="flex flex-wrap gap-2 mb-4">
        {selectedMembers.map((id) => {
          const student = students.find((s) => s.studentId === id);
          if (!student) return null;
          return (
            <span
              key={id}
              className="px-2 py-1 bg-teal-200 text-teal-800 rounded-full text-xs sm:text-sm"
            >
              {student.name}
            </span>
          );
        })}
      </div>

      {/* قائمة الطلاب */}
      <div className="mb-6 grid grid-cols-1 sm:grid-cols-2 gap-3">
        {students.map((s) => (
          <label
            key={s.id}
            className="flex items-center space-x-2 p-2 border rounded hover:bg-gray-50 cursor-pointer"
          >
            <input
              type="checkbox"
              checked={selectedMembers.includes(s.studentId!)}
              onChange={() => s.studentId && toggleMember(s.studentId)}
              className="h-4 w-4"
            />
            <span className="text-sm sm:text-base">{s.name} ({s.studentId})</span>
          </label>
        ))}
      </div>

      {/* زر التأكيد */}
      <button
        onClick={handleConfirm}
        className="bg-teal-600 hover:bg-teal-700 transition text-white px-4 py-2 rounded w-full text-sm sm:text-base"
      >
        Confirm
      </button>
    </div>
  );
}
