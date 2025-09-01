import { useState } from "react"
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Pencil, Trash2, Plus } from "lucide-react"
import { DragDropContext, Droppable, Draggable, DropResult } from "@hello-pangea/dnd"

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
    tasks: [{ id: "3", title: "Design database schema" }],
  },
  {
    id: "done",
    title: "Done",
    tasks: [{ id: "4", title: "Select project topic" }],
  },
]

export function KanbanBoard() {
  const [columns, setColumns] = useState<Column[]>(initialData)
  const [newTask, setNewTask] = useState<{ [key: string]: string }>({})
  const [editingTask, setEditingTask] = useState<{ id: string; title: string } | null>(null)

  // Add a new task to a column
  const addTask = (colId: string) => {
    if (!newTask[colId]) return
    setColumns(columns.map(col =>
      col.id === colId
        ? { ...col, tasks: [...col.tasks, { id: Date.now().toString(), title: newTask[colId] }] }
        : col
    ))
    setNewTask({ ...newTask, [colId]: "" })
  }

  // Delete a task from a column
  const deleteTask = (colId: string, taskId: string) => {
    setColumns(columns.map(col =>
      col.id === colId
        ? { ...col, tasks: col.tasks.filter(t => t.id !== taskId) }
        : col
    ))
  }

  // Save edits after modifying a task title
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

  // Handle drag and drop logic
  const handleDragEnd = (result: DropResult) => {
    const { source, destination } = result
    if (!destination) return // If dropped outside any column

    if (source.droppableId === destination.droppableId) {
      // Case 1: Reordering inside the same column
      const column = columns.find(c => c.id === source.droppableId)!
      const newTasks = Array.from(column.tasks)
      const [moved] = newTasks.splice(source.index, 1)
      newTasks.splice(destination.index, 0, moved)

      setColumns(columns.map(c => c.id === column.id ? { ...c, tasks: newTasks } : c))
    } else {
      // Case 2: Moving task between different columns
      const sourceCol = columns.find(c => c.id === source.droppableId)!
      const destCol = columns.find(c => c.id === destination.droppableId)!
      const sourceTasks = Array.from(sourceCol.tasks)
      const destTasks = Array.from(destCol.tasks)

      const [moved] = sourceTasks.splice(source.index, 1)
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
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {columns.map((col) => (
          <Droppable key={col.id} droppableId={col.id}>
            {(provided) => (
              <div
                ref={provided.innerRef}
                {...provided.droppableProps}
                className="bg-gray-50 rounded-lg p-4 shadow-sm min-h-[300px]"
              >
                <h2 className="font-semibold mb-3">{col.title}</h2>

                {/* Render tasks as draggable items */}
                {col.tasks.map((task, index) => (
                  <Draggable key={task.id} draggableId={task.id} index={index}>
                    {(provided, snapshot) => (
                      <Card
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}
                        className={`p-3 mb-2 border flex justify-between items-center cursor-grab ${
                          snapshot.isDragging ? "bg-teal-100" : "bg-white"
                        }`}
                      >
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
                    )}
                  </Draggable>
                ))}
                {provided.placeholder}

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
            )}
          </Droppable>
        ))}
      </div>
    </DragDropContext>
  )
}
