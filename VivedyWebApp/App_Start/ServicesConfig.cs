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
        public Task<int> Send(string to, string subject, string body)
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
                return Task.FromResult(1);
            }

            catch
            {
                return Task.FromResult(0);
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
        public async Task<int> SendRegisterEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Thank you for regestering on our website. Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            return await mailService.Send(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'forgot password' email to the user with a security check callback url
        /// </summary>
        public async Task<int> SendForgotPasswordEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Password Resetting";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to reset password for your account on <a href=\"vivedy.azurewebsites.net/Home/Index\">vivedy.azurewebsites.net</a>.";
            EmailService mailService = new EmailService();
            return await mailService.Send(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Sends a 'changes email' email to the user with a security check callback url
        /// </summary>
        public async Task<int> SendChangedEmailEmailTo(ApplicationUser user, string callbackUrl)
        {
            string subject = "Email Confirmation";
            string mailbody = "<b>Hi " + user.Name + "</b><br/>You have changed your email address on our website.<br/>Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
            EmailService mailService = new EmailService();
            return await mailService.Send(user.Email, subject, mailbody);
        }

        /// <summary>
        /// Returns List of items for dropdown with roles
        /// </summary>
        public async Task<List<SelectListItem>> GetRoleSelectListItems(string selectedRole = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<IdentityRole> roles = await db.Roles.ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (IdentityRole role in roles)
            {
                items.Add(new SelectListItem()
                {
                    Value = role.Name,
                    Text = role.Name,
                    Selected = (selectedRole != null && selectedRole == role.Name) ? true : false
                });
            }
            return items;
        }

        /// <summary>
        /// Returns the name of the role GetRoleName
        /// </summary>
        public string GetRoleName(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Roles.Find(id).Name;
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
            List<TEntity> entities = await dbSet.ToListAsync();
            return entities;
        }

        /// <summary>
        /// Returns TEntity with the id provided
        /// </summary>
        public async Task<TEntity> Details(string id)
        {
            TEntity entity = await dbSet.FindAsync(id);
            return entity;
        }

        /// <summary>
        /// Saves an entity without an Id, which is assigned automatically.
        /// Can have overrides for entities that require some other automatic modifications prior to creation
        /// <returns>The saved TEntity</returns>
        /// </summary>
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            TEntity result = dbSet.Add(entity);
            int saved = await db.SaveChangesAsync();
            return (saved == 1) ? result : null;
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
        public async Task<int> Delete(TEntity entity)
        {
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
                                        where movie.ClosingDate > DateTime.Now
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
                                       where movie.ClosingDate > DateTime.Now
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
            List<Movie> movies = await dbSet.Where(m => m.ReleaseDate <= DateTime.Now
                                            && m.ClosingDate > DateTime.Now)
                                            .OrderByDescending(m => m.ViewerRating)
                                            .ToListAsync();
            return movies;
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public async Task<List<Movie>> GetAllComming()
        {
            List<Movie> movies = await dbSet.Where(m => m.ReleaseDate > DateTime.Now)
                                            .OrderByDescending(m => m.ViewerRating)
                                            .ToListAsync();
            return movies;
        }

        /// <summary>
        /// Returns a List of all not yet resealed movies
        /// </summary>
        public async Task<List<Movie>> GetAllNotClosed()
        {
            List<Movie> movies = await dbSet.Where(m => m.ClosingDate > DateTime.Now)
                                            .OrderByDescending(m => m.ViewerRating)
                                            .ToListAsync();
            return movies;
        }

        /// <summary>
        /// Returns a List of top x current movies based on Viewer Rating
        /// </summary>
        public async Task<List<Movie>> GetTopShowing(int x)
        {
            List<Movie> movies = await dbSet.Where(m => m.ClosingDate > DateTime.Now && m.ReleaseDate < DateTime.Now)
                                .OrderByDescending(m => m.ViewerRating)
                                .Take(x)
                                .ToListAsync();
            return movies;
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up Price and ViewerRating
        /// </summary>
        public override async Task<Movie> Create(Movie entity)
        {
            entity.Price = (float)Math.Round(entity.Price, 2);
            entity.ViewerRating = (float)Math.Round(entity.ViewerRating, 1);
            Movie result = await base.Create(entity);
            return result;
        }

        /// <summary>
        /// Returns List of items for dropdown with movies
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems(string select = null)
        {
            List<Movie> movies = await dbSet.Where(m => m.ClosingDate > DateTime.Now)
                                            .OrderByDescending(m => m.ViewerRating)
                                            .ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Movie movie in movies)
            {
                items.Add(new SelectListItem()
                {
                    Value = movie.Id,
                    Text = movie.Name,
                    Selected = (select != null && select == movie.Id) ? true : false
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
                                         from scr in
                                                 from scr in db.Screenings
                                                 where scr.MovieId == id
                                                 group scr by scr.RoomId into scrs
                                                 select scrs.FirstOrDefault()
                                         join room in db.Rooms on scr.RoomId equals room.Id
                                         group room by room.CinemaId into rooms
                                         select rooms.FirstOrDefault()
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
            DateTime nowDate = DateTime.Now;
            TimeSpan nowTime = nowDate.TimeOfDay;
            List<Screening> screenings = await dbSet.Where(s => s.Movie.ClosingDate > DateTime.Now 
                                                    && s.StartDate > nowDate
                                                    && s.StartTime > nowTime)
                                                    .Include(s => s.Movie)
                                                    .OrderBy(s => s.StartTime)
                                                    .OrderBy(s => s.StartDate)
                                                    .ToListAsync();
            return screenings;
        }

        /// <summary>
        /// Returns all screenings with movies and rooms joined
        /// </summary>
        public async Task<List<Screening>> AllToListWithMoviesAndRooms() {
            List<Screening> screenings = await dbSet.Include(s => s.Movie)
                                                    .Include(s => s.Room)
                                                    .OrderBy(s => s.StartTime)
                                                    .OrderBy(s => s.StartDate)
                                                    .ToListAsync();
            return screenings;
        }

        /// <summary>
        /// Returns the screeening with the movie joined
        /// </summary>
        public async Task<Screening> DetailsWithMovie(string id)
        {
            Screening screening = await dbSet.Where(s => s.Id == id).Include(s => s.Movie).FirstOrDefaultAsync();
            return screening;
        }

        /// <summary>
        /// Returns the screeening with the movie and room joined
        /// </summary>
        public async Task<Screening> DetailsWithMovieAndRoom(string id)
        {
            Screening screening = await dbSet.Where(s => s.Id == id).Include(s => s.Movie).Include(s => s.Room).FirstOrDefaultAsync();
            return screening;
        }

        /// <summary>
        /// Generates and saves a set of screenigs for a list of days, where a screening is created for each time (from provided list) for that day.
        /// <returns>List of saved screenings</returns>
        /// </summary>
        public async Task<int> GenerateFrom(Screening model, List<DateTime> days, List<TimeSpan> times)
        {
            days.Sort((a, b) => a.CompareTo(b));
            times.Sort((a, b) => a.CompareTo(b));

            List<Screening> screenings = new List<Screening>();

            foreach(DateTime day in days)
            {
                foreach(TimeSpan time in times)
                {
                    Screening screening = new Screening()
                    {
                        Id = Guid.NewGuid().ToString(),
                        StartDate = day,
                        StartTime = time,
                        MovieId = model.MovieId,
                        RoomId = model.RoomId
                    };
                    if (await IsDuringMovieShowing(screening, model.Movie)
                        && await NoneOverlapWith(screening, model.Movie.Duration))
                    {
                        // check if previous new screening edns before this one starts
                        if (screenings.Last().StartDate.Add(screenings.Last().StartTime).Add(model.Movie.Duration) 
                            <= screening.StartDate.Add(screening.StartTime))
                        {
                            screenings.Add(screening);
                        }
                    }
                }
            }
            
            return await SaveRange(screenings);
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
        public async Task<bool> NoneOverlapWith(Screening scr, TimeSpan duration)
        {
            //all screenings in the room
            List<Screening> screenings = await dbSet.Where(s => s.RoomId == scr.RoomId)
                                                    .OrderBy(s => s.StartTime)
                                                    .OrderBy(s => s.StartDate)
                                                    .ToListAsync();
            if (screenings.Count == 0)
            {
                //if no screenings then nothing can overlap
                return true;
            }
            //screening that starts closest to the comparing screening
            DateTime origStartDateTime = scr.StartDate.Add(scr.StartTime);
            Screening closestPrev = screenings.FindLast(s => s.StartDate.Add(s.StartTime) <= origStartDateTime);
            closestPrev.Movie = await db.Movies.FindAsync(closestPrev.MovieId);
            if(closestPrev.StartDate.Add(closestPrev.StartTime.Add(closestPrev.Movie.Duration)) > origStartDateTime)
            {
                //if the previous screening ends after the comparing scr, then they overlap
                return false;
            }

            //at this points it is clear that the screeing isn't ovelaping with anything before it

            Screening closestNext;
            try
            {
                //there might be no more screenigns
                closestNext = screenings.ElementAt(screenings.IndexOf(closestPrev) + 1);
            }
            //there are no screenings after, so can't overlap
            catch (ArgumentOutOfRangeException) { return true; }

            DateTime finishTime = scr.StartDate.Add(scr.StartTime.Add(duration));
            if(finishTime > closestNext.StartDate.Add(closestNext.StartTime))
            {
                //if the next closest scr starts before the comapring finishes, they overlap
                return false;
            }
            else
            {
                //if the next starts after, then they don't overlap
                return true;
            }
        }

        /// <summary>
        /// Checks if the screening starts after the movie's release
        /// </summary>
        public async Task<bool> IsDuringMovieShowing(Screening screening, Movie movie = null)
        {
            screening.Movie = movie == null ? await db.Movies.FindAsync(screening.MovieId) : movie;
            bool result = screening.StartDate.Add(screening.StartTime) >= screening.Movie.ReleaseDate
                && (screening.StartDate.Add(screening.StartTime.Add(screening.Movie.Duration)) < screening.Movie.ClosingDate);
            return result;
        }

        /// <summary>
        /// Returns List of items for dropdown with screenigns grouped by movie name
        /// </summary>
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            DateTime nowDate = DateTime.Now;
            TimeSpan nowTime = nowDate.TimeOfDay;
            var screenings = await dbSet.Include(s => s.Movie)
                                        .Include(s => s.Room)
                                        .Include(s => s.Room.Cinema)
                                        .Where(s => s.Movie.ClosingDate > nowDate
                                        && s.StartDate > nowDate
                                        && s.StartTime > nowTime)
                                        .OrderBy(s => s.StartTime)
                                        .OrderBy(s => s.StartDate)
                                        .OrderBy(s => s.Room.Name)
                                        .OrderBy(s => s.Room.Cinema.Name)
                                        .OrderBy(s => s.Movie.Name)
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
                        Text = screening.Room.Cinema.Name + " | " + screening.Room.Name + " | " + screening.StartDate.ToString("dd/MM/yyyy") + " | " + screening.StartTime.ToString(@"hh\:mm"),
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
            DateTime nowDate = DateTime.Now;
            TimeSpan nowTime = nowDate.TimeOfDay;
            List<Screening> screenings = await dbSet.Include(s => s.Room)
                                                    .Where(s => s.MovieId == movieId
                                                    && s.Room.CinemaId == cinemaId
                                                    && s.StartDate > nowDate
                                                    && s.StartTime > nowTime)
                                                    .ToListAsync();
            var bookings = (await db.Bookings.Include(b => b.Screening)
                            .Include(b => b.Screening.Room)
                            .Where(b => b.Screening.MovieId == movieId
                            && b.Screening.Room.CinemaId == cinemaId
                            && b.Screening.StartDate > nowDate
                            && b.Screening.StartTime > nowTime)
                            .ToListAsync()).GroupBy(b => b.ScreeningId);
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
                            seats += getCount(b.Seats);
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

        /// <summary>
        /// Local function to get number of seats in the seats string
        /// </summary>
        private int getCount(string seats)
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
            DateTime nowDate = DateTime.Now;
            TimeSpan nowTime = nowDate.TimeOfDay;
            List<Booking> bookings = await dbSet.Include(b => b.Screening)
                                .Include(b => b.Screening.Movie)
                                .Include(b => b.Screening.Room)
                                .Include(b => b.Screening.Room.Cinema)
                                .Where(b => b.UserEmail == email 
                                && b.Screening.StartDate > nowDate
                                && b.Screening.StartTime > nowTime)
                                .OrderBy(b => b.Screening.StartTime)
                                .OrderBy(b => b.Screening.StartDate)
                                .ToListAsync();
            return bookings;
        }

        /// <summary>
        /// Reutrns all seats from bookings for screening with the Id
        /// </summary>
        public async Task<List<int>> GetSeatsForScreening(string id)
        {
            List<string> seatStrings = await dbSet.Where(b => b.ScreeningId == id)
                                                .Select(b => b.Seats).ToListAsync();
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
        public async Task<int> SendConfirmationEmail(Booking booking)
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
                                    $"<h4><b>Date:</b> {screening.StartDate.ToString("dd MMMM yyyy")}</h4>" +
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
            return await mailService.Send(booking.UserEmail, subject, mailbody);
        }

        /// <summary>
        /// Override of the base CreateFrom method. Rounds up PayedAmout
        /// </summary>
        public override async Task<Booking> Create(Booking entity)
        {
            entity.PayedAmout = (float)Math.Round(entity.PayedAmout, 2);
            Booking result = await base.Create(entity);
            return result;
        }

        /// <summary>
        /// Checks if any of the selected seats overlap with already booked seats        
        /// </summary>
        public async Task<bool> AnySeatsOverlapWith(List<int> seats, string id)
        {
            List<int> allSeats = await GetSeatsForScreening(id);
            bool result = allSeats.Intersect(seats).Any();
            return result;
        }
    
        /// <summary>
        /// Returns all bookings with the screenings joined
        /// </summary>
        /// <returns></returns>
        public async Task<List<Booking>> AllToListWithScreeningsAndMovies()
        {
            List<Booking> bookings = await dbSet.Include(b => b.Screening)
                                                .Include(b => b.Screening.Movie)
                                                .OrderBy(b => b.Screening.StartTime)
                                                .OrderBy(b => b.Screening.StartDate)
                                                .OrderBy(b => b.Screening.Movie.Name)
                                                .ToListAsync();
            return bookings;
        }

        /// <summary>
        /// Returns a bookings with the screening joined
        /// </summary>
        /// <returns></returns>
        public async Task<Booking> DetailsWithScreeningAndMovie(string Id)
        {
            Booking booking = await dbSet.Where(b => b.Id == Id)
                                        .Include(b => b.Screening)
                                        .Include(b => b.Screening.Movie)
                                        .FirstOrDefaultAsync();
            return booking;
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
        public async Task<List<SelectListItem>> GetSelectListItems(string select = null)
        {
            List<Room> rooms = await dbSet.Include(r => r.Cinema)
                                            .OrderBy(r => r.Name)
                                            .OrderBy(r => r.Cinema.Name)
                                            .ToListAsync();
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
                        Group = roomsGroup,
                        Selected = (select != null && select == room.Id) ? true : false
                    });
                }
            }
            return items;
        }

        /// <summary>
        /// Returns List of all rooms with the Cinema details joined
        /// </summary>
        public async Task<List<Room>> AllToListWithCinemas()
        {
            List<Room> rooms = await dbSet.Include(r => r.Cinema).OrderBy(r => r.Name).OrderBy(r => r.Cinema.Name).ToListAsync();
            return rooms;
        }
    
        /// <summary>
        /// Returns a room entity with the cinema joined
        /// </summary>
        public async Task<Room> DetailsWithCinema(string id)
        {
            Room room = await dbSet.Where(r => r.Id == id).Include(r => r.Cinema).FirstOrDefaultAsync();
            return room;
        }

        /// <summary>
        /// Returns List of items for dropdown with room layout types
        /// </summary>
        public List<SelectListItem> GetSelectLayoutListItems(string select = null)
        {
            List<SelectListItem> items = new List<SelectListItem>() { 
                new SelectListItem(){ Value = "Small", Text = "Small" },
                new SelectListItem(){ Value = "Medium", Text = "Medium" },
                new SelectListItem(){ Value = "Large", Text = "Large" }
            };
            items.Find(l => l.Value == select).Selected = true;
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
        public async Task<List<SelectListItem>> GetSelectListItems(string select = null)
        {
            List<Cinema> cinemas = await dbSet.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Cinema cinema in cinemas)
            {
                items.Add(new SelectListItem()
                {
                    Value = cinema.Id,
                    Text = cinema.Name,
                    Selected = (select != null && select == cinema.Id) ? true : false
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
