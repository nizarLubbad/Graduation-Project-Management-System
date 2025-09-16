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
import { Task, Column, User } from "../types/types";
import { useAuth } from "../context/AuthContext";

interface KanbanProps {
  teamId: string;
}

const initialData: Column[] = [
  { id: "todo", title: "To Do", color: "bg-gray-100", tasks: [] },
  { id: "in-progress", title: "In Progress", color: "bg-blue-100", tasks: [] },
  { id: "review", title: "Review", color: "bg-yellow-100", tasks: [] },
  { id: "done", title: "Done", color: "bg-green-100", tasks: [] },
];

export function KanbanBoard({ teamId }: KanbanProps) {
  const { user } = useAuth();
  const storageKey = `kanbanTasks_${teamId}`;
  const [columns, setColumns] = useState<Column[]>(initialData);
  const [newTask, setNewTask] = useState<{ [key: string]: Partial<Task> }>({});
  const [showForm, setShowForm] = useState<{ [key: string]: boolean }>({});
  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [teamMembersData, setTeamMembersData] = useState<
    { id: string; name: string }[]
  >([]);

  const isSupervisor = user?.role === "supervisor"; // 🔑 تحديد إذا كان مشرف

  // Fetch team members
  useEffect(() => {
    if (!user?.team?.members) return;
    const storedUsers: User[] = JSON.parse(
      localStorage.getItem("users") || "[]"
    );
    const membersData = user.team.members.map((id) => {
      const member = storedUsers.find((u) => u.studentId === id || u.id === id);
      return { id, name: member?.name || `Unknown (${id})` };
    });
    setTeamMembersData(membersData);
  }, [user?.team?.members]);

  // Fetch tasks
  useEffect(() => {
    const stored = localStorage.getItem(storageKey);
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        if (Array.isArray(parsed)) setColumns(parsed);
        else setColumns(initialData);
      } catch {
        setColumns(initialData);
      }
    }
  }, [storageKey]);

  const saveToStorage = (cols: Column[]) => {
    localStorage.setItem(storageKey, JSON.stringify(cols));
  };

  const addTask = (colId: string) => {
    if (!user || !newTask[colId]?.title) return;
    const task: Task = {
      id: Date.now().toString(),
      title: newTask[colId]?.title || "Untitled Task",
      description: newTask[colId]?.description || "",
      priority: newTask[colId]?.priority || "medium",
      dueDate:
        newTask[colId]?.dueDate || new Date().toISOString().split("T")[0],
      createdBy: user.name,
      status: colId,
      assignees: newTask[colId]?.assignees || [],
    };
    const updatedColumns = columns.map((c) =>
      c.id === colId ? { ...c, tasks: [...c.tasks, task] } : c
    );
    setColumns(updatedColumns);
    saveToStorage(updatedColumns);
    setNewTask({ ...newTask, [colId]: {} });
    setShowForm({ ...showForm, [colId]: false });
    Swal.fire("Added!", "Task has been added.", "success");
  };

  const deleteTask = (colId: string, taskId: string) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
    }).then((result) => {
      if (result.isConfirmed) {
        const updatedColumns = columns.map((c) =>
          c.id === colId
            ? { ...c, tasks: c.tasks.filter((t) => t.id !== taskId) }
            : c
        );
        setColumns(updatedColumns);
        saveToStorage(updatedColumns);
        Swal.fire("Deleted!", "Task has been deleted.", "success");
      }
    });
  };

  const saveEdit = (colId: string) => {
    if (!editingTask) return;
    const updatedColumns = columns.map((col) =>
      col.id === colId
        ? {
            ...col,
            tasks: col.tasks.map((t) =>
              t.id === editingTask.id ? editingTask : t
            ),
          }
        : col
    );
    setColumns(updatedColumns);
    saveToStorage(updatedColumns);
    setEditingTask(null);
    Swal.fire("Updated!", "Task has been updated.", "success");
  };

  const handleDragEnd = (result: DropResult) => {
    if (isSupervisor) return; // 🔒 منع المشرف من السحب والإفلات
    const { source, destination } = result;
    if (!destination) return;
    const sourceCol = columns.find((c) => c.id === source.droppableId)!;
    const destCol = columns.find((c) => c.id === destination.droppableId)!;
    const sourceTasks = Array.from(sourceCol.tasks);
    const destTasks = Array.from(destCol.tasks);
    const [moved] = sourceTasks.splice(source.index, 1);
    moved.status = destCol.id;
    destTasks.splice(destination.index, 0, moved);
    const updatedColumns = columns.map((c) => {
      if (c.id === sourceCol.id) return { ...c, tasks: sourceTasks };
      if (c.id === destCol.id) return { ...c, tasks: destTasks };
      return c;
    });
    setColumns(updatedColumns);
    saveToStorage(updatedColumns);
  };

  const getPriorityColor = (priority: "high" | "medium" | "low") => {
    if (priority === "high") return "bg-red-200 text-red-800";
    if (priority === "medium") return "bg-yellow-200 text-yellow-800";
    return "bg-green-200 text-green-800";
  };

  const renderAssigneesCheckbox = (
    colId: string,
    taskAssignees: string[] = []
  ) => {
    if (isSupervisor) return null; // 🔒 المشرف ما بيوزع مهام
    return (
      <div className="mt-2">
        <label className="font-semibold mb-1 block">Assign Members:</label>
        <div className="flex flex-col gap-1 max-h-40 overflow-y-auto border p-2 rounded">
          {teamMembersData.map((member) => {
            const checked = taskAssignees.includes(member.id);
            return (
              <label key={member.id} className="flex items-center gap-2">
                <input
                  type="checkbox"
                  checked={checked}
                  onChange={(e) => {
                    let newAssignees: string[];
                    if (e.target.checked)
                      newAssignees = [...taskAssignees, member.id];
                    else
                      newAssignees = taskAssignees.filter(
                        (id) => id !== member.id
                      );

                    if (editingTask)
                      setEditingTask({
                        ...editingTask,
                        assignees: newAssignees,
                      });
                    else
                      setNewTask({
                        ...newTask,
                        [colId]: { ...newTask[colId], assignees: newAssignees },
                      });
                  }}
                />
                {member.name}
              </label>
            );
          })}
        </div>
      </div>
    );
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
                className={`${col.color} rounded-lg p-4 shadow-sm min-h-[350px] flex flex-col`}
              >
                <h2 className="font-semibold mb-3 text-center">{col.title}</h2>

                {col.tasks.map((task, index) => (
                  <Draggable
                    key={task.id}
                    draggableId={task.id}
                    index={index}
                    isDragDisabled={isSupervisor}
                  >
                    {(provided) => (
                      <div
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}
                        className="mb-2"
                      >
                        <Card
                          className={`p-3 rounded-md shadow-sm ${getPriorityColor(
                            task.priority
                          )}`}
                        >
                          {editingTask?.id === task.id ? (
                            !isSupervisor && (
                              <div className="flex flex-col gap-2">
                                <Input
                                  value={editingTask.title}
                                  onChange={(e) =>
                                    setEditingTask({
                                      ...editingTask,
                                      title: e.target.value,
                                    })
                                  }
                                  placeholder="Task Title"
                                />
                                <Textarea
                                  value={editingTask.description}
                                  onChange={(e) =>
                                    setEditingTask({
                                      ...editingTask,
                                      description: e.target.value,
                                    })
                                  }
                                  placeholder="Description"
                                />
                                <Input
                                  type="date"
                                  value={
                                    editingTask.dueDate?.split("T")[0] || ""
                                  }
                                  onChange={(e) =>
                                    setEditingTask({
                                      ...editingTask,
                                      dueDate: e.target.value,
                                    })
                                  }
                                />
                                <select
                                  value={editingTask.priority}
                                  onChange={(e) =>
                                    setEditingTask({
                                      ...editingTask,
                                      priority: e.target.value as
                                        | "high"
                                        | "medium"
                                        | "low",
                                    })
                                  }
                                  className="border p-1 rounded"
                                >
                                  <option value="high">High 🔴</option>
                                  <option value="medium">Medium 🟡</option>
                                  <option value="low">Low 🟢</option>
                                </select>
                                {renderAssigneesCheckbox(
                                  col.id,
                                  editingTask.assignees
                                )}
                                <div className="flex gap-2">
                                  <Button
                                    size="sm"
                                    onClick={() => saveEdit(col.id)}
                                  >
                                    Save
                                  </Button>
                                  <Button
                                    size="sm"
                                    variant="secondary"
                                    onClick={() => setEditingTask(null)}
                                  >
                                    Cancel
                                  </Button>
                                </div>
                              </div>
                            )
                          ) : (
                            <>
                              <p className="font-medium">{task.title}</p>
                              <p className="text-sm text-gray-600">
                                {task.description}
                              </p>

                              {/* Assignees */}
                              <div className="flex flex-wrap mt-1 gap-1">
                                {task.assignees.map((id) => {
                                  const member = teamMembersData.find(
                                    (m) => m.id === id
                                  );
                                  return (
                                    <span
                                      key={id}
                                      className="inline-block bg-teal-200 text-teal-800 px-2 py-1 rounded-full text-xs"
                                    >
                                      {member?.name || id}
                                    </span>
                                  );
                                })}
                              </div>

                              {/* أزرار التعديل والحذف - الطلاب فقط */}
                              {!isSupervisor && (
                                <div className="flex gap-2 mt-2">
                                  <Button
                                    size="icon"
                                    variant="ghost"
                                    onClick={() => setEditingTask(task)}
                                  >
                                    Edit
                                  </Button>
                                  <Button
                                    size="icon"
                                    variant="ghost"
                                    onClick={() => deleteTask(col.id, task.id)}
                                  >
                                    Delete
                                  </Button>
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

                {/* زر الإضافة - الطلاب فقط */}
                {!isSupervisor && !showForm[col.id] && (
                  <Button
                    className="mt-3"
                    onClick={() => setShowForm({ ...showForm, [col.id]: true })}
                  >
                    Add Task
                  </Button>
                )}

                {/* الفورم - الطلاب فقط */}
                {!isSupervisor && showForm[col.id] && (
                  <div className="mt-3 flex flex-col gap-2 border-t pt-2">
                    <Input
                      placeholder="Task title..."
                      value={newTask[col.id]?.title || ""}
                      onChange={(e) =>
                        setNewTask({
                          ...newTask,
                          [col.id]: {
                            ...newTask[col.id],
                            title: e.target.value,
                          },
                        })
                      }
                    />
                    <Textarea
                      placeholder="Description..."
                      value={newTask[col.id]?.description || ""}
                      onChange={(e) =>
                        setNewTask({
                          ...newTask,
                          [col.id]: {
                            ...newTask[col.id],
                            description: e.target.value,
                          },
                        })
                      }
                    />
                    <Input
                      type="date"
                      value={newTask[col.id]?.dueDate || ""}
                      onChange={(e) =>
                        setNewTask({
                          ...newTask,
                          [col.id]: {
                            ...newTask[col.id],
                            dueDate: e.target.value,
                          },
                        })
                      }
                    />

                    {renderAssigneesCheckbox(
                      col.id,
                      newTask[col.id]?.assignees
                    )}
                    <select
                      value={newTask[col.id]?.priority || "medium"}
                      onChange={(e) =>
                        setNewTask({
                          ...newTask,
                          [col.id]: {
                            ...newTask[col.id],
                            priority: e.target.value as
                              | "high"
                              | "medium"
                              | "low",
                          },
                        })
                      }
                      className="border p-1 rounded"
                    >
                      <option value="high">High 🔴</option>
                      <option value="medium">Medium 🟡</option>
                      <option value="low">Low 🟢</option>
                    </select>
                    <div className="flex gap-2 mt-2">
                      <Button onClick={() => addTask(col.id)}>Add Task</Button>
                      <Button
                        variant="secondary"
                        onClick={() =>
                          setShowForm({ ...showForm, [col.id]: false })
                        }
                      >
                        Cancel
                      </Button>
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
