import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";


export default function EditProfile() {
  const { user, setUser } = useAuth();
  const navigate = useNavigate();

  const [name, setName] = useState(user?.name ?? "");
  const [email, setEmail] = useState(user?.email ?? "");
  const [department, setDepartment] = useState(user?.department ?? "");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");

  if (!user) return null;

  const isStudent = user.role.toLowerCase() === "student";
  const API_URL = import.meta.env.VITE_API_URL;

  const handleSave = async () => {
    try {
      // تحقق من كلمة السر الحالية فقط إذا تم إدخال كلمة جديدة
      if (newPassword && currentPassword !== user.password) {
        Swal.fire({
          icon: "error",
          title: "Wrong Password",
          text: "The current password is incorrect.",
        });
        return;
      }

      // إنشاء payload كامل مع القيم الحالية لأي حقل لم يتم تغييره
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const payload: any = {
        name: name || user.name,
        email: email || user.email,
        password: newPassword || user.password,
      };
      if (isStudent) payload.department = department || user.department;

      console.log("Payload to send:", payload);

      const endpoint = isStudent
        ? `${API_URL}/api/Students/${user.userId}`
        : `${API_URL}/api/Supervisors/${user.userId}`;

      const res = await fetch(endpoint, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${user.token}`,
        },
        body: JSON.stringify(payload),
      });

      const data = await res.json();
      console.log("📥 Response from API:", data);

      if (!res.ok) throw new Error(data.message || "Failed to update profile");

      // تحديث البيانات في الـ context
      setUser!({ ...user, ...payload });

      Swal.fire({
        icon: "success",
        title: "Profile Updated",
        text: "Your profile has been updated successfully",
        confirmButtonColor: "green",
      });

      setCurrentPassword("");
      setNewPassword("");
    } catch (err) {
      Swal.fire({
        icon: "error",
        title: "Update Failed",
        text: "Something went wrong while updating profile.",
      });
      console.error("❌ Error updating profile:", err);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <div className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-lg">
        <div className="flex items-center mb-6">
          <div
            className="mr-3 cursor-pointer text-black hover:text-gray-700 text-xl font-bold"
            onClick={() =>
              navigate(
                isStudent
                  ? "/dashboard/student/KanbanBoard"
                  : "/dashboard/supervisor"
              )
            }
          >
            ←
          </div>
          <h2 className="text-2xl font-bold text-gray-800">Edit Profile</h2>
        </div>

        <label className="block mb-3">
          <span className="text-gray-700">Name</span>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="mt-1 block w-full px-3 py-2 border rounded-lg"
          />
        </label>

        <label className="block mb-3">
          <span className="text-gray-700">Email</span>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="mt-1 block w-full px-3 py-2 border rounded-lg"
          />
        </label>

        {isStudent && (
          <label className="block mb-3">
            <span className="text-gray-700">Department</span>
            <input
              type="text"
              value={department}
              onChange={(e) => setDepartment(e.target.value)}
              className="mt-1 block w-full px-3 py-2 border rounded-lg"
            />
          </label>
        )}

        <label className="block mb-3">
          <span className="text-gray-700">Current Password</span>
          <input
            type="password"
            value={currentPassword}
            onChange={(e) => setCurrentPassword(e.target.value)}
            placeholder="Enter current password"
            className="mt-1 block w-full px-3 py-2 border rounded-lg"
          />
        </label>

        <label className="block mb-3">
          <span className="text-gray-700">New Password</span>
          <input
            type="password"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            placeholder="Leave blank to keep current password"
            className="mt-1 block w-full px-3 py-2 border rounded-lg"
          />
        </label>

        <button
          onClick={handleSave}
          className="bg-black text-white px-4 py-2 rounded-lg hover:bg-gray-800 w-full"
        >
          Save Changes
        </button>
      </div>
    </div>
  );
}
