public class TimesheetEntry
{
    public int Id { get; set; } // Unique identifier for each entry
    public int EmployeeId { get; set; } // Employee ID
    public DateTime Date { get; set; } // Date of the timesheet entry
    public double HoursWorked { get; set; } // Hours worked by the employee
    // Add other relevant properties (e.g., project/task, description)
}