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

    // Create the team
    const newTeam: Team = {
      teamId: user.studentId,
      leaderId: user.studentId,
      teamName,
      members: [user.studentId, ...selectedMembers],
    };

    // Update students
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const updatedUsers = storedUsers.map(u => {
      if (u.studentId && newTeam.members.includes(u.studentId)) {
        return { ...u, status: true, team: { id: newTeam.teamId, name: newTeam.teamName } };
      }
      return u;
    });
    localStorage.setItem("users", JSON.stringify(updatedUsers));

    // Save the team
    const storedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
    localStorage.setItem("teams", JSON.stringify([...storedTeams, newTeam]));

    // Update current user context
    login({ ...user, status: true, team: { id: newTeam.teamId, name: newTeam.teamName } });

    // Show alert and navigate to booking supervisor
    Swal.fire({
      icon: "success",
      title: "Team Created!",
      text: `You have created your team: ${newTeam.teamName}`,
      confirmButtonText: "Choose a Supervisor"
    }).then(() => {
      navigate("/booking-supervisor"); // Direct the student to choose a supervisor
    });
  };

  return (
    <div className="p-6 max-w-2xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-2xl font-bold text-teal-700 mb-4">Create Team</h2>
      <input type="text" placeholder="Team Name" value={teamName} onChange={(e) => setTeamName(e.target.value)} className="w-full p-2 border rounded mb-4" />
      <h3 className="text-lg font-semibold mb-2">Select Team Members ({selectedMembers.length})</h3>
      <div className="flex flex-wrap gap-2 mb-4">
        {selectedMembers.map((id) => {
          const student = students.find((s) => s.studentId === id);
          if (!student) return null;
          return <span key={id} className="px-2 py-1 bg-teal-200 text-teal-800 rounded-full text-sm">{student.name}</span>;
        })}
      </div>
      <div className="mb-4 space-y-2">
        {students.map((s) => (
          <label key={s.id} className="flex items-center space-x-2">
            <input type="checkbox" checked={selectedMembers.includes(s.studentId!)} onChange={() => s.studentId && toggleMember(s.studentId)} />
            <span>{s.name} ({s.studentId})</span>
          </label>
        ))}
      </div>
      <button onClick={handleConfirm} className="bg-teal-600 text-white px-4 py-2 rounded w-full">Confirm</button>
    </div>
  );
}
