namespace VivedyWebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using VivedyWebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VivedyWebApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "VivedyWebApp.Models.ApplicationDbContext";
        }

        protected override void Seed(VivedyWebApp.Models.ApplicationDbContext context)
        {
            //This method will be called after migrating to the latest version.

            //Uncomment the line to populate the database with data
            //AddEntities();
        }

        private void AddEntities()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole()
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Visitor"))
            {
                var role = new IdentityRole()
                {
                    Name = "Visitor"
                };
                roleManager.Create(role);
            }

            Cinema[] cinemas = new Cinema[]
            {
                new Cinema()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Albany",
                    Address = "219 Don McKinnon Drive, Albany, Auckland 0632"
                },
                new Cinema()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Newmarket",
                    Address = "64 Broadway, Newmarket, Auckland 1023"
                },
                new Cinema()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Sylvia Park",
                    Address = "286 Mount Wellington Highway, Mount Wellington, Auckland 1060"
                }
            };
            _context.Cinemas.AddRange(cinemas);
            _context.SaveChanges();

            Room[] rooms = new Room[]
            {
                // Sylvia Park
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = cinemas[2].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 2",
                    SeatsLayout = "small",
                    CinemaId = cinemas[2].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[2].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 4",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[2].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 5",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[2].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 6",
                    SeatsLayout = "large",
                    CinemaId = cinemas[2].Id
                },

                // Newmarket
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = cinemas[1].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 2",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[1].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[1].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 4",
                    SeatsLayout = "large",
                    CinemaId = cinemas[1].Id
                },

                // Albany
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = cinemas[0].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 2",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[0].Id
                },
                new Room()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = cinemas[0].Id
                }
            };
            _context.Rooms.AddRange(rooms);
            _context.SaveChanges();

            DateTime now = DateTime.Now;
            Movie[] movies = new Movie[]
            {
                new Movie()
                {
                    Id = "8323b6bd-b6b9-42b2-b97b-576cb908c073",
                    Name = "How to Train Your Dragon: The Hidden World",
                    Rating = 10,
                    ViewerRating = 4.5f,
                    Category = "Animation",
                    Description = "Hiccup aims to unite the vikings and the dragons in order to bring peace on the island of Berk. However, he must stop the evil Grimmel and his devious plans to wipe out all dragons.",
                    Duration = new TimeSpan(1,45,0),
                    Price = 14,
                    TrailerUrl = "https://www.youtube.com/embed/naW9U8MiUY0",
                    ReleaseDate = now.AddDays(-25),
                    ClosingDate = now.AddDays(3)
                },
                new Movie()
                {
                    Id = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    Name = "Thor: Ragnarok",
                    Rating = 14,
                    ViewerRating = 4.7f,
                    Category = "Action",
                    Description = "Deprived of his mighty hammer Mjolnir, Thor must escape the other side of the universe to save his home, Asgard, from Hela, the goddess of death.",
                    Duration = new TimeSpan(2,10,0),
                    Price = 21,
                    TrailerUrl = "https://www.youtube.com/embed/ue80QwXMRHg",
                    ReleaseDate = now.AddDays(-22),
                    ClosingDate = now.AddDays(6)
                },
                new Movie()
                {
                    Id = "70da58b3-2018-42fd-a2ab-6ce9ce8dbeb7",
                    Name = "Joker",
                    Rating = 16,
                    ViewerRating = 4.3f,
                    Category = "Drama",
                    Description = "Arthur Fleck, a party clown, leads an impoverished life with his ailing mother. However, when society shuns him and brands him as a freak, he decides to embrace the life of crime and chaos.",
                    Duration = new TimeSpan(2,0,2),
                    Price = 20,
                    TrailerUrl = "https://www.youtube.com/embed/zAGVQLHvwOY",
                    ReleaseDate = now.AddDays(-19),
                    ClosingDate = now.AddDays(9)
                },
                new Movie()
                {
                    Id = "834b4a02-e7d7-436b-b0b5-58f0a521bc76",
                    Name = "Cruella",
                    Rating = 12,
                    ViewerRating = 3.9f,
                    Category = "Comedy",
                    Description = "Estella is a young and clever grifter who's determined to make a name for herself in the fashion world. She soon meets a pair of thieves who appreciate her appetite for mischief, and together they build a life for themselves on the streets of London. However, when Estella befriends fashion legend Baroness von Hellman, she embraces her wicked side to become the raucous and revenge-bent Cruella.",
                    Duration = new TimeSpan(2,14,0),
                    Price = 21,
                    TrailerUrl = "https://www.youtube.com/embed/gmRKv7n2If8",
                    ReleaseDate = now.AddDays(-16),
                    ClosingDate = now.AddDays(12)
                },
                new Movie()
                {
                    Id = "350d19c2-561a-48e6-910c-9541bdce469b",
                    Name = "Army of the Dead",
                    Rating = 16,
                    ViewerRating = 4.4f,
                    Category = "Horror",
                    Description = "After a zombie outbreak in Las Vegas, a group of mercenaries takes the ultimate gamble by venturing into the quarantine zone for the greatest heist ever.",
                    Duration = new TimeSpan(2,28,0),
                    Price = 22,
                    TrailerUrl = "https://www.youtube.com/embed/tI1JGPhYBS8",
                    ReleaseDate = now.AddDays(-13),
                    ClosingDate = now.AddDays(15)
                },
                new Movie()
                {
                    Id = "401b05cc-76ad-4f0c-887e-fc310fc03ec0",
                    Name = "The Marksman",
                    Rating = 16,
                    ViewerRating = 4.1f,
                    Category = "Thriller",
                    Description = "Jim is a former Marine who lives a solitary life as a rancher along the Arizona-Mexican border. But his peaceful existence soon comes crashing down when he tries to protect a boy on the run from members of a vicious cartel.",
                    Duration = new TimeSpan(1,48,0),
                    Price = 14,
                    TrailerUrl = "https://www.youtube.com/embed/B_T0F36YEi0",
                    ReleaseDate = now.AddDays(-10),
                    ClosingDate = now.AddDays(18)
                },
                new Movie()
                {
                    Id = "e6e72458-4fcc-40b2-a772-fcc81e93164e",
                    Name = "The Little Things",
                    Rating = 14,
                    ViewerRating = 3.6f,
                    Category = "Crime",
                    Description = "Deputy Sheriff Joe \"Deke\" Deacon joins forces with Sgt. Jim Baxter to search for a serial killer who's terrorizing Los Angeles. As they track the culprit, Baxter is unaware that the investigation is dredging up echoes of Deke's past, uncovering disturbing secrets that could threaten more than his case.",
                    Duration = new TimeSpan(2,8,0),
                    Price = 20,
                    TrailerUrl = "https://www.youtube.com/embed/1HZAnkxdYuA",
                    ReleaseDate = now.AddDays(-7),
                    ClosingDate = now.AddDays(21)
                },
                new Movie()
                {
                    Id = "365aaaf3-0121-42ff-8a39-b5afcd054ec4",
                    Name = "The Unholy",
                    Rating = 14,
                    ViewerRating = 4.1f,
                    Category = "Horror",
                    Description = "A girl inexplicably gains the power to heal the sick after a supposed visitation from the Virgin Mary. As word spreads and people flock to witness her miracles, a disgraced journalist visits the small New England town to investigate. However, when strange events start to occur, he soon wonders if these phenomena are the result of something more sinister.",
                    Duration = new TimeSpan(1,39,0),
                    Price = 13,
                    TrailerUrl = "https://www.youtube.com/embed/5zne4Rb37Ns",
                    ReleaseDate = now.AddDays(-4),
                    ClosingDate = now.AddDays(27)
                },
                new Movie()
                {
                    Id = "cc7c5039-8b51-494b-affb-0a1f6ea6c9c0",
                    Name = "Raya and the Last Dragon",
                    Rating = 8,
                    ViewerRating = 3.9f,
                    Category = "Family",
                    Description = "Raya, a warrior, sets out to track down Sisu, a dragon, who transferred all her powers into a magical gem which is now scattered all over the kingdom of Kumandra, dividing its people.",
                    Duration = new TimeSpan(1,57,0),
                    Price = 15,
                    TrailerUrl = "https://www.youtube.com/embed/1VIZ89FEjYI",
                    ReleaseDate = now.AddDays(2),
                    ClosingDate = now.AddDays(30)
                },
                new Movie()
                {
                    Id = "79fc64b8-8727-4114-a9b2-4048f859276b",
                    Name = "The Dig",
                    Rating = 13,
                    ViewerRating = 3.5f,
                    Category = "Drama",
                    Description = "In the late 1930s, wealthy landowner Edith Pretty hires amateur archaeologist Basil Brown to investigate the mounds on her property in England. He and his team discover a ship from the Dark Ages while digging up a burial ground.",
                    Duration = new TimeSpan(1,52,0),
                    Price = 15,
                    TrailerUrl = "https://www.youtube.com/embed/JZQz0rkNajo",
                    ReleaseDate = now.AddDays(5),
                    ClosingDate = now.AddDays(33)
                }
            };
            _context.Movies.AddRange(movies);
            _context.SaveChanges();

            Screening[] screenings = new Screening[]
            {
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(4).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[6].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(4).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[10].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(1).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(2).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(3).AddHours(4),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                },
                new Screening()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = now.AddDays(4).AddHours(1),
                    MovieId = movies[7].Id,
                    RoomId = rooms[9].Id
                }
            };
            _context.Screenings.AddRange(screenings);
            _context.SaveChanges();

            Booking[] bookings = new Booking[]
            {
                new Booking()
                {
                    Id = Guid.NewGuid().ToString(),
                    Seats = "54,42,43,55,56,44,",
                    PayedAmout = 78f,
                    VerificationTime = now,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = screenings[7].Id
                },
                new Booking()
                {
                    Id = Guid.NewGuid().ToString(),
                    Seats = "16,17,18,19,51,39,40,52,41,",
                    PayedAmout = 117f,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = screenings[7].Id
                },
                new Booking()
                {
                    Id = Guid.NewGuid().ToString(),
                    Seats = "43,44,45,33,32",
                    PayedAmout = 65f,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = screenings[8].Id
                },
                new Booking()
                {
                    Id = Guid.NewGuid().ToString(),
                    Seats = "21,22,23,24,25,",
                    PayedAmout = 65f,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = screenings[8].Id
                }
            };
            _context.Bookings.AddRange(bookings);
            _context.SaveChanges();
        }
    }
}
