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
using VivedyWebApp.Models.ViewModels;

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
        public void Send(string to, string subject, string body)
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
        public void SendRegisterEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Thank you for regestering on our website. Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            mailService.Send(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'forgot password' email to the user with a security check callback url
        /// </summary>
        public void SendForgotPasswordEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Password Resetting";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to reset password for your account on <a href=\"vivedy.azurewebsites.net/Home/Index\">vivedy.azurewebsites.net</a>.";
            EmailService mailService = new EmailService();
            mailService.Send(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'changes email' email to the user with a security check callback url
        /// </summary>
        public void SendChangedEmailEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>You have changed your email address on our website.<br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            mailService.Send(user.Email, subject, mailbody);
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
        public async Task<List<TEntity>> AllToList()
        {
            return await dbSet.ToListAsync();
        }

        /// <summary>
        /// Returns TEntity with the id provided
        /// </summary>
        public async Task<TEntity> Details(string id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Saves an entity if all members initialized.
        /// <returns>The saved TEntity</returns>
        /// </summary>
        public async Task<TEntity> Create(TEntity entity)
        {
            TEntity result = dbSet.Add(entity);
            int saved = await db.SaveChangesAsync();
            return (saved == 1) ? result : null;
        }

        /// <summary>
        /// Saves an entity without an Id, which is assigned automatically.
        /// <returns>The saved TEntity</returns>
        /// </summary>
        public virtual async Task<TEntity> CreateFrom(TEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            return await Create(entity);
        }

        /// <summary>
        /// Saves changes to the TEntity. Can be overwritten to save only particular values
        /// </summary>
        public virtual async Task<TEntity> Edit(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            int saved = await db.SaveChangesAsync();
            return (saved == 1) ? entity : null;
        }

        /// <summary>
        /// Deletes the TEntity from the context
        /// </summary>
        public async Task<int> Delete(string id)
        {
            TEntity entity = dbSet.Find(id);
            dbSet.Remove(entity);
            int saved = await db.SaveChangesAsync();
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
        public async Task<List<SelectListItem>> GetCategoriesSelectListItems()
        {
            List<string> categories = await (from movie in dbSet
                                       orderby movie.Category
                                       group movie by movie.Category into movies
                                       select movies.FirstOrDefault().Category).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (string category in categories)
            {
                items.Add(new SelectListItem()
                {
                    Value = category,
                    Text = category
                });
            }
            items.Add(new SelectListItem()
            {
                Value = "Category",
                Text = "Category",
                Disabled = true,
                Selected = true
            });
            return items;
        }

        /// <summary>
        /// Returns a List of all ratings of existing movies
        /// </summary>
        public async Task<List<SelectListItem>> GetRatingsSelectListItems()
        {
            List<int> ratings = await (from movie in dbSet
                                       orderby movie.Rating
                                       group movie by movie.Rating into movies
                                       select movies.FirstOrDefault().Rating).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (int rating in ratings)
            {
                items.Add(new SelectListItem()
                {
                    Value = "+" + rating,
                    Text = "+" + rating
                });
            }
            items.Add(new SelectListItem()
            {
                Value = "Rating",
                Text = "Rating",
                Disabled = true,
                Selected = true
            });
            return items;
        }

        /// <summary>
        /// Returns a List of all currently resealed movies
        /// </summary>
        public async Task<List<Movie>> GetAllShowing()
        {
            return await dbSet.Where(
                m => m.ReleaseDate <= DateTime.Now
                && m.ClosingDate > DateTime.Now)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public async Task<List<Movie>> GetAllComming()
        {
            return await dbSet.Where(
                m => m.ReleaseDate > DateTime.Now)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public async Task<List<Movie>> GetAllNotClosed()
        {
            return await dbSet.Where(m => m.ClosingDate > DateTime.Now).ToListAsync();
        }

        /// <summary>
        /// Returns a List of top x current movies based on Viewer Rating
        /// </summary>
        public async Task<List<Movie>> GetTopNotClosed(int x)
        {
            return await dbSet.Where(m => m.ClosingDate > DateTime.Now).OrderByDescending(m => m.ViewerRating).Take(x).ToListAsync();
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up Price and ViewerRating
        /// </summary>
        public override async Task<Movie> CreateFrom(Movie entity)
        {
            entity.Price = (float)Math.Round(entity.Price, 2);
            entity.ViewerRating = (float)Math.Round(entity.ViewerRating, 1);
            return await base.CreateFrom(entity);
        }

        /// <summary>
        /// Returns List of items for dropdown with movies
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            List<Movie> movies = await dbSet.Where(m => m.ClosingDate > DateTime.Now).OrderByDescending(m => m.ViewerRating).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Movie movie in movies)
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
        /// Returns all cinemas for the movie with the Id
        /// </summary>
        public async Task<List<Cinema>> GetCinemasForMovie(string id)
        {
            List<Cinema> cinemas = await (from room in
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
                                    select cinema).ToListAsync();
            return cinemas;
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
        /// Reutrns all Screenings which are yet to be shown
        /// </summary>
        public async Task<List<Screening>> GetAllComming()
        {
            DateTime now = DateTime.Now;
            List<Screening> screenings = await (from s in dbSet
                                          join m in db.Movies on s.MovieId equals m.Id
                                          where m.ClosingDate > now && s.StartTime > now
                                          select s).ToListAsync();
            return screenings;
        }

        /// <summary>
        /// Returns the screeening with the movie attached to it
        /// </summary>
        public async Task<Screening> DetailsWithMovie(string id)
        {
            return await dbSet.Where(s => s.Id == id).Include(s => s.Movie).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Generates and saves a set of screenigs for a list of days, where a screening is created for each time (from provided list) for that day.
        /// <returns>List of saved screenings</returns>
        /// </summary>
        public async Task<List<Screening>> GenerateFrom(Screening model, List<DateTime> days, List<DateTime> times)
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
            return (await SaveRange(screenings) > 0) ? screenings : null;
        }

        /// <summary>
        /// Saves a range of screenings
        /// </summary>
        public async Task<int> SaveRange(List<Screening> range)
        {
            db.Screenings.AddRange(range);
            int saved = await db.SaveChangesAsync();
            return saved;
        }

        /// <summary>
        /// Checks if the screening overlaps in time with any other screening in the room it is assigned to
        /// </summary>
        public async Task<bool> AnyOverlapWith(Screening screening, TimeSpan duration)
        {
            List<Screening> screenings = await dbSet.Where(s => s.RoomId == screening.RoomId)
                                            .OrderBy(s => s.StartTime)
                                            .ToListAsync();
            if(screenings == null) 
            { 
                return false; 
            }
            DateTime originalFinishTime = screening.StartTime.Add(duration);
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
            Movie previousMovie = await db.Movies.FindAsync(previousScr.MovieId);
            DateTime previousScrFinishTime = previousScr.StartTime.Add(previousMovie.Duration);
            if(previousScrFinishTime > screening.StartTime) 
            { 
                return true; 
            }
            TimeSpan availableTime = nextScr.StartTime.Subtract(previousScrFinishTime);
            if(availableTime < duration) {
                return true; 
            }
            else {
                return false; 
            }
        }

        /// <summary>
        /// Checks if the screening starts after the movie's release
        /// </summary>
        public async Task<bool> IsDuringMovieShowing(Screening screening, Movie movie = null)
        {
            screening.Movie = movie == null ? await db.Movies.FindAsync(screening.MovieId) : movie;
            return screening.StartTime < screening.Movie.ReleaseDate
                && (screening.StartTime + screening.Movie.Duration) < screening.Movie.ClosingDate;
        }

        /// <summary>
        /// Returns List of items for dropdown with screenigns grouped by movie name
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            DateTime now = DateTime.Now;
            var screenings = await dbSet
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .Include(s => s.Room.Cinema)
                .Where(s => s.Movie.ClosingDate > now && s.StartTime > now)
                .OrderBy(s => s.Movie.Name)
                .OrderBy(s => s.Room.Cinema.Name)
                .OrderBy(s => s.Room.Name)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
            var groups = screenings.GroupBy(s => s.MovieId);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var group in groups)
            {
                var roomsGroup = new SelectListGroup() { Name = screenings.Find(s => s.MovieId == group.Key).Movie.Name };
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

        /// <summary>
        /// Returns details for all screenings for the movie which are in a selected cinema (including booked seats)
        /// </summary>
        public async Task<List<ScreeningDetails>> GetAllForMovieInCinema(string movieId, string cinemaId)
        {
            DateTime now = DateTime.Now;
            List<Screening> screenings = await (from s in db.Screenings
                                         join r in db.Rooms on s.RoomId equals r.Id
                                         where s.MovieId == movieId
                                         && r.CinemaId == cinemaId
                                         && s.StartTime > now
                                         select s).ToListAsync();
            var bookings = (from b in db.Bookings
                            join s in db.Screenings on b.ScreeningId equals s.Id
                            join r in db.Rooms on s.RoomId equals r.Id
                            where s.MovieId == movieId
                            && r.CinemaId == cinemaId
                            && s.StartTime > now
                            group b by b.ScreeningId);
            List<ScreeningDetails> list = new List<ScreeningDetails>();
            for (int i = 0; i < screenings.Count(); i++)
            {
                int seats = 0;
                foreach (var group in bookings)
                {
                    if (group.Key == screenings[i].Id)
                    {
                        foreach (var b in group)
                        {
                            seats += convCount(b.Seats);
                        }
                    }
                }
                list.Add(new ScreeningDetails
                {
                    Id = screenings[i].Id,
                    StartTime = screenings[i].StartTime,
                    BookedSeats = seats
                });
            }
            return list;
        }
        private int convCount(string seats)
        {
            int result = 0;
            foreach (string seat in seats.Split(','))
            {
                if (seat != null && seat != "") { result++; }
            }
            return result;
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
        public async Task<List<Booking>> GetAllComingForUser(string email)
        {
            DateTime now = DateTime.Now;
            return await (from b in dbSet
                    join s in db.Screenings on b.ScreeningId equals s.Id
                    join m in db.Movies on s.MovieId equals m.Id
                    join r in db.Rooms on s.RoomId equals r.Id
                    join c in db.Cinemas on r.CinemaId equals c.Id
                    where b.UserEmail == email
                    && s.StartTime > now
                    select b).ToListAsync();
        }

        /// <summary>
        /// Reutrns all seats from bookings for screening with the Id
        /// </summary>
        public async Task<List<int>> GetSeatsForScreening(string id)
        {
            List<string> seatStrings = await (from b in dbSet
                                        where b.ScreeningId == id
                                        select b.Seats).ToListAsync();
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
        /// Sends a 'booking confirmation' email for the booking
        /// </summary>
        public async void SendConfirmationEmail(Booking booking)
        {
            string htmlSeats = "";
            List<int> seats = ConvertSeatsToIntList(booking.Seats);
            foreach (int seat in seats)
            {
                htmlSeats += $"<li>{seat}</li>";
            }
            Screening screening = await db.Screenings.Where(s => s.Id == booking.ScreeningId).Include(s => s.Movie).FirstOrDefaultAsync();

            //Generating content to put into the QR code for later validation
            //Includes booking Id and UserEmail
            string qrCodeData = "VIVEDYBOOKING_" + booking.Id;
            string subject = "Booking Confirmation";
            ApplicationUser user = await db.Users.Where(u => u.Email == booking.UserEmail).FirstAsync();
            string greeting = user != null ? "<b>Hi " + user.Name + "</b><br/>" : "";
            
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
            mailService.Send(booking.UserEmail, subject, mailbody);
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up PayedAmout
        /// </summary>
        public override async Task<Booking> CreateFrom(Booking entity)
        {
            entity.PayedAmout = (float)Math.Round(entity.PayedAmout, 2);
            return await base.CreateFrom(entity);
        }

        /// <summary>
        /// Checks if any of the selected seats overlap with already booked seats        
        /// </summary>
        public async Task<bool> AnySeatsOverlapWith(List<int> seats, string id)
        {
            List<int> allSeats = await GetSeatsForScreening(id);
            return allSeats.Intersect(seats).Any();
        }
    }

    public class RoomsManager : GenericManager<Room>
    {
        /// <summary>
        /// Constructor for cases when a RoomsManager object is initialized as a DataManager member
        /// </summary>
        public RoomsManager(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Returns List of items for dropdown with rooms grouped by cinemas
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            List<Room> rooms = await dbSet.Include(r => r.Cinema).OrderBy(r => r.Name).OrderBy(r => r.Cinema.Name).ToListAsync();
            var groups = rooms.GroupBy(r => r.CinemaId);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var group in groups)
            {
                var roomsGroup = new SelectListGroup() { Name = rooms.Find(r => r.CinemaId == group.Key).Cinema.Name };
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
    }

    public class CinemasManager : GenericManager<Cinema>
    {
        /// <summary>
        /// Constructor for cases when a CinemasManager object is initialized as a DataManager member
        /// </summary>
        public CinemasManager(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Returns List of items for dropdown with cinemas
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            List<Cinema> cinemas = await AllToList();
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
    }

    public class Entities
    {
        public Entities()
        {
            db = new ApplicationDbContext();
            Movies = new MoviesManager(db);
            Screenings = new ScreeningsManager(db);
            Bookings = new BookingsManager(db);
            Cinemas = new CinemasManager(db);
            Rooms = new RoomsManager(db);
        }

        private readonly ApplicationDbContext db;

        public readonly MoviesManager Movies;

        public readonly ScreeningsManager Screenings;

        public readonly BookingsManager Bookings;

        public readonly CinemasManager Cinemas;

        public readonly RoomsManager Rooms;
    }
}
