
import { useState } from "react"
import { Card } from "@/components/ui/card"


type Task = {
  id: string
  title: string
}


type Column = {
  id: string
  title: string
  tasks: Task[]
}


// --- Commit 2: Add multiple columns ---
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

  return (
// --- Commit 3: Add styling to board and cards ---
// Update wrapper to show 3 columns on larger screens
<div className="grid grid-cols-1 md:grid-cols-3 gap-4">

      {columns.map((col) => (
        <div key={col.id} className="bg-gray-50 rounded-lg p-4 shadow-sm">
          <h2 className="font-semibold mb-3">{col.title}</h2>
          {col.tasks.map((task) => (
            <Card key={task.id} className="p-3 mb-2 border">
              {task.title}
            </Card>
          ))}
        </div>
      ))}
    </div>
  )
}
