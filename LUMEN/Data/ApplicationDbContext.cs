namespace LUMEN.Data;

using LUMEN.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<Badge> Badges { get; set; }
    public DbSet<UserBadge> UserBadges { get; set; }
    public DbSet<XPLog> XPLogs { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<DailyTrack> DailyTracks { get; set; }
    public DbSet<VirtualLabSession> VirtualLabSessions { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<DiscussionThread> DiscussionThreads { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<StudyGroup> StudyGroups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.StudyGroup)
            .WithMany(sg => sg.Members)
            .HasForeignKey(gm => gm.StudyGroupId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.User)
            .WithMany()
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StudyGroup>()
    .HasOne<ApplicationUser>()
    .WithMany()
    .HasForeignKey(sg => sg.CreatedBy)
    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Subject)
            .WithMany()
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<QuizAttempt>()
            .HasOne(qa => qa.Quiz)
            .WithMany()
            .HasForeignKey(qa => qa.QuizId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<QuizAttempt>()
            .HasOne(qa => qa.User)
            .WithMany()
            .HasForeignKey(qa => qa.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.Badge)
            .WithMany()
            .HasForeignKey(ub => ub.BadgeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.User)
            .WithMany()
            .HasForeignKey(ub => ub.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Bookmark>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<XPLog>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<DailyTrack>()
            .HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Thread)
            .WithMany(t => t.Posts)
            .HasForeignKey(p => p.ThreadId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}