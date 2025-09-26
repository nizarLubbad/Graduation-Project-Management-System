
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { User} from "../types/types";


export default function LoginForm({ onSwitch }: { onSwitch: () => void }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const { login } = useAuth();
  const baseUrl = import.meta.env.VITE_API_URL;

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
  
    try {
      // تسجيل الدخول
      const res = await fetch(`${baseUrl}/api/Auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });
  
      const data = await res.json();
      if (!res.ok) throw new Error(data.message || "Login failed");
  
      // تحديد الدور بشكل موحد
      const normalizedRole = data.role?.toLowerCase() === "supervisor" ? "supervisor" : "student";
  
      let user: User = {
        userId: data.userId,
        name: data.name,
        email: data.email || email,
        role: normalizedRole,
        department: data.department,
        status: data.status ?? false,
        token: data.token,
      };
  
      // 🔹 جلب الفريق الحالي إذا كان الطالب
      if (normalizedRole === "student") {
        try {
          const allTeamsRes = await fetch(`${baseUrl}/api/Teams`, {
            headers: { Authorization: `Bearer ${user.token}` },
          });
          if (allTeamsRes.ok) {
            const allTeams = await allTeamsRes.json();
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const myTeam = allTeams.find((team: any) =>
              team.memberStudentIds.includes(user.userId)
            );
            if (myTeam) user = { ...user, team: myTeam, status: true };
          }
        } catch (err) {
          console.error("Failed to fetch team after login", err);
        }
      }
  
      login(user);
  
      // توجيه حسب الدور
      if (normalizedRole === "student") navigate("/dashboard/student/KanbanBoard");
      else navigate("/dashboard/supervisor/SupBoard");
    } catch (err) {
      if (err instanceof Error) setError(err.message);
      else setError("Something went wrong");
    }
  };
  

  return (
    <form onSubmit={handleLogin} className="bg-gray-50 shadow-xl rounded-2xl p-6 w-full max-w-md mx-auto">
      <h2 className="text-2xl font-bold mb-2 text-center text-teal-700">Welcome</h2>
      {error && <p className="text-red-500 text-center mb-4">{error}</p>}

      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />

      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />

      <button
        type="submit"
        className="w-full p-3 mt-4 bg-teal-700 text-white rounded-xl"
      >
        Login
      </button>

      <p className="mt-4 text-center text-sm">
        Don’t have an account?{" "}
        <span onClick={onSwitch} className="text-blue-600 cursor-pointer">
          Register
        </span>
      </p>
    </form>
  );
}
