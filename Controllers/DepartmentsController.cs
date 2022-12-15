using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunRiseCo.Data;
using SunRiseCo.Models;

namespace SunRiseCo.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // The Connection to Database Data
        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GreetVisitor()
        {
            return "Welcome to Our Page";
        }

        public string GreetUser(string name, string address)
        {
            return "Hi, " + name + "!" + "and His Address is " + address;
        }

        public double GetPrice(double cost, double profitRatio)
        {
            return cost + cost * profitRatio;
        }

        //public Employee emp1 = new Employee()
        //{
        //    ID = 1001,
        //    Name = "Hady Ahmed",
        //    Position = "Developer",
        //    Salary = 8500
        //};
        [HttpGet]
        public IActionResult Details(int id)
        {
            Department emp = _context.Departments.Include(d => d.Employees).FirstOrDefault(e => e.Id == id);

            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        //    List<Employee> employees = new List<Employee>(){
        //     new Employee()
        //        {
        //            ID = 2001,
        //            Name = "Ahmed",
        //            Position = "IT Manager",
        //            Salary = 15000
        //        },

        //         new Employee()
        //        {
        //            ID = 2002,
        //            Name = "Mahmoud",
        //            Position = "Tester",
        //            Salary = 6000
        //        },

        //         new Employee()
        //        {
        //            ID = 2003,
        //            Name = "Aliaa",
        //            Position = "Engineer",
        //            Salary = 10000
        //        },
        //          new Employee()
        //        {
        //            ID = 1,
        //            Name = "LordStark",
        //            Position = "CEO",
        //            Salary = 1000000
        //        },
        //};

        [HttpGet]
        public IActionResult GetIndexView()
        {
            return View("Index", _context.Departments);
        }

        //public IActionResult GetCeodetails(string pos)
        //{
        //    Employee emp = _context.Employees.FirstOrDefault(e => e.Position == pos);

        //    if (emp == null)
        //    {
        //        return NotFound();
        //    }
        //    return View("Details", emp);
        //}
        [HttpGet]
        public IActionResult GetCreateView()
        {
            //ViewBag.Country = "Egypt";

            return View("Create");
        }

        [ValidateAntiForgeryToken] // Prevents CRF Attacks
        [HttpPost]
        public IActionResult AddDepartment([Bind("ID , DepName, Description")] Department dept)
        {
            if (ModelState.IsValid == true)
            {
                _context.Departments.Add(dept);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Create");
            }
        }

        public IActionResult EditDepartment(int id)
        {
            Department dept = _context.Departments.FirstOrDefault(e => e.Id == id);

            if (dept == null)
            {
                return NotFound();
            }
            return View("Edit", dept);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditCurrent([Bind("ID , DepName, Description")] Department dept, int id)
        {
            if (id != dept.Id)
            {
                return BadRequest();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Departments.Update(dept);
                    _context.SaveChanges();
                    return RedirectToAction("GetIndexView");
                }
                else
                {
                    return View("Edit");
                }
            }
        }

        public IActionResult DeleteDept(int id)
        {
            Department dept = _context.Departments.FirstOrDefault(e => e.Id == id);

            if (dept == null)
            {
                return NotFound();
            }

            return View("Delete", dept);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCurrent(int id)
        {
            Department dept = _context.Departments.FirstOrDefault(e => e.Id == id);

            _context.Departments.Remove(dept);
            _context.SaveChanges();

            return RedirectToAction("GetIndexView");
        }
    }
}

