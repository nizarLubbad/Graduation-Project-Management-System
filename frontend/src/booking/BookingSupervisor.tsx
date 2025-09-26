import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User, Team } from "../types/types";

interface SupervisorWithRemaining extends User {
  remainingTeams: number;
}

export default function BookingSupervisor() {
  const { user, updateUserTeam } = useAuth();
  const navigate = useNavigate();

  const [team, setTeam] = useState<Team | null>(null);
  const [supervisors, setSupervisors] = useState<SupervisorWithRemaining[]>([]);
  const [selectedSupervisor, setSelectedSupervisor] = useState<number | null>(null);
  const [projectTitle, setProjectTitle] = useState<string>("");
  const [projectDescription, setProjectDescription] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  const baseUrl = import.meta.env.VITE_API_URL;

  // 🔹 جلب بيانات الفريق والمشرفين
  useEffect(() => {
    const fetchTeamAndSupervisors = async () => {
      if (!user) return;

      try {
        const resTeams = await fetch(`${baseUrl}/api/Teams`);
        const allTeamsRaw = await resTeams.json();
        const allTeams: Team[] = Array.isArray(allTeamsRaw)
          ? allTeamsRaw
          : allTeamsRaw.teams || [];

        // العثور على فريق الطالب الحالي
        const myTeam = allTeams.find((t) =>
          t.memberStudentIds.some((id) => Number(id) === Number(user.userId))
        );

        if (!myTeam) {
          Swal.fire({
            icon: "warning",
            title: "No team found",
            text: "You must create a team first.",
          });
          navigate("/dashboard/student");
          return;
        }

        setTeam(myTeam);
        if (updateUserTeam) updateUserTeam(myTeam);

        // جلب جميع المستخدمين
        const resUsers = await fetch(`${baseUrl}/api/Auth/getUsers`);
        const rawUsers = await resUsers.json();
        const allUsers: User[] = Array.isArray(rawUsers)
          ? rawUsers
          : rawUsers.users || [];

        // فلترة المشرفين المتاحين مع عدد الفرق المتبقية
        const availableSupervisors: SupervisorWithRemaining[] = [];
        for (const u of allUsers) {
          if (u.role && u.role.toLowerCase() === "supervisor") {
            const resRemaining = await fetch(
              `${baseUrl}/api/Supervisors/${u.userId}/remaining-teams`
            );
            const remainingRaw = await resRemaining.json();
            const remaining =
              typeof remainingRaw === "number"
                ? remainingRaw
                : remainingRaw.remainingTeams ?? 0;

            if (remaining > 0) {
              availableSupervisors.push({ ...u, remainingTeams: remaining });
            }
          }
        }

        setSupervisors(availableSupervisors);
      } catch (err) {
        Swal.fire({
          icon: "error",
          title: "Error fetching data",
          text: err instanceof Error ? err.message : "Something went wrong",
        });
      }
    };

    fetchTeamAndSupervisors();
  }, [user, navigate, updateUserTeam]);

  // 🔹 إنشاء المشروع وحجز المشرف
  const handleBooking = async () => {
    if (!team || !selectedSupervisor || !projectTitle || !projectDescription) return;

    setLoading(true);
    try {
      // 1️⃣ إنشاء المشروع وإرجاعه (مع الـ id)
      const projectRes = await fetch(`${baseUrl}/api/Project`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          projectTitle,
          description: projectDescription,
          supervisorId: selectedSupervisor,
          teamId: team.teamId,
          isCompleted: false,
        }),
      });

      if (!projectRes.ok) {
        throw new Error(`Failed to create project (status: ${projectRes.status})`);
      }

      const createdProject = await projectRes.json();
      console.log("✅ Created Project from backend:", createdProject);

      // 2️⃣ حجز المشرف
      const res = await fetch(
        `${baseUrl}/api/Supervisors/${selectedSupervisor}/book-team/${team.teamId}`,
        { method: "POST" }
      );

      if (!res.ok) {
        throw new Error(`Failed to book supervisor (status: ${res.status})`);
      }

      const supervisorName =
        supervisors.find((s) => s.userId === selectedSupervisor)?.name ?? "Unknown";

      // 3️⃣ تحديث الـ context مباشرة (مع project.id)
      if (updateUserTeam) {
        updateUserTeam({
          ...team,
          supervisorId: selectedSupervisor,
          supervisorName,
          project: {
            id: createdProject.id, // ✅ تخزين الـ id
            projectTitle,
            description: projectDescription,
            supervisorId: selectedSupervisor,
            teamId: team.teamId,
            isCompleted: false,
          },
        });
      }

      // 4️⃣ تحديث الحالة المحلية
      setTeam({
        ...team,
        supervisorId: selectedSupervisor,
        supervisorName,
        project: {
          id: createdProject.id, // ✅ تخزين الـ id
          projectTitle,
          description: projectDescription,
          supervisorId: selectedSupervisor,
          teamId: team.teamId,
          isCompleted: false,
        },
      });

      Swal.fire({
        icon: "success",
        title: "Supervisor booked and project created!",
        confirmButtonText: "Go to Dashboard",
      }).then(() => {
        navigate("/dashboard/student");
      });

      setSelectedSupervisor(null);
      setProjectTitle("");
      setProjectDescription("");
    } catch (err) {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: err instanceof Error ? err.message : "Something went wrong",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-6 max-w-3xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-xl font-bold text-teal-700 mb-4 text-center">
        Book a Supervisor
      </h2>

      {team && (
        <>
          <p className="mb-2">
            <strong>Team:</strong> {team.teamName}
          </p>
          {team.supervisorName && (
            <p className="mb-2">
              <strong>Current Supervisor:</strong> {team.supervisorName}
            </p>
          )}
          {team.project && (
            <p className="mb-2">
              <strong>Project:</strong> {team.project.projectTitle}
              {team.project.id && (
                <span className="ml-2 text-gray-500">[ID: {team.project.id}]</span>
              )}
            </p>
          )}

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

          <select
            onChange={(e) => setSelectedSupervisor(Number(e.target.value))}
            value={selectedSupervisor || ""}
            className="w-full p-2 border rounded mb-4"
          >
            <option value="">Select a supervisor</option>
            {supervisors.map((s) => (
              <option key={s.userId} value={s.userId}>
                {s.name} ({s.remainingTeams} slots remaining)
              </option>
            ))}
          </select>

          <button
            onClick={handleBooking}
            disabled={
              !selectedSupervisor || !projectTitle || !projectDescription || loading
            }
            className="w-full bg-teal-600 text-white px-4 py-2 rounded disabled:opacity-50"
          >
            {loading ? "Processing..." : "Confirm Booking"}
          </button>
        </>
      )}
    </div>
  );
}
