﻿// <auto-generated />
using Libria.Repository.EFCore.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Libria.Repository.EFCore.Tests.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20180903132612_v003")]
    partial class v003
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Libria.Repository.EFCore.Tests.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Prop1");

                    b.HasKey("Id");

                    b.ToTable("TestEntity");

                    b.HasData(
                        new { Id = 1, Prop1 = "Prop1" },
                        new { Id = 2, Prop1 = "Prop1" },
                        new { Id = 3, Prop1 = "Prop1_3" },
                        new { Id = 4, Prop1 = "Prop1_4" },
                        new { Id = 5, Prop1 = "Prop1" },
                        new { Id = 6, Prop1 = "Prop1" },
                        new { Id = 7, Prop1 = "Prop1" },
                        new { Id = 8, Prop1 = "Prop1" },
                        new { Id = 9, Prop1 = "Prop1" },
                        new { Id = 10, Prop1 = "Prop1" },
                        new { Id = 11, Prop1 = "Prop1" },
                        new { Id = 12, Prop1 = "Prop1" },
                        new { Id = 13, Prop1 = "Prop1" },
                        new { Id = 14, Prop1 = "Prop1" },
                        new { Id = 15, Prop1 = "Prop1" },
                        new { Id = 16, Prop1 = "Prop1" }
                    );
                });

            modelBuilder.Entity("Libria.Repository.EFCore.Tests.TestEntityNavigation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId");

                    b.Property<string>("Prop1");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("TestEntityNavigation");

                    b.HasData(
                        new { Id = 1, EntityId = 1, Prop1 = "NavigationProp1" }
                    );
                });

            modelBuilder.Entity("Libria.Repository.EFCore.Tests.TestEntityNavigation", b =>
                {
                    b.HasOne("Libria.Repository.EFCore.Tests.TestEntity", "Entity")
                        .WithMany("EntityNavigations")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
