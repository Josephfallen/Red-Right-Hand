using System.ComponentModel;
using Exiled.API.Interfaces;
using RRR.Roles;

namespace RRR.Configs
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Should debug messages be shown in a server console.")]
        public bool Debug { get; set; } = false;

        [Description("How many seconds before a spawnwave occurs should it calculate the spawn chance")]
        public int SpawnWaveCalculation { get; set; } = 10;

        [Description("Options for Alpha-1 spawn:")]
        public SpawnManager SpawnManager { get; private set; } = new SpawnManager();

        [Description("Options for Alpha-1 Captain:")]
        public RRRLeader RRRLeader { get; private set; } = new RRRLeader();

        [Description("Options for Alpha-1 Assistant Captain:")]
        public RRRAssistantCaptain RRRAssistantCaptain { get; private set; } = new RRRAssistantCaptain();

        [Description("Options for Alpha-1 Sergeant:")]
        public RRRSergeant RRRSergeant { get; private set; } = new RRRSergeant();

        [Description("Options for Alpha-1 Private:")]
        public RRRPrivate RRRPrivate { get; private set; } = new RRRPrivate();

    }
}