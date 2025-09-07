import { Card, CardContent } from "@/components/ui/card";

interface Project {
  id: number;
  title: string;
  description: string;
  student: string;
  year: number;
  grade: string;
  category: string;
  tags: string[];
  image: string;
}

const projects: Project[] = [
  {
    id: 1,
    title: "Smart Campus Navigation System",
    description: "An AI-powered mobile app that helps students navigate the campus using AR technology.",
    student: "Sarah Johnson",
    year: 2023,
    grade: "A+",
    category: "Computer Science",
    tags: ["AI", "Mobile App", "AR"],
    image: "https://via.placeholder.com/400x200?text=Smart+Campus",
  },
  {
    id: 2,
    title: "Sustainable Energy Management Dashboard",
    description: "A web-based dashboard for monitoring and optimizing energy consumption in smart buildings.",
    student: "Ahmed Hassan",
    year: 2023,
    grade: "A",
    category: "Engineering",
    tags: ["IoT", "Sustainability", "Dashboard"],
    image: "https://via.placeholder.com/400x200?text=Energy+Dashboard",
  },
  {
    id: 3,
    title: "Digital Art Therapy Platform",
    description: "An interactive platform combining digital art creation with therapeutic guidance.",
    student: "Emma Rodriguez",
    year: 2022,
    grade: "A+",
    category: "Digital Arts",
    tags: ["Digital Art", "Therapy", "Interactive"],
    image: "https://via.placeholder.com/400x200?text=Art+Therapy",
  },
];

export default function ProjectHistory() {
  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Project History</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {projects.map((project) => (
          <Card
            key={project.id}
            className="shadow-lg rounded-2xl overflow-hidden hover:shadow-xl transition-shadow duration-300"
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
                <span className="text-sm font-bold text-gray-600">{project.grade}</span>
              </div>
              <p className="text-gray-600 text-sm mb-2">{project.description}</p>
              <p className="text-sm text-gray-500">👤 {project.student}</p>
              <div className="flex gap-2 mt-2 flex-wrap">
                {project.tags.map((tag, idx) => (
                  <span
                    key={idx}
                    className="bg-gray-100 text-gray-600 text-xs px-2 py-1 rounded"
                  >
                    {tag}
                  </span>
                ))}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
}
