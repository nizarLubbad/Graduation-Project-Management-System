import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

interface StudentRouterProps {
  children: React.ReactNode;
}

export default function StudentRouter({ children }: StudentRouterProps) {
  const { user } = useAuth();

  // لو ما في مستخدم، روح للصفحة الرئيسية
  if (!user) return <Navigate to="/" />;

  // حول role للسمول وقارن
  if ((user.role || "").toLowerCase() !== "student") return <Navigate to="/" />;

  // لو ما في فريق، روح لإنشاء الفريق
  if (!user.team) return <Navigate to="/create-team" />;

  return <>{children}</>;
}
