﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PokeLikeAPI.Data;

#nullable disable

namespace PRO05_BACK_Gaj_Khalos_Max.Migrations
{
    [DbContext(typeof(PokeLikeDbContext))]
    [Migration("20241121115047_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PokeLikeAPI.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Likes")
                        .HasColumnType("integer")
                        .HasColumnName("likes");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("ThemeId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("theme_id");

                    b.HasKey("Id")
                        .HasName("pk_collections");

                    b.ToTable("collections", (string)null);
                });

            modelBuilder.Entity("PokeLikeAPI.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApiUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("api_url");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<int>("Likes")
                        .HasColumnType("integer")
                        .HasColumnName("likes");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_pokemon");

                    b.ToTable("pokemon", (string)null);
                });

            modelBuilder.Entity("PokeLikeAPI.Models.PokemonCollections", b =>
                {
                    b.Property<int>("PokemonId")
                        .HasColumnType("integer")
                        .HasColumnName("pokemon_id");

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer")
                        .HasColumnName("collection_id");

                    b.HasKey("PokemonId", "CollectionId")
                        .HasName("pk_pokemon_collections");

                    b.HasIndex("CollectionId")
                        .HasDatabaseName("ix_pokemon_collections_collection_id");

                    b.ToTable("pokemon_collections", (string)null);
                });

            modelBuilder.Entity("PokeLikeAPI.Models.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_themes");

                    b.ToTable("themes", (string)null);
                });

            modelBuilder.Entity("PokeLikeAPI.Models.PokemonCollections", b =>
                {
                    b.HasOne("PokeLikeAPI.Models.Collection", "Collection")
                        .WithMany("PokemonCollections")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pokemon_collections_collections_collection_id");

                    b.HasOne("PokeLikeAPI.Models.Pokemon", "Pokemon")
                        .WithMany()
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pokemon_collections_pokemon_pokemon_id");

                    b.Navigation("Collection");

                    b.Navigation("Pokemon");
                });

            modelBuilder.Entity("PokeLikeAPI.Models.Collection", b =>
                {
                    b.Navigation("PokemonCollections");
                });
#pragma warning restore 612, 618
        }
    }
}