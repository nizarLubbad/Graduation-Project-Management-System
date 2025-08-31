import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function LoginForm({ onSwitch }: { onSwitch: () => void }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const { login } = useAuth();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();

    const storedUser = localStorage.getItem("user");
    if (!storedUser) {
      setError("No account found. Please register first.");
      return;
    }

    const parsedUser = JSON.parse(storedUser);

    if (parsedUser.email === email && parsedUser.password === password) {
      login(parsedUser);
      if (parsedUser.role === "student") {
        navigate("/dashboard/student");
      } else {
        navigate("/dashboard/supervisor");
      }
    } else {
      setError("Invalid email or password");
    }
  };

  return (
    <form
      onSubmit={handleLogin}
      className="bg-gray-50 shadow-xl rounded-2xl p-8 w-full max-w-md mx-auto"
    >
      <h2 className="text-3xl font-bold mb-2 text-center text-teal-700">Welcome </h2>
      <p className="text-gray-500 text-center mb-6">
      Sign in to your Graduation Project Management Account
      </p>

      {error && (
        <p className="text-red-500 text-center mb-4 font-medium">{error}</p>
      )}

      <div className="space-y-3">
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
      </div>

      <button
        type="submit"
        className="mt-6 w-full bg-teal-700 text-white p-3 rounded-xl font-semibold hover:bg-teal-800 transition shadow-md hover:shadow-lg"
      >
        Login
      </button>

      <p className="mt-4 text-center text-sm">
        Don’t have an account?{" "}
        <span
          className="text-blue-600 cursor-pointer font-medium hover:underline"
          onClick={onSwitch}
        >
          Register
        </span>
      </p>
    </form>
  );
}
