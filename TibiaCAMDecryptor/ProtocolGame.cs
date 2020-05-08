using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class ProtocolGame {
        public static void ParsePacket(Recording recording, Packet packet) {
            byte[] packetData = packet.GetPacketData();
            InputMessage inputMessage = new InputMessage(packetData);
            byte lastPacketHead = 0;
            try {
                while (inputMessage.getPosition() < inputMessage.getLength()) {
                    byte packetHead = inputMessage.getByte();
                    switch (packetHead) {
                        case 0xA:
                            ParseLogin(inputMessage);
                            break;
                        case 0x14:
                            ParseDisconnectClient(inputMessage);
                            break;
                        case 0x16:
                            ParseWaitList(inputMessage);
                            break;
                        case 0x1E:
                            break;
                        case 0x64:
                            ParseMapDescription(recording, inputMessage);
                            break;
                        case 0x65:
                            ParseNorthMove(inputMessage);
                            break;
                        case 0x66:
                            ParseEastMove(inputMessage);
                            break;
                        case 0x67:
                            ParseSouthMove(inputMessage);
                            break;
                        case 0x68:
                            ParseWestMove(inputMessage);
                            break;
                        case 0x69:
                            ParseUpdateTile(inputMessage);
                            break;
                        case 0x6A:
                            ParseAddTileItem(inputMessage);
                            break;
                        case 0x6B:
                            ParseUpdateTileItem(inputMessage);
                            break;
                        case 0x6C:
                            ParseRemoveTileItem(inputMessage);
                            break;
                        case 0x6D:
                            ParseMoveCreature(inputMessage);
                            break;
                        case 0x6E:
                            ParseContainer(inputMessage);
                            break;
                        case 0x6F:
                            ParseCloseContainer(inputMessage);
                            break;
                        case 0x70:
                            ParseAddContainerItem(inputMessage);
                            break;
                        case 0x71:
                            ParseUpdateContainerItem(inputMessage);
                            break;
                        case 0x72:
                            ParseRemoveContainerItem(inputMessage);
                            break;
                        case 0x78:
                        case 0x79:
                            ParseInventoryItem(inputMessage, packetHead);
                            break;
                        case 0x7D:
                        case 0x7E:
                            ParseTradeItemRequest(inputMessage);
                            break;
                        case 0x7F:
                            break;
                        case 0x82:
                            ParseWorldLight(inputMessage);
                            break;
                        case 0x83:
                            ParseMagicEffect(inputMessage);
                            break;
                        case 0x84:
                            ParseAnimatedText(inputMessage);
                            break;
                        case 0x85:
                            ParseDistanceShoot(inputMessage);
                            break;
                        case 0x86:
                            ParseCreatureSquare(inputMessage);
                            break;
                        case 0x8C:
                            ParseCreatureHealth(inputMessage);
                            break;
                        case 0x8D:
                            ParseCreatureLight(inputMessage);
                            break;
                        case 0x8E:
                            ParseCreatureOutfit(inputMessage);
                            break;
                        case 0x8F:
                            ParseChangeSpeed(inputMessage);
                            break;
                        case 0x90:
                            ParseCreatureSkull(inputMessage);
                            break;
                        case 0x91:
                            ParseCreatureShield(inputMessage);
                            break;
                        case 0x96:
                            ParseTextWindow(inputMessage);
                            break;
                        case 0x97:
                            ParseHouseWindow(inputMessage);
                            break;
                        case 0xA0:
                            ParsePlayerStats(inputMessage);
                            break;
                        case 0xA1:
                            ParsePlayerSkills(inputMessage);
                            break;
                        case 0xA2:
                            ParsePlayerIcons(inputMessage);
                            break;
                        case 0xA3: // cancel target
                            break;
                        case 0xAA:
                            ParseCreatureSpeak(inputMessage);
                            break;
                        case 0xAB:
                            ParseChannelsDialog(inputMessage);
                            break;
                        case 0xAC:
                            ParseChannel(inputMessage);
                            break;
                        case 0xAD:
                            ParseOpenPrivateChannel(inputMessage);
                            break;
                        case 0xAE:
                            ParseRuleViolationsChannel(inputMessage);
                            break;
                        case 0xAF:
                            ParseRemoveReport(inputMessage);
                            break;
                        case 0xB0:
                            ParsesRuleViolationCancel(inputMessage);
                            break;
                        case 0xB1:
                            ParseLockRuleViolation(inputMessage);
                            break;
                        case 0xB4:
                            ParseTextMessage(inputMessage);
                            break;
                        case 0xB2:
                            ParseCreatePrivateChannel(inputMessage);
                            break;
                        case 0xB3:
                            ParseClosePrivate(inputMessage);
                            break;
                        case 0xB5:
                            ParseCancelWalk(inputMessage);
                            break;
                        case 0xBE:
                            ParseFloorChangeUp(inputMessage);
                            break;
                        case 0xBF:
                            ParseFloorChangeDown(inputMessage);
                            break;
                        case 0xC8:
                            ParseOutfitWindow(inputMessage);
                            break;
                        case 0xD2:
                            ParseVIP(inputMessage);
                            break;
                        case 0xD3:
                            ParseVIPLogin(inputMessage);
                            break;
                        case 0xD4:
                            ParseVIPLogout(inputMessage);
                            break;
                        default:
                            throw new Exception("Invalid packet head (maybe different version?)");
                    }

                    lastPacketHead = packetHead;
                }
            } catch (Exception ex) {
                //Console.WriteLine("Corrupted packet.");
                recording.HasProblem = true;
            }
        }

        private static void ParseDisconnectClient(InputMessage input) {
            input.getString();
        }

        private static void ParseWaitList(InputMessage input) {
            input.getString();
            input.getByte();
        }

        private static void ParseRuleViolationsChannel(InputMessage input) {
            ushort size = input.getU16();
        }

        private static void ParseRemoveReport(InputMessage input) {
            input.getString();
        }

        private static void ParsesRuleViolationCancel(InputMessage input) {
            input.getString();
        }

        private static void ParseLockRuleViolation(InputMessage input) {
            
        }

        private static void ParseHouseWindow(InputMessage input) {
            input.getByte();
            input.getU32();
            input.getString();
        }

        private static void ParseClosePrivate(InputMessage input) {
            input.getU16();
        }

        private static void ParseTradeItemRequest(InputMessage input) {
            input.getString();
            byte tradeItemsSize = input.getByte();
            for (int index = 0; index < tradeItemsSize; index++) {
                GetThing(input);
            }
        }

        private static void ParseCreatePrivateChannel(InputMessage input) {
            input.getU16();
            input.getString();
        }

        private static void ParseChannelsDialog(InputMessage input) {
            byte channelsSize = input.getByte();
            for (int index = 0; index < channelsSize; index++) {
                input.getU16();
                input.getString();
            }
        }

        private static void ParseChannel(InputMessage input) {
            input.getU16();
            input.getString();
        }

        private static void ParseOutfitWindow(InputMessage input) {
            input.getOutfit();
            input.getByte();
            input.getByte();
        }

        private static void ParseTextWindow(InputMessage input) {
            input.getU32();
            input.getU16();
            input.getU16();
            input.getString();
        }

        private static void ParseCreatureSkull(InputMessage input) {
            input.getU32();
            input.getByte();
        }

        private static void ParseCreatureShield(InputMessage input) {
            input.getU32();
            input.getByte();
        }

        private static void ParseUpdateTile(InputMessage input) {
            Location location = input.getLocation();
            var thingId = input.PeekU16();

            if (thingId == 0xFF01) {
                input.getU16();
            } else {
                parseTileDescription(input, location);
                input.getU16();
            }
        }

        private static void ParseOpenPrivateChannel(InputMessage input) {
            input.getString();
        }

        private static void ParseCancelWalk(InputMessage input) {
            input.getByte();
        }

        private static void ParseFloorChangeUp(InputMessage input) {
            Location myPos = Player.location;
            myPos = new Location(myPos.X, myPos.Y, myPos.Z - 1);
            //Console.WriteLine("Z: " + myPos.Z);
            var tiles = new List<Tile>();
            if (myPos.Z == 7) {
                int skip = 0;
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 5, 18, 14, 3, ref skip); //(floor 7 and 6 already set)
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 4, 18, 14, 4, ref skip);
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 3, 18, 14, 5, ref skip);
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 2, 18, 14, 6, ref skip);
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 1, 18, 14, 7, ref skip);
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, 0, 18, 14, 8, ref skip);
            } else if (myPos.Z > 7) {
                int skip = 0;
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, myPos.Z - 2, 18, 14, 3, ref skip);
            }

            Player.location = new Location(myPos.X + 1, myPos.Y + 1, myPos.Z);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseFloorChangeDown(InputMessage input) {
            Location myPos = Player.location;
            myPos = new Location(myPos.X, myPos.Y, myPos.Z + 1);

            //going from surface to underground

            var tiles = new List<Tile>();

            int skipTiles = 0;
            if (myPos.Z == 8) {
                int j, i;
                for (i = myPos.Z, j = -1; i < (int)myPos.Z + 3; ++i, --j)
                    parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, i, 18, 14, j, ref skipTiles);
            }
            //going further down
            else if (myPos.Z > 8 && myPos.Z < 14)
                parseFloorDescription(input, tiles, myPos.X - 8, myPos.Y - 6, myPos.Z + 2, 18, 14, -3, ref skipTiles);

            Player.location = new Location(myPos.X - 1, myPos.Y - 1, myPos.Z);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseCreatureOutfit(InputMessage input) {
            input.getU32();
            input.getOutfit();
        }

        private static void ParseVIPLogout(InputMessage input) {
            input.getU32();
        }

        private static void ParseChangeSpeed(InputMessage input) {
            input.getU32();
            input.getU16();
        }

        private static void ParseVIPLogin(InputMessage input) {
            input.getU32();
        }

        private static void ParseUpdateContainerItem(InputMessage input) {
            input.getByte();
            input.getByte();
            GetThing(input);
        }

        private static void ParseDistanceShoot(InputMessage input) {
            input.getLocation();
            input.getLocation();
            input.getByte();
        }

        private static void ParseAddContainerItem(InputMessage input) {
            input.getByte();
            GetThing(input);
        }

        private static void ParseRemoveContainerItem(InputMessage input) {
            input.getByte();
            input.getByte();
        }

        private static void ParseCloseContainer(InputMessage input) {
            input.getByte();
        }

        private static void ParseContainer(InputMessage input) {
            input.getByte();
            input.getU16();
            input.getString();
            input.getByte();
            input.getByte();
            byte containerSize = input.getByte();
            for (int index = 0; index < containerSize; index++) {
                GetThing(input);
            }
        }

        private static void ParseRemoveTileItem(InputMessage input) {
            Location location = input.getLocation();
            var stack = input.getByte();

            if (location.IsCreature) //TODO: Veirificar o porque disso.
                return;

            Tile tile = Instance.Map.GetTile(location);
            if (tile == null) {
                //Console.WriteLine("Remove tile item location not exists");
                return;
            }

            var thing = tile.GetThing(stack);
            if (thing == null) { // The client will send update tile.
                //Console.WriteLine("Remove tile item stack location not exists");
                return;
            }

            tile.RemoveThing(stack);
        }

        private static void ParseAddTileItem(InputMessage input) {
            Location location = input.getLocation();
            Thing thing = GetThing(input);
            Tile tile = Instance.Map.GetTile(location);

            if (tile == null) {
                //Console.WriteLine("Add tile item location not exists");
                return;
            }

            tile.AddThing(thing);
            Instance.Map.SetTile(tile);
        }

        private static void ParseCreatureSpeak(InputMessage input) {
            //input.getU32();
            input.getString();

            byte type = input.getByte();
            switch (type) {
                case 1:
                case 2:
                case 3:
                case 0x10:
                case 0x11:
                    input.getLocation();
                    break;
                case 5:
                case 0xA:
                case 0xE:
                case 0xC:
                    input.getU16();
                    break;
                case 6:
                    input.getU16();
                    break;
                default:
                    break;
            }

            input.getString();
        }

        private static void ParseAnimatedText(InputMessage input) {
            input.getLocation();
            input.getByte();
            input.getString();
        }

        private static void ParseCreatureHealth(InputMessage input) {
            input.getU32();
            input.getByte();
        }

        private static void ParseEastMove(InputMessage input) {
            var location = new Location(Player.location.X + 1, Player.location.Y, Player.location.Z);
            Player.location = location;

            var tiles = new List<Tile>();
            GetMapDescription(input, tiles, location.X + 9, location.Y - 6, location.Z, 1, 14);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseNorthMove(InputMessage input) {
            var location = new Location(Player.location.X, Player.location.Y - 1, Player.location.Z);
            Player.location = location;

            var tiles = new List<Tile>();
            GetMapDescription(input, tiles, location.X - 8, location.Y - 6, location.Z, 18, 1);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseSouthMove(InputMessage input) {
            var location = new Location(Player.location.X, Player.location.Y + 1, Player.location.Z);
            Player.location = location;

            var tiles = new List<Tile>();
            GetMapDescription(input, tiles, location.X - 8, location.Y + 7, location.Z, 18, 1);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseWestMove(InputMessage input) {
            var location = new Location(Player.location.X - 1, Player.location.Y, Player.location.Z);
            Player.location = location;

            var tiles = new List<Tile>();
            GetMapDescription(input, tiles, location.X - 8, location.Y - 6, location.Z, 1, 14);
            Instance.Map.OnMapUpdated(tiles);
        }

        private static void ParseCreatureSquare(InputMessage input) {
            input.getU32();
            input.getByte();
        }

        private static void ParseMoveCreature(InputMessage input) {
            Location oldLocation = input.getLocation();
            var oldStack = input.getByte();
            Location newLocation = input.getLocation();

            if (oldLocation.IsCreature) {
                var creatureId = oldLocation.GetCretureId(oldStack);
                Creature creature = new Creature(creatureId);

                if (creature == null)
                    return;

                var tile = Instance.Map.GetTile(newLocation);
                if (tile == null)
                    return;

                tile.AddThing(creature);
                Instance.Map.SetTile(tile);
            } else {
                Tile tile = Instance.Map.GetTile(oldLocation);
                if (tile == null)
                    return;

                Thing thing = tile.GetThing(oldStack);
                Creature creature = thing as Creature;
                if (creature == null)
                    return; //The client will send update tile.

                tile.RemoveThing(oldStack);
                Instance.Map.SetTile(tile);

                tile = Instance.Map.GetTile(newLocation);
                if (tile == null)
                    return;

                tile.AddThing(creature);
                Instance.Map.SetTile(tile);

                //update creature direction
                if (oldLocation.X > newLocation.X) {
                    creature.LookDirection = Direction.DIRECTION_WEST;
                    creature.TurnDirection = Direction.DIRECTION_WEST;
                } else if (oldLocation.X < newLocation.X) {
                    creature.LookDirection = Direction.DIRECTION_EAST;
                    creature.TurnDirection = Direction.DIRECTION_EAST;
                } else if (oldLocation.Y > newLocation.Y) {
                    creature.LookDirection = Direction.DIRECTION_NORTH;
                    creature.TurnDirection = Direction.DIRECTION_NORTH;
                } else if (oldLocation.Y < newLocation.Y) {
                    creature.LookDirection = Direction.DIRECTION_SOUTH;
                    creature.TurnDirection = Direction.DIRECTION_SOUTH;
                }
            }
        }

        private static void ParseUpdateTileItem(InputMessage input) {
            Location location = input.getLocation();
            var stack = input.getByte();
            var thing = GetThing(input);

            if (!location.IsCreature) {
                //get tile
                Tile tile = Instance.Map.GetTile(location);
                if (tile == null) {
                    //Console.WriteLine("Update tile item location not exists");
                    return;
                }

                var oldThing = tile.GetThing(stack);
                if (oldThing == null) {
                    //Console.WriteLine("Update tile item stack location not exists");
                    return; // the client will send update tile.
                }

                tile.ReplaceThing(stack, thing);
                Instance.Map.SetTile(tile);
            }
        }

        private static void ParsePlayerIcons(InputMessage input) {
            input.getByte();
        }

        private static void ParseTextMessage(InputMessage input) {
            input.getByte();
            input.getString();
        }

        private static void ParseVIP(InputMessage input) {
            input.getU32();
            input.getString();
            input.getByte();
        }

        private static void ParsePlayerSkills(InputMessage input) {
            for (int index = 0; index < 7; index++) {
                input.getByte();
                input.getByte();
            }
        }

        private static void ParseCreatureLight(InputMessage input) {
            input.getU32();
            input.getByte();
            input.getByte();
        }

        private static void ParseWorldLight(InputMessage input) {
            input.getByte();
            input.getByte();
        }

        private static void ParsePlayerStats(InputMessage input) {
            input.getU16();
            input.getU16();
            input.getU16();
            input.getU32();
            input.getByte();
            input.getByte();
            input.getU16();
            input.getU16();
            input.getByte();
            input.getByte();
        }

        private static void ParseInventoryItem(InputMessage input, byte packetHead) {
            if (packetHead == 0x78) {
                input.getByte();
                GetItem(input, 65535);
            } else if (packetHead == 0x79)
                input.getByte();
        }

        private static void ParseMagicEffect(InputMessage input) {
            input.getLocation();
            input.getByte();
        }

        public static void ParseLogin(InputMessage input) {
            input.getU32();
            input.getByte();
            input.getByte();
            byte accessLevel = input.getByte();
            if (accessLevel == 1) {
                byte loop = input.getByte();
                for (byte b = 0; b < 32; b++)
                    input.getByte();
            }
        }

        public static void ParseMapDescription(Recording recording, InputMessage input) {
            Location playerLocation = input.getLocation();
            if (recording.Location == null)
                recording.Location = playerLocation;

            Player.location = playerLocation;

            var tiles = new List<Tile>();
            GetMapDescription(input, tiles, Player.location.X - 8, Player.location.Y - 6, Player.location.Z, 18, 14);
            Instance.Map.OnMapUpdated(tiles);
        }

        public static void GetMapDescription(InputMessage input, List<Tile> tiles, int x, int y, int z, int width, int height) {
            int startz, endz, zstep;
            //calculate map limits
            //Console.WriteLine(x + "," + y + "," + z);

            if (z > 7) {
                startz = z - 2;
                endz = Math.Min(16 - 1, z + 2);
                zstep = 1;
            } else {
                startz = 7;
                endz = 0;
                zstep = -1;
            }

            int skipTiles = 0;
            for (int nz = startz; nz != endz + zstep; nz += zstep)
                parseFloorDescription(input, tiles, x, y, nz, width, height, z - nz, ref skipTiles);
        }

        private static void parseFloorDescription(InputMessage message, List<Tile> tiles, int x, int y, int z, int width, int height, int offset, ref int skipTiles) {
            for (int nx = 0; nx < width; nx++) {
                for (int ny = 0; ny < height; ny++) {
                    if (skipTiles == 0) {
                        var tileOpt = message.PeekU16();
                        // Decide if we have to skip tiles
                        // or if it is a real tile
                        if (tileOpt == 0xFF11) {
                            message.getU16(); // skip 0xFF11
                            return;
                        }

                        //Console.WriteLine(tileOpt.ToString("X"));
                        if (tileOpt >= 0xFF00)
                            skipTiles = (short)(message.getU16() & 0xFF);
                        else {
                            //real tile so read tile
                            tiles.Add(parseTileDescription(message, new Location(x + nx + offset, y + ny + offset, z)));
                            //tiles.Add();
                            skipTiles = (short)(message.getU16() & 0xFF);
                        }
                    } else
                        skipTiles--;
                }
            }
        }

        private static Tile parseTileDescription(InputMessage message, Location location) {
            Tile tile = new Tile(location);

            while (message.PeekU16() < 0xFF00)
                tile.AddThing(GetThing(message));

            //Console.WriteLine("Tile added: things count = " + tile.things.Count);
            if (Instance.Map.GetTile(location) == null)
                Instance.Map.SetTile(tile);

            return tile;
        }

        private static Thing GetThing(InputMessage message) {
            //get thing type
            var thingId = message.getU16();

            if (thingId == 0x0061 || thingId == 0x0062) {
                //Console.WriteLine("Creature baby.");
                //creatures
                Creature creature = null;
                if (thingId == 0x0062) {
                    uint creatureId = message.getU32();
                    creature = new Creature(creatureId);
                    if (creature == null)
                        throw new Exception("[GetThing] (0x0062) Can't find the creature.");

                    byte creatureHealth = message.getByte();
                    //Console.WriteLine("Creature with id: " + creatureId + " Life: " + creatureHealth);
                } else if (thingId == 0x0061) { //creature is not known
                    uint creatureId = message.getU32();
                    //Console.WriteLine("Removing creature: " + creatureId);
                    uint creatureIdNew = message.getU32();
                    creature = new Creature(creatureIdNew);
                    creature.Name = message.getString();
                    creature.Health = message.getByte();
                }

                var direction = (Direction)message.getByte();
                creature.LookDirection = direction;
                creature.TurnDirection = direction;

                creature.Outfit = message.getOutfit();
                creature.LightLevel = message.getByte();
                creature.LightColor = message.getByte();
                creature.Speed = message.getU16();
                creature.Skull = message.getByte();
                creature.Shield = message.getByte();

                return creature;
            } else if (thingId == 0x0063) {
                uint creatureId = message.getU32();
                Creature creature = new Creature(creatureId);
                if (creature == null)
                    throw new Exception("[GetThing] (0x0063)  Can't find the creature in the battle list.");

                creature.TurnDirection = (Direction)message.getByte();

                return creature;
            } else
                return GetItem(message, thingId);
        }

        private static Item GetItem(InputMessage message, ushort itemid) {
            //Console.WriteLine("ID: " + itemid);
            if (itemid == ushort.MaxValue)
                itemid = message.getU16();

            ItemType type = Instance.items.Get(itemid);
            if (type == null)
                throw new Exception("[GetItem] (" + itemid + ") Can't find the item type."); //dat

            byte Count = 0;
            byte Subtype = 0;

            if (type.IsStackable)
                Count = message.getByte();
            else if (type.IsSplash || type.IsFluidContainer)
                Subtype = message.getByte();

            return new Item(type, Count, Subtype);
        }

        public static OtMap map;

        public static void Map_Updated(object sender, MapUpdatedEventArgs e) {
            try {
                lock (map) {
                    foreach (var tile in e.Tiles) {
                        var index = tile.Location.ToIndex();

                        OtTile mapTile = map.GetTile(tile.Location);
                        if (mapTile != null)
                            continue;
                        else if (mapTile == null)
                            mapTile = new OtTile(tile.Location);

                        mapTile.Clear();

                        for (int i = 0; i < tile.ThingCount; i++) {
                            var thing = tile.GetThing(i);

                            if (thing is Creature) {
                                var creature = thing as Creature;

                                if (creature.Type == CreatureType.PLAYER || (creature.Type == CreatureType.MONSTER) || (creature.Type == CreatureType.NPC))
                                    continue;

                                map.AddCreature(new OtCreature { Id = creature.Id, Location = creature.Location, Name = creature.Name, Type = creature.Type });
                            } else if (thing is Item) {
                                var item = tile.GetThing(i) as Item;

                                var itemType = Instance.otItems.GetItemBySpriteId((ushort)item.Id);
                                if (itemType == null) {
                                    //Console.WriteLine("Tibia item not in items.otb. Details: item id " + item.Id.ToString());
                                    continue;
                                }

                                OtItem mapItem = OtItem.Create(itemType);

                                if (mapItem.Type.IsStackable)
                                    mapItem.SetAttribute(OtItemAttribute.COUNT, item.Count);
                                else if (mapItem.Type.Group == OtItemGroup.Splash || mapItem.Type.Group == OtItemGroup.FluidContainer)
                                    mapItem.SetAttribute(OtItemAttribute.COUNT, OtConverter.TibiaFluidToOtFluid(item.SubType));

                                mapTile.AddItem(mapItem);
                            }
                        }

                        map.SetTile(mapTile);
                    }
                }
            } catch (Exception ex) {
                //Console.WriteLine("[Error] Unable to convert tibia tile to open tibia tile. Details: " + ex.Message);
            }
        }
    }
}
