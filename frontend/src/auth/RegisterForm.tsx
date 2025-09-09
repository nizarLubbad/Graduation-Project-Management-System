import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { User } from "../context/AuthContext";

export default function RegisterForm({ onSwitch }: { onSwitch: () => void }) {
  const [name, setName] = useState("");
  const [studentId, setStudentId] = useState("");
  const [department, setDepartment] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState<"student" | "supervisor">("student");
  const [error, setError] = useState(""); 

  const navigate = useNavigate();
  const { login } = useAuth();

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();

   
    const storedUsers = JSON.parse(localStorage.getItem("users") || "[]");

   
    const existingUser = storedUsers.find((u: User) => u.email === email);
    if (existingUser) {
      setError("This email is already registered.");
      return;
    }

   
    const newUser: User & { studentId?: string; department?: string; password?: string } = {
      id: Date.now().toString(),
      name,
      email,
      password,
      role,
      studentId,
      department,
    };

   
    const updatedUsers = [...storedUsers, newUser];
    localStorage.setItem("users", JSON.stringify(updatedUsers));

    login(newUser);

    if (role === "student") {
      navigate("/dashboard/student");
    } else {
      navigate("/dashboard/supervisor");
    }
  };

  return (
    <form
      onSubmit={handleRegister}
      className="bg-gray-50 shadow-xl rounded-2xl p-8 w-full max-w-md mx-auto"
    >
      <h2 className="text-3xl font-bold mb-2 text-center text-teal-700">Create Account</h2>
      <p className="text-gray-500 text-center mb-6">
        Join the Graduation Project Management System
      </p>

     
      {error && <p className="text-red-500 text-center mb-4 font-medium">{error}</p>}

      <div className="space-y-3">
        <input
          type="text"
          placeholder="Full Name"
          className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
        <input
          type="email"
          placeholder="Email"
          className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <select
          className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
          value={role}
          onChange={(e) => setRole(e.target.value as "student" | "supervisor")}
        >
          <option value="student">Student</option>
          <option value="supervisor">Supervisor</option>
        </select>
        {role === "student" && (
          <>
            <input
              type="text"
              placeholder="Student ID"
              className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
              value={studentId}
              onChange={(e) => setStudentId(e.target.value)}
              required
            />
            <input
              type="text"
              placeholder="Department (Optional)"
              className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-teal-500"
              value={department}
              onChange={(e) => setDepartment(e.target.value)}
            />
          </>
        )}
      </div>

      <button
        type="submit"
        className="mt-6 w-full bg-teal-700 text-white p-3 rounded-xl font-semibold hover:bg-teal-800 transition shadow-md hover:shadow-lg"
      >
        Create Account
      </button>

      <p className="mt-4 text-center text-sm">
        Already have an account?{" "}
        <span
          className="text-blue-600 cursor-pointer font-medium hover:underline"
          onClick={onSwitch}
        >
          Sign in
        </span>
      </p>
    </form>
  );
}
