using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VivedyWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VivedyWebApp
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        //Custom method for sending email over SMTP server
        public Task SendAsync(string to, string subject, string body)
        {
            // Plug in your email service here to send an email.
            string from = "vivedycinemas@gmail.com"; //From address 
            MailMessage message = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new NetworkCredential("vivedycinemas@gmail.com", "Techtorium12345");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        /// <summary>
        /// Sends a 'register' email to the user with a security check callback url
        /// </summary>
        public async void SendRegisterEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Thank you for regestering on our website. Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            await mailService.SendAsync(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'forgot password' email to the user with a security check callback url
        /// </summary>
        public async void SendForgotPasswordEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Password Resetting";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to reset password for your account on <a href=\"vivedy.azurewebsites.net/Home/Index\">vivedy.azurewebsites.net</a>.";
            EmailService mailService = new EmailService();
            await mailService.SendAsync(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'changes email' email to the user with a security check callback url
        /// </summary>
        public async void SendChangedEmailEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>You have changed your email address on our website.<br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            await mailService.SendAsync(user.Email, subject, mailbody);
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class GenericManager<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Constructor for cases when a manager object is initialized as a DataManager member
        /// </summary>
        public GenericManager(ApplicationDbContext db)
        {
            this.db = db;
            dbSet = db.Set<TEntity>();
        }
        protected readonly ApplicationDbContext db;

        protected readonly DbSet<TEntity> dbSet;

        /// <summary>
        /// Returns all entities as a List
        /// </summary>
        public List<TEntity> AllToList()
        {
            return dbSet.ToList();
        }

        /// <summary>
        /// Returns TEntity with the id provided
        /// </summary>
        public TEntity Details(string id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Saves an entity if all members initialized.
        /// <returns>The saved TEntity</returns>
        /// </summary>
        public TEntity Create(TEntity entity)
        {
            TEntity result = dbSet.Add(entity);
            int saved = db.SaveChanges();
            return (saved == 1) ? result : null;
        }

        /// <summary>
        /// Saves an entity without an Id, which is assigned automatically.
        /// <returns>The saved TEntity</returns>
        /// </summary>
        public virtual TEntity CreateFrom(TEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            return Create(entity);
        }

        /// <summary>
        /// Saves changes to the TEntity. Can be overwritten to save only particular values
        /// </summary>
        public virtual TEntity Edit(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            int saved = db.SaveChanges();
            return (saved == 1) ? entity : null;
        }

        /// <summary>
        /// Deletes the TEntity from the context
        /// </summary>
        public int Delete(string id)
        {
            TEntity entity = dbSet.Find(id);
            dbSet.Remove(entity);
            int saved = db.SaveChanges();
            return saved;
        }
    }

    public class MoviesManager : GenericManager<Movie>
    {
        /// <summary>
        /// Constructor for cases when a MoviesManager object is initialized as a DataManager member
        /// </summary>
        public MoviesManager(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Returns a List of all categories of existing movies
        /// </summary>
        public List<string> GetAllCategories()
        {
            List<string> categories = (from movie in dbSet
                                       orderby movie.Category 
                                       group movie by movie.Category into movies
                                       select movies.FirstOrDefault().Category).ToList();
            return categories;
        }

        /// <summary>
        /// Returns a List of all ratings of existing movies
        /// </summary>
        public List<string> GetAllRatingss()
        {
            List<int> ratings = (from movie in dbSet
                                       orderby movie.Rating
                                       group movie by movie.Rating into movies
                                       select movies.FirstOrDefault().Rating).ToList();
            List<string> mappedRatings = new List<string>();
            foreach (int rating in ratings)
            {
                string r = "+" + rating;
                if (mappedRatings.Contains(r))
                {
                    continue;
                }
                else
                {
                    mappedRatings.Add(r);
                }
            }
            return mappedRatings;
        }

        /// <summary>
        /// Returns a List of all currently resealed movies
        /// </summary>
        public List<Movie> GetAllReleased()
        {
            return dbSet.Where(
                m => m.ReleaseDate <= DateTime.Now
                && m.ClosingDate > DateTime.Now)
                .ToList();
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public List<Movie> GetAllCommingSoon()
        {
            return dbSet.Where(
                m => m.ReleaseDate > DateTime.Now)
                .ToList();
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public List<Movie> GetAllNotClosed()
        {
            return dbSet.Where(
                m => m.ClosingDate > DateTime.Now)
                .ToList();
        }

        /// <summary>
        /// Returns a List of top x current movies based on Viewer Rating
        /// </summary>
        public List<Movie> GetTop(int x)
        {
            return GetAllNotClosed().OrderBy(m => m.ViewerRating).Take(x).ToList();
        }

        /// <summary>
        /// Checks if the movie has already been released
        /// </summary>
        public bool IsReleased(Movie movie)
        {
            return movie.ReleaseDate <= DateTime.Now && movie.ClosingDate > DateTime.Now;
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up Price and ViewerRating
        /// </summary>
        public override Movie CreateFrom(Movie entity)
        {
            entity.Price = (float)Math.Round(entity.Price, 2);
            entity.ViewerRating = (float)Math.Round(entity.ViewerRating, 1);
            return base.CreateFrom(entity);
        }
    }

    public class ScreeningsManager : GenericManager<Screening>
    {
        /// <summary>
        /// Constructor for cases when a ScreeningsManager object is initialized as a DataManager member
        /// </summary>
        public ScreeningsManager(ApplicationDbContext db) : base(db)
        {
        }
        
        /// <summary>
        /// Saves a range of screenings
        /// </summary>
        public int SaveRange(List<Screening> range)
        {
            db.Screenings.AddRange(range);
            int saved = db.SaveChanges();
            return saved;
        }

        /// <summary>
        /// Reutrns all Screenings for a movie with the id
        /// </summary>
        public List<Screening> GetAllForMovie(string id)
        {
            return dbSet.Where(s => s.MovieId == id).ToList();
        }

        /// <summary>
        /// Reutrns all Screenings which are yet to be shown
        /// </summary>
        public List<Screening> GetAllComming()
        {
            return dbSet.Where(s =>s.StartTime > DateTime.Now).ToList();
        }

        /// <summary>
        /// Returns the screeening with the movie attached to it
        /// </summary>
        public Screening GetDetailsWithMovie(string id)
        {
            return (from scr in dbSet
                    join m in db.Movies on scr.MovieId equals m.Id
                    where scr.Id == id
                    select scr).FirstOrDefault();
        }

        /// <summary>
        /// Generates and saves a set of screenigs for a list of days, where a screening is created for each time (from provided list) for that day.
        /// <returns>List of saved screenings</returns>
        /// </summary>
        public List<Screening> GenerateFrom(Screening model, List<DateTime> days, List<DateTime> times)
        {
            List<Screening> screenings = new List<Screening>();
            foreach(DateTime day in days)
            {
                foreach(DateTime time in times)
                {
                    screenings.Add(new Screening()
                    {
                        Id = Guid.NewGuid().ToString(),
                        StartTime = new DateTime(day.Year, day.Month, day.Day, time.Hour, time.Minute, time.Second),
                        MovieId = model.MovieId,
                        RoomId = model.RoomId
                    });
                }
            }
            dbSet.AddRange(screenings);
            int saved = db.SaveChanges();
            return (saved == 1) ? screenings : null;
        }

        /// <summary>
        /// Checks if the screening overlaps in time with any other screening in the room it is assigned to
        /// </summary>
        public bool AnyOverlapWith(Screening screening)
        {
            List<Screening> screenings = dbSet.Where(s => s.RoomId == screening.RoomId)
                                            .OrderBy(s => s.StartTime)
                                            .ToList();
            if(screenings == null) 
            { 
                return false; 
            }
            Movie originalMovie = db.Movies.Find(screening.MovieId);
            DateTime originalFinishTime = screening.StartTime.Add(originalMovie.Duration);
            Screening nextScr = screenings.Find(s => originalFinishTime <= s.StartTime);
            if(nextScr == null) 
            { 
                return false; 
            }
            Screening previousScr;
            try
            {
                previousScr = screenings.ElementAt(screenings.IndexOf(nextScr) - 1);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            DateTime previousScrFinishTime = previousScr.StartTime.Add(db.Movies.Find(previousScr.MovieId).Duration);
            if(previousScrFinishTime > screening.StartTime) 
            { 
                return true; 
            }
            TimeSpan availableTime = nextScr.StartTime.Subtract(previousScrFinishTime);
            if(availableTime < originalMovie.Duration) {
                return true; 
            }
            else {
                return false; 
            }
        }
    }

    public class BookingsManager : GenericManager<Booking>
    {
        /// <summary>
        /// Constructor for cases when a BookingsManager object is initialized as a DataManager member
        /// </summary>
        public BookingsManager(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Reutrns all bookings of the user with the email with screening, movie, room, and cinema attached
        /// </summary>
        public List<Booking> GetAllComingForUser(string email)
        {
            return (from b in dbSet
                    join s in db.Screenings on b.ScreeningId equals s.Id
                    join m in db.Movies on s.MovieId equals m.Id
                    join r in db.Rooms on s.RoomId equals r.Id
                    join c in db.Cinemas on r.CinemaId equals c.Id
                    where b.UserEmail == email
                    && s.StartTime > DateTime.Now
                    select b).ToList();
        }

        /// <summary>
        /// Reutrns all seats from bookings for screening with the Id
        /// </summary>
        public List<int> GetSeatsForScreening(string id)
        {
            List<string> seatStrings = (from b in dbSet
                                        where b.ScreeningId == id
                                        select b.Seats).ToList();
            List<int> seats = new List<int>();
            if (seatStrings != null)
            {
                foreach (string set in seatStrings)
                {
                    foreach (string seat in set.Split(','))
                    {
                        if (seat != null && seat != "") { seats.Add(Convert.ToInt32(seat)); }
                    }
                }
            }
            return seats;
        }

        /// <summary>
        /// Converts the seats from a Booking.Seats into a list if strings
        /// <returns>List of strings</returns>
        /// </summary>
        public List<int> ConvertSeatsToIntList(string seats)
        {
            List<int> result = new List<int>();
            foreach (string seat in seats.Split(','))
            {
                if (seat != null && seat != "") { result.Add(Convert.ToInt32(seat)); }
            }
            return result;
        }

        /// <summary>
        /// Converts the seats from a Booking.Seats into a list if int
        /// <returns>List of int</returns>
        /// </summary>
        public List<string> ConvertSeatsToStringList(string seats)
        {
            List<string> result = new List<string>();
            foreach (string seat in seats.Split(','))
            {
                if (seat != null && seat != "") { result.Add(seat); }
            }
            return result;
        }
        
        /// <summary>
        /// Sends a 'booking confirmation' email for the booking
        /// </summary>
        public async void SendBookingConfirmationEmail(Booking booking)
        {
            string htmlSeats = "";
            List<string> seats = ConvertSeatsToStringList(booking.Seats);
            foreach (string seat in seats)
            {
                htmlSeats += $"<li>{seat}</li>";
            }
            Screening screening = (from scr in db.Screenings
                                   join m in db.Movies on scr.MovieId equals m.Id
                                   where scr.Id == booking.ScreeningId
                                   select scr).FirstOrDefault();

            //Generating content to put into the QR code for later validation
            //Includes booking Id and UserEmail
            string dataToEncode = booking.Id;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(dataToEncode);
            string qrCodeData = "VIVEDYBOOKING_" + Convert.ToBase64String(plainTextBytes);
            string subject = "Booking Confirmation";
            List<ApplicationUser> Users = db.Users.Where(user => user.Email == booking.UserEmail).ToList();
            string greeting = Users.Count() > 0 ? "<b>Hi " + Users[0].Name + "</b><br/>" : "";
            
            //Generating an HTML body for the email
            string mailbody = $"<div id=\"mainEmailContent\" style=\"-webkit-text-size-adjust: 100%; font-family: Verdana,sans-serif;\">" +
                                $"<img style=\"display: block; margin-left: auto; margin-right: auto; height: 3rem; width: 3rem;\" src=\"http://vivedy.azurewebsites.net/favicon.ico\">" +
                                greeting +
                                $"<b><h2 style=\"text-align: center;\">Thank you for purchasing tickets at our website!</h2></b>" +
                                $"<p>Below are details of your purchase.</p>" +
                                $"<i><p>Please present this email when you arrive to the cinema to the our stuuf at the entrance to the auditorium.</p></i>" +
                                $"<div style=\"box-sizing: inherit; padding: 0.01em 16px; margin-top: 16px; margin-bottom: 16px; box-shadow: 0 2px 5px 0 rgba(0,0,0,0.16),0 2px 10px 0 rgba(0,0,0,0.12);\">" +
                                    $"<h3>{screening.Movie.Name}</h3>" +
                                    $"<h4><b>Date:</b> {screening.StartTime.ToString("dd MMMM yyyy")}</h4>" +
                                    $"<h4><b>Time:</b> {screening.StartTime.ToString("hh:mm tt")}</h4>" +
                                    $"<h4><b>Your seats:</b> </h4>" +
                                    $"<ul>" +
                                        $"{htmlSeats}" +
                                    $"</ul>" +
                                    $"<h4><b>Total amount paid:</b> ${booking.PayedAmout}</h4>" +
                                    $"<br>" +
                                    $"<img style=\"display: block; margin-left: auto; margin-right: auto;\" src=\"https://api.qrserver.com/v1/create-qr-code/?size=250&bgcolor=255-255-255&color=9-10-15&qzone=0&data=" + qrCodeData + "\" alt=\"Qrcode\">" +
                                    $"<br>" +
                                $"</div>" +
                                $"<p>Go to our <a href=\"vivedy.azurewebsites.net\">website</a> to find more movies!</p>" +
                              $"</div>";
            EmailService mailService = new EmailService();
            await mailService.SendAsync(booking.UserEmail, subject, mailbody);
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up PayedAmout
        /// </summary>
        public override Booking CreateFrom(Booking entity)
        {
            entity.PayedAmout = (float)Math.Round(entity.PayedAmout, 2);
            return base.CreateFrom(entity);
        }

    }

    public class Entities
    {
        public Entities()
        {
            db = new ApplicationDbContext();
            Movies = new MoviesManager(db);
            Screenings = new ScreeningsManager(db);
            Bookings = new BookingsManager(db);
            Cinemas = new GenericManager<Cinema>(db);
            Rooms = new GenericManager<Room>(db);
        }

        private readonly ApplicationDbContext db;

        public readonly MoviesManager Movies;

        public readonly ScreeningsManager Screenings;

        public readonly BookingsManager Bookings;

        public readonly GenericManager<Cinema> Cinemas;

        public readonly GenericManager<Room> Rooms;

        /// <summary>
        /// Returns all cinemas for the movie with the Id
        /// </summary>
        public List<Cinema> GetCinemasForMovie(string id)
        {
            List<Cinema> cinemas = (from room in 
                                        (from scr in 
                                                (from scr in db.Screenings
                                                where scr.MovieId == id 
                                                group scr by scr.RoomId into scrs
                                                select scrs.FirstOrDefault())
                                            join room in db.Rooms on scr.RoomId equals room.Id 
                                            group room by room.CinemaId into rooms 
                                            select rooms.FirstOrDefault())
                                    join cinema in db.Cinemas on room.CinemaId equals cinema.Id
                                    orderby cinema.Name
                                    select cinema).ToList();
            return cinemas;
        }

        /// <summary>
        /// Returns all screenings for the movie which are in a selected cinema
        /// </summary>
        public List<Screening> GetScreeningsForMovieInCinema(string movieId, string cinemaId)
        {
            List<Screening> screenings = (from scr in db.Screenings
                                          join room in db.Rooms on scr.Id equals room.Id
                                          where scr.MovieId == movieId 
                                          && room.CinemaId == cinemaId
                                          select scr).ToList();
            return screenings;
        }

        /// <summary>
        /// Returns List of items for dropdown with movies
        /// </summary>
        public List<SelectListItem> GetMovieSelectListItems()
        {
            List<Movie> movies = Movies.GetAllNotClosed();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach(Movie movie in movies)
            {
                items.Add(new SelectListItem()
                {
                    Value = movie.Id,
                    Text = movie.Name
                });
            }
            return items;
        }

        /// <summary>
        /// Returns List of items for dropdown with rooms grouped by cinemas
        /// </summary>
        public List<SelectListItem> GetRoomSelectListItems()
        {
            var rooms = (from room in db.Rooms
                         join cinema in db.Cinemas on room.CinemaId equals cinema.Id
                         orderby room.Name, cinema.Name
                         group room by room.CinemaId);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var group in rooms)
            {
                var roomsGroup = new SelectListGroup() { Name = group.First().Cinema.Name };
                foreach (var room in group)
                {
                    items.Add(new SelectListItem()
                    {
                        Value = room.Id,
                        Text = room.Name,
                        Group = roomsGroup
                    });
                }
            }
            return items;
        }

        /// <summary>
        /// Returns List of items for dropdown with cinemas
        /// </summary>
        public List<SelectListItem> GetCinemaSelectListItems()
        {
            List<Cinema> cinemas = db.Cinemas.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Cinema cinema in cinemas)
            {
                items.Add(new SelectListItem()
                {
                    Value = cinema.Id,
                    Text = cinema.Name
                });
            }
            return items;
        }

        /// <summary>
        /// Returns List of items for dropdown with screenigns grouped by movie name
        /// </summary>
        public List<SelectListItem> GetScreeningSelectListItems()
        {
            var screenings = (from s in db.Screenings
                           join m in db.Movies on s.MovieId equals m.Id
                           join r in db.Rooms on s.RoomId equals r.Id
                           join c in db.Cinemas on r.CinemaId equals c.Id
                           orderby m.Name, c.Name, r.Name, s.StartTime
                           group s by s.Movie.Name);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var group in screenings)
            {
                var roomsGroup = new SelectListGroup() { Name = group.First().Movie.Name };
                foreach (var screening in group)
                {
                    items.Add(new SelectListItem()
                    {
                        Value = screening.Id,
                        Text = screening.Room.Cinema.Name + " | " + screening.Room.Name + " | " + screening.StartTime.ToString("dd/MM/yy H:mm"),
                        Group = roomsGroup
                    });
                }
            }
            return items;
        }
    }
}
