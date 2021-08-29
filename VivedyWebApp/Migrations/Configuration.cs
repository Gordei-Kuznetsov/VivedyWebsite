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
                    Id = "ba1c35e5-9c68-4459-901d-771d9ec59822",
                    Name = "Albany",
                    Address = "219 Don McKinnon Drive, Albany, Auckland 0632"
                },
                new Cinema()
                {
                    Id = "8f4a903b-5438-4570-84fd-76e414d6408c",
                    Name = "Newmarket",
                    Address = "64 Broadway, Newmarket, Auckland 1023"
                },
                new Cinema()
                {
                    Id = "79099ae1-1ed2-470c-bebe-87a74022a089",
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
                    Id = "a66f342b-b765-46a1-9266-72a2afcae032",
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },
                new Room()
                {
                    Id = "0a8a1b78-6ffb-43e2-bc85-d267c2f02f99",
                    Name = "Room 2",
                    SeatsLayout = "small",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },
                new Room()
                {
                    Id = "4093c20a-b6da-4364-9bb1-745287c404ed",
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },
                new Room()
                {
                    Id = "8fafec28-47fa-411e-b287-207d5eec4c20",
                    Name = "Room 4",
                    SeatsLayout = "medium",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },
                new Room()
                {
                    Id = "3b0df583-c92f-4687-9c6c-02fce865a4f3",
                    Name = "Room 5",
                    SeatsLayout = "medium",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },
                new Room()
                {
                    Id = "73c562cd-c5ae-4aed-a27d-e50d1d901d7a",
                    Name = "Room 6",
                    SeatsLayout = "large",
                    CinemaId = "79099ae1-1ed2-470c-bebe-87a74022a089"
                },

                // Newmarket
                new Room()
                {
                    Id = "f451615e-41d7-4e40-ac35-8fcf9fb8d660",
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = "8f4a903b-5438-4570-84fd-76e414d6408c"
                },
                new Room()
                {
                    Id = "d29d43d3-04f6-4cda-9256-b36f4fa5cd61",
                    Name = "Room 2",
                    SeatsLayout = "medium",
                    CinemaId = "8f4a903b-5438-4570-84fd-76e414d6408c"
                },
                new Room()
                {
                    Id = "d8c2b980-5200-4bb7-b7d9-59c013c20411",
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = "8f4a903b-5438-4570-84fd-76e414d6408c"
                },
                new Room()
                {
                    Id = "58e972d8-d905-4762-bb28-875ecffef345",
                    Name = "Room 4",
                    SeatsLayout = "large",
                    CinemaId = "8f4a903b-5438-4570-84fd-76e414d6408c"
                },

                // Albany
                new Room()
                {
                    Id = "8927f698-ee9d-4490-862b-9dccab7f4703",
                    Name = "Room 1",
                    SeatsLayout = "small",
                    CinemaId = "ba1c35e5-9c68-4459-901d-771d9ec59822"
                },
                new Room()
                {
                    Id = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5",
                    Name = "Room 2",
                    SeatsLayout = "medium",
                    CinemaId = "ba1c35e5-9c68-4459-901d-771d9ec59822"
                },
                new Room()
                {
                    Id = "1f05b3f1-9905-4cc5-b3a3-38278fd75d01",
                    Name = "Room 3",
                    SeatsLayout = "medium",
                    CinemaId = "ba1c35e5-9c68-4459-901d-771d9ec59822"
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
                    Id = "00933004-96cf-4d47-87fb-c4f9f9e1ad5e",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "3e271e85-0620-4ff7-8480-55eeb8ccf499",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "3fe390f2-239a-4124-9c3f-3be7851d4654",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "549979d3-0c79-46ab-bfe5-20a9663611f1",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "5fe26106-3d63-4782-b16b-6b50e8dc0785",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "63ee0e82-9b91-407d-aad7-de9b69058af6",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "668f9f0c-0269-450e-989a-cc530f259948",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "692b2906-b990-43db-8f93-9b2351e58f6d",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "6a6652d2-ae03-42ad-9e7c-b58073334d43",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "70adfc70-4460-4c3c-bcc4-c595c5f3482f",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "79d47d94-3bf6-43ab-a457-79ece982c3e0",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "86e2dde2-7387-4e36-a8d6-2f8cf880aa77",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "936f8cdc-ce52-47d4-91bd-fe219e1218a0",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "988b7a58-6dc7-4b57-b830-08df1abd9314",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                },
                new Screening()
                {
                    Id = "a8b63b1f-7f05-419c-908d-14f0e89428a3",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "bb74286a-7f77-445b-b100-978fc45f596d",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "cec34da1-6d8d-439a-ab2d-f0b7c2b1be3c",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "e8185066-e649-48e6-acb8-10d09ca0a3e7",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "ef2c13d7-1f61-470b-bb2d-8c0f00ad5bff",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "8927f698-ee9d-4490-862b-9dccab7f4703"
                },
                new Screening()
                {
                    Id = "fe8ca752-cb77-4f71-9481-61c9c317ca75",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "f451615e-41d7-4e40-ac35-8fcf9fb8d660"
                },
                new Screening()
                {
                    Id = "ff6e1bbf-a81a-41fa-8ba9-a0e634fabd4b",
                    StartTime = now.AddDays(2),
                    MovieId = "3f48d8e1-8de6-4829-8ecf-ad3eded4251c",
                    RoomId = "6af902ab-33e7-4bb8-85e5-ac0f50deb1a5"
                }
            };
            _context.Screenings.AddRange(screenings);
            _context.SaveChanges();

            Booking[] bookings = new Booking[]
            {
                new Booking()
                {
                    Id = "7b84fb9b-24c7-4687-b316-23f768ad4667",
                    Seats = "54,42,43,55,56,44,",
                    PayedAmout = 126f,
                    VerificationTime = now,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = "3fe390f2-239a-4124-9c3f-3be7851d4654"
                },
                new Booking()
                {
                    Id = "c169fef2-3080-4c12-a8e7-54d2fbb87d52",
                    Seats = "16,17,18,19,51,39,40,52,41,",
                    PayedAmout = 189f,
                    UserEmail = "vivedycinemas@gmail.com",
                    ScreeningId = "3fe390f2-239a-4124-9c3f-3be7851d4654"
                }
            };
            _context.Bookings.AddRange(bookings);
            _context.SaveChanges();
        }
    }
}
