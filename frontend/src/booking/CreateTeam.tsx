// ========================================
// CreateTeam.tsx
// Feature: Students can create a new team
// ========================================

import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useAuth } from "../context/AuthContext";
import { User, Team } from "../types/types";

export default function CreateTeam() {
  const { user, login } = useAuth();
  const navigate = useNavigate();
  useEffect(() => {
    if (!user || !user.studentId) return;
    if (user.status) navigate("/dashboard/student");

    const storedUsers: User[] = JSON.parse(localStorage.getItem("users") || "[]");
    const availableStudents = storedUsers.filter(
      (u) => u.role === "student" && !u.status && u.studentId !== user.studentId
    );
    setStudents(availableStudents);
  }, [user, navigate]);

  // ----------------------------------------
  // feat: implement member selection
  // اختيار/إلغاء اختيار أعضاء الفريق
  // ----------------------------------------
  const toggleMember = (studentId: string) => {
    setSelectedMembers((prev) =>
      prev.includes(studentId) ? prev.filter((id) => id !== studentId) : [...prev, studentId]
    );
  };}
