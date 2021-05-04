using HouseRentManagementSystem.Models;
using HouseRentManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace HouseRentManagementSystem.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
            
        }
        public ActionResult ViewDetails(int? Id)
        {
            
            if (Id == null)
            {
                return HttpNotFound();
            }
            Database1Entities1 dc = new Database1Entities1();
            
            var acc = dc.Houses.Where(s => s.PropId == Id).FirstOrDefault();
            var SellerEmail = acc.Email;
            var sellerdetails = dc.RegisterUsers.Where(s => s.Email == SellerEmail).SingleOrDefault();
            string Emailid = User.Identity.Name;
            var acc1 = dc.RegisterUsers.Where(s => s.Email == Emailid).FirstOrDefault();

            if (acc == null)
            {
                return HttpNotFound();
            }
            ViewBag.FName = sellerdetails.FirstName;
            ViewBag.contact = sellerdetails.MobileNo;
            ViewBag.email = sellerdetails.Email;
            ViewBag.image = sellerdetails.ProfileImage;
            return View(acc);
        }
        public ActionResult ViewDetails1(int? Id)
        {
            dynamic dy = new ExpandoObject();
            dy.Housedata=getHouses(Id);
            dy.RegisterUserData = getRegisterUser(Id);


            return View(dy);
        }

        public House getHouses(int? Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Houses.Where(s => s.PropId == Id).FirstOrDefault();
            
            return acc;
        }

        public RegisterUser getRegisterUser(int? Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Houses.Where(s => s.PropId == Id).FirstOrDefault();
            var SellerEmail = acc.Email;
            var sellerdetails = dc.RegisterUsers.Where(s => s.Email == SellerEmail).FirstOrDefault();
            return sellerdetails;
        }
        public ActionResult UserData()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Emailid = User.Identity.Name;
            var acc = dc.RegisterUsers.FirstOrDefault(s => s.Email.Equals(Emailid));
            Session["Name"] = Convert.ToString(acc.FirstName+" "+acc.LastName);
            Session["Name1"] = Convert.ToString(acc.FirstName);
            return View(from House in dc.Houses
                        select House);
        }
        public ActionResult UserData1()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Emailid = User.Identity.Name;
            var acc = dc.RegisterUsers.FirstOrDefault(s => s.Email.Equals(Emailid));
            List<RegisterUser> register = new List<RegisterUser>();
            register.Add(acc);
            ViewBag.image = acc.ProfileImage;
            
            return View(register.AsEnumerable());
        }
        public ActionResult Edit1(int? Id)
        {
            if (Id== null)
            {
                return HttpNotFound();
            }
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterUsers.Where(s => s.Id == Id).FirstOrDefault();
            if (acc == null)
            {
                return HttpNotFound();
            }

            return View(acc);
        }

        [HttpPost]
        public ActionResult Edit1([Bind(Include = "Id,FirstName,LastName,Email,Password,MobileNo,AadharNo")] RegisterUser Registers, HttpPostedFileBase image)
        {
            Database1Entities1 dc = new Database1Entities1();
            //  RegisterUser reglog = new RegisterUser();

            if (image != null)
            {
                Registers.ProfileImage = new byte[image.ContentLength];
                image.InputStream.Read(Registers.ProfileImage, 0, image.ContentLength);
            }
            if (ModelState.IsValid)

            {
                dc.Entry(Registers).State = EntityState.Modified;
                dc.SaveChanges();
                return RedirectToAction("UserData1");
            }
            return View(Registers);
        }

    

       
        public ActionResult EditProfile(int Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterUsers.Where(s => s.Id == Id).FirstOrDefault();
            
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }
        [HttpPost]
        public ActionResult EditProfile(RegisterUser Registers)
        {
            Database1Entities1 dc = new Database1Entities1();
            dc.Entry(Registers).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("UserData", "User");
        }



        [HttpPost]
        public ActionResult SaveBookingDetails(Booking AddProp, String imageUrl)
        {
            using (var databaseContext = new Database1Entities1())
            {
                System.Diagnostics.Debug.WriteLine("Add Prop:"+AddProp.PropName);
                //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                Booking reglog = new Booking();
                var userWithSameEmail = databaseContext.Bookings.Where(m => m.BookId == AddProp.BookId).FirstOrDefault(); //checking if the emailid already exits for any user
                var HouseEmail = databaseContext.Houses.Where(m => m.PropName == AddProp.PropName).FirstOrDefault();                                                                                                                     //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                string Emailid = User.Identity.Name;                                //If user is present, then true is returned.
                var Userdataa = databaseContext.RegisterUsers.Where(m => m.Email == Emailid).FirstOrDefault();                                                                                                                                                                                                //Save all details in RegitserUser object
                if (userWithSameEmail == null)
                {

                    reglog.PropName = AddProp.PropName;
                    reglog.PropType = AddProp.PropType;
                    reglog.PropStatus = AddProp.PropStatus;
                    reglog.PropRent = AddProp.PropRent;
                    reglog.PropAddress = AddProp.PropAddress;
                    reglog.PropContact = AddProp.PropContact;
                    reglog.Book = "request";
                    reglog.PropDescription = AddProp.PropDescription;
                    reglog.PropSize = AddProp.PropSize;
                    reglog.PropFacing = AddProp.PropFacing;
                    reglog.PetFriendly = AddProp.PetFriendly;
                    reglog.AvailableFor = AddProp.AvailableFor;
                    reglog.Flooring = AddProp.Flooring;
                    reglog.AvailableFrom = AddProp.AvailableFrom;

                  
                  

                    reglog.CustEmail = Emailid;
                    reglog.SellEmail = HouseEmail.Email;
                    reglog.SFirstName = Userdataa.FirstName;
                    reglog.SLastName = Userdataa.LastName;
                    reglog.SMobileNumber = Userdataa.MobileNo;
                    reglog.SProfileImage = Userdataa.ProfileImage;


                    try
                    {
                        databaseContext.Bookings.Add(reglog);
                        databaseContext.SaveChanges();
                       

                        ViewBag.saved = "Registration Successful!";
                        return RedirectToAction("BookSuccess");
                    }
                    catch (NullReferenceException ex)
                    {
                           ViewBag.Message = "Somthing Wrong Please Try Again"+ex;
                         return RedirectToAction("ViewDetails");
                        
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting  
                                // the current instance as InnerException  
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }
                else
                {
                    

                    return View("ViewDetails");
                }
            }


        }
        public ActionResult BookSuccess()
        {
            return View();
        }
        public ActionResult UserBooking()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Emailid = User.Identity.Name;
            var acc = dc.Bookings.Where(s => s.CustEmail == Emailid).OrderByDescending(s=>s.Book);
            return View(acc);
        }
       
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.Where(s => s.BookId == Id).FirstOrDefault();
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.Where(s => s.BookId == Id).FirstOrDefault();

            dc.Bookings.Remove(acc);

            dc.SaveChanges();
            return RedirectToAction("BookSuccess");
        }

        

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Account");
        }
        

    }
}