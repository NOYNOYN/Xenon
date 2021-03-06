﻿#region

using System;
using Discord;

#endregion

namespace Xenon.Core.Discord.Addons.Interactive.Paginator
{
    public class PaginatedAppearanceOptions
    {
        public static PaginatedAppearanceOptions Default = new PaginatedAppearanceOptions();
        public IEmote Back = new Emoji("◀");

        public IEmote First = new Emoji("⏮");

        public string FooterFormat = "Page {0}/{1}";

        public IEmote Last = new Emoji("⏭");
        public IEmote Next = new Emoji("▶");
        public IEmote Stop = new Emoji("⏹");

        public TimeSpan? Timeout = null;
    }
}