
export interface User {
  userId: number;              // ID عام للمستخدم (من API)
  id?: number;                 // ← أضفته (لو الـ API يرجع id عام)
  studentId?: number;          // ← أضفته (لو الطالب عنده studentId)
  name: string;
  email: string;
  role: "student" | "supervisor";
  department?: string;
  status?: boolean;  // true إذا عنده فريق
  token?: string;    // يرجع من /login
  team?: Team;
  password?: string; // إذا تحتاجه وقت التسجيل
}
export interface TeamDto {
  teamId: number;
  teamName: string;
  createdDate: string;
  memberCount: number;
  projectTitle?: string;
  supervisorName?: string;
  memberStudentIds: number[];
}

export interface Supervisor extends User {
  maxTeams: number;       // الحد الأقصى للفرق
  currentTeams: number;   // عدد الفرق المحجوزة
}

export interface Team {
  teamId: number;
  teamName: string;
  createdDate: string;
  memberCount: number;
  projectTitle: string | null;
  supervisorName: string | null;
  supervisorId: number | null;
  memberStudentIds: number[];
  projectDescription?: string | null;
  project?: Assignment;
}

export interface ProjectFile {
  id: number;
  title: string;
  url: string;
  studentId: number;
  teamId: number;
  studentName?: string; // اسم الطالب اللي رفع الملف
  date?: string;        // تاريخ الرفع
}

export interface Assignment {
  id :number;
  projectTitle: string;
  description: string;
  supervisorId: number;
  teamId: number;
  isCompleted: boolean;
}
export interface ProjectFile {
  id: number;
  title: string;
  url: string;
  date?: string;
  studentId: number;
  teamId: number;
}

export interface Feedback {
  feedbackId: number;
  content: string;
  date: string;
  supervisorId: number;
  supervisorName: string;
  teamId: number;
  projectName?: string;
  replies?: Reply[];
}

export interface Reply {
  id: number;
  content: string;
  date: string;
  authorId: number;
  authorName: string;
  authorRole: "student" | "supervisor";
  supervisorName ?: string;
  studentName?: string;

}


// Supervisor الآن امتداد من User لضمان وجود كل الخصائص المطلوبة
// export interface Supervisor extends User {
//   role: "supervisor";  // override role
//   studentId?: never;
//   department?: never;
//   status?: never;
//   team?: never;
//   maxTeams?: number;
//   currentTeams?: number;
// }

// export interface Team {
//   teamId: string;
//   teamName: string;
//   leaderId: string;
//   members: string[];
//   projectTitle?: string;
//   projectDescription?: string;
// }

// export interface Assignment {
//   id: string;
//   teamId: string;
//   teamName: string;
//   members: string[];
//   supervisorName: string;
//   projectTitle: string;
//   projectDescription: string;
//   status?: "todo" | "in-progress" | "done"; // أضفنا هذا
// }

// export interface Reply {
//   id: string;
//   authorId: string;
//   authorName: string;
//   authorRole: "student" | "supervisor"; // مين كاتب الرد
//   message: string;
//   date: string;
// }

// export interface Feedback {
//   id: string;
//   projectId: string;
//   projectName: string;  
//   supervisorId: string;
//   studentId: string;
//   message: string;
//   date: string;
//   supervisorName: string;
//   replies?: Reply[];
// // }
// export interface ProjectFile {
//   id: string;
//   projectId: string;
//   projectName: string;
//   uploaderId: string;
//   uploaderName: string;
//   uploaderRole: "student" | "supervisor";
//   fileName: string;
//   fileUrl: string;
//   date: string;
// }
//  export interface ProjectDisplay {
//   id: string;
//   title: string;
//   teamName: string;
//   description?: string; 
//   members: string[];
// }
// export interface Task {
//   id: string;
//   title: string;
//   description: string;
//   priority: "high" | "medium" | "low";
//   dueDate: string;
//   createdBy: string;
//   status: string;
//   assignees: string[];
//   files?: { id: string; fileName: string; fileUrl: string }[]; // إذا بدك تحتفظ بملفات لكل مهمة

//   // خصائص مؤقتة للرفع
//   showUpload?: boolean;
//   newFileName?: string;
//   newFileUrl?: string;

// }


export interface Task {
  id: string;
  title: string;
  description: string;
  priority: "high" | "medium" | "low";
  dueDate?: string;
  status: string;
  createdBy?: string;
  assignees?: string[];
}


export interface Column {
  id: string;
  title: string;
  color: string;
  tasks: Task[];
}


// export interface Project {
//   id: number;
//   title: string;
//   description: string;
//   student: string;
//   supervisor: string;
//   year: number;
//   grade: string;
//   category: string;
//   tags: string[];
//   image: string;
//   abstract: string;
//   technologies: string[];
// }


