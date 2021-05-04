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
        
        //The form's data in Register view is posted to this method.
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
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
            var fromEmailPassword = "Sangavi@2000"; // Replace with actual password

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
                
                //  UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
               
                    smtp.Send(message);
                
               /* catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                        ex.ToString());
                }*/
            
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
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
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
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
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page

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
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
           
                //create database context using Entity framework
                using (var databaseContext = new Database1Entities1())
                {

                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    RegisterUser reglog = new RegisterUser();
                    var userWithSameEmail = databaseContext.RegisterUsers.Where(m => m.Email == registerDetails.Email).FirstOrDefault(); //checking if the emailid already exits for any user
                    //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                    //If user is present, then true is returned.
                    //Save all details in RegitserUser object
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
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.

            //create database context using Entity framework
            using (var databaseContext = new Database1Entities1())
            {

                //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                RegisterCustomer reglog = new RegisterCustomer();
                var userWithSameEmail = databaseContext.RegisterCustomers.Where(m => m.Email == registerDetails.Email).FirstOrDefault(); //checking if the emailid already exits for any user
                                                                                                                                     //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                                                                                                                                     //If user is present, then true is returned.
                                                                                                                                     //Save all details in RegitserUser object
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

        //function to check if User is valid or not
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new Database1Entities1())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }

        //Seller Login
        [HttpPost]
        public ActionResult LoginCustomer(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser1(model);
                if (isValidUser != null)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Admin", "House");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    TempData["SellerLogin"] = "Wrong Username and password combination !";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //function to check if User is valid or not
        public RegisterCustomer IsValidUser1(LoginViewModel model)
        {
            using (var dataContext = new Database1Entities1())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterCustomer user = dataContext.RegisterCustomers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }
         

      

    }
}


