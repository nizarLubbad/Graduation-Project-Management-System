/* eslint-disable @typescript-eslint/no-explicit-any */
// src/components/EditProfile.tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import type { User } from "../types/types";

export default function EditProfile(): JSX.Element | null {
  const { user, setUser } = useAuth();
  const navigate = useNavigate();

  const [name, setName] = useState(user?.name ?? "");
  const [email, setEmail] = useState(user?.email ?? "");
  const [department, setDepartment] = useState(user?.department ?? "");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");

  if (!user) return null;

  const isStudent = user.role?.toLowerCase() === "student";
  const API_URL = import.meta.env.VITE_API_URL || "";

  const handleSave = async () => {
    try {
      // إذا المستخدم يريد يغيّر الباسورد لازم يدخل الباسورد الحالي
      if (newPassword && !currentPassword) {
        Swal.fire({
          icon: "error",
          title: "Current password required",
          text: "Please enter your current password to change it.",
        });
        return;
      }

      // بناء payload — تأكد ما نرسل undefined
      const payload: Record<string, any> = {
        name: name.trim() || user.name,
        email: email.trim() || user.email,
      };

      if (isStudent) payload.department = department.trim() || user.department;

      if (newPassword) {
        // نرسل الباسورد الجديد و currentPassword للتحقق على السيرفر
        payload.password = newPassword;
        payload.currentPassword = currentPassword;
      } else if (user.password) {
        // لو مخزون محلياً ضمن user نرسله حتى لا يفقد السيرفر قيمة الباسورد
        // (لو ما مخزون فلن نرسل password لتجنّب إرسال undefined)
        payload.password = user.password;
      }

      console.log("📤 Payload to send:", payload);

      const endpoint = isStudent
        ? `${API_URL}/api/Students/${user.userId}`
        : `${API_URL}/api/Supervisors/${user.userId}`;

      const res = await fetch(endpoint, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          ...(user.token ? { Authorization: `Bearer ${user.token}` } : {}),
        },
        body: JSON.stringify(payload),
      });

      // حاول نقرأ JSON لكن تعامل مع حالة عدم وجود body
      let data: any = null;
      try {
        data = await res.json();
      } catch (e) {
        // ممكن لا يكون هناك JSON في الاستجابة — نسمح بذلك
      console.log("⚠️ No JSON body in response", e);
      
      }
      console.log("📥 Response from API:", res.status, data);

      if (!res.ok) {
        // استخرج رسالة مفيدة من الـ response إذا كانت موجودة
        const serverMsg =
          data?.message ||
          (data && typeof data === "object" ? JSON.stringify(data) : null) ||
          `${res.status} ${res.statusText}`;
        throw new Error(serverMsg);
      }

      // حدّث الـ Context بدون حفظ currentPassword
      const updatedUser: User = {
        ...user,
        name: payload.name,
        email: payload.email,
        password: payload.password ?? user.password,
        ...(isStudent ? { department: payload.department } : {}),
      } as User;

      setUser?.(updatedUser);

      Swal.fire({
        icon: "success",
        title: "Profile Updated",
        text: "Your profile has been updated successfully",
      });

      // مسح حقول الباسورد
      setCurrentPassword("");
      setNewPassword("");
    } catch (err) {
      console.error("❌ Error updating profile:", err);
      Swal.fire({
        icon: "error",
        title: "Update Failed",
        text: err instanceof Error ? err.message : "Something went wrong while updating profile.",
      });
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <div className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-lg">
        <div className="flex items-center mb-6">
          <div
            className="mr-3 cursor-pointer text-black hover:text-gray-700 text-xl font-bold"
            onClick={() =>
              navigate(isStudent ? "/dashboard/student/KanbanBoard" : "/dashboard/supervisor")
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
            placeholder="Enter current password (required to change password)"
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
