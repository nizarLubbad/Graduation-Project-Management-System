export interface User {
  id: string;
  name: string;
  email: string;
  password: string;
  role: "student" | "supervisor";
  studentId?: string;
  department?: string;
  status?: boolean;
  team?: { id: string; name: string ; members: string[] };
}

// Supervisor الآن امتداد من User لضمان وجود كل الخصائص المطلوبة
export interface Supervisor extends User {
  role: "supervisor";  // override role
  studentId?: never;
  department?: never;
  status?: never;
  team?: never;
  maxTeams?: number;
  currentTeams?: number;
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

export interface Reply {
  id: string;
  authorId: string;
  authorName: string;
  authorRole: "student" | "supervisor"; // مين كاتب الرد
  message: string;
  date: string;
}

export interface Feedback {
  id: string;
  projectId: string;
  projectName: string;  
  supervisorId: string;
  studentId: string;
  message: string;
  date: string;
  supervisorName: string;
  replies?: Reply[];
}
export interface ProjectFile {
  id: string;
  projectId: string;
  projectName: string;
  uploaderId: string;
  uploaderName: string;
  uploaderRole: "student" | "supervisor";
  fileName: string;
  fileUrl: string;
  date: string;
}
 export interface ProjectDisplay {
  id: string;
  title: string;
  teamName: string;
  description?: string; 
  members: string[];
}
export interface Task {
  id: string;
  title: string;
  description: string;
  priority: "high" | "medium" | "low";
  dueDate: string;
  createdBy: string;
  status: string;
  assignees: string[];
  files?: { id: string; fileName: string; fileUrl: string }[]; // إذا بدك تحتفظ بملفات لكل مهمة

  // خصائص مؤقتة للرفع
  showUpload?: boolean;
  newFileName?: string;
  newFileUrl?: string;

}

export interface Column {
  id: string;
  title: string;
  color: string;
  tasks: Task[];
}


export interface Project {
  id: number;
  title: string;
  description: string;
  student: string;
  supervisor: string;
  year: number;
  grade: string;
  category: string;
  tags: string[];
  image: string;
  abstract: string;
  technologies: string[];
}


