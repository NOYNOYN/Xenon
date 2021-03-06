﻿#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Xenon.Core.Discord.Addons.Interactive;
using Xenon.Services;
using Xenon.Services.External;

#endregion

namespace Xenon.Core
{
    public class CommandBase : InteractiveBase<ShardedCommandContext>
    {
        private Server _oldServer;
        public Server Server;
        public CachingService Caching { get; set; }
        public Configuration Configuration { get; set; }
        public Random Random { get; set; }
        public HttpClient HttpClient { get; set; }

        public DatabaseService Database { get; set; }

        protected override void BeforeExecute(CommandInfo command)
        {
            var executionObj = Caching.ExecutionObjects[Context.Message.Id];
            Server = executionObj?.Server;
            _oldServer = Server;
            if (Server == null || Context.Guild == null) return;
            Server.ServerId = Context.Guild.Id;
            Server.Name = Context.Guild.Name;
        }

        protected override void AfterExecute(CommandInfo command)
        {
            var executionObj = Caching.ExecutionObjects[Context.Message.Id];
            Database.Execute(x =>
            {
                if (Server != null) x.Store(Server);
                if (x.Advanced.HasChanges) x.SaveChanges();
            });
            Caching.ExecutionObjects.Remove(Context.Message.Id);
        }

        public EmbedBuilder NormalizeEmbed(EmbedBuilder embed, ColorType colorType = ColorType.Normal,
            bool withRequested = false)
        {
            return embed.NormalizeEmbed(colorType, Random, Server, withRequested, Context);
        }

        public EmbedBuilder NormalizeEmbed(string title, string description, ColorType colorType = ColorType.Normal,
            bool withRequested = false)
        {
            return UtilService.NormalizeEmbed(title, description, colorType, Random, Server, withRequested, Context);
        }

        public async Task<IUserMessage> ReplyEmbedAsync(string title, string description,
            ColorType colorType = ColorType.Normal, bool withRequested = false)
        {
            return await ReplyAsync(embed: NormalizeEmbed(title, description, colorType, withRequested).Build());
        }

        public async Task<IUserMessage> ReplyEmbedAsync(EmbedBuilder embed, ColorType colorType = ColorType.Normal,
            bool withRequested = false)
        {
            return await ReplyAsync(embed: NormalizeEmbed(embed, colorType, withRequested).Build());
        }
    }
}