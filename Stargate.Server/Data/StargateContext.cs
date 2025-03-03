using Microsoft.EntityFrameworkCore;
using Stargate.Server.Data.Models;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Stargate.Server.Data
{
    [ExcludeFromCodeCoverage]
    public class StargateContext : DbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<Person> People { get; set; }
        public DbSet<AstronautDetail> AstronautDetails { get; set; }
        public DbSet<AstronautDuty> AstronautDuties { get; set; }

        public DbSet<PersonAstronaut> PersonAstronauts { get; set; }

        public StargateContext(DbContextOptions<StargateContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            //add seed data
            modelBuilder.Entity<Person>()
                .HasData(
                    new Person
                    {
                        Id = 1,
                        Name = "Neil Armstrong"
                    },
                    new Person
                    {
                        Id = 2,
                        Name = "Joseph Acaba"
                    },
                    new Person
                    {
                        Id = 3,
                        Name = "Deniz Burnham"
                    },
                    new Person
                    {
                        Id = 4,
                        Name = "Zena Cardman"
                    },
                    new Person
                    {
                        Id = 5,
                        Name = "Christopher Cassidy"
                    },
                    new Person
                    {
                        Id = 6,
                        Name = "Raja Chari"
                    },
                    new Person
                    {
                        Id = 7,
                        Name = "Dan Carson"
                    },
                    new Person
                    {
                        Id = 8,
                        Name = "Dallas Davis"
                    }
                );

            modelBuilder.Entity<AstronautDetail>()
                .HasData(
                    // Neil Armstrong
                    new AstronautDetail
                    {
                        Id = 1,
                        PersonId = 1,
                        CurrentRank = "1LT",
                        CurrentDutyTitle = "RETIRED",
                        CareerStartDate = new DateTime(1962, 1, 1),
                        CareerEndDate = new DateTime(1971, 7, 31) 
                    },
                    // Joseph Acaba
                    new AstronautDetail
                    {
                        Id = 2,
                        PersonId = 2,
                        CurrentRank = "LTCOL",
                        CurrentDutyTitle = "Commander",
                        CareerStartDate = new DateTime(2004, 1, 1)
                    },
                    // Deniz Burnham
                    new AstronautDetail
                    {
                        Id = 3,
                        PersonId = 3,
                        CurrentRank = "SPC4",
                        CurrentDutyTitle = "Mission Specialist",
                        CareerStartDate = new DateTime(2021, 1, 1)
                    },
                    // Zena Cardman
                    new AstronautDetail
                    {
                        Id = 4,
                        PersonId = 4,
                        CurrentRank = "SGT",
                        CurrentDutyTitle = "Command Pilot",
                        CareerStartDate = new DateTime(2017, 1, 1)
                    },
                    // Christopher Cassidy
                    new AstronautDetail
                    {
                        Id = 5,
                        PersonId = 5,
                        CurrentRank = "SPC2",
                        CurrentDutyTitle = "RETIRED",
                        CareerStartDate = new DateTime(2004, 1, 1),
                        CareerEndDate = new DateTime(2011, 12, 31)
                    },
                    // Raja Chari
                    new AstronautDetail
                    {
                        Id = 6,
                        PersonId = 6,
                        CurrentRank = "SPC4",
                        CurrentDutyTitle = "Pilot",
                        CareerStartDate = new DateTime(2017, 1, 1)                        
                    }
                );

            modelBuilder.Entity<AstronautDuty>()
                .HasData(
                    // "Neil Armstrong"
                    new AstronautDuty
                    {
                        Id = 1,
                        PersonId = 1,
                        Rank = "SPC1",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(1962, 1, 1),
                        DutyEndDate = new DateTime(1962, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 2,
                        PersonId = 1,
                        Rank = "SPC2",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(1963, 1, 1),
                        DutyEndDate = new DateTime(1964, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 3,
                        PersonId = 1,
                        Rank = "SGT",
                        DutyTitle = "Pilot",
                        DutyStartDate = new DateTime(1965, 1, 1),
                        DutyEndDate = new DateTime(1966, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 4,
                        PersonId = 1,
                        Rank = "2LT",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(1967, 1, 1),
                        DutyEndDate = new DateTime(1967, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 5,
                        PersonId = 1,
                        Rank = "1LT",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(1968, 1, 1),
                        DutyEndDate = new DateTime(1971, 7, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 6,
                        PersonId = 1,
                        Rank = "1LT",
                        DutyTitle = "RETIRED",
                        DutyStartDate = new DateTime(1971, 8, 1)
                    },
                    // Joseph Acaba
                    new AstronautDuty
                    {
                        Id = 7,
                        PersonId = 2,
                        Rank = "SPC2",
                        DutyTitle = "Flight Engineer",
                        DutyStartDate = new DateTime(2004, 1, 1),
                        DutyEndDate = new DateTime(2005, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 8,
                        PersonId = 2,
                        Rank = "SPC4",
                        DutyTitle = "Flight Engineer",
                        DutyStartDate = new DateTime(2006, 1, 1),
                        DutyEndDate = new DateTime(2007, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 9,
                        PersonId = 2,
                        Rank = "SGT",
                        DutyTitle = "Pilot",
                        DutyStartDate = new DateTime(2008, 1, 1),
                        DutyEndDate = new DateTime(2009, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 10,
                        PersonId = 2,
                        Rank = "MSGT",
                        DutyTitle = "Command Pilot",
                        DutyStartDate = new DateTime(2010, 1, 1),
                        DutyEndDate = new DateTime(2013, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 11,
                        PersonId = 2,
                        Rank = "2LT",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(2014, 1, 1),
                        DutyEndDate = new DateTime(2015, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 12,
                        PersonId = 2,
                        Rank = "1LT",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(2016, 1, 1),
                        DutyEndDate = new DateTime(2017, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 13,
                        PersonId = 2,
                        Rank = "CAPT",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(2018, 1, 1),
                        DutyEndDate = new DateTime(2021, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 14,
                        PersonId = 2,
                        Rank = "MAJ",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(2022, 1, 1),
                        DutyEndDate = new DateTime(2023, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 15,
                        PersonId = 2,
                        Rank = "LTCOL",
                        DutyTitle = "Commander",
                        DutyStartDate = new DateTime(2024, 1, 1)
                    },
                    // Deniz Burnham
                    new AstronautDuty
                    {
                        Id = 16,
                        PersonId = 3,
                        Rank = "SPC2",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2021, 1, 1),
                        DutyEndDate = new DateTime(2022, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 17,
                        PersonId = 3,
                        Rank = "SPC3",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2023, 1, 1),
                        DutyEndDate = new DateTime(2024, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 18,
                        PersonId = 3,
                        Rank = "SPC4",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2025, 1, 1)
                    },
                    // Zena Cardman 
                    new AstronautDuty
                    {
                        Id = 19,
                        PersonId = 4,
                        Rank = "SPC1",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2017, 1, 1),
                        DutyEndDate = new DateTime(2018, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 20,
                        PersonId = 4,
                        Rank = "SPC3",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2019, 1, 1),
                        DutyEndDate = new DateTime(2020, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 21,
                        PersonId = 4,
                        Rank = "SPC4",
                        DutyTitle = "Pilot",
                        DutyStartDate = new DateTime(2021, 1, 1),
                        DutyEndDate = new DateTime(2023, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 22,
                        PersonId = 4,
                        Rank = "SGT",
                        DutyTitle = "Command Pilot",
                        DutyStartDate = new DateTime(2024, 1, 1)
                    },
                    // Christopher Cassidy
                    new AstronautDuty
                    {
                        Id = 23,
                        PersonId = 5,
                        Rank = "SPC1",
                        DutyTitle = "Flight Engineer",
                        DutyStartDate = new DateTime(2004, 1, 1),
                        DutyEndDate = new DateTime(2009, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 24,
                        PersonId = 5,
                        Rank = "SPC2",
                        DutyTitle = "Flight Engineer",
                        DutyStartDate = new DateTime(2010, 1, 1),
                        DutyEndDate = new DateTime(2011, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 25,
                        PersonId = 5,
                        Rank = "SPC2",
                        DutyTitle = "RETIRED",
                        DutyStartDate = new DateTime(2012, 1, 1)
                    },
                    // Raja Chari
                    new AstronautDuty
                    {
                        Id = 26,
                        PersonId = 6,
                        Rank = "SPC1",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2017, 1, 1),
                        DutyEndDate = new DateTime(2019, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 27,
                        PersonId = 6,
                        Rank = "SPC2",
                        DutyTitle = "Mission Specialist",
                        DutyStartDate = new DateTime(2020, 1, 1),
                        DutyEndDate = new DateTime(2021, 12, 31)
                    },
                    new AstronautDuty
                    {
                        Id = 28,
                        PersonId = 6,
                        Rank = "SPC4",
                        DutyTitle = "Pilot",
                        DutyStartDate = new DateTime(2022, 1, 1)
                    }
                );
        }
    }
}
