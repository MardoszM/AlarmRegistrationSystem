﻿// <auto-generated />
using AlarmRegistrationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlarmRegistrationSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190418152541_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlarmRegistrationSystem.Models.Machine", b =>
                {
                    b.Property<int>("MachineID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand");

                    b.Property<string>("Location");

                    b.Property<string>("MachineUniqueId");

                    b.Property<string>("Model");

                    b.HasKey("MachineID");

                    b.ToTable("Machines");
                });
#pragma warning restore 612, 618
        }
    }
}