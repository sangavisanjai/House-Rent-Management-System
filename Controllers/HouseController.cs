using HouseRentManagementSystem.Models;
using HouseRentManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace HouseRentManagementSystem.Controllers
{
    public class HouseController : Controller
    {
        // GET: House

        public ActionResult Index()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Email = User.Identity.Name;
            var obj = dc.RegisterUsers.FirstOrDefault(e => e.Email.Equals(Email));
            List<RegisterUser> register = new List<RegisterUser>();
            register.Add(obj);
            ViewBag.image = obj.ProfileImage;
            return View(register.AsEnumerable());
        }
        public ActionResult Admin()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Email = User.Identity.Name;
          /*//  var obj = dc.RegisterUsers.FirstOrDefault(e => e.Email.Equals(Email));
            var hou = dc.RegisterUsers.Where(s => s.Email == Email).SingleOrDefault();
            Session["UserName"] = Convert.ToString(hou.FirstName+" "+ hou.LastName);*/
            return View();

        }
        public ActionResult Addproperty()
        {

            return View();
        }
        public ActionResult ViewProperty()
        {
            string Emailid = User.Identity.Name;
            Database1Entities1 dc = new Database1Entities1();

            ViewBag.useremail = User.Identity.Name; ;
            return View(from hu in dc.Houses
                        select hu);
        }

        public ActionResult WaitList()
        {
            string Emailid = User.Identity.Name;
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.Where(s=>s.SellEmail== Emailid && s.Book == "request");
            return View(acc);
        }
        
        public ActionResult Acceptrequest(Booking Bookings, int? id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.FirstOrDefault(s => s.BookId == id);
            var hou = dc.Houses.Where(s => s.PropName == acc.PropName).FirstOrDefault();
            hou.BookDetails = "Decline";
            acc.Book = "Accepted";
            dc.SaveChanges();
            return RedirectToAction("WaitList", "House");
        }
       
     
        public ActionResult Declinerequest(Booking Bookings, int? id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.FirstOrDefault(s => s.BookId == id);

            acc.Book = "Declined";
            dc.SaveChanges();
            return RedirectToAction("WaitList", "House");

        }
        public ActionResult BookedList()
        {
            string Emailid = User.Identity.Name;
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Bookings.Where(s => s.SellEmail == Emailid && s.Book == "Accepted");
            return View(acc);
        }
        public ActionResult UserData()
        {
            Database1Entities1 dc = new Database1Entities1();

            var view = from House in dc.RegisterUsers
                       select House;
            return View(view);
        }

        public ActionResult UserDetails()
        {
            Database1Entities1 dc = new Database1Entities1();
            string Emailid = User.Identity.Name;
            var acc = dc.RegisterCustomers.FirstOrDefault(s => s.Email.Equals(Emailid));
            List<RegisterCustomer> register = new List<RegisterCustomer>();
            register.Add(acc);
            ViewBag.image = acc.ProfileImage;

            return View(register.AsEnumerable());
        }
        public ActionResult Edit1(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterCustomers.Where(s => s.Id == Id).FirstOrDefault();
            if (acc == null)
            {
                return HttpNotFound();
            }

            return View(acc);
        }

        [HttpPost]
        public ActionResult Edit1([Bind(Include = "Id,FirstName,LastName,Email,Password,MobileNo,AadharNo")] RegisterCustomer registerCustomer, HttpPostedFileBase image)
        {
            Database1Entities1 dc = new Database1Entities1();
              

            if (image != null)
            {
                registerCustomer.ProfileImage = new byte[image.ContentLength];
                image.InputStream.Read(registerCustomer.ProfileImage, 0, image.ContentLength);
            }
            if (ModelState.IsValid)

            {
               
                dc.Entry(registerCustomer).State = EntityState.Modified;
                dc.SaveChanges();
                return RedirectToAction("UserDetails");
            }
            return View(registerCustomer);
        }


        public ActionResult Edit(int Id)
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
        public ActionResult Edit(RegisterUser Registers)
        {
            Database1Entities1 dc = new Database1Entities1();
            dc.Entry(Registers).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("Admin", "House");
        }
       
        public ActionResult EditProperty(int Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Houses.Where(s => s.PropId == Id).FirstOrDefault();

            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }
        [HttpPost]
        public ActionResult EditProperty(House Houses)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.Houses.FirstOrDefault(s => s.PropId==Houses.PropId);
            acc.PropName = Houses.PropName;
            acc.PropName = Houses.PropName;
            acc.PropRent = Houses.PropRent;
            acc.PropDescription = Houses.PropDescription;
            acc.PropStatus = Houses.PropStatus;
            acc.PropType = Houses.PropType;
            acc.PropAddress = Houses.PropAddress;
            acc.PropContact = Houses.PropContact;
            acc.PropSize = Houses.PropSize;
            acc.PropFacing = Houses.PropFacing;
            acc.PetFriendly = Houses.PetFriendly;
            acc.AvailableFor = Houses.AvailableFor;
            acc.Flooring = Houses.Flooring;
            acc.AvailableFrom = Houses.AvailableFrom;
            dc.SaveChanges();
            return RedirectToAction("Admin", "House");
        }
        public ActionResult Details(int? Id)
        {
           if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterUsers.Where(s => s.Id == Id).FirstOrDefault();
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View("Admin");
        }
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterUsers.Where(s => s.Id == Id).FirstOrDefault();
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }

        // POST: PersonalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            Database1Entities1 dc = new Database1Entities1();
            var acc = dc.RegisterUsers.Where(s => s.Id == Id).FirstOrDefault();
            dc.RegisterUsers.Remove(acc);
            dc.SaveChanges();
            return RedirectToAction("Admin");
        }


        [HttpPost]
        public ActionResult SaveAddpropertyDetails(Addproperty PropertyDetails, IEnumerable<HttpPostedFileBase> image1)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
           
                using (var databaseContext = new Database1Entities1())
                {

                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    House reglog = new House();
                    var userWithSameName = databaseContext.Houses.Where(m => m.PropName == PropertyDetails.PropName).FirstOrDefault(); //checking if the emailid already exits for any user
                    string userId = User.Identity.Name;                                                                                                     //var user = databaseContext.RegisterUsers.Where(query => query.Email.Equals(registerDetails.Email)).SingleOrDefault();
                                                                                                                                                            //If user is present, then true is returned.
                                                                                                                                                            //Save all details in RegitserUser object
                    if (userWithSameName == null)
                    {

                        reglog.PropName = PropertyDetails.PropName;
                        reglog.PropRent = PropertyDetails.Proprent;
                        reglog.PropDescription = PropertyDetails.PropDescription;
                        reglog.PropStatus = PropertyDetails.PropStatus;
                        reglog.PropType = PropertyDetails.PropType;
                        reglog.PropAddress = PropertyDetails.PropAddress;
                        reglog.PropContact = PropertyDetails.PropContact;
                        reglog.PropSize = PropertyDetails.PropSize;
                        reglog.PropFacing = PropertyDetails.PropFacing;
                        reglog.PetFriendly = PropertyDetails.PetFriendly;
                        reglog.AvailableFor = PropertyDetails.AvailableFor;
                        reglog.Flooring = PropertyDetails.Flooring;
                        reglog.AvailableFrom = PropertyDetails.AvailableFrom;
                        reglog.Email = userId;
                    reglog.BookDetails = "Added";
                    int c = 1;
                   foreach(var image in image1)
                    {
                        if (c == 1) { 
                        reglog.HImg1 = new byte[image.ContentLength];
                        image.InputStream.Read(reglog.HImg1, 0, image.ContentLength);
                            

                        }
                        else if (c == 2)
                        {
                            reglog.HImg2 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg2, 0, image.ContentLength);
                            

                        }
                       else if (c == 3)
                        {
                            reglog.HImg3 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg3, 0, image.ContentLength);
                            

                        }
                        else if (c == 4)
                        {
                            reglog.HImg4 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg4, 0, image.ContentLength);
                            

                        }
                        else if (c == 5)
                        {
                            reglog.HImg5 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg5, 0, image.ContentLength);
                        }
                        else if (c == 6)
                        {
                            reglog.HImg6 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg6, 0, image.ContentLength);
                        }
                        else if (c == 7)
                        {
                            reglog.HImg7 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg7, 0, image.ContentLength);
                        }
                        else if (c ==8)
                        {
                            reglog.HImg8 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg8, 0, image.ContentLength);
                        }
                        else if (c == 9)
                        {
                            reglog.HImg9 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg9, 0, image.ContentLength);
                        }
                        else if (c == 10)
                        {
                            reglog.HImg10 = new byte[image.ContentLength];
                            image.InputStream.Read(reglog.HImg10, 0, image.ContentLength);
                        }
                        c++;
                    }
                      /*  if (image1 != null)
                        {
                            reglog.HImg1 = new byte[image1[0].ContentLength];
                            image1[0].InputStream.Read(reglog.HImg1, 0, image1[0].ContentLength);
                        reglog.HImg2 = new byte[image1[1].ContentLength];
                        image1[1].InputStream.Read(reglog.HImg2, 0, image1[1].ContentLength);
                        reglog.HImg3 = new byte[image1[2].ContentLength];
                        image1[2].InputStream.Read(reglog.HImg3, 0, image1[2].ContentLength);
                        reglog.HImg4 = new byte[image1[3].ContentLength];
                        image1[3].InputStream.Read(reglog.HImg4, 0, image1[3].ContentLength);
                    }*/
                        try
                        {
                            databaseContext.Houses.Add(reglog);
                            databaseContext.SaveChanges();


                            ViewBag.saved = "House Added Successful!";
                            return View("Admin");
                        }
                        catch (NullReferenceException ex)
                        {
                            ViewBag.Message = "Somthing Wrong Please Try Again" + ex;
                            return View("Admin");
                        }


                    }
                    else
                    {
                        ViewBag.Message = "House Name Already Exists";

                        return View("AddProperty");
                    }
                }
            
             

        }



        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index","Account");
        }
    }
}