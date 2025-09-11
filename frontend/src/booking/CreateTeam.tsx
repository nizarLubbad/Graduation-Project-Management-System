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
}
