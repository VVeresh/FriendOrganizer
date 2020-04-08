using Prism.Events;

namespace FriendOrganizer.UI.Event
{
    public class OpenFriendDetailViewEvent : PubSubEvent<int?>       // Inherit for Prism generic class; int is event argument
    {
    }
}
