using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BlockBuster.Models;
using System.Threading.Tasks;
using BlockBuster.ViewModels;

namespace BlockBuster.Controllers
{
  public class AccountController : Controller
  {
    private readonly BlockBusterContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly RoleManager<IdentityRole> _roleManager;


    // var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


    //    if(!roleManager.RoleExists("ROLE NAME"))
    //    {
    //       var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
    //       role.Name = "ROLE NAME";
    //       roleManager.Create(role);

    //     }
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, BlockBusterContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      var user = new ApplicationUser { Email = model.Email, UserName = model.UserName };
      var role = new IdentityRole();
      role.Name = "User";
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      IdentityResult roleResult = await _roleManager.CreateAsync(role);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}