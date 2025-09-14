import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Assignment, User, ProjectDisplay, Supervisor } from "../types/types";

export default function SupBoard() {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<ProjectDisplay[]>([]);
  const [currentSupervisor, setCurrentSupervisor] = useState<Supervisor | null>(null);
  const [maxTeams, setMaxTeams] = useState<number | null>(null);
  const [inputValue, setInputValue] = useState<number>(0);
  const [bookedTeams, setBookedTeams] = useState<number>(0);

  useEffect(() => {
    const userData: User | null = JSON.parse(localStorage.getItem("currentUser") || "null");
    if (!userData || userData.role !== "supervisor") return;

    const supervisor = userData as Supervisor;
    setCurrentSupervisor(supervisor);

    const storedUsers = JSON.parse(localStorage.getItem("users") || "[]") as User[];
    const supervisorData = storedUsers.find(u => u.id === supervisor.id) as Supervisor | undefined;

    const assignments = JSON.parse(localStorage.getItem("supervisorAssignments") || "[]") as Assignment[];
    const myProjects = assignments
      .filter(a => a.supervisorName === supervisor.name)
      .map(a => ({
        id: a.teamId,
        title: a.projectTitle || a.teamName,
        teamName: a.teamName,
        description: a.projectDescription || "",
        members: a.members,
      }));

    setProjects(myProjects);

    if (supervisorData) {
      setMaxTeams(supervisorData.maxTeams ?? null);
      setBookedTeams(supervisorData.currentTeams ?? myProjects.length);
    }
  }, []);

  const handleSetMax = () => {
    if (!currentSupervisor) return;
    const updatedUsers = ((JSON.parse(localStorage.getItem("users") || "[]")) as User[]).map(u => {
      if (u.id === currentSupervisor.id && u.role === "supervisor") {
        return { ...u, maxTeams: inputValue, currentTeams: 0 } as Supervisor;
      }
      return u;
    });
    localStorage.setItem("users", JSON.stringify(updatedUsers));
    setMaxTeams(inputValue);
    setBookedTeams(0);
  };

  const remainingSlots = maxTeams !== null ? maxTeams - bookedTeams : 0;

  if (maxTeams === null) {
    return (
      <div className="p-6 max-w-md mx-auto bg-white shadow-lg rounded-xl space-y-4">
        <h2 className="text-2xl font-bold mb-2">Set Maximum Teams</h2>
        <input
          type="number"
          min={1}
          value={inputValue}
          onChange={e => setInputValue(Number(e.target.value))}
          className="w-full p-2 border rounded"
        />
        <button
          onClick={handleSetMax}
          className="w-full bg-teal-600 text-white px-4 py-2 rounded"
        >
          Save
        </button>
      </div>
    );
  }

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold mb-4">Team Overview</h1>

      <div className="grid grid-cols-3 gap-4 mb-4">
        <div className="p-4 bg-white shadow rounded-xl border border-gray-200 text-center">
          <p className="font-semibold text-gray-700">Max Teams</p>
          <p className="text-lg font-bold">{maxTeams}</p>
        </div>
        <div className="p-4 bg-white shadow rounded-xl border border-gray-200 text-center">
          <p className="font-semibold text-gray-700">Booked Teams</p>
          <p className="text-lg font-bold">{bookedTeams}</p>
        </div>
        <div className="p-4 bg-white shadow rounded-xl border border-gray-200 text-center">
          <p className="font-semibold text-gray-700">Remaining Slots</p>
          <p className="text-lg font-bold">{remainingSlots}</p>
        </div>
      </div>

      <h2 className="text-2xl font-bold mb-4">Supervised Projects</h2>

      {projects.length === 0 ? (
        <p className="p-6 text-gray-600">No projects are currently under your supervision.</p>
      ) : (
        <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {projects.map(project => (
            <li
              key={project.id}
              className="p-4 bg-white/90 rounded-xl shadow-md shadow-gray-400 cursor-pointer transition hover:shadow-lg hover:shadow-gray-500"
              onClick={() => navigate(`/dashboard/supervisor/kanban/${project.id}/Kanban`)}
            >
              <p className="font-semibold text-lg mb-1">{project.title}</p>
              <p className="text-sm text-gray-500">Team: {project.teamName}</p>
              {project.description && (
                <p className="text-sm text-gray-400 mb-1">
                  Description: {project.description}
                </p>
              )}
              <p className="text-sm text-gray-500">
                Members: {project.members.join(", ")}
              </p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
