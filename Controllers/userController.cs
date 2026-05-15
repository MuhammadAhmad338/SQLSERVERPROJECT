using Microsoft.AspNetCore.Mvc;

class UserController : Controller
{
    private static List<User> users = new List<User>();

    public IActionResult Index()
    {
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);
        return RedirectToAction("Index");
    }
}