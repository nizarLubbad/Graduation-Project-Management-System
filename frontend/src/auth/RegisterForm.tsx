import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User } from "../types/types";

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
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");

    if (storedUsers.find((u) => u.email === email)) {
      setError("This email is already registered.");
      return;
    }

    const newUser: User = {
      id: Date.now().toString(),
      name,
      email,
      password,
      role,
      studentId: role === "student" ? studentId : undefined,
      department: role === "student" ? department : undefined,
      status: role === "student" ? false : undefined,
      team: undefined,
    };

    localStorage.setItem("users", JSON.stringify([...storedUsers, newUser]));
    login(newUser);

    if (role === "student") {
      const updatedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
      const students = updatedUsers.filter((u) => u.role === "student");
      const supervisors = updatedUsers.filter((u) => u.role === "supervisor");

      const availableStudents = students.filter((u) => u.status === false);
      const busyStudents = students.filter((u) => u.status === true);

      if (availableStudents.length < 2) {
        Swal.fire({
          title: "Warning",
          text: "Not enough students available to create a team.",
          icon: "warning",
          iconColor: "gray", 
          confirmButtonText: "Go to Dashboard",
          confirmButtonColor: "green" 

        }).then(() => navigate("/dashboard/student"));
      } else if (supervisors.length === 0) {
        Swal.fire({
          title: "Warning",
          text: "No supervisors are registered yet.",
          iconColor: "gray", 
          icon: "warning",
          confirmButtonText: "Go to Dashboard",
          confirmButtonColor: "green" 

        }).then(() => navigate("/dashboard/student"));
      } else if (students.length >= 2 && availableStudents.length === 0 && busyStudents.length > 0) {
        Swal.fire({
          title: "Warning",
          text: "No available students to create a team.",
          iconColor: "gray", 
          icon: "warning",
          confirmButtonText: "Go to Dashboard",
            confirmButtonColor: "green" 
        }).then(() => navigate("/dashboard/student"));
      } else {
        Swal.fire({
          title: "Welcome!",
          text: "You have registered successfully.",
          icon: "success",
          showCancelButton: true,
          confirmButtonText: "Create Team Now",
          cancelButtonText: "Skip for Now",
        }).then((result) => {
          if (result.isConfirmed) navigate("/create-team");
          else navigate("/dashboard/student");
        });
      }
    } else {
      Swal.fire({
        title: "Welcome Supervisor!",
        text: "You have registered successfully.",
        icon: "success",
        confirmButtonText: "Go to Dashboard",
      }).then(() => navigate("/dashboard/supervisor"));
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
        onChange={(e) => setName(e.target.value)}
        required
        className="w-full p-3 border rounded-lg mb-2"
      />
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
      <select
        value={role}
        onChange={(e) => setRole(e.target.value as "student" | "supervisor")}
        className="w-full p-3 border rounded-lg mb-2"
      >
        <option value="student">Student</option>
        <option value="supervisor">Supervisor</option>
      </select>
      {role === "student" && (
        <>
          <input
            type="text"
            placeholder="Student ID"
            value={studentId}
            onChange={(e) => setStudentId(e.target.value)}
            required
            className="w-full p-3 border rounded-lg mb-2"
          />
          <input
            type="text"
            placeholder="Department"
            value={department}
            onChange={(e) => setDepartment(e.target.value)}
            className="w-full p-3 border rounded-lg mb-2"
          />
        </>
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
