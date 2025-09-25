import { createContext, useContext, useState, ReactNode } from "react";
import { User, Team } from "../types/types";

interface AuthContextType {
  user?: User;
  setUser?: (user: User | undefined) => void; 
  login: (user: User) => void;
  logout: () => void;
  updateUserTeam?: (team: Team) => void;
}

const AuthContext = createContext<AuthContextType>({
  login: () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | undefined>(undefined);

  const login = (u: User) => setUser(u);
  const logout = () => setUser(undefined);

  const updateUserTeam = (team: Team) => {
    if (user) setUser({ ...user, team, status: true });
  };

  return (
    <AuthContext.Provider value={{ user, setUser, login, logout, updateUserTeam }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
