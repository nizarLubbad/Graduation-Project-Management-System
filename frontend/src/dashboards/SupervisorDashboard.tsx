function SupervisorDashboard() {
    return (
      <div className="p-6 min-h-screen bg-gray-50">
        <h1 className="text-3xl font-bold mb-6">👨‍🏫 Supervisor Dashboard</h1>
  
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">📋 Student Projects</h2>
            <p>Review and manage assigned projects.</p>
          </div>
  
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">📝 Feedback</h2>
            <p>Provide feedback on student submissions.</p>
          </div>
  
          <div className="bg-white shadow-md p-4 rounded-lg">
            <h2 className="text-xl font-semibold mb-2">📊 Reports</h2>
            <p>View project reports and progress tracking.</p>
          </div>
        </div>
      </div>
    );
  }
  
  export default SupervisorDashboard;
  