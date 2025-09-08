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

    
    const storedUsers = localStorage.getItem("users");
    if (!storedUsers) {
      setError("No account found. Please register first.");
      return;
    }

    const parsedUsers = JSON.parse(storedUsers);


    const foundUser = parsedUsers.find(
      (u: { email: string; password: string }) =>
        u.email === email && u.password === password
    );

    if (foundUser) {
      login(foundUser);
      if (foundUser.role === "student") {
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
      className="bg-gray-50 shadow-xl rounded-2xl p-6 sm:p-8 w-full max-w-md mx-4 sm:mx-auto"
    >
      <h2 className="text-2xl sm:text-3xl font-bold mb-2 text-center text-teal-700">
        Welcome
      </h2>
      <p className="text-gray-500 text-center mb-6 text-sm sm:text-base">
        Sign in to your Graduation Project Management Account
      </p>

      {error && (
        <p className="text-red-500 text-center mb-4 font-medium text-sm sm:text-base">
          {error}
        </p>
      )}

      <div className="space-y-3">
        {/* Email */}
        <input
          type="email"
          placeholder="Email"
          className="w-full p-2.5 sm:p-3 border rounded-lg focus:ring-2 focus:ring-teal-500 text-sm sm:text-base"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        {/* Password */}
        <input
          type="password"
          placeholder="Password"
          className="w-full p-2.5 sm:p-3 border rounded-lg focus:ring-2 focus:ring-teal-500 text-sm sm:text-base"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>

      {/* Login button */}
      <button
        type="submit"
        className="mt-6 w-full bg-teal-700 text-white p-2.5 sm:p-3 rounded-xl font-semibold hover:bg-teal-800 transition shadow-md hover:shadow-lg text-sm sm:text-base"
      >
        Login
      </button>

      {/* Switch to Register */}
      <p className="mt-4 text-center text-xs sm:text-sm">
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
