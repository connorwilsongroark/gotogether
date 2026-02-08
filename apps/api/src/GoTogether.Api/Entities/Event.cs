using System.ComponentModel.DataAnnotations;
using GoTogether.Features.Events;

namespace GoTogether.Entities;

public sealed class Event
{
    public Guid Id {get; set;}

    // Ownership & Auditing properties
    public string OrganizerUserId {get; set;} = null!;
    public DateTimeOffset CreatedAt {get; set;}
    public DateTimeOffset? UpdatedAt {get; set;}

    // What & where
    public Guid PlaceId {get; set;}
    public Place Place {get; set;} = null!;

    public string Title {get; set;} = null!;
    public string? Description {get; set;}

    // When
    public DateTimeOffset StartsAt {get;set;}
    public DateTimeOffset EndsAt{get;set;}
    public string? TimeZoneId {get; set;}

    // Status
    public EventStatus Status {get; set;} = EventStatus.Scheduled;
    public bool IsCanceled => Status == EventStatus.Canceled;

    // Attendees
    public List<EventAttendee> Attendees {get; set;} = new();

}