using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.v1.Models;

public partial class ApiDbContext : DbContext
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentComment> DocumentComments { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Subdepartment> Subdepartments { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<Workingcalendar> Workingcalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("app_users_pkey");

            entity.ToTable("app_users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName).HasColumnName("user_name");
            entity.Property(e => e.UserPassword).HasColumnName("user_password");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("documents_pkey");

            entity.ToTable("documents");

            entity.HasIndex(e => e.DocumentId, "idx_documents_id");

            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.DateApproval)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_approval");
            entity.Property(e => e.DateEdit)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_edit");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(255)
                .HasColumnName("document_type");
            entity.Property(e => e.Field)
                .HasMaxLength(255)
                .HasColumnName("field");
            entity.Property(e => e.HasComments)
                .HasDefaultValue(false)
                .HasColumnName("has_comments");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Documents)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("documents_author_id_fkey");
        });

        modelBuilder.Entity<DocumentComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("document_comments_pkey");

            entity.ToTable("document_comments");

            entity.HasIndex(e => e.CommentId, "idx_document_comments_id");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_created");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_updated");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Author).WithMany(p => p.DocumentComments)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_comments_author_id_fkey");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentComments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_comments_document_id_fkey");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EventName)
                .HasMaxLength(255)
                .HasColumnName("event_name");
            entity.Property(e => e.EventType)
                .HasMaxLength(255)
                .HasColumnName("event_type");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Subdepartment>(entity =>
        {
            entity.HasKey(e => e.SubdepartmentId).HasName("subdepartments_pkey");

            entity.ToTable("subdepartments");

            entity.HasIndex(e => e.SubdepartmentName, "subdepartments_subdepartment_name_key").IsUnique();

            entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.SubdepartmentName)
                .HasMaxLength(255)
                .HasColumnName("subdepartment_name");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("workers_pkey");

            entity.ToTable("workers");

            entity.HasIndex(e => e.FullName, "idx_workers_full_name");

            entity.HasIndex(e => e.Phone, "workers_phone_key").IsUnique();

            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsSubdepartmentHead)
                .HasDefaultValue(false)
                .HasColumnName("is_subdepartment_head");
            entity.Property(e => e.JobPosition)
                .HasMaxLength(255)
                .HasColumnName("job_position");
            entity.Property(e => e.Office)
                .HasMaxLength(10)
                .HasColumnName("office");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.SubdepartmentName)
                .HasMaxLength(255)
                .HasColumnName("subdepartment_name");

            entity.HasOne(d => d.SubdepartmentNameNavigation).WithMany(p => p.Workers)
                .HasPrincipalKey(p => p.SubdepartmentName)
                .HasForeignKey(d => d.SubdepartmentName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workers_subdepartment_name_fkey");

            entity.HasMany(d => d.Events).WithMany(p => p.Workers)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkersEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("workers_events_event_id_fkey"),
                    l => l.HasOne<Worker>().WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("workers_events_worker_id_fkey"),
                    j =>
                    {
                        j.HasKey("WorkerId", "EventId").HasName("workers_events_pkey");
                        j.ToTable("workers_events");
                        j.IndexerProperty<int>("WorkerId").HasColumnName("worker_id");
                        j.IndexerProperty<int>("EventId").HasColumnName("event_id");
                    });
        });

        modelBuilder.Entity<Workingcalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workingcalendar_pk");

            entity.ToTable("workingcalendar", tb => tb.HasComment("Список дней исключений в производственном календаре"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Exceptiondate)
                .HasComment("День-исключение")
                .HasColumnName("exceptiondate");
            entity.Property(e => e.Isworkingday)
                .HasComment("0 - будний день, но законодательно принят выходным; 1 - сб или вс, но является рабочим")
                .HasColumnName("isworkingday");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
