import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User } from "../types/types";

export default function RegisterForm({ onSwitch }: { onSwitch: () => void }) {
  const [name, setName] = useState("");
  const [department, setDepartment] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState<"student" | "supervisor">("student");
  const [userId, setUserId] = useState<number>(0);
  const [error, setError] = useState("");
  const [, setUsers] = useState<User[]>([]);

  const navigate = useNavigate();
  const { login } = useAuth();
  const baseUrl = "https://backendteam-001-site1.qtempurl.com";

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const res = await fetch(`${baseUrl}/api/Auth/getUsers`);
        const data = await res.json();
        const allUsers: User[] = Array.isArray(data) ? data : data.users || [];

        const normalized = allUsers.map(u => ({
          ...u,
          role: u.role?.toLowerCase() as "student" | "supervisor",
          status: Boolean(u.status),
        }));

        normalized.forEach(u =>
          console.log(`ID:${u.userId} | Name:${u.name} | Role:${u.role} | Status(Boolean):${u.status}`)
        );

        setUsers(normalized);
      } catch (err) {
        console.error("❌ Error fetching users:", err);
      }
    };
    fetchUsers();
  }, []);

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      const endpoint =
        role === "student"
          ? "/api/Auth/register/student"
          : "/api/Auth/register/supervisor";

      const payload =
        role === "student"
          ? { name, email, password, userId, department }
          : { name, email, password, userId, department: "General" };

      const res = await fetch(`${baseUrl}${endpoint}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || "Registration failed");
      }

      // تسجيل الدخول مباشرة
      const loginRes = await fetch(`${baseUrl}/api/Auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });
      const loginData = await loginRes.json();
      if (!loginRes.ok) throw new Error(loginData.message || "Login failed");

      login({
        userId: loginData.userId?.toString() || "",
        name: loginData.name || "",
        email: loginData.email || email,
        role: (loginData.role?.toLowerCase() as "student" | "supervisor") || "student",
        status: Boolean(loginData.status),
        password,
        token: loginData.token || "",
      });

      // 🔹 طباعة معلومات المستخدم المسجل
      console.log("✅ Logged in user info:", {
        userId: loginData.userId,
        name: loginData.name,
        email: loginData.email,
        role: loginData.role,
        status: loginData.status,
      });

      // 🔹 إعادة جلب المستخدمين بعد التسجيل
      const usersRes = await fetch(`${baseUrl}/api/Auth/getUsers`);
      const usersJson = await usersRes.json();
      const allUsers: User[] = Array.isArray(usersJson)
        ? usersJson
        : usersJson.users || [];

      const normalizedUsers = allUsers.map(u => ({
        ...u,
        role: u.role?.toLowerCase() as "student" | "supervisor",
        status: Boolean(u.status),
      }));

      setUsers(normalizedUsers);

      // 🔹 تقسيم حسب الدور
      const students = normalizedUsers.filter(u => u.role === "student");
      const availableStudents = students.filter(u => !u.status);
      const busyStudents = students.filter(u => u.status);

      // 🔹 طباعة الطلاب المتاحين وغير المتاحين
      console.log("🟢 Available Students:", availableStudents);
      console.log("🔴 Busy Students:", busyStudents);

      // 🔹 الرسائل والتنقل
      if (role === "student") {
        Swal.fire({
          title: "Welcome!",
          text: "You have registered successfully.",
          icon: "success",
          showCancelButton: true,
          confirmButtonText: "Create Team Now",
          cancelButtonText: "Skip for Now",
        }).then(result => {
          if (result.isConfirmed) navigate("/create-team");
          else navigate("/dashboard/student");
        });
      } else {
        Swal.fire({
          title: "Welcome Supervisor!",
          text: "You have registered successfully.",
          icon: "success",
          confirmButtonText: "Go to Dashboard",
        }).then(() => navigate("/dashboard/supervisor"));
      }
    } catch (err) {
      if (err instanceof Error) setError(err.message);
      else setError("Something went wrong");
    }
  };

  return (
    <form
      onSubmit={handleRegister}
      className="bg-gray-50 shadow-xl rounded-2xl p-8 w-full max-w-md mx-auto"
    >
      <h2 className="text-3xl font-bold mb-2 text-center text-teal-700">
        Create Account
      </h2>
      {error && <p className="text-red-500 text-center mb-4">{error}</p>}

      <input
        type="text"
        placeholder="Full Name"
        value={name}
        onChange={e => setName(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />
      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={e => setEmail(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={e => setPassword(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />
      <input
        type="text"
        placeholder="User ID"
        value={userId}
        onChange={e => setUserId(Number(e.target.value))}
        className="w-full p-3 border rounded-lg mb-2"
      />
      <select
        value={role}
        onChange={e => setRole(e.target.value as "student" | "supervisor")}
        className="w-full p-3 border rounded-lg mb-2"
      >
        <option value="student">Student</option>
        <option value="supervisor">Supervisor</option>
      </select>
      {role === "student" && (
        <input
          type="text"
          placeholder="Department"
          value={department}
          onChange={e => setDepartment(e.target.value)}
          className="w-full p-3 border rounded-lg mb-2"
        />
      )}
      <button
        type="submit"
        className="w-full p-3 mt-4 bg-teal-700 text-white rounded-xl"
      >
        Create Account
      </button>
      <p className="mt-4 text-center text-sm">
        Already have an account?{" "}
        <span onClick={onSwitch} className="text-blue-600 cursor-pointer">
          Sign in
        </span>
      </p>
    </form>
  );
}
