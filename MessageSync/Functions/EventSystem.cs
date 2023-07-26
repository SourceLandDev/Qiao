using LiteLoader;
using LiteLoader.Event;
using MC;
using MessageSync.Utils;
using Qiao;
using System.Globalization;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MessageSync.Functions;
internal static class EventSystem
{
    private static string _prePlayer;
    internal static void SetupPlayer()
    {
        PlayerChatEvent.Event += ev =>
        {
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.MessageThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate(ev.Player.Xuid == _prePlayer ? "message.tochat.repeat" : "message.tochat", APIHelper.GetUserName(ev.Player), ev.Message.Escape().Format()));
            _prePlayer = ev.Player.Xuid;
            return true;
        };
        PlayerJoinEvent.Event += ev =>
        {
            if (Main.Config.MessageThreadId == Main.Config.InfoThreadId)
            {
                _prePlayer = default;
            }
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.connected", APIHelper.GetUserName(ev.Player), GlobalService.Level.ActivePlayerCount));
            return true;
        };
        PlayerLeftEvent.Event += ev =>
        {
            if (Main.Config.MessageThreadId == Main.Config.InfoThreadId)
            {
                _prePlayer = default;
            }
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.disconnected", APIHelper.GetUserName(ev.Player), GlobalService.Level.ActivePlayerCount - 1));
            return true;
        };
        PlayerDieEvent.Event += ev =>
        {
            if (Main.Config.MessageThreadId == Main.Config.InfoThreadId)
            {
                _prePlayer = default;
            }
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead", APIHelper.GetUserName(ev.Player), ev.DamageSource.Cause switch
            {
                ActorDamageCause.Override => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.override"],
                ActorDamageCause.Contact => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.contact"],
                ActorDamageCause.EntityAttack => Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead.cause.entityattack", (ev.DamageSource.Entity.IsPlayer ? APIHelper.GetUserName(new(ev.DamageSource.Entity.Intptr)) : Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.mob", string.IsNullOrWhiteSpace(ev.DamageSource.Entity.NameTag) ? I18n.Get(ev.DamageSource.Entity.EntityLocName) : ev.DamageSource.Entity.NameTag)).Escape()),
                ActorDamageCause.Projectile => Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead.cause.projectile", (ev.DamageSource.Entity.Owner.IsPlayer ? APIHelper.GetUserName(ev.DamageSource.Entity.PlayerOwner) : Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.mob", string.IsNullOrWhiteSpace(ev.DamageSource.Entity.Owner.NameTag) ? I18n.Get(ev.DamageSource.Entity.Owner.EntityLocName) : ev.DamageSource.Entity.Owner.NameTag)).Escape()),
                ActorDamageCause.Suffocation => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.suffocation"],
                ActorDamageCause.Fall => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.fall"],
                ActorDamageCause.Fire => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.fire"],
                ActorDamageCause.FireTick => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.firetick"],
                ActorDamageCause.Lava => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.lava"],
                ActorDamageCause.Drowning => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.drowning"],
                ActorDamageCause.BlockExplosion => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.blockexplosion"],
                ActorDamageCause.EntityExplosion => Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead.cause.entityexplosion", (ev.DamageSource.Entity.IsPlayer ? APIHelper.GetUserName(new(ev.DamageSource.Entity.Intptr)) : Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.mob", string.IsNullOrWhiteSpace(ev.DamageSource.Entity.NameTag) ? I18n.Get(ev.DamageSource.Entity.EntityLocName) : ev.DamageSource.Entity.NameTag)).Escape()),
                ActorDamageCause.Void => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.void"],
                ActorDamageCause.Suicide => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.suicide"],
                ActorDamageCause.Magic => Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead.cause.magic", (ev.DamageSource.Entity.IsPlayer ? APIHelper.GetUserName(new(ev.DamageSource.Entity.Intptr)) : Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.mob", string.IsNullOrWhiteSpace(ev.DamageSource.Entity.NameTag) ? I18n.Get(ev.DamageSource.Entity.EntityLocName) : ev.DamageSource.Entity.NameTag)).Escape()),
                ActorDamageCause.Wither => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.wither"],
                ActorDamageCause.Starve => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.starve"],
                ActorDamageCause.Anvil => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.anvil"],
                ActorDamageCause.Thorns => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.thorns"],
                ActorDamageCause.FallingBlock => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.fallingblock"],
                ActorDamageCause.Piston => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.piston"],
                ActorDamageCause.FlyIntoWall => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.flyintowall"],
                ActorDamageCause.Magma => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.magma"],
                ActorDamageCause.Fireworks => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.fireworks"],
                ActorDamageCause.Lightning => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.lightning"],
                ActorDamageCause.Charging => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.charging"],
                ActorDamageCause.Temperature => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.temperature"],
                ActorDamageCause.Freezing => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.freezing"],
                ActorDamageCause.Stalactite => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.stalactite"],
                ActorDamageCause.Stalagmite => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.stalagmite"],
                ActorDamageCause.SonicBoom => Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.dead.cause.sonicBoom"],
                _ => Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.dead.cause.unknown", ev.DamageSource.Cause),
            }));
            return true;
        };
    }
    internal static void SetupServer()
    {
        Bot.Received += async (_, update) =>
        {
            string outmsg = default;
            if (update.Type != UpdateType.Message || update.Message.Chat.Id != Main.Config.ChatId)
            {
                return;
            }
            switch (update.Message.Type)
            {
                case MessageType.Text:
                    if (!update.Message.Text.StartsWith("/"))
                    {
                        outmsg = update.Message.Text;
                        break;
                    }
                    User me = await Bot.Client.GetMeAsync();
                    if (!update.Message.Text.EndsWith($"@{me.Username}"))
                    {
                        return;
                    }
                    await Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.commandfeedback", (await Bot.Client.GetChatAdministratorsAsync(update.Message.Chat.Id)).Any((chatMember) => chatMember.User.Id == update.Message.From.Id) ? Level.RuncmdEx(update.Message.Text[1..(update.Message.Text.Length - me.Username.Length - 1)]).Item2 : Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.commandfeedback.notop"]), update.Message.MessageId);
                    return;
                case MessageType.Unknown:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.unknown"];
                    break;
                case MessageType.Photo:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.photo", update.Message.Caption);
                    break;
                case MessageType.Audio:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.audio", update.Message.Audio.FileName, update.Message.Caption);
                    break;
                case MessageType.Video:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.video", update.Message.Video.FileName, update.Message.Video.Duration, update.Message.Caption);
                    break;
                case MessageType.Voice:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.voice", update.Message.Voice.Duration, update.Message.Caption);
                    break;
                case MessageType.Document:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.document", update.Message.Document.FileName, update.Message.Caption);
                    break;
                case MessageType.Sticker:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.sticker", update.Message.Sticker.Emoji);
                    break;
                case MessageType.Location:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.location"];
                    break;
                case MessageType.Contact:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.contact", update.Message.Contact.FirstName + update.Message.Contact.LastName);
                    break;
                case MessageType.Venue:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.venue", update.Message.Venue.Title);
                    break;
                case MessageType.Game:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.game", update.Message.Game.Title);
                    break;
                case MessageType.VideoNote:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.videonote", update.Message.VideoNote.Duration, update.Message.Caption);
                    break;
                case MessageType.Invoice:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.invoice", update.Message.Invoice.Title);
                    break;
                case MessageType.SuccessfulPayment:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.successfulpayment"];
                    break;
                case MessageType.WebsiteConnected:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.websiteconnected"];
                    break;
                case MessageType.ChatMembersAdded:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.chatmembersadded", update.Message.NewChatMembers[0].FirstName + update.Message.NewChatMembers[0].LastName);
                    break;
                case MessageType.ChatMemberLeft:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.chatmemberleft", update.Message.LeftChatMember.FirstName + update.Message.LeftChatMember.LastName);
                    break;
                case MessageType.ChatTitleChanged:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.chattitlechanged", update.Message.NewChatTitle);
                    break;
                case MessageType.ChatPhotoChanged:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.chatphotochanged"];
                    break;
                case MessageType.MessagePinned:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.messagepinned", update.Message.PinnedMessage.Type == MessageType.Text ? update.Message.PinnedMessage.Text : update.Message.PinnedMessage.Type);
                    break;
                case MessageType.ChatPhotoDeleted:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.chatphotodeleted"];
                    break;
                case MessageType.GroupCreated:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.groupcreated"];
                    break;
                case MessageType.SupergroupCreated:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.supergroupcreated"];
                    break;
                case MessageType.ChannelCreated:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.channelcreated"];
                    break;
                case MessageType.MigratedToSupergroup:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.migratedtosupergroup"];
                    break;
                case MessageType.MigratedFromGroup:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.migratedfromgroup"];
                    break;
                case MessageType.Poll:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.poll", update.Message.Poll.Question);
                    break;
                case MessageType.Dice:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.type.dice", update.Message.Dice.Value, update.Message.Dice.Emoji is "🏀" or "⚽" ? "5" : update.Message.Dice.Emoji == "🎰" ? "64" : "6");
                    break;
                case MessageType.MessageAutoDeleteTimerChanged:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.messageautodeletetimerchanged"];
                    break;
                case MessageType.ProximityAlertTriggered:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.proximityalerttriggered"];
                    break;
                case MessageType.WebAppData:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.webappdata"];
                    break;
                case MessageType.VideoChatScheduled:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.videochatscheduled"];
                    break;
                case MessageType.VideoChatStarted:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.videochatstarted"];
                    break;
                case MessageType.VideoChatEnded:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.videochatended"];
                    break;
                case MessageType.VideoChatParticipantsInvited:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.videochatparticipantsinvited"];
                    break;
                case MessageType.Animation:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.animation"];
                    break;
                case MessageType.ForumTopicCreated:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.forumtopiccreated"];
                    break;
                case MessageType.ForumTopicClosed:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.forumtopicclosed"];
                    break;
                case MessageType.ForumTopicReopened:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.forumtopicreopened"];
                    break;
                case MessageType.ForumTopicEdited:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.forumtopicedited"];
                    break;
                case MessageType.GeneralForumTopicHidden:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.generalforumtopichidden"];
                    break;
                case MessageType.GeneralForumTopicUnhidden:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.generalforumtopicunhidden"];
                    break;
                case MessageType.WriteAccessAllowed:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.writeaccessallowed"];
                    break;
                default:
                    outmsg = Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.type.unsupportedtype"];
                    break;
            }
            if (string.IsNullOrWhiteSpace(outmsg))
            {
                return;
            }
            outmsg = Main.EmojiHelper[CultureInfo.CurrentCulture.Name]._languageData.Aggregate(outmsg, (current, emoji) => current.Replace(emoji.Key, emoji.Value));
            Level.BroadcastText(update.Message.ReplyToMessage is null || (update.Message.IsTopicMessage ?? false) && update.Message.MessageThreadId == update.Message.ReplyToMessage.MessageId ?
                Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.toserver", update.Message.Date.AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours), update.Message.SenderChat == null ? update.Message.From.FirstName + update.Message.From.LastName : update.Message.SenderChat.Title, outmsg) :
                Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.toserver.reply", update.Message.Date.AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours), update.Message.SenderChat == null ? update.Message.From.FirstName + update.Message.From.LastName : update.Message.SenderChat.Title, update.Message.ReplyToMessage.SenderChat == null ? update.Message.ReplyToMessage.From.Id == Bot.Client.BotId ? string.Empty : update.Message.ReplyToMessage.From.FirstName + update.Message.ReplyToMessage.From.LastName : update.Message.ReplyToMessage.SenderChat.Title, outmsg), TextType.Raw);
            _prePlayer = default;
        };
        ServerStartedEvent.Event += ev =>
        {
            if (string.IsNullOrWhiteSpace(Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.server.start"]))
            {
                return true;
            }
            Task task = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.server.start"]);
            if (Main.Config.SyncMode)
            {
                task.Wait();
            }
            return true;
        };
        ServerStoppedEvent.Event += ev =>
        {
            if (string.IsNullOrWhiteSpace(Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.server.stop"]))
            {
                return true;
            }
            Task task = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.InfoThreadId, Main.I18nHelper[CultureInfo.CurrentCulture.Name]["message.server.stop"]);
            if (Main.Config.SyncMode)
            {
                task.Wait();
            }
            return true;
        };
    }
}
