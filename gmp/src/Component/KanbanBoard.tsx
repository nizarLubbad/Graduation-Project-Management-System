
import { useState } from "react"
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Pencil, Trash2, Plus } from "lucide-react"


type Task = {
  id: string
  title: string
}


type Column = {
  id: string
  title: string
  tasks: Task[]
}



const initialData: Column[] = [
  {
    id: "todo",
    title: "To Do",
    tasks: [
      { id: "1", title: "Write proposal" },
      { id: "2", title: "Collect references" },
    ],
  },
  {
    id: "in-progress",
    title: "In Progress",
    tasks: [
      { id: "3", title: "Design database schema" },
    ],
  },
  {
    id: "done",
    title: "Done",
    tasks: [
      { id: "4", title: "Select project topic" },
    ],
  },
]


export function KanbanBoard() {
  const [columns, setColumns] = useState<Column[]>(initialData)

  const [newTask, setNewTask] = useState<{ [key: string]: string }>({})
  const [editingTask, setEditingTask] = useState<{ id: string; title: string } | null>(null)

  const addTask = (colId: string) => {
    if (!newTask[colId]) return
    setColumns(columns.map(col =>
      col.id === colId
        ? { ...col, tasks: [...col.tasks, { id: Date.now().toString(), title: newTask[colId] }] }
        : col
    ))
    setNewTask({ ...newTask, [colId]: "" })
  }

  const deleteTask = (colId: string, taskId: string) => {
    setColumns(columns.map(col =>
      col.id === colId
        ? { ...col, tasks: col.tasks.filter(t => t.id !== taskId) }
        : col
    ))
  }

  const saveEdit = (colId: string) => {
    if (!editingTask) return
    setColumns(columns.map(col =>
      col.id === colId
        ? {
            ...col,
            tasks: col.tasks.map(t =>
              t.id === editingTask.id ? { ...t, title: editingTask.title } : t
            ),
          }
        : col
    ))
    setEditingTask(null)
  }


 return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
      {columns.map((col) => (
        <div key={col.id} className="bg-gray-50 rounded-lg p-4 shadow-sm">
          <h2 className="font-semibold mb-3">{col.title}</h2>

          {/* Render tasks */}
          {col.tasks.map((task) => (
            <Card key={task.id} className="p-3 mb-2 border flex justify-between items-center">
              {editingTask?.id === task.id ? (
                <div className="flex w-full gap-2">
                  <Input
                    value={editingTask.title}
                    onChange={(e) => setEditingTask({ ...editingTask, title: e.target.value })}
                  />
                  <Button size="sm" onClick={() => saveEdit(col.id)}>Save</Button>
                </div>
              ) : (
                <>
                  <span>{task.title}</span>
                  <div className="flex gap-2">
                    <Button size="icon" variant="ghost" onClick={() => setEditingTask(task)}>
                      <Pencil className="h-4 w-4" />
                    </Button>
                    <Button size="icon" variant="ghost" onClick={() => deleteTask(col.id, task.id)}>
                      <Trash2 className="h-4 w-4 text-red-500" />
                    </Button>
                  </div>
                </>
              )}
            </Card>
          ))}

          {/* Input to add new task */}
          <div className="mt-3 flex gap-2">
            <Input
              placeholder="New task..."
              value={newTask[col.id] || ""}
              onChange={(e) => setNewTask({ ...newTask, [col.id]: e.target.value })}
            />
            <Button size="icon" onClick={() => addTask(col.id)}>
              <Plus className="h-4 w-4" />
            </Button>
          </div>
        </div>
      ))}
    </div>
  )
}
