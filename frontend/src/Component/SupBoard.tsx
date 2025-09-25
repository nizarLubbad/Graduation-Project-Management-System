import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

 export interface Project {
  id: number;
  projectName: string;
  description: string;
  isCompleted: boolean;
  supervisorId: number | null;
  supervisorName: string | null;
  teamId: number;
  teamName: string;
}

interface Team {
  teamId: number;
  teamName: string;
  memberStudentIds: number[];
}

interface User {
  userId: number;
  name: string;
  role?: string;
}

export default function SupBoard() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [projects, setProjects] = useState<Project[]>([]);
  const [teams, setTeams] = useState<Team[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [currentSupervisor, setCurrentSupervisor] = useState<any>(null);
  const [maxTeams, setMaxTeams] = useState<number | null>(null);
  const [inputValue, setInputValue] = useState<number>(0);
  const [bookedTeams, setBookedTeams] = useState<number>(0);

  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  useEffect(() => {
    const fetchData = async () => {
      if (!user) return;

      try {
        const userId = user.userId;

        // جلب بيانات المشرف
        const resUser = await fetch(`${baseUrl}/api/Auth/${userId}`);
        const supervisorData = await resUser.json();
        setCurrentSupervisor(supervisorData);

        // جلب كل المشاريع
        const resProjects = await fetch(`${baseUrl}/api/Project`);
        const allProjects: Project[] = await resProjects.json();
        const myProjects = allProjects.filter(p => p.supervisorId === userId);
        setProjects(myProjects);
        setBookedTeams(myProjects.length);

        // جلب كل الفرق
        const resTeams = await fetch(`${baseUrl}/api/Teams`);
        const allTeams: Team[] = await resTeams.json();
        setTeams(allTeams);

        // جلب كل المستخدمين
        const resUsers = await fetch(`${baseUrl}/api/Auth/getUsers`);
        const allUsers: User[] | { users: User[] } = await resUsers.json();
        setUsers(Array.isArray(allUsers) ? allUsers : allUsers.users || []);
        

        // جلب العدد المتبقي للفرق
        const resRemaining = await fetch(`${baseUrl}/api/Supervisors/${userId}/remaining-teams`);
        const remainingData = await resRemaining.json();
        const remaining = typeof remainingData === "number" ? remainingData : remainingData.remainingTeams ?? 0;

        setMaxTeams(remaining + myProjects.length);
        setInputValue(remaining + myProjects.length);

      } catch (err) {
        console.error("Error fetching data:", err);
      }
    };

    fetchData();
  }, [user]);

  const handleSetMax = async () => {
    if (!currentSupervisor) return;

    try {
      const res = await fetch(
        `${baseUrl}/api/Supervisors/${currentSupervisor.userId}/max-teams`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: inputValue.toString(),
        }
      );

      if (!res.ok) throw new Error(`Failed to update max teams (status: ${res.status})`);

      const resUpdated = await fetch(`${baseUrl}/api/Auth/${currentSupervisor.userId}`);
      const updatedSupervisor = await resUpdated.json();
      setCurrentSupervisor(updatedSupervisor);
      setMaxTeams(Number(inputValue));
    } catch (err) {
      console.error("Error updating max teams:", err);
    }
  };

  const remainingSlots = maxTeams !== null ? maxTeams - bookedTeams : 0;

  // دالة للحصول على أسماء أعضاء الفريق
  const getTeamMembersNames = (teamId: number) => {
    const team = teams.find(t => t.teamId === teamId);
    if (!team) return "No members";
    const memberNames = team.memberStudentIds.map(id => {
      const student = users.find(u => u.userId === id);
      return student ? student.name : `ID: ${id}`;
    });
    return memberNames.join(", ");
  };

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold mb-4">Team Overview</h1>

      <div className="grid grid-cols-3 gap-4 mb-4">
        <div className="p-4 bg-white shadow rounded-xl border border-gray-200 text-center">
          <p className="font-semibold text-gray-700">Max Teams</p>
          <input
            type="number"
            min={bookedTeams}
            value={inputValue}
            onChange={e => setInputValue(Number(e.target.value))}
            className="w-full mt-2 p-2 border rounded text-center"
          />
          <button
            onClick={handleSetMax}
            className="mt-2 w-full bg-teal-600 text-white px-4 py-1 rounded"
          >
            Save
          </button>
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
              onClick={() => navigate(`/dashboard/supervisor/kanban/${project.teamId}/Kanban`)}
            >
              <p className="font-semibold text-lg mb-1">{project.projectName}</p>
              <p className="text-sm text-gray-500">Team: {project.teamName}</p>
              {project.description && (
                <p className="text-sm text-gray-400 mb-1">Description: {project.description}</p>
              )}
              <p className="text-sm text-gray-500">Members: {getTeamMembersNames(project.teamId)}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
