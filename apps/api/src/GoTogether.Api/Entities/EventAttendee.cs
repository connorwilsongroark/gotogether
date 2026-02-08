using GoTogether.Features.Events;

namespace GoTogether.Entities;

public sealed class EventAttendee
{
    public Guid EventId {get; set;}
    public Event Event {get; set;} = null!;

    public string UserId {get; set;} = null!; // Auth0 id
    public DateTimeOffset AddedAt {get; set;}

    public DateTimeOffset? RespondedAt {get; set;}
    public RsvpStatus RsvpStatus {get; set;} = RsvpStatus.Invited;

}