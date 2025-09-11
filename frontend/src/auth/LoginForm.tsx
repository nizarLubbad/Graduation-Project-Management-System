import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { User } from "../types/types";

export default function LoginForm({ onSwitch }: { onSwitch: () => void }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const { login } = useAuth();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const foundUser = storedUsers.find((u) => u.email === email && u.password === password);

    if (foundUser) {
      login(foundUser);
      if (foundUser.role === "student") navigate("/dashboard/student");
      else navigate("/dashboard/supervisor");
    } else {
      setError("Invalid email or password");
    }
  };

  return (
    <form onSubmit={handleLogin} className="bg-gray-50 shadow-xl rounded-2xl p-6 w-full max-w-md mx-auto">
      <h2 className="text-2xl font-bold mb-2 text-center text-teal-700">Welcome</h2>
      {error && <p className="text-red-500 text-center mb-4">{error}</p>}
      <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required className="w-full p-3 border rounded-lg mb-2" />
      <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required className="w-full p-3 border rounded-lg mb-2" />
      <button type="submit" className="w-full p-3 mt-4 bg-teal-700 text-white rounded-xl">Login</button>
      <p className="mt-4 text-center text-sm">Don’t have an account? <span onClick={onSwitch} className="text-blue-600 cursor-pointer">Register</span></p>
    </form>
  );
}
