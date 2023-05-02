﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BUTR.Site.NexusMods.Server.Contexts;
using BUTR.Site.NexusMods.Server.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BUTR.Site.NexusMods.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "hstore");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.AutocompleteEntity", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string[]>("Values")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("values");

                    b.HasKey("Type")
                        .HasName("autocomplete_entity_pkey");

                    b.ToTable("autocomplete_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.CrashReportEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("exception");

                    b.Property<string>("GameVersion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("game_version");

                    b.Property<string[]>("InvolvedModIds")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("involved_mod_ids");

                    b.Property<CrashReportEntityMetadata>("Metadata")
                        .HasColumnType("jsonb")
                        .HasColumnName("metadata");

                    b.Property<Dictionary<string, string>>("ModIdToVersion")
                        .IsRequired()
                        .HasColumnType("hstore")
                        .HasColumnName("mod_id_to_version");

                    b.Property<string[]>("ModIds")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("mod_ids");

                    b.Property<int[]>("ModNexusModsIds")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("mod_nexusmods_ids");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("crash_report_entity_pkey");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("GameVersion");

                    b.HasIndex("Version");

                    b.ToTable("crash_report_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.CrashReportFileEntity", b =>
                {
                    b.Property<string>("Filename")
                        .HasColumnType("text")
                        .HasColumnName("filename");

                    b.Property<Guid>("crash_report_id")
                        .HasColumnType("uuid");

                    b.HasKey("Filename")
                        .HasName("crash_report_file_entity_pkey");

                    b.HasIndex("crash_report_id")
                        .IsUnique();

                    b.ToTable("crash_report_file", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.CrashReportIgnoredFilesEntity", b =>
                {
                    b.Property<string>("Filename")
                        .HasColumnType("text")
                        .HasColumnName("filename");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Filename")
                        .HasName("crash_report_ignored_files_pkey");

                    b.ToTable("crash_report_ignored_files", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.DiscordLinkedRoleTokensEntity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("access_token");

                    b.Property<DateTimeOffset>("AccessTokenExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("access_token_expires_at");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.HasKey("UserId")
                        .HasName("discord_linked_role_tokens_pkey");

                    b.ToTable("discord_linked_role_tokens", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsArticleEntity", b =>
                {
                    b.Property<int>("NexusModsArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("NexusModsArticleId"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("author_name");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_date");

                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("NexusModsArticleId")
                        .HasName("nexusmods_article_entity_pkey");

                    b.ToTable("nexusmods_article_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsExposedModsEntity", b =>
                {
                    b.Property<int>("NexusModsModId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_mod_id");

                    b.Property<DateTime>("LastCheckedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_checked_date");

                    b.Property<string[]>("ModIds")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("mod_ids");

                    b.HasKey("NexusModsModId")
                        .HasName("nexusmods_exposed_mods_entity_pkey");

                    b.ToTable("nexusmods_exposed_mods_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsFileUpdateEntity", b =>
                {
                    b.Property<int>("NexusModsModId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_mod_id");

                    b.Property<DateTime>("LastCheckedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_checked_date");

                    b.HasKey("NexusModsModId")
                        .HasName("nexusmods_file_update_entity_pkey");

                    b.ToTable("nexusmods_file_update_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsModEntity", b =>
                {
                    b.Property<int>("NexusModsModId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_mod_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int[]>("UserIds")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("user_ids");

                    b.HasKey("NexusModsModId")
                        .HasName("nexusmods_mod_entity_pkey");

                    b.ToTable("nexusmods_mod_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsModManualLinkedModuleIdEntity", b =>
                {
                    b.Property<string>("ModuleId")
                        .HasColumnType("text")
                        .HasColumnName("mod_id");

                    b.Property<int>("NexusModsModId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_id");

                    b.HasKey("ModuleId")
                        .HasName("mod_nexus_mods_manual_link_pkey");

                    b.ToTable("mod_nexusmods_manual_link", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsModManualLinkedNexusModsUsersEntity", b =>
                {
                    b.Property<int>("NexusModsModId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_mod_id");

                    b.Property<int[]>("AllowedNexusModsUserIds")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("allowed_nexusmods_user_ids");

                    b.HasKey("NexusModsModId")
                        .HasName("nexusmods_mod_manual_link_nexusmods_users_pkey");

                    b.ToTable("nexusmods_mod_manual_link_nexusmods_users", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserAllowedModuleIdsEntity", b =>
                {
                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string[]>("AllowedModuleIds")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("allowed_mod_ids");

                    b.HasKey("NexusModsUserId")
                        .HasName("user_allowed_mods_pkey");

                    b.ToTable("user_allowed_mods", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserCrashReportEntity", b =>
                {
                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("crash_report_id")
                        .HasColumnType("uuid");

                    b.HasKey("NexusModsUserId")
                        .HasName("user_crash_report_entity_pkey");

                    b.HasIndex("crash_report_id");

                    b.ToTable("user_crash_report_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserMetadataEntity", b =>
                {
                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<Dictionary<string, string>>("Metadata")
                        .IsRequired()
                        .HasColumnType("hstore")
                        .HasColumnName("metadata");

                    b.HasKey("NexusModsUserId")
                        .HasName("user_metadata_pkey");

                    b.ToTable("user_metadata", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserRoleEntity", b =>
                {
                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("NexusModsUserId")
                        .HasName("user_role_pkey");

                    b.ToTable("user_role", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserToDiscordEntity", b =>
                {
                    b.Property<int>("NexusModsUserId")
                        .HasColumnType("integer")
                        .HasColumnName("nexusmods_user_id");

                    b.Property<string>("DiscordId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discord_user_id");

                    b.HasKey("NexusModsUserId")
                        .HasName("nexusmods_to_discord_pkey");

                    b.ToTable("nexusmods_to_discord", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.QuartzExecutionLogEntity", b =>
                {
                    b.Property<long>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("log_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("LogId"));

                    b.Property<DateTimeOffset>("DateAddedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_added_utc");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text")
                        .HasColumnName("error_message");

                    b.Property<QuartzExecutionLogDetailEntity>("ExecutionLogDetail")
                        .HasColumnType("jsonb")
                        .HasColumnName("execution_log_detail");

                    b.Property<DateTimeOffset?>("FireTimeUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("fire_time_utc");

                    b.Property<bool?>("IsException")
                        .HasColumnType("boolean")
                        .HasColumnName("is_exception");

                    b.Property<bool?>("IsSuccess")
                        .HasColumnType("boolean")
                        .HasColumnName("is_success");

                    b.Property<bool?>("IsVetoed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vetoed");

                    b.Property<string>("JobGroup")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("job_group");

                    b.Property<string>("JobName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("job_name");

                    b.Property<TimeSpan?>("JobRunTime")
                        .HasColumnType("interval")
                        .HasColumnName("job_run_time");

                    b.Property<string>("LogType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("log_type");

                    b.Property<string>("MachineName")
                        .HasColumnType("text");

                    b.Property<string>("Result")
                        .HasColumnType("text")
                        .HasColumnName("result");

                    b.Property<int?>("RetryCount")
                        .HasColumnType("integer")
                        .HasColumnName("retry_count");

                    b.Property<string>("ReturnCode")
                        .HasColumnType("text")
                        .HasColumnName("return_code");

                    b.Property<string>("RunInstanceId")
                        .HasColumnType("text")
                        .HasColumnName("run_instance_id");

                    b.Property<DateTimeOffset?>("ScheduleFireTimeUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("_schedule_fire_time_utc");

                    b.Property<string>("TriggerGroup")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("trigger_group");

                    b.Property<string>("TriggerName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("trigger_name");

                    b.HasKey("LogId")
                        .HasName("quartz_execution_log_entity_pkey");

                    b.HasIndex("RunInstanceId");

                    b.HasIndex("DateAddedUtc", "LogType");

                    b.HasIndex("TriggerName", "TriggerGroup", "JobName", "JobGroup", "DateAddedUtc");

                    b.ToTable("quartz_execution_log_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.StatisticsCrashScoreInvolvedEntity", b =>
                {
                    b.Property<string>("GameVersion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("game_version");

                    b.Property<int>("InvolvedCount")
                        .HasColumnType("integer")
                        .HasColumnName("involved_count");

                    b.Property<string>("ModId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mod_id");

                    b.Property<string>("ModVersion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version");

                    b.Property<int>("NotInvolvedCount")
                        .HasColumnType("integer")
                        .HasColumnName("not_involved_count");

                    b.Property<int>("RawValue")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.Property<double>("Score")
                        .HasColumnType("double precision")
                        .HasColumnName("crash_score");

                    b.Property<int>("TotalCount")
                        .HasColumnType("integer")
                        .HasColumnName("total_count");

                    b.ToTable("crash_score_involved_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.StatisticsTopExceptionsTypeEntity", b =>
                {
                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("count");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.ToTable("top_exceptions_type_entity", (string)null);
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.CrashReportFileEntity", b =>
                {
                    b.HasOne("BUTR.Site.NexusMods.Server.Models.Database.CrashReportEntity", "CrashReport")
                        .WithOne()
                        .HasForeignKey("BUTR.Site.NexusMods.Server.Models.Database.CrashReportFileEntity", "crash_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_crash_report_file_entity_crash_report_id");

                    b.Navigation("CrashReport");
                });

            modelBuilder.Entity("BUTR.Site.NexusMods.Server.Models.Database.NexusModsUserCrashReportEntity", b =>
                {
                    b.HasOne("BUTR.Site.NexusMods.Server.Models.Database.CrashReportEntity", "CrashReport")
                        .WithMany()
                        .HasForeignKey("crash_report_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_user_crash_report_entity_crash_report_entity_crash_report_id");

                    b.Navigation("CrashReport");
                });
#pragma warning restore 612, 618
        }
    }
}
