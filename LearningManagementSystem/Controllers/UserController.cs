﻿using LearningManagementSystem.Models;
using LearningManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearningManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public UserManager<IdentityUser> userManager => HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();


        public ActionResult Index()
        {

            List<ApplicationUser> studIndex = new List<ApplicationUser>();

            foreach (var s in studIndex)
            {

                studIndex.Add(new ApplicationUser()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    CourseId = s.CourseId,
                    Email = s.Email,

                });

                return RedirectToAction("Index");

            }

            return View(context.Users.Where(d => d.CourseId != null));
        }
        public ActionResult Index1()
        {
           
            var abc = context.Users.Where(t => t.CourseId == null);
           
            List<TeacherViewModel> teacherViewModel = new List<TeacherViewModel>();
           

            foreach (var ab in abc)
            {
                teacherViewModel.Add(new TeacherViewModel()

                {
                    FirstName = ab.FirstName,
                    LastName = ab.LastName,
                    // CourseId = s.CourseId,
                    Email = ab.Email

                });
               

            }
            return View(teacherViewModel);

        }


        // GET: User

        [Authorize(Roles = "Teacher")]
        public ActionResult Register()
        {

            // ApplicationDbContext context = new ApplicationDbContext();

            var viewModel = new RegisterViewModel();


            viewModel.Roles = context.Roles.ToList();
            viewModel.Courses = context.Courses.ToList();

            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            var userStore = new UserStore<ApplicationUser>(context);


            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            
            if (ModelState.IsValid)
            {

                var courses = context.Courses.ToList();

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CourseId = Convert.ToInt32(model.CourseId)
                };

                
                var identityResult = userManager.Create(user, model.Password);

               
                if (identityResult.Succeeded)
                {
                   
                    var Role = context.Roles.FirstOrDefault(x => x.Id == model.RoleId);

                    var User = userManager.FindByName(model.Email);
                    userManager.AddToRole(User.Id, Role.Name);
                                                      


                    return RedirectToAction("Index", "Home");
                }
                                            

                ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());
                
            }

            model.Courses = context.Courses.ToList();

            model.Roles = context.Roles.ToList();

            return View(model);

        }
        
        [Authorize(Roles = "Teacher")]
        public ActionResult Register1()
        {

            var viewModel = new RegisterViewModel();


            viewModel.Roles = context.Roles.ToList();
            viewModel.Courses = context.Courses.ToList();

            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Register1(RegisterViewModel model)
        {
            var userStore = new UserStore<ApplicationUser>(context);


            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                 
                };

                var identityResult = userManager.Create(user, model.Password);

                if (identityResult.Succeeded)
                {
                  
                    var Role = context.Roles.FirstOrDefault(x => x.Id == model.RoleId);

                    var User = userManager.FindByName(model.Email);
                    userManager.AddToRole(User.Id, Role.Name);

                    return RedirectToAction("Index", "Home");
                }


                ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());

            }
                     
            model.Roles = context.Roles.ToList();

            return View(model);
        }
        
            public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);

            EditViewModel ev = new EditViewModel(user);


            if (user == null)
            {
                return HttpNotFound();
            }

            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email")]EditViewModel editv)
        {
            if (ModelState.IsValid)
            {

                var uv = context.Users.FirstOrDefault(x => x.Id == editv.Id);
                uv.FirstName = editv.FirstName;
                uv.LastName = editv.LastName;
                uv.Email = editv.Email;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editv);
        }
    }
}