using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace gizzy;

public class InstructorController : Controller
{
    private readonly ApplicationDbContext _context;
    public InstructorController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View(_context.Instructors.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Instructor Instructor)
    {
        if (await _context.Instructors.AnyAsync(c => c.FirstName == Instructor.FirstName))
        {
            ModelState.AddModelError("FirstName", "Instructor already registered");
        }
        else if (Instructor.FirstName == null)
        {
            ModelState.AddModelError("FirstName", "FirstName cannot be empty");
        }
        else if (ContainsNumbers(Instructor.FirstName))
        {
            ModelState.AddModelError("FirstName", "Cannot contain numbers");
        }

        // Ensure the first letter of the FirstName is capitalized
        Instructor.FirstName = CapitalizeFirstLetter(Instructor.FirstName);

        if (ModelState.IsValid)
        {
            await _context.Instructors.AddAsync(Instructor);
            await _context.SaveChangesAsync();
            TempData["success"] = $"{Instructor.FirstName} Added Successfully!";
            return RedirectToAction("Index");

        }
        return View();

    }

    private bool ContainsNumbers(string input)
    {
        return input.Any(char.IsDigit);
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public async Task<IActionResult> Edit(Guid Id)
    {
        var item = await _context.Instructors.FindAsync(Id);
        if (item == null)
        {
            return View("NotFoundView");
        }
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Instructor instructor)
    {
        var existingInstructor = await _context.Instructors
        .FirstOrDefaultAsync(c => c.FirstName == instructor.FirstName && c.Id != instructor.Id);

        if (existingInstructor != null)
        {
            ModelState.AddModelError("FirstName", "Instructor with this FirstName already exists");
        }
        else if (instructor.FirstName == null)
        {
            ModelState.AddModelError("FirstName", "FirstName cannot be empty");
        }
        else if (ContainsNumbers(instructor.FirstName))
        {
            ModelState.AddModelError("FirstName", "Cannot contain numbers");
        }

        // Ensure the first letter of the FirstName is capitalized
        instructor.FirstName = CapitalizeFirstLetter(instructor.FirstName);

        if (ModelState.IsValid)
        {
            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();
            TempData["success"] = $" Updated Successfully!";
            return RedirectToAction("Index");
        }
        return View();
    }

    public async Task<IActionResult> Delete(Guid Id)
    {

        var item = await _context.Instructors.FindAsync(Id);
        if (item == null)
        {
            return View("NotFoundView");
        }
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Instructor instructor)
    {
        if (instructor == null)
        {
            return View("NotFoundView");
        }
        _context.Instructors.Remove(instructor);
        TempData["success"] = $"instructor Excluded Successfully!";
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public IActionResult NotFoundView()
    {
        return View();
    }

}

