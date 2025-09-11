export interface User {
    id: string;
    name: string;
    email: string;
    password: string;
    role: "student" | "supervisor";
    studentId?: string;
    department?: string;
    status?: boolean;
    team?: { id: string; name: string };
  }
  
  // Supervisor الآن امتداد من User لضمان وجود كل الخصائص المطلوبة
  export interface Supervisor extends User {
    role: "supervisor";  // override role
    studentId?: never;
    department?: never;
    status?: never;
    team?: never;
  }
  
  export interface Team {
    teamId: string;
    teamName: string;
    leaderId: string;
    members: string[];
    projectTitle?: string;
    projectDescription?: string;
  }
  
  export interface Assignment {
    id: string;
    teamId: string;
    teamName: string;
    members: string[];
    supervisorName: string;
    projectTitle: string;
    projectDescription: string;
    status?: "todo" | "in-progress" | "done"; // أضفنا هذا
  }
  
  