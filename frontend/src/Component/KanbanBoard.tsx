/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState, useEffect } from "react";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  DragDropContext,
  Droppable,
  Draggable,
  DropResult,
} from "@hello-pangea/dnd";
import Swal from "sweetalert2";
import { Task, Column } from "../types/types";
import { useAuth } from "../context/AuthContext";
import api from "../api";

interface KanbanProps {
  teamId: string;
}

// الأعمدة تبقى strings لكن نستخدم map للـ status من API
const initialData: Column[] = [
  { id: "ToDo", title: "To Do", color: "bg-gray-100", tasks: [] },
  { id: "Doing", title: "In Progress", color: "bg-blue-100", tasks: [] },
  { id: "Done", title: "Done", color: "bg-green-100", tasks: [] },
];

const statusMap: { [key: number]: string } = { 1: "ToDo", 2: "Doing", 3: "Done" };
const priorityMap: { [key: number]: "low" | "medium" | "high" } = { 1: "low", 2: "medium", 3: "high" };
const priorityNumberMap: { [key: string]: number } = { low: 1, medium: 2, high: 3 };

export function KanbanBoard({ teamId }: KanbanProps) {
  const { user } = useAuth();
  const [columns, setColumns] = useState<Column[]>(initialData);
  const [newTask, setNewTask] = useState<{ [key: string]: Partial<Task> }>({});
  const [showForm, setShowForm] = useState<{ [key: string]: boolean }>({});
  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [teamMembersData, setTeamMembersData] = useState<{ id: number; name: string }[]>([]);

  const isSupervisor = user?.role.toLowerCase() === "supervisor";

  // --- جلب أعضاء الفريق ---
  useEffect(() => {
    const fetchTeamMembers = async () => {
      if (!user?.team?.memberStudentIds) return;

      try {
        const res = await api.get("/api/Students/all");
        console.log("All students from API:", res.data.students);

        const membersData = user.team.memberStudentIds.map((id: number) => {
          const member = res.data.students.find((s: any) => s.userId === id);
          console.log(`Mapping memberId ${id} to`, member);
          return { id, name: member?.name || `Unknown (${id})` };
        });

        setTeamMembersData(membersData);
        console.log("Final teamMembersData:", membersData);
      } catch (err) {
        console.error("❌ Error fetching team members:", err);
      }
    };

    fetchTeamMembers();
  }, [user?.team?.memberStudentIds]);

  // --- جلب المهام ---
  const fetchTasks = async () => {
    try {
      const res = await api.get(`/api/KanbanTask/team/${teamId}`);
      const tasks = res.data;
      console.log("Fetched tasks from API:", tasks);

      const updated = initialData.map((col) => {
        const colTasks = tasks
          .filter((t: any) => statusMap[t.status] === col.id)
          .map((t: any) => ({
            id: t.id.toString(),
            title: t.title,
            description: t.description,
            priority: priorityMap[t.priority] || "medium",
            dueDate: t.dueDate,
            status: statusMap[t.status],
            assignees: t.assignedStudentNames || [],
          }));

        console.log(`Tasks for column ${col.id}:`, colTasks);
        return { ...col, tasks: colTasks };
      });

      setColumns(updated);
      console.log("Tasks after fetch:", updated);
    } catch (err) {
      console.error("❌ Error fetching tasks:", err);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [teamId]);

  // --- إضافة مهمة ---
  const addTask = async (colId: string) => {
    if (!newTask[colId]?.title) return;

    try {
      console.log("Adding task:", newTask[colId]);

      const res = await api.post("/api/KanbanTask", {
        title: newTask[colId]?.title,
        description: newTask[colId]?.description,
        teamId: Number(teamId),
        dueDate: newTask[colId]?.dueDate,
        priority: priorityNumberMap[newTask[colId]?.priority || "medium"],
        assignedStudentNames: newTask[colId]?.assignees || [],
      });

      console.log("API response after adding task:", res.data);

      await fetchTasks();
      setNewTask({ ...newTask, [colId]: {} });
      setShowForm({ ...showForm, [colId]: false });

      Swal.fire("Added!", "Task has been added.", "success");
    } catch (err) {
      console.error("❌ Error adding task:", err);
      Swal.fire("Error", "Failed to add task.", "error");
    }
  };

  // --- تعديل مهمة ---
  const saveEdit = async () => {
    if (!editingTask) return;

    try {
      console.log("Saving edited task:", editingTask);

      await api.put(`/api/KanbanTask/${editingTask.id}`, {
        title: editingTask.title,
        description: editingTask.description,
        dueDate: editingTask.dueDate,
        priority: priorityNumberMap[editingTask.priority || "medium"],
        teamId: Number(teamId),
        assignedStudentNames: editingTask.assignees,
      });

      await fetchTasks();
      setEditingTask(null);
      Swal.fire("Updated!", "Task has been updated.", "success");
    } catch (err) {
      console.error("❌ Error updating task:", err);
      Swal.fire("Error", "Failed to update task.", "error");
    }
  };

  // --- حذف مهمة ---
  const deleteTask = async (colId: string, taskId: string) => {
    Swal.fire({
      title: "Are you sure?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          await api.delete(`/api/KanbanTask/${taskId}`);
          await fetchTasks();
          Swal.fire("Deleted!", "Task has been deleted.", "success");
        } catch {
          Swal.fire("Error", "Failed to delete task.", "error");
        }
      }
    });
  };

  // --- سحب وإفلات ---
  const handleDragEnd = async (result: DropResult) => {
    const { source, destination } = result;
    if (!destination) return;
  
    console.log("Drag result:", result);
  
    const sourceColIndex = columns.findIndex(c => c.id === source.droppableId);
    const destColIndex = columns.findIndex(c => c.id === destination.droppableId);
    const sourceCol = columns[sourceColIndex];
    const destCol = columns[destColIndex];
    const task = sourceCol.tasks[source.index];
  
    if (source.droppableId === destination.droppableId) {
      const updatedTasks = Array.from(sourceCol.tasks);
      updatedTasks.splice(source.index, 1);
      updatedTasks.splice(destination.index, 0, task);
      const newColumns = [...columns];
      newColumns[sourceColIndex] = { ...sourceCol, tasks: updatedTasks };
      setColumns(newColumns);
      return;
    }
  
    const sourceTasks = Array.from(sourceCol.tasks);
    sourceTasks.splice(source.index, 1);
    const destTasks = Array.from(destCol.tasks);
    destTasks.splice(destination.index, 0, task);
  
    const newColumns = [...columns];
    newColumns[sourceColIndex] = { ...sourceCol, tasks: sourceTasks };
    newColumns[destColIndex] = { ...destCol, tasks: destTasks };
    setColumns(newColumns);
  
    try {
      // تحويل اسم العمود إلى رقم مباشرة لإرسال الـ status
      const statusNumberMap: { [key: string]: number } = { ToDo: 1, Doing: 2, Done: 3 };
      const newStatus = statusNumberMap[destination.droppableId];
  
      console.log(`Updating task ${task.id} status to number:`, newStatus);
  
      await api.patch("/api/KanbanTask/status", {
        taskId: Number(task.id),
        status: newStatus, // نرسل الرقم مباشرة
      });
    } catch (err) {
      console.error("❌ Error moving task:", err);
      Swal.fire("Error", "Failed to move task.", "error");
    }
  };
  

  // --- ألوان الأولوية ---
  const getPriorityColor = (priority: "low" | "medium" | "high") => {
    if (priority === "high") return "bg-red-200 text-red-800";
    if (priority === "medium") return "bg-yellow-200 text-yellow-800";
    return "bg-green-200 text-green-800";
  };

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <div className="grid sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 p-4">
        {columns.map((col) => (
          <Droppable key={col.id} droppableId={col.id}>
            {(provided) => (
              <div
                ref={provided.innerRef}
                {...provided.droppableProps}
                className={`${col.color} rounded-lg p-4 shadow-sm min-h-[350px]`}
              >
                <h2 className="font-semibold mb-3 text-center">{col.title}</h2>

                {col.tasks.map((task, index) => (
                  <Draggable key={task.id} draggableId={task.id} index={index} isDragDisabled={isSupervisor}>
                    {(provided) => (
                      <div ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps} className="mb-2">
                        <Card className={`p-3 rounded-md shadow-sm ${getPriorityColor(task.priority)}`}>
                          {editingTask?.id === task.id ? (
                            <div className="flex flex-col gap-2">
                              <Input value={editingTask.title} onChange={(e) => setEditingTask({ ...editingTask, title: e.target.value })} />
                              <Textarea value={editingTask.description} onChange={(e) => setEditingTask({ ...editingTask, description: e.target.value })} />
                              <div className="flex flex-col gap-1">
                                <span className="font-semibold text-sm">Assignees:</span>
                                {teamMembersData.map((member) => (
                                  <label key={member.id} className="flex items-center gap-1 text-sm">
                                    <input
                                      type="checkbox"
                                      checked={editingTask.assignees?.includes(member.name) || false}
                                      onChange={(e) => {
                                        const newAssignees = e.target.checked
                                          ? [...(editingTask.assignees || []), member.name]
                                          : (editingTask.assignees || []).filter((n) => n !== member.name);
                                        setEditingTask({ ...editingTask, assignees: newAssignees });
                                      }}
                                    />
                                    {member.name}
                                  </label>
                                ))}
                              </div>
                              <input type="date" value={editingTask.dueDate || ""} onChange={(e) => setEditingTask({ ...editingTask, dueDate: e.target.value })} />
                              <select value={editingTask.priority || "medium"} onChange={(e) => setEditingTask({ ...editingTask, priority: e.target.value as "low" | "medium" | "high" })}>
                                <option value="low">Low</option>
                                <option value="medium">Medium</option>
                                <option value="high">High</option>
                              </select>
                              <div className="flex gap-2 mt-2">
                                <Button size="sm" onClick={saveEdit}>Save</Button>
                                <Button size="sm" variant="secondary" onClick={() => setEditingTask(null)}>Cancel</Button>
                              </div>
                            </div>
                          ) : (
                            <>
                              <p className="font-medium">{task.title}</p>
                              <p className="text-sm text-gray-600">{task.description}</p>
                              <p className="text-xs text-gray-500">Assignees: {task.assignees?.join(", ") || "None"}</p>
                              <p className="text-xs text-gray-500">Due: {task.dueDate || "N/A"}</p>
                              {!isSupervisor && (
                                <div className="flex gap-2 mt-2">
                                  <Button size="sm" variant="ghost" onClick={() => setEditingTask(task)}>Edit</Button>
                                  <Button size="sm" variant="ghost" onClick={() => deleteTask(col.id, task.id)}>Delete</Button>
                                </div>
                              )}
                            </>
                          )}
                        </Card>
                      </div>
                    )}
                  </Draggable>
                ))}

                {provided.placeholder}

                {!isSupervisor && !showForm[col.id] && (
                  <Button className="mt-3" onClick={() => setShowForm({ ...showForm, [col.id]: true })}>Add Task</Button>
                )}

                {!isSupervisor && showForm[col.id] && (
                  <div className="mt-3 flex flex-col gap-2 border-t pt-2">
                    <Input placeholder="Task title..." value={newTask[col.id]?.title || ""} onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], title: e.target.value } })} />
                    <Textarea placeholder="Description..." value={newTask[col.id]?.description || ""} onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], description: e.target.value } })} />
                    <div className="flex flex-col gap-1">
                      <span className="font-semibold text-sm">Assignees:</span>
                      {teamMembersData.map((member) => (
                        <label key={member.id} className="flex items-center gap-1 text-sm">
                          <input
                            type="checkbox"
                            checked={newTask[col.id]?.assignees?.includes(member.name) || false}
                            onChange={(e) => {
                              const selected = newTask[col.id]?.assignees || [];
                              const newAssignees = e.target.checked ? [...selected, member.name] : selected.filter((n) => n !== member.name);
                              setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], assignees: newAssignees } });
                            }}
                          />
                          {member.name}
                        </label>
                      ))}
                    </div>
                    <input type="date" value={newTask[col.id]?.dueDate || ""} onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], dueDate: e.target.value } })} />
                    <select value={newTask[col.id]?.priority || "medium"} onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], priority: e.target.value as "low" | "medium" | "high" } })}>
                      <option value="low">Low</option>
                      <option value="medium">Medium</option>
                      <option value="high">High</option>
                    </select>
                    <div className="flex gap-2 mt-2">
                      <Button onClick={() => addTask(col.id)}>Add Task</Button>
                      <Button variant="secondary" onClick={() => setShowForm({ ...showForm, [col.id]: false })}>Cancel</Button>
                    </div>
                  </div>
                )}
              </div>
            )}
          </Droppable>
        ))}
      </div>
    </DragDropContext>
  );
}
