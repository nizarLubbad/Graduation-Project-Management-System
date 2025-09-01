import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { FileText, Kanban } from "lucide-react"

import { KanbanBoard } from "../Component/KanbanBoard"


export function StudentDashboard() {
  return (
    <div className="space-y-6">
      <Tabs defaultValue="overview" className="w-full">
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="overview" className="flex items-center gap-2">
            <FileText className="h-4 w-4" />
            Overview
          </TabsTrigger>
          <TabsTrigger value="kanban" className="flex items-center gap-2">
            <Kanban className="h-4 w-4" />
            Task Board
          </TabsTrigger>
        </TabsList>

     

        <TabsContent value="kanban" className="space-y-6">
          <KanbanBoard />
        </TabsContent>
      </Tabs>
    </div>
  )
}
