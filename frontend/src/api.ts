// src/apiService.ts
import axios from "axios";

const api = axios.create({
  baseURL: "https://backendteam-001-site1.qtempurl.com", // ✅ هذا هو الـ Base URL
  headers: {
    "Content-Type": "application/json",
  },
});

//  توكن لو في JWT (بعد تسجيل الدخول)
// تقدر تضيفه هنا أو في interceptor:
// api.interceptors.request.use((config) => {
//   const token = localStorage.getItem("token");
//   if (token) config.headers.Authorization = `Bearer ${token}`;
//   return config;
// });

export default api;
