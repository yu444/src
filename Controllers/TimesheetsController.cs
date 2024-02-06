using Microsoft.AspNetCore.Mvc;
    using CsvHelper;
using System.Globalization;

namespace TimesheetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetsController : ControllerBase
    {
        private readonly string _csvFilePath = "timesheets.csv"; // Path to your CSV file

        // GET /api/timesheets
        [HttpGet]
        public IActionResult GetTimesheets()
        {
            try
            {
                var timesheets = ReadTimesheetsFromCsv();
                return Ok(timesheets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving timesheets: {ex.Message}");
            }
        }

        // GET /api/timesheets/{id}
        [HttpGet("{id}")]
        public IActionResult GetTimesheetById(int id)
        {
            try
            {
                var timesheet = ReadTimesheetsFromCsv().FirstOrDefault(t => t.EmployeeId == id);
                if (timesheet == null)
                    return NotFound($"Timesheet with ID {id} not found.");
                return Ok(timesheet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving timesheet: {ex.Message}");
            }
        }

        // POST /api/timesheets
        [HttpPost]
        public IActionResult AddTimesheet(TimesheetEntry newEntry)
        {
            try
            {
                var timesheets = ReadTimesheetsFromCsv();
                newEntry.Id = timesheets.Max(t => t.Id) + 1; // Assign a new ID
                timesheets.Add(newEntry);
                WriteTimesheetsToCsv(timesheets);
                return CreatedAtAction(nameof(GetTimesheetById), new { id = newEntry.Id }, newEntry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding timesheet: {ex.Message}");
            }
        }

        // PUT /api/timesheets/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTimesheet(int id, TimesheetEntry updatedEntry)
        {
            try
            {
                var timesheets = ReadTimesheetsFromCsv();
                var existingEntry = timesheets.FirstOrDefault(t => t.Id == id);
                if (existingEntry == null)
                    return NotFound($"Timesheet with ID {id} not found.");

                existingEntry.Date = updatedEntry.Date;
                existingEntry.HoursWorked = updatedEntry.HoursWorked;
                WriteTimesheetsToCsv(timesheets);
                return Ok(existingEntry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating timesheet: {ex.Message}");
            }
        }

        // DELETE /api/timesheets/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTimesheet(int id)
        {
            try
            {
                var timesheets = ReadTimesheetsFromCsv();
                var existingEntry = timesheets.FirstOrDefault(t => t.Id == id);
                if (existingEntry == null)
                    return NotFound($"Timesheet with ID {id} not found.");

                timesheets.Remove(existingEntry);
                WriteTimesheetsToCsv(timesheets);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting timesheet: {ex.Message}");
            }
        }

        // Helper methods
        private List<TimesheetEntry> ReadTimesheetsFromCsv()
        {
            // Read data from the CSV file and return a list of TimesheetEntry objects
            // Implement your logic here
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<TimesheetEntry>();
            return records.ToList();
        }

        private void WriteTimesheetsToCsv(List<TimesheetEntry> timesheets)
        {
            // Write data to the CSV file
            // Implement your logic here
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(timesheets);
        }
    }
}