import { useState, useEffect } from "react"
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Pencil, Trash2, Plus, Calendar, User } from "lucide-react"
import { DragDropContext, Droppable, Draggable, DropResult } from "@hello-pangea/dnd"
import { useAuth } from "../context/AuthContext"

type Task = {
  id: string
  title: string
  description: string
  priority: "high" | "medium" | "low"
  dueDate: string
  createdBy: string
  status: string
}

type Column = {
  id: string
  title: string
  color: string
  tasks: Task[]
}

const initialData: Column[] = [
  { id: "todo", title: "To Do", color: "bg-gray-100", tasks: [] },
  { id: "in-progress", title: "In Progress", color: "bg-blue-100", tasks: [] },
  { id: "review", title: "Review", color: "bg-yellow-100", tasks: [] },
  { id: "done", title: "Done", color: "bg-green-100", tasks: [] },
]

const priorityBadge = (p: string) => {
  if (p === "high") return "bg-red-100 text-red-700 border border-red-200"
  if (p === "medium") return "bg-yellow-100 text-yellow-700 border border-yellow-200"
  return "bg-green-100 text-green-700 border border-green-200"
}

export function KanbanBoard() {
  const { user } = useAuth()
  const [columns, setColumns] = useState<Column[]>(initialData)
  const [newTask, setNewTask] = useState<{ [key: string]: Partial<Task> }>({})
  const [editingTask, setEditingTask] = useState<Task | null>(null)

  useEffect(() => {
    const stored = localStorage.getItem("kanbanTasks")
    if (stored) setColumns(JSON.parse(stored))
  }, [])

  useEffect(() => {
    localStorage.setItem("kanbanTasks", JSON.stringify(columns))
  }, [columns])

  const addTask = (colId: string) => {
    if (!newTask[colId]?.title || !user) return
    setColumns(columns.map(col =>
      col.id === colId
        ? {
            ...col,
            tasks: [
              ...col.tasks,
              {
                id: Date.now().toString(),
                title: newTask[colId]?.title || "",
                description: newTask[colId]?.description || "",
                priority: (newTask[colId]?.priority as "high" | "medium" | "low") || "medium",
                dueDate: newTask[colId]?.dueDate || new Date().toISOString().split("T")[0],
                createdBy: user.name,
                status: colId,
              },
            ],
          }
        : col
    ))
    setNewTask({ ...newTask, [colId]: {} })
  }

  const deleteTask = (colId: string, taskId: string) => {
    setColumns(columns.map(col =>
      col.id === colId ? { ...col, tasks: col.tasks.filter(t => t.id !== taskId) } : col
    ))
  }

  const saveEdit = (colId: string) => {
    if (!editingTask) return
    setColumns(columns.map(col =>
      col.id === colId
        ? { ...col, tasks: col.tasks.map(t => (t.id === editingTask.id ? editingTask : t)) }
        : col
    ))
    setEditingTask(null)
  }

  const handleDragEnd = (result: DropResult) => {
    const { source, destination } = result
    if (!destination) return

    if (source.droppableId === destination.droppableId) {
      const column = columns.find(c => c.id === source.droppableId)!
      const newTasks = Array.from(column.tasks)
      const [moved] = newTasks.splice(source.index, 1)
      newTasks.splice(destination.index, 0, moved)

      setColumns(columns.map(c => (c.id === column.id ? { ...c, tasks: newTasks } : c)))
    } else {
      const sourceCol = columns.find(c => c.id === source.droppableId)!
      const destCol = columns.find(c => c.id === destination.droppableId)!
      const sourceTasks = Array.from(sourceCol.tasks)
      const destTasks = Array.from(destCol.tasks)

      const [moved] = sourceTasks.splice(source.index, 1)
      moved.status = destCol.id
      destTasks.splice(destination.index, 0, moved)

      setColumns(columns.map(c => {
        if (c.id === sourceCol.id) return { ...c, tasks: sourceTasks }
        if (c.id === destCol.id) return { ...c, tasks: destTasks }
        return c
      }))
    }
  }

  return (
    <DragDropContext onDragEnd={handleDragEnd}>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 p-4">
        {columns.map(col => (
          <Droppable key={col.id} droppableId={col.id}>
            {(provided) => (
              <div
                ref={provided.innerRef}
                {...provided.droppableProps}
                className={`${col.color} rounded-lg p-4 shadow-sm min-h-[350px] flex flex-col`}
              >
                <h2 className="font-semibold mb-3 text-center">{col.title}</h2>

                {col.tasks.map((task, index) => (
                  <Draggable key={task.id} draggableId={task.id} index={index}>
                    {(provided, snapshot) => (
                      <div
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}
                        className={`mb-2 ${snapshot.isDragging ? "opacity-70" : ""}`}
                      >
                        <Card className="p-3 border bg-white rounded-md shadow-sm">
                          {editingTask?.id === task.id ? (
                            <div className="flex flex-col gap-2">
                              <Input
                                value={editingTask.title}
                                onChange={(e) => setEditingTask({ ...editingTask, title: e.target.value })}
                              />
                              <Textarea
                                value={editingTask.description}
                                onChange={(e) => setEditingTask({ ...editingTask, description: e.target.value })}
                              />
                              <Input
                                type="date"
                                value={editingTask.dueDate}
                                onChange={(e) => setEditingTask({ ...editingTask, dueDate: e.target.value })}
                              />
                       
                              <select
                                value={editingTask.priority}
                                onChange={(e) =>
                                  setEditingTask({ ...editingTask, priority: e.target.value as "high" | "medium" | "low" })
                                }
                                className="border rounded px-2 py-1 text-sm"
                              >
                                <option value="high">High</option>
                                <option value="medium">Medium</option>
                                <option value="low">Low</option>
                              </select>
                              <Button size="sm" onClick={() => saveEdit(col.id)}>Save</Button>
                            </div>
                          ) : (
                            <>
                              <div className="mb-2">
                                <p className="font-medium">{task.title}</p>
                                <p className="text-sm text-gray-600">{task.description}</p>
                              </div>
                              <div className="flex justify-between items-center text-xs">
                                <span className={`px-2 py-0.5 rounded-full font-medium ${priorityBadge(task.priority)}`}>
                                  {task.priority}
                                </span>
                                <div className="flex items-center gap-1">
                                  <Calendar className="h-3 w-3" />
                                  {task.dueDate}
                                </div>
                              </div>
                              <div className="flex justify-between items-center mt-2 text-xs text-gray-500">
                                <div className="flex items-center gap-1">
                                  <User className="h-3 w-3" />
                                  {task.createdBy}
                                </div>
                                <div className="flex gap-2">
                                  <Button size="icon" variant="ghost" onClick={() => setEditingTask(task)}>
                                    <Pencil className="h-4 w-4" />
                                  </Button>
                                  <Button size="icon" variant="ghost" onClick={() => deleteTask(col.id, task.id)}>
                                    <Trash2 className="h-4 w-4 text-red-500" />
                                  </Button>
                                </div>
                              </div>
                            </>
                          )}
                        </Card>
                      </div>
                    )}
                  </Draggable>
                ))}

                {provided.placeholder}

                {/* Add New Task */}
                <div className="mt-3 flex flex-col gap-2">
                  <Input
                    placeholder="Task title..."
                    value={newTask[col.id]?.title || ""}
                    onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], title: e.target.value } })}
                  />
                  <Textarea
                    placeholder="Description..."
                    value={newTask[col.id]?.description || ""}
                    onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], description: e.target.value } })}
                  />
                  <Input
                    type="date"
                    value={newTask[col.id]?.dueDate || ""}
                    onChange={(e) => setNewTask({ ...newTask, [col.id]: { ...newTask[col.id], dueDate: e.target.value } })}
                  />
                  {/* 🔽 اختيار البريورتي عند الإضافة */}
                  <select
                    value={newTask[col.id]?.priority || "medium"}
                    onChange={(e) =>
                      setNewTask({
                        ...newTask,
                        [col.id]: { ...newTask[col.id], priority: e.target.value as "high" | "medium" | "low" },
                      })
                    }
                    className="border rounded px-2 py-1 text-sm"
                  >
                    <option value="high">High</option>
                    <option value="medium">Medium</option>
                    <option value="low">Low</option>
                  </select>
                  <Button size="sm" onClick={() => addTask(col.id)} disabled={!user}>
                    <Plus className="h-4 w-4 mr-1" /> Add
                  </Button>
                </div>
              </div>
            )}
          </Droppable>
        ))}
      </div>
    </DragDropContext>
  )
}
