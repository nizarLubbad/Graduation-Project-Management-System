function StudentDashboard() {
    return (
      <div className="p-6 min-h-screen bg-gray-100">
        <h1 className="text-3xl font-bold mb-6">🎓 Student Dashboard</h1>
  
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">📌 Current Project</h2>
            <p>Project: AI-based Smart Parking</p>
            <p>Status: In Progress</p>
          </div>
  
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">⏰ Deadlines</h2>
            <ul className="list-disc ml-5">
              <li>Proposal: Sept 15</li>
              <li>Mid Report: Oct 10</li>
              <li>Final Report: Dec 1</li>
            </ul>
          </div>
  
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">✅ Milestones</h2>
            <ul className="list-disc ml-5">
              <li>Proposal Submitted</li>
              <li>Mid Report Pending</li>
            </ul>
          </div>
        </div>
      </div>
    );
  }
  
  export default StudentDashboard;
  