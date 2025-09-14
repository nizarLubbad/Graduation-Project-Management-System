import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { Team, Supervisor, Assignment, User } from "../types/types";

export default function BookingSupervisor() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [team, setTeam] = useState<Team | null>(null);
  const [supervisors, setSupervisors] = useState<Supervisor[]>([]);
  const [selectedSupervisor, setSelectedSupervisor] = useState<Supervisor | null>(null);
  const [projectTitle, setProjectTitle] = useState("");
  const [projectDescription, setProjectDescription] = useState("");
  const [remainingSlots, setRemainingSlots] = useState<number>(0);

  function isSupervisor(u: User): u is Supervisor {
    return u.role === "supervisor";
  }

  useEffect(() => {
    if (!user?.studentId) return;

    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");

    const availableSupervisors = storedUsers.filter(isSupervisor).filter(s => {
      const max = s.maxTeams || 0;
      const booked = s.currentTeams || 0;
      return max - booked > 0;
    });

    setSupervisors(availableSupervisors);

    const storedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
    const myTeam = storedTeams.find((t) => t.leaderId === user.studentId);
    if (myTeam) {
      setTeam(myTeam);
      setProjectTitle(myTeam.projectTitle || "");
      setProjectDescription(myTeam.projectDescription || "");
    }
  }, [user]);

  useEffect(() => {
    if (!selectedSupervisor) {
      setRemainingSlots(0);
      return;
    }
    const max = selectedSupervisor.maxTeams || 0;
    const booked = selectedSupervisor.currentTeams || 0;
    setRemainingSlots(max - booked);
  }, [selectedSupervisor]);

  const handleSubmit = () => {
    if (!team || !selectedSupervisor || !projectTitle.trim()) {
      Swal.fire({ icon: "error", title: "Missing Info", text: "Complete all fields." });
      return;
    }

    if (remainingSlots <= 0) {
      Swal.fire({
        icon: "error",
        title: "No Slots Available",
        text: `${selectedSupervisor.name} cannot take more teams.`,
      });
      return;
    }

    const storedAssignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");

    const memberNames = storedUsers
      .filter((u) => team.members.includes(u.studentId!))
      .map((u) => u.name);

    const newAssignment: Assignment = {
      id: Date.now().toString(),
      teamId: team.teamId,
      teamName: team.teamName,
      members: memberNames,
      supervisorName: selectedSupervisor.name,
      projectTitle,
      projectDescription,
    };

    localStorage.setItem("supervisorAssignments", JSON.stringify([...storedAssignments, newAssignment]));

    const updatedUsers: User[] = storedUsers.map(u => {
      if (isSupervisor(u) && u.id === selectedSupervisor.id) {
        return { ...u, currentTeams: (u.currentTeams || 0) + 1 };
      }
      return u;
    });
    localStorage.setItem("users", JSON.stringify(updatedUsers));

    const updatedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]").map((t: Team) =>
      t.teamId === team.teamId ? { ...t, projectTitle, projectDescription } : t
    );
    localStorage.setItem("teams", JSON.stringify(updatedTeams));

    Swal.fire({
      icon: "success",
      title: "Supervisor Booked!",
      text: `You booked ${selectedSupervisor.name} successfully.`,
    }).then(() => navigate("/dashboard/student"));
  };

  if (!team) return <p className="text-center mt-10">Loading your team...</p>;

  return (
    <div className="p-6 max-w-3xl mx-auto bg-white shadow-lg rounded-xl space-y-4">
      <h2 className="text-2xl font-bold text-teal-700 mb-4">Book a Supervisor</h2>
      <p className="mb-4"><strong>Team Name:</strong> {team.teamName}</p>

      <input
        type="text"
        placeholder="Project Title"
        value={projectTitle}
        onChange={(e) => setProjectTitle(e.target.value)}
        className="w-full p-2 border rounded mb-2"
      />

      <textarea
        placeholder="Project Description"
        value={projectDescription}
        onChange={(e) => setProjectDescription(e.target.value)}
        className="w-full p-2 border rounded mb-4"
      />

      <h3 className="text-lg font-semibold mb-2">Select Supervisor</h3>
      <select
        value={selectedSupervisor?.id || ""}
        onChange={(e) =>
          setSelectedSupervisor(supervisors.find((s) => s.id === e.target.value) || null)
        }
        className="w-full p-2 border rounded mb-2"
      >
        <option value="">Select a supervisor</option>
        {supervisors.map((s) => (
          <option key={s.id} value={s.id}>
            {s.name} (Remaining Slots: {(s.maxTeams || 0) - (s.currentTeams || 0)})
          </option>
        ))}
      </select>

      {selectedSupervisor && (
        <p className="text-sm font-semibold mb-4">
          Remaining Slots: {remainingSlots}
        </p>
      )}

      <button
        onClick={handleSubmit}
        className="w-full bg-teal-600 text-white px-4 py-2 rounded"
      >
        Confirm Booking
      </button>
    </div>
  );
}
