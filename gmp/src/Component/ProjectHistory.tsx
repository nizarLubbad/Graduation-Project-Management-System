import { useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogHeader } from "@/components/ui/dialog";
import { DialogTitle } from "@radix-ui/react-dialog";
import { Input } from "@/components/ui/input";

interface Project {
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

const projects: Project[] = [
  {
    id: 1,
    title: "Smart Campus Navigation System",
    description: "An AI-powered mobile app that helps students navigate the campus using AR technology.",
    student: "Sarah Johnson",
    supervisor: "Prof. John Smith",
    year: 2023,
    grade: "A+",
    category: "Computer Science",
    tags: ["AI", "Mobile App", "AR"],
    image: "https://via.placeholder.com/400x200?text=Smart+Campus",
    abstract: "A mobile application using AR to provide students with seamless navigation across campus.",
    technologies: ["React Native", "TensorFlow", "Firebase"]
  },
  {
    id: 2,
    title: "Sustainable Energy Management Dashboard",
    description: "A web-based dashboard for monitoring and optimizing energy consumption in smart buildings.",
    student: "Ahmed Hassan",
    supervisor: "Prof. Lisa Wang",
    year: 2023,
    grade: "A",
    category: "Engineering",
    tags: ["IoT", "Sustainability", "Dashboard"],
    image: "https://via.placeholder.com/400x200?text=Energy+Dashboard",
    abstract: "An innovative solution for real-time energy monitoring and optimization in smart buildings, featuring predictive analytics and automated control systems.",
    technologies: ["React", "Node.js", "MongoDB", "IoT Sensors"]
  },
  {
    id: 3,
    title: "Digital Art Therapy Platform",
    description: "An interactive platform combining digital art creation with therapeutic guidance.",
    student: "Emma Rodriguez",
    supervisor: "Dr. Emily Carter",
    year: 2022,
    grade: "A+",
    category: "Digital Arts",
    tags: ["Digital Art", "Therapy", "Interactive"],
    image: "https://via.placeholder.com/400x200?text=Art+Therapy",
    abstract: "A therapeutic platform allowing users to express emotions through digital art while guided by professional therapists.",
    technologies: ["Vue.js", "Express.js", "MySQL"]
  }
];

export default function ProjectHistory() {
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState("All");
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);

  // فلترة المشاريع
  const filteredProjects = projects.filter((p) => {
    const matchesSearch =
      p.title.toLowerCase().includes(search.toLowerCase()) ||
      p.description.toLowerCase().includes(search.toLowerCase());

    const matchesCategory = filter === "All" || p.category === filter;

    return matchesSearch && matchesCategory;
  });

  const categories = ["All", "Computer Science", "Engineering", "Digital Arts"];

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-2">Project History</h1>
      <p className="text-gray-500 mb-4">
        Explore inspiring graduation projects from previous students
      </p>

      {/* البحث والفلاتر */}
      <div className="flex gap-2 mb-6 flex-wrap">
        <Input
          placeholder="Search projects..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="w-64"
        />
        {categories.map((c) => (
          <Button
            key={c}
            variant={filter === c ? "default" : "outline"}
            onClick={() => setFilter(c)}
          >
            {c}
          </Button>
        ))}
      </div>

      
      {/* Grid of Projects */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredProjects.map((project) => (
    <Card
    key={project.id}
    className="shadow-md rounded-2xl overflow-hidden transition-all duration-300 hover:shadow-xl hover:-translate-y-2"
  >
  
        
            <div className="relative">
              <img
                src={project.image}
                alt={project.title}
                className="w-full h-40 object-cover rounded-t-2xl"
              />
              <span className="absolute top-2 right-2 bg-white text-gray-800 px-2 py-1 rounded text-xs font-semibold shadow">
                {project.year}
              </span>
            </div>

            <CardContent className="p-4">
              <div className="flex justify-between items-center mb-2">
                <h2 className="text-lg font-semibold">{project.title}</h2>
                <span className="bg-gray-100 text-gray-600 text-xs px-2 py-1 rounded">{project.grade}</span>
              </div>
              <p className="text-gray-600 text-sm mb-2">{project.description}</p>
              <p className="text-sm text-gray-500">👤 {project.student}</p>
              <div className="flex gap-2 mt-2 flex-wrap">
                {project.tags.map((tag, id) => (
                  <span
                    key={id}
                    className="bg-gray-100 text-gray-600 text-xs px-2 py-1 rounded"
                  >
                    {tag}
                  </span>
                ))}
              </div>

              <Button
  className="w-full mt-4 bg-black-200 text-gray-800 hover:bg-gray-300 transition-colors duration-200"
  onClick={() => setSelectedProject(project)}
>
                View Details
              </Button>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Dialog for Project Details */}
      <Dialog open={!!selectedProject} onOpenChange={() => setSelectedProject(null)}>
  <DialogContent className="max-w-3xl max-h-[80vh] overflow-y-auto">
    {selectedProject && (
      <>
        <DialogHeader>
          <DialogTitle className="text-xl font-bold">
            {selectedProject.title}
          </DialogTitle>
        </DialogHeader>

        <img
          src={selectedProject.image}
          alt={selectedProject.title}
          className="w-full h-60 object-cover rounded-lg mb-4"
        />

        {/* Project Details + Abstract */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          {/* Left column: Project Details */}
          <div>
            <h3 className="font-semibold mb-2">Project Details</h3>
            <p><strong>Author:</strong> {selectedProject.student}</p>
            <p><strong>Supervisor:</strong> {selectedProject.supervisor}</p>
            <p><strong>Category:</strong> {selectedProject.category}</p>
            <p><strong>Year:</strong> {selectedProject.year}</p>
            <p><strong>Grade:</strong> {selectedProject.grade}</p>
          </div>

          {/* Right column: Abstract */}
          <div>
            <h3 className="font-semibold mb-2">Abstract</h3>
            <p className="text-gray-600">{selectedProject.abstract}</p>
          </div>
        </div>

        {/* Technologies */}
        <div className="mb-4">
          <h3 className="font-semibold mb-2">Technologies Used</h3>
          <div className="flex gap-2 flex-wrap">
            {selectedProject.technologies.map((tech, idx) => (
              <span
                key={idx}
                className="bg-gray-100 text-gray-600 text-xs px-2 py-1 rounded"
              >
                {tech}
              </span>
            ))}
          </div>
        </div>

        {/* Tags */}
        <div>
          <h3 className="font-semibold mb-2">Tags</h3>
          <div className="flex gap-2 flex-wrap">
            {selectedProject.tags.map((tag, idx) => (
              <span
                key={idx}
                className="bg-blue-100 text-blue-600 text-xs px-2 py-1 rounded"
              >
                {tag}
              </span>
            ))}
          </div>
        </div>
      </>
    )}
  </DialogContent>
</Dialog>

    </div>
  );
}
