//Account Controller 
using HouseRentManagementSystem.Models;
using HouseRentManagementSystem.ViewModel;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Data.Entity;

namespace HouseRentManagementSystem.Controllers
{
    public class AccountController : Controller
    {

        // Return Home page.
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult LoginUser()
        {
            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();

        }

        //Return Register view
        public ActionResult Register()
        {
            return View();
        }

       
        public ActionResult AboutUs()
        {
            return View();
        }
        
        
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
//
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("sangavisanjai222@gmail.com", "support@HouseRentSystem");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Sangavi@2000"; 

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Your H Password";
                body = "Hi,<br/>We have received a request to reset your HouseRent Management System account password associated with this email address. If you have not placed this request, you can safely ignore this email and we assure you that your account is completely secure." +
                    "If you do need to change your HouseRent Password, you can use the link given below." +
                    "<br/><br/><a href=" + link + "><button> Reset Password link</button></a>";
            }


            var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                
                
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
               
                    smtp.Send(message);
                
             
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
             
            string message = "";
            

            using (Database1Entities1 dc = new Database1Entities1())
            {
                var account = dc.RegisterUsers.Where(a => a.Email == EmailID).FirstOrDefault();
                var accountCustomer = dc.RegisterCustomers.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.message1 = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }



        public ActionResult ResetPassword(string id)
        {
            

            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (Database1Entities1 dc = new Database1Entities1())
            {
                var user = dc.RegisterUsers.Where(a => a.ResetPasswordCode == id).SingleOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (Database1Entities1 dc = new Database1Entities1())
                {
                    var user = dc.RegisterUsers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails, HttpPostedFileBase image)
        {
            
                using (var databaseContext = new Database1Entities1())
                {

                    
                    RegisterUser reglog = new RegisterUser();
                    var userWithSameEmail = databaseContext.RegisterUsers.Where(m => m.Email == registerDetails.Email).FirstOrDefault(); 
                    if (userWithSameEmail == null)
                    {
                        
                        reglog.FirstName = registerDetails.FirstName;
                        reglog.LastName = registerDetails.LastName;
                        reglog.Email = registerDetails.Email;
                        reglog.MobileNo = registerDetails.MobileNo;
                        reglog.Password = registerDetails.Password;
                        reglog.AadharNo = registerDetails.AadharNo;
                    if (image != null)
                    {
                        reglog.ProfileImage = new byte[image.ContentLength];
                        image.InputStream.Read(reglog.ProfileImage, 0, image.ContentLength);
                    }


                    try
                    {
                            databaseContext.RegisterUsers.Add(reglog);
                            databaseContext.SaveChanges();


                            ViewBag.saved = "Registration Successful!";
                            return RedirectToAction("Login");
                        }
                        catch (NullReferenceException ex)
                        {
                            ViewBag.Message = "Somthing Wrong Please Try Again"+ex;
                            return View("Register");
                        }

                        TempData["ErrorMessage"] = "Registration Is Success";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "Email Already Exists";

                        return View("Test");
                    }
                }

           
        }

        [HttpPost]
        public ActionResult SaveRegisterCustomer(Register registerDetails, HttpPostedFileBase image)
        {
            
            using (var databaseContext = new Database1Entities1())
            {


                RegisterCustomer reglog = new RegisterCustomer();
                var userWithSameEmail = databaseContext.RegisterCustomers.Where(m => m.Email == registerDetails.Email).FirstOrDefault(); 
                                                                                                                                     
                if (userWithSameEmail == null)
                {
                    reglog.FirstName = registerDetails.FirstName;
                    
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.MobileNo = registerDetails.MobileNo;
                    reglog.Password = registerDetails.Password;
                    reglog.AadharNo = registerDetails.AadharNo;
                    if (image != null)
                    {
                        reglog.ProfileImage = new byte[image.ContentLength];
                        image.InputStream.Read(reglog.ProfileImage, 0, image.ContentLength);
                    }


                    try
                    {
                        databaseContext.RegisterCustomers.Add(reglog);
                        databaseContext.SaveChanges();


                        ViewBag.saved = "Registration Successful!";
                        return RedirectToAction("Login");
                    }
                    catch (NullReferenceException ex)
                    {
                        ViewBag.Message = "Somthing Wrong Please Try Again"+ex;
                        return View("Register");
                    }

                    TempData["ErrorMessage"] = "Registration Is Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Email Already Exists";

                    return View("Register");
                }
            }


        }


        public ActionResult Login()
        {
            return View();
        }
       
        //Customer Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                
                var isValidUser = IsValidUser(model);
                if (isValidUser != null)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("UserData", "User");
                }
                else
                {

                    TempData["CustomerLogin"] = "Wrong Username and password combination Login !";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                
                return View(model);
            }
        }

        
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new Database1Entities1())
            {
                
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                
                if (user == null)
                    return null;
                
                else
                    return user;
            }
        }

        //Seller Login
        [HttpPost]
        public ActionResult LoginCustomer(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {

                
                var isValidUser = IsValidUser1(model);
                if (isValidUser != null)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Admin", "House");
                }
                else
                {
                    
                    TempData["SellerLogin"] = "Wrong Username and password combination !";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                
                return View(model);
            }
        }

        
        public RegisterCustomer IsValidUser1(LoginViewModel model)
        {
            using (var dataContext = new Database1Entities1())
            {
                
                RegisterCustomer user = dataContext.RegisterCustomers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                
                if (user == null)
                    return null;
                
                else
                    return user;
            }
        }
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }
         

      

    }
}


