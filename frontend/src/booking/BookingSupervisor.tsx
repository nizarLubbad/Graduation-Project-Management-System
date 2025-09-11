
import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { Team, Supervisor, Assignment, User } from "../types/types";

export default function BookingSupervisor() {
  const { user } = useAuth();
  const navigate = useNavigate();

  // ================================
  // Local states for handling data
  // ================================
  const [team, setTeam] = useState<Team | null>(null);
  const [supervisors, setSupervisors] = useState<Supervisor[]>([]);
  const [selectedSupervisor, setSelectedSupervisor] = useState<Supervisor | null>(null);
  const [projectTitle, setProjectTitle] = useState("");
  const [projectDescription, setProjectDescription] = useState("");

  // ================================
  // Type guard to ensure user is supervisor
  // ================================
  function isSupervisor(u: User): u is Supervisor {
    return u.role === "supervisor";
  }

  // ================================
  // Load supervisors & team info on mount
  // ================================
  useEffect(() => {
    if (!user?.studentId) return;

    // Get supervisors from users
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const supervisorList = storedUsers.filter(isSupervisor);
    setSupervisors(supervisorList);

    // Get the team for current student
    const storedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]");
    const myTeam = storedTeams.find((t) => t.leaderId === user.studentId);
    if (myTeam) {
      setTeam(myTeam);
      setProjectTitle(myTeam.projectTitle || "");
      setProjectDescription(myTeam.projectDescription || "");
    }
  }, [user]);

  // ================================
  // Handle booking supervisor
  // ================================
  const handleSubmit = () => {
    if (!team || !selectedSupervisor || !projectTitle.trim()) {
      Swal.fire({ icon: "error", title: "Missing Info", text: "Complete all fields." });
      return;
    }

    // Get assignments and users
    const storedAssignments: Assignment[] = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]");
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");

    // Get names of team members
    const memberNames = storedUsers
      .filter((u) => team.members.includes(u.studentId!))
      .map((u) => u.name);

    // New assignment object
    const newAssignment: Assignment = {
      id: Date.now().toString(),
      teamId: team.teamId,
      teamName: team.teamName,
      members: memberNames,
      supervisorName: selectedSupervisor.name,
      projectTitle,
      projectDescription,
    };

    // Save assignment
    localStorage.setItem("supervisorAssignments", JSON.stringify([...storedAssignments, newAssignment]));

    // Update project details inside team
    const updatedTeams: Team[] = JSON.parse(localStorage.getItem("teams") || "[]").map((t: Team) =>
      t.teamId === team.teamId ? { ...t, projectTitle, projectDescription } : t
    );
    localStorage.setItem("teams", JSON.stringify(updatedTeams));

    // Show success message & navigate back
    Swal.fire({
      icon: "success",
      title: "Supervisor Booked!",
      text: `You booked ${selectedSupervisor.name} successfully.`,
    }).then(() => navigate("/dashboard/student"));
  };

  // ================================
  // UI Rendering
  // ================================
  if (!team) return <p className="text-center mt-10">Loading your team...</p>;

  return (
    <div className="p-6 max-w-3xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-2xl font-bold text-teal-700 mb-4">Book a Supervisor</h2>
      <p className="mb-4"><strong>Team Name:</strong> {team.teamName}</p>

      {/* Project title input */}
      <input
        type="text"
        placeholder="Project Title"
        value={projectTitle}
        onChange={(e) => setProjectTitle(e.target.value)}
        className="w-full p-2 border rounded mb-2"
      />

      {/* Project description textarea */}
      <textarea
        placeholder="Project Description"
        value={projectDescription}
        onChange={(e) => setProjectDescription(e.target.value)}
        className="w-full p-2 border rounded mb-4"
      />

      {/* Supervisor selection */}
      <h3 className="text-lg font-semibold mb-4">Select Supervisor</h3>
      <select
        value={selectedSupervisor?.id || ""}
        onChange={(e) =>
          setSelectedSupervisor(supervisors.find((s) => s.id === e.target.value) || null)
        }
        className="w-full p-2 border rounded mb-4"
      >
        <option value="">Select a supervisor</option>
        {supervisors.map((s) => (
          <option key={s.id} value={s.id}>{s.name}</option>
        ))}
      </select>

      {/* Confirm button */}
      <button
        onClick={handleSubmit}
        className="w-full bg-teal-600 text-white px-4 py-2 rounded"
      >
        Confirm Booking
      </button>
    </div>
  );
}