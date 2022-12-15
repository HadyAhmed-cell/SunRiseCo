using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunRiseCo.Data;
using SunRiseCo.Models;

namespace SunRiseCo.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        // The Connection to Database Data
        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            Employee emp = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.ID == id);

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
        //[Route("[controller]/ListEmployees")]
        public IActionResult GetIndexView(string? search, string? sortType, string? sortOrder, int pageSize = 2, int pageNumber = 1)
        {

            IQueryable<Employee> emps = _context.Employees.AsQueryable();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                search = search.Trim();
                emps = _context.Employees.Where(e => e.Name.Contains(search));
                ViewBag.CurrentSearch = search;
            }

            if (!string.IsNullOrWhiteSpace(sortType) && !string.IsNullOrWhiteSpace(sortOrder))
            {
                if (sortType == "FullName")
                {
                    if (sortOrder == "asc")
                    {
                        emps = emps.OrderBy(e => e.Name);
                    }
                    else if (sortOrder == "desc")
                    {
                        emps.OrderByDescending(e => e.Name);
                    }
                }

                else if (sortType == "Position")
                {
                    if (sortOrder == "asc")
                    {
                        emps = emps.OrderBy(e => e.Position);
                    }
                    else if (sortOrder == "desc")
                    {
                        emps.OrderByDescending(e => e.Position);
                    }
                }

                else if (sortType == "Salary")
                {
                    if (sortOrder == "asc")
                    {
                        emps = emps.OrderBy(e => e.Salary);
                    }
                    else if (sortOrder == "desc")
                    {
                        emps.OrderByDescending(e => e.Salary);
                    }
                }
            }


            emps = emps.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.CurrentSearch = search;
            return View("Index", emps);
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
            ViewBag.AllDepartments = _context.Departments.ToList();

            //ViewData["FormalCountry"] = "Arab Republic Of Egypt";

            //TempData["Capital"] = "Cairo";

            return View("Create");
        }

        [ValidateAntiForgeryToken] // Prevents CRF Attacks
        [HttpPost]
        public IActionResult AddEmployee([Bind("ID , Name, Position , Salary , Bonus , PhoneNo , Email , Password , HiringDateTime , BirthDate , AttendanceTime , LeavingTime , CreatedAt , LastUpdatedAt , DepartmentId , ConfirmPassword , ConfirmEmail")] Employee emp, IFormFile? imageFile)
        {
            if (ModelState.IsValid == true)
            {
                if (imageFile == null)
                {
                    emp.ImageUrl = "\\images\\No_Image.png";
                }
                else
                {
                    string imgExtension = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\images\\" + imgName;
                    emp.ImageUrl = imgUrl;

                    string imgPath = _environment.WebRootPath + imgUrl;

                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                }

                emp.CreatedAt = DateTime.Now;
                _context.Employees.Add(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create");
            }
        }

        public IActionResult EditEmployee(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.ID == id);

            if (emp == null)
            {
                return NotFound();
            }

            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Edit", emp);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditCurrent([Bind("ID , Name, Position , Salary , Bonus , PhoneNo , Email , Password , HiringDateTime , BirthDate , AttendanceTime , LeavingTime , CreatedAt , LastUpdatedAt , DepartmentId , ConfirmPassword , ConfirmEmail , ImageUrl")] Employee emp, int id, IFormFile? imageFile)
        {
            if (id != emp.ID)
            {
                return BadRequest();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null)
                    {
                        if (emp.ImageUrl != "\\images\\No_Image.png")
                        {
                            string oldImagePath = _environment.WebRootPath + emp.ImageUrl;

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                        }

                        string imgExtension = Path.GetExtension(imageFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgUrl = "\\images\\" + imgName;
                        emp.ImageUrl = imgUrl;

                        string imgPath = _environment.WebRootPath + imgUrl;

                        FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                        imageFile.CopyTo(imgStream);
                        imgStream.Dispose();
                    }


                    emp.LastUpdatedAt = DateTime.Now;
                    _context.Employees.Update(emp);
                    _context.SaveChanges();
                    return RedirectToAction("GetIndexView");
                }
                else
                {
                    ViewBag.AllDepartments = _context.Departments.ToList();
                    return View("Edit");
                }
            }
        }

        public IActionResult DeleteEmp(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.ID == id);

            if (emp == null)
            {
                return NotFound();
            }

            return View("Delete", emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCurrent(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.ID == id);

            if (emp.ImageUrl != "\\images\\No_Image.png")
            {
                string imgPath = _environment.WebRootPath + emp.ImageUrl;

                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();

            return RedirectToAction("GetIndexView");
        }
    }
}