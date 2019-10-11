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
    [Migration("20190916111406_NotificationUpdate")]
    partial class NotificationUpdate
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

                    b.Property<int?>("NotificationID");

                    b.Property<string>("Text");

                    b.HasKey("DescriptionID");

                    b.HasIndex("NotificationID");

                    b.ToTable("Descriptions");
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

                    b.Property<int>("MachineUniqueID");

                    b.Property<string>("MainDescription");

                    b.HasKey("NotificationID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AlarmRegistrationSystem.Models.Description", b =>
                {
                    b.HasOne("AlarmRegistrationSystem.Models.Notification")
                        .WithMany("Descriptions")
                        .HasForeignKey("NotificationID");
                });
#pragma warning restore 612, 618
        }
    }
}
