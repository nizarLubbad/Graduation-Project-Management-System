
export interface User {
  userId: number;              
  id?: number;                
  studentId?: number;        
  name: string;
  email: string;
  role: "student" | "supervisor";
  department?: string;
  status?: boolean;  
  token?: string;    
  team?: Team;
  password?: string; 
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
  maxTeams: number;      
  currentTeams: number; 
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
  studentName?: string;
  date?: string;        
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




