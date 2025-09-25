import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User, Team } from "../types/types";

export default function CreateTeam() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  const [students, setStudents] = useState<User[]>([]);
  const [selectedMembers, setSelectedMembers] = useState<number[]>([]);
  const [teamName, setTeamName] = useState("");
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  useEffect(() => {
    if (!user || !user.token) {
      Swal.fire("Unauthorized", "Please login first.", "warning");
      navigate("/");
      return;
    }
    if (user.status) navigate("/dashboard/student");

    const fetchStudents = async () => {
      try {
        const res = await fetch(`${baseUrl}/api/Students/all`, {
          headers: { Authorization: `Bearer ${user.token}` },
        });
        const raw = await res.json();
        if (!raw.success) throw new Error("API returned success = false");

        const available = raw.students.filter(
          (s: User) => !s.status && s.userId !== user.userId
        );
        setStudents(available);
      } catch {
        Swal.fire("Error", "Failed to load students", "error");
      }
    };
    fetchStudents();
  }, [user, navigate]);

  const toggleMember = (studentId: number) => {
    setSelectedMembers(prev =>
      prev.includes(studentId)
        ? prev.filter(id => id !== studentId)
        : [...prev, studentId]
    );
  };

  const handleConfirm = async () => {
    if (!teamName.trim()) {
      Swal.fire("Warning", "Please enter a team name.", "warning");
      return;
    }
    const memberIds = Array.from(new Set([user!.userId, ...selectedMembers]));

    try {
      const createRes = await fetch(`${baseUrl}/api/Teams/create`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${user?.token}`,
        },
        body: JSON.stringify({ teamName: teamName.trim(), memberStudentIds: memberIds }),
      });
      const createdTeam: Team = await createRes.json();

      await fetch(`${baseUrl}/api/Teams/${createdTeam.teamId}/update-student-status`, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${user?.token}`,
        },
        body: JSON.stringify({}),
      });

      login({ ...user!, status: true, team: createdTeam });

      Swal.fire({
        icon: "success",
        title: "Team Created!",
        text: `You have created your team: ${createdTeam.teamName}`,
        confirmButtonText: "Choose a Supervisor",
      }).then(() => navigate("/booking-supervisor"));
    } catch (err) {
      Swal.fire("Error", err instanceof Error ? err.message : "Something went wrong", "error");
    }
  };

  return (
    <div className="p-6 max-w-3xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-xl font-bold text-teal-700 mb-4 text-center">Create Team</h2>
      <p className="mb-4 text-center font-semibold text-gray-700">Leader: <span className="text-teal-700">{user?.name}</span></p>

      <input
        type="text"
        placeholder="Team Name"
        value={teamName}
        onChange={e => setTeamName(e.target.value)}
        className="w-full p-2 border rounded mb-4"
      />

      <h3 className="mb-2">Selected Members ({selectedMembers.length})</h3>
      <div className="flex flex-wrap gap-2 mb-4">
        {selectedMembers.map(id => {
          const student = students.find(s => s.userId === id);
          if (!student) return null;
          return <span key={id} className="px-2 py-1 bg-teal-200 text-teal-800 rounded">{student.name}</span>;
        })}
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 mb-6">
        {students.map(s => (
          <label key={s.userId} className="flex items-center p-2 border rounded cursor-pointer hover:bg-gray-50">
            <input type="checkbox" checked={selectedMembers.includes(s.userId)} onChange={() => toggleMember(s.userId)} className="h-4 w-4" />
            <span className="ml-2">{s.name}</span>
          </label>
        ))}
      </div>

      <button onClick={handleConfirm} className="w-full bg-teal-600 text-white px-4 py-2 rounded">Confirm</button>
    </div>
  );
}
