﻿// <auto-generated />
using System;
using AlarmRegistrationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlarmRegistrationSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191205113226_BrakesToJoiningPeriods")]
    partial class BrakesToJoiningPeriods
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlarmRegistrationSystem.Models.Description", b =>
                {
                    b.Property<int>("DescriptionID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author");

                    b.Property<DateTime>("LastModification");

                    b.Property<int>("NotificationID");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("DescriptionID");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.EmergencySubassembly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("EmergencySubassemblies");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.JoiningPeriod", b =>
                {
                    b.Property<int>("JoiningPeriodId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("From");

                    b.Property<int>("NotificationId");

                    b.Property<DateTime>("To");

                    b.HasKey("JoiningPeriodId");

                    b.ToTable("JoiningPeriods");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.Machine", b =>
                {
                    b.Property<int>("MachineID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .IsRequired();

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("MachineUniqueId")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<bool>("State");

                    b.HasKey("MachineID");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Declarant");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("MachineUniqueID")
                        .IsRequired();

                    b.Property<string>("MainDescription")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("State");

                    b.HasKey("NotificationID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.NotificationES", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ESId");

                    b.Property<int>("NotificationId");

                    b.HasKey("Id");

                    b.ToTable("NotificationEs");
                });
#pragma warning restore 612, 618
        }
    }
}
