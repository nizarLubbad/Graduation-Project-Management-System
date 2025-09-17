import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User } from "../types/types";

export default function EditProfile() {
  const { user, login } = useAuth();
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [department, setDepartment] = useState("");
  const [status, setStatus] = useState(false);

  useEffect(() => {
    if (user) {
      setName(user.name || "");
      setEmail(user.email || "");
      setDepartment(user.department || "");
      setStatus(user.status || false);
    }
  }, [user]);

  const handleSave = () => {
    if (!user) return;

    // تحديث قائمة المستخدمين في localStorage
    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const updatedUsers = storedUsers.map(u =>
      u.id === user.id ? { ...u, name, email, department, status } : u
    );

    localStorage.setItem("users", JSON.stringify(updatedUsers));

    // تحديث الـ context
    login({ ...user, name, email, department, status });

    Swal.fire({
      icon: "success",
      title: "Profile Updated",
      text: "Your profile has been updated successfully!",
      confirmButtonText: "OK"
    });
  };

  return (
    <div className="p-6 max-w-xl mx-auto bg-white shadow-lg rounded-xl">
      <h2 className="text-2xl font-bold text-teal-700 mb-4">Edit Profile</h2>

      <div className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Name</label>
          <input
            type="text"
            value={name}
            onChange={e => setName(e.target.value)}
            className="w-full p-2 border rounded mt-1"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Email</label>
          <input
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            className="w-full p-2 border rounded mt-1"
          />
        </div>

        {user?.role === "student" && (
          <div>
            <label className="block text-sm font-medium text-gray-700">Department</label>
            <input
              type="text"
              value={department}
              onChange={e => setDepartment(e.target.value)}
              className="w-full p-2 border rounded mt-1"
            />
          </div>
        )}

        <div className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={status}
            onChange={e => setStatus(e.target.checked)}
          />
          <label className="text-sm text-gray-700">Active Status</label>
        </div>

        <button
          onClick={handleSave}
          className="bg-teal-600 text-white px-4 py-2 rounded w-full hover:bg-teal-700"
        >
          Save Changes
        </button>
      </div>
    </div>
  );
}
