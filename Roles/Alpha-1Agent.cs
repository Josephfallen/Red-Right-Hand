using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace RRR.Roles
{
    [CustomRole(RoleTypeId.NtfSergeant)]
    public class RRRSergeant : CustomRole
    {
        public override uint Id { get; set; } = 11;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override int MaxHealth { get; set; } = 150;
        public override string Name { get; set; } = "Alpha-1 Sergeant";
        public override string Description { get; set; } = "The 05 has ordered you to regain control of the site, Complete the mission";
        public override string CustomInfo { get; set; } = "Alpha-1 Sergeant";
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override List<string> Inventory { get; set; } = new()
        {
            $"{ItemType.KeycardMTFCaptain}",
            $"{ItemType.GunE11SR}",
            $"{ItemType.SCP1853}",
            $"{ItemType.GunCOM15}",
            $"{ItemType.Adrenaline}",
            $"{ItemType.Radio}",
            $"{ItemType.GrenadeHE}",
            $"{ItemType.ArmorCombat}"
        };

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleTypeId.NtfSergeant,
                    Chance = 100
                }
            }
        };
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
        {
            { AmmoType.Nato556, 80 },
            { AmmoType.Nato9, 80 },
        };
    }
}
